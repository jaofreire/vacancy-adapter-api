﻿using CurriculumAdapter.API.Response;

namespace CurriculumAdapter.API.Services.Interface
{
    public interface IAdaptService
    {
        Task<APIResponse<AssistantResponse>> SendPromptToAssistant(string recaptchaToken, string preRequisites, string userPrompt, IFormFile file);
    }
}
