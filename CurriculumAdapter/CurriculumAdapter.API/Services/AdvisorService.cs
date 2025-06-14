using CurriculumAdapter.API.Data.Repositories;
using CurriculumAdapter.API.Data.Repositories.Interfaces;
using CurriculumAdapter.API.Models.Enums;
using CurriculumAdapter.API.Models.Logs;
using CurriculumAdapter.API.Response;
using CurriculumAdapter.API.Services.Interface;
using CurriculumAdapter.API.Utils;
using OpenAI;
using OpenAI.Assistants;
using OpenAI.Embeddings;
using OpenAI.Files;
using System.Security.Claims;
using System.Text;



namespace CurriculumAdapter.API.Services
{
    public class AdvisorService : IAdvisorService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJobsCollectionRepository _jobsCollectionRepository;
        private readonly IFeatureUsageLogRepository _featureUsageLogRepository;
        private readonly string _apiKeyOpenAI;
        private readonly string _assistantId;

        public AdvisorService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IJobsCollectionRepository jobsCollectionRepository, IFeatureUsageLogRepository featureUsageLogRepository)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _jobsCollectionRepository = jobsCollectionRepository;
            _featureUsageLogRepository = featureUsageLogRepository;
            _apiKeyOpenAI = _configuration["OpenAI:ApiKey"] ?? Environment.GetEnvironmentVariable("OPEN_AI_API_KEY")!;
            _assistantId = _configuration["OpenAI:AdvisorAssistantId"] ?? Environment.GetEnvironmentVariable("ADVISOR_ASSISTANT_ID")!;
        }

#pragma warning disable OPENAI001
        public async Task<APIResponse<string>> SendPromptToAdvisorAssistant(string curriculumData)
        {
            var userType = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (userType == UserTypeEnum.Default.ToString())
            {
                var userFeatureUsageLogsToday = await _featureUsageLogRepository.Get(
                    x => x.UserId == userId
                    && x.UsageDate.Date == DateTime.Now.Date
                    && x.FeatureName == FeatureNameEnum.Advisor.ToString());

                if (userFeatureUsageLogsToday.Count() >= 2)
                    return new APIResponse<string>(false, 400, "Limite gratuito diário atingido");
            }

            var openAIClient = new OpenAIClient(_apiKeyOpenAI);
            var assistantClient = openAIClient.GetAssistantClient();

            var assistant = await assistantClient.GetAssistantAsync(_assistantId);

            if (assistant is not null)
            {
                //var uploadResponse = await UploadFile(fileClient, file);
                var embedding = await GenerateEmbedding(curriculumData);
                var similarJobs = await _jobsCollectionRepository.SearchJobsBySimilarVectors(embedding);

                var knowledgeBase = new StringBuilder();

                foreach (var job in similarJobs)
                {
                    knowledgeBase.AppendLine(job.Payload["Título"].StringValue);
                    knowledgeBase.AppendLine(job.Payload["Descrição"].StringValue);
                    knowledgeBase.AppendLine(job.Payload["Detalhes adicionais"].StringValue);
                    knowledgeBase.AppendLine(job.Payload["Data extração da vaga"].StringValue);
                }
                    

                var threadOptions = new ThreadCreationOptions()
                {
                    InitialMessages = { $"Base de conhecimento: {knowledgeBase.ToString()}. Dados do curriculo: {curriculumData}" },
                    //ToolResources = assistant.Value.ToolResources
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

                    if (message.Role == MessageRole.Assistant)
                    {
                        var messageContent = message.Content[0].Text;

                        var featureUsageLog = new FeatureUsageLogModel(userId, FeatureNameEnum.Advisor);

                        await _featureUsageLogRepository.Register(featureUsageLog);
                        await _featureUsageLogRepository.Commit();

                        return new APIResponse<string>(true, 200, "Prompt enviado e processado com sucesso!", messageContent, null);
                    }


                }

            }

            return new APIResponse<string>(true, 400, "Não foi possível enviar prompt para o modelo AdvisorAssistant");
        }


        private async Task<ReadOnlyMemory<float>> GenerateEmbedding(string curriculum)
        {
            var embeddingClient = new EmbeddingClient("text-embedding-3-small", _apiKeyOpenAI);

            var embeddingGenerationOptions = new EmbeddingGenerationOptions()
            {
                Dimensions = 1536
            };

            var embeddingGenerated = await embeddingClient.GenerateEmbeddingAsync(curriculum, embeddingGenerationOptions);

            return embeddingGenerated.Value.ToFloats();
        }

    }
}
