using CurriculumAdapter.API.Data.Context;
using CurriculumAdapter.API.Data.Repositories.Interfaces;
using CurriculumAdapter.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CurriculumAdapter.API.Data.Repositories
{
    public class UserRepository(DatabaseContext context) : IUserRepository
    {
        private readonly DatabaseContext _context = context;

        public async Task Register(UserModel model)
            => await _context.Users.AddAsync(model);

        public async Task<IEnumerable<UserModel>> GetAll()
            => await _context.Users.ToListAsync();

        public async Task<UserModel> GetById(Guid id)
            => await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<UserModel>> Get(Expression<Func<UserModel, bool>> predicate)
            => await _context.Users.Where(predicate).ToListAsync();


        public void Update(UserModel model)
            => _context.Users.Update(model);


        public void Delete(UserModel model)
            => _context.Users.Remove(model);


        public Task Commit()
            => _context.SaveChangesAsync();
        
    }
}
