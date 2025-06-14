using CurriculumAdapter.API.Data.Repositories;
using CurriculumAdapter.API.Data.Repositories.Interfaces;
using CurriculumAdapter.API.Models.Logs;
using CurriculumAdapter.API.Response;
using CurriculumAdapter.API.Services.Interface;
using CurriculumAdapter.API.Utils;
using OpenAI;
using OpenAI.Assistants;
using OpenAI.Files;
using System.Security.Claims;
using CurriculumAdapter.API.Models.Enums;

namespace CurriculumAdapter.API.Services
{
    public class AdaptService : IAdaptService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFeatureUsageLogRepository _featureUsageLogRepository;
        private readonly string _apiKeyOpenAI;
        private readonly string _assistantId;

        public AdaptService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IFeatureUsageLogRepository featureUsageLogRepository)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _featureUsageLogRepository = featureUsageLogRepository;
            _apiKeyOpenAI = _configuration["OpenAI:ApiKey"] ?? Environment.GetEnvironmentVariable("OPEN_AI_API_KEY")!;
            _assistantId = _configuration["OpenAI:AssistantIdGPT4.1Mini"] ?? Environment.GetEnvironmentVariable("ASSISTANT_ID_GPT_41_MINI")!;
        }

        #pragma warning disable OPENAI001
        public async Task<APIResponse<AssistantResponse>> SendPromptToAssistant(string recaptchaToken ,string description, string userSkills, IFormFile file)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
                if (!await RecaptchaUtils.ValidateRecaptcha(recaptchaToken, _configuration["ReCaptcha:SecretKey"] ?? Environment.GetEnvironmentVariable("RECAPTCHA_SECRET_KEY")!))
                    return new APIResponse<AssistantResponse>(false, 401, "Token recaptcha inválido", null, null);


            var userType = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (userType == UserTypeEnum.Default.ToString())
            {
                var userFeatureUsageLogsToday = await _featureUsageLogRepository.Get(
                    x => x.UserId == userId
                    && x.UsageDate.Date == DateTime.Now.Date
                    && x.FeatureName == FeatureNameEnum.CurriculumAdapter.ToString());

                if (userFeatureUsageLogsToday.Count() >= 2)
                    return new APIResponse<AssistantResponse>(false, 400, "Limite gratuito diário atingido");
            }

            //var inputPrompt = PromptUtils.GenerateCurriculumAdapterPrompt(description, userSkills);

            var openAIClient = new OpenAIClient(_apiKeyOpenAI);
            var assistantClient = openAIClient.GetAssistantClient();
            var fileClient = openAIClient.GetOpenAIFileClient();

            var assistant = await assistantClient.GetAssistantAsync(_assistantId);

            if(assistant is not null)
            {
                var uploadResponse = await UploadFile(fileClient, file);

                if (!string.IsNullOrEmpty(uploadResponse.Id))
                {

                    assistant.Value.ToolResources.FileSearch.NewVectorStores.Add(new VectorStoreCreationHelper([uploadResponse.Id]));

                    var threadOptions = new ThreadCreationOptions()
                    {
                        InitialMessages = { $"Descrição: {description}. Texto de competências: {userSkills}" },
                        ToolResources = assistant.Value.ToolResources
                    };

                    var threadRun = await assistantClient.CreateThreadAndRunAsync(assistant.Value.Id, threadOptions);

                    do
                    {
                        threadRun = await assistantClient.GetRunAsync(threadRun.Value.ThreadId, threadRun.Value.Id);
                    } 
                    while (!threadRun.Value.Status.IsTerminal);

                    var assistantMessages = assistantClient.GetMessages(threadRun.Value.ThreadId, new MessageCollectionOptions() { Order = MessageCollectionOrder.Ascending });

                    foreach (var message in assistantMessages)
                    {

                        if(message.Role == MessageRole.Assistant)
                        {
                            var messageContent = message.Content[0].Text;

                            string[] messageSplited = messageContent.Split("@#$%", 2);

                            var assistantResponse = new AssistantResponse()
                            {
                                AdaptedCurriculumContent = messageSplited[0],
                                ChangesExplanation = messageSplited[1]
                            };

                            var featureUsageLog = new FeatureUsageLogModel(userId, FeatureNameEnum.CurriculumAdapter);

                            await _featureUsageLogRepository.Register(featureUsageLog);
                            await _featureUsageLogRepository.Commit();

                            return new APIResponse<AssistantResponse>(true, 200, "Prompt enviado e processado com sucesso!", assistantResponse, null);
                        }


                    }


                }

            }

            return new APIResponse<AssistantResponse>(false, 400, "Não foi possível enviar prompt para o modelo assistente");
        }

        private static async Task<OpenAIFile> UploadFile(OpenAIFileClient fileClient ,IFormFile file)
        {
            using(var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;

                return await fileClient.UploadFileAsync(stream, file.FileName.Trim(), FileUploadPurpose.Assistants);
            }
        }

    }
}
