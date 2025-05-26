using CurriculumAdapter.API.Data.Repositories;
using CurriculumAdapter.API.Data.Repositories.Interfaces;
using CurriculumAdapter.API.Response;
using CurriculumAdapter.API.Services.Interface;
using CurriculumAdapter.API.Utils;
using OpenAI;
using OpenAI.Assistants;
using OpenAI.Embeddings;
using OpenAI.Files;
using System.Text;



namespace CurriculumAdapter.API.Services
{
    public class AdvisorService : IAdvisorService
    {
        private readonly IConfiguration _configuration;
        private readonly IJobsCollectionRepository _jobsCollectionRepository;
        private readonly string _apiKeyOpenAI;
        private readonly string _assistantId;

        public AdvisorService(IConfiguration configuration, IJobsCollectionRepository jobsCollectionRepository)
        {
            _configuration = configuration;
            _jobsCollectionRepository = jobsCollectionRepository;
            _apiKeyOpenAI = _configuration["OpenAI:ApiKey"] ?? Environment.GetEnvironmentVariable("OPEN_AI_API_KEY")!;
            _assistantId = _configuration["OpenAI:AdvisorAssistantId"] ?? Environment.GetEnvironmentVariable("ADVISOR_ASSISTANT_ID")!;
        }

#pragma warning disable OPENAI001
        public async Task<APIResponse<string>> SendPromptToAdvisorAssistant(string curriculumData)
        {
            var openAIClient = new OpenAIClient(_apiKeyOpenAI);
            var assistantClient = openAIClient.GetAssistantClient();
            //var fileClient = openAIClient.GetOpenAIFileClient();

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

        private async Task<OpenAIFile> UploadFile(OpenAIFileClient fileClient, IFormFile file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;

                return await fileClient.UploadFileAsync(stream, file.FileName.Trim(), FileUploadPurpose.Assistants);
            }
        }
    }
}
