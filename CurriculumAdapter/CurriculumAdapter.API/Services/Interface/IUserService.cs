using CurriculumAdapter.API.DTOs;
using CurriculumAdapter.API.Models;
using CurriculumAdapter.API.Response;

namespace CurriculumAdapter.API.Services.Interface
{
    public interface IUserService
    {
        Task<APIResponse<UserModel>> Register(UserModel model);
        Task<APIResponse<UserModel>> GetAll();
        Task<APIResponse<UserModel>> GetById(Guid id);
        Task<APIResponse<UserModel>> Update(Guid id, UserModel model);
        Task<APIResponse<UserModel>> Delete(Guid id);

    }
}
