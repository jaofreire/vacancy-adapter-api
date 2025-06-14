using CurriculumAdapter.API.DTOs;
using CurriculumAdapter.API.Response;

namespace CurriculumAdapter.API.Services.Interface
{
    public interface IAuthService
    {
        Task<APIResponse<string>> Login(LoginInputDTO loginInput);
    }
}
