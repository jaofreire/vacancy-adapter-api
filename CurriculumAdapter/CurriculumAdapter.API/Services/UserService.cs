using CurriculumAdapter.API.Data.Repositories.Interfaces;
using CurriculumAdapter.API.DTOs;
using CurriculumAdapter.API.Models;
using CurriculumAdapter.API.Response;
using CurriculumAdapter.API.Services.Interface;
using CurriculumAdapter.API.Models.Enums;
using CurriculumAdapter.API.Utils;

namespace CurriculumAdapter.API.Services
{
    public class UserService(IUserRepository repository) : IUserService
    {
        private readonly IUserRepository _repository = repository;

        public async Task<APIResponse<UserModel>> Register(RegisterUserInputDTO input)
        {
            var existsSameEmailUser = await _repository.Get(x => x.Email == input.Email);

            if (existsSameEmailUser.Any())
                return new APIResponse<UserModel>(false, 400, "Já existe uma conta com este Email, tente fazer Login ou recuperar sua senha");

            var passwordHash = PasswordHashUtils.CreateHashPassword(input.Password);

            var newUser = new UserModel()
            {
                Type = UserTypeEnum.Default,
                FirstName = input.FirstName,
                LastName = input.LastName,
                Email = input.Email,
                PasswordHash = passwordHash,
            };

            //CRIAR HASH DA SENHA
            await _repository.Register(newUser);
            await _repository.Commit();

            return new APIResponse<UserModel>(true, 200, "Usuário cadastrado com sucesso!");
        }

        public async Task<APIResponse<UserModel>> GetAll()
        {
            var users = await _repository.GetAll();

            return new APIResponse<UserModel>(true, 200, "Usuários buscados com sucesso!", null, users);
        }

        public async Task<APIResponse<UserModel>> GetById(Guid id)
        {
            var user = await _repository.GetById(id);

            if(user is null)
                return new APIResponse<UserModel>(false, 404, "Usuário não encontrado");

            return new APIResponse<UserModel>(true, 200, "Usuário encontrado com sucesso!", user, null);
        }

        public async Task<APIResponse<UserModel>> Update(Guid id, UserModel model)
        {
            return new APIResponse<UserModel>(false, 400, "Endpoint Em implementação");
        }

        public async Task<APIResponse<UserModel>> Delete(Guid id)
        {
            var user = await _repository.GetById(id);

            if (user is null)
                return new APIResponse<UserModel>(false, 404, "Usuário não encontrado");

            _repository.Delete(user);
            await _repository.Commit();

            return new APIResponse<UserModel>(true, 200, "Usuário removido com sucesso!");
        }
    }
}
