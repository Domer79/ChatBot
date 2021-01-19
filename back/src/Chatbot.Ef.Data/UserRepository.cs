using System;
using System.Linq;
using System.Threading.Tasks;
using Chatbot.Abstractions.Repositories;
using Chatbot.Common;
using Chatbot.Model.DataModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Chatbot.Ef.Data
{
    public class UserRepository: IUserRepository
    {
        private readonly ChatbotContext _context;

        public UserRepository(ChatbotContext context)
        {
            _context = context;
        }

        public Task<User[]> GetAll()
        {
            return _context.Users.ToArrayAsync();
        }

        public Task<User> GetById(Guid id)
        {
            return _context.Users.SingleOrDefaultAsync(_ => _.Id == id);
        }

        public async Task<User> Upsert(User user)
        {
            if (user.Id == Guid.Empty)
            {
                user.Id = Guid.NewGuid();
                user.IsActive = true;
                user.Password = "0x0".GetBytes();
                _context.Add(user);
            }
            else
            {
                var entityEntry = _context.Update(user);
                entityEntry.Property(_ => _.Number).IsModified = false;
                entityEntry.Property(_ => _.Password).IsModified = false;
            }

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> SetPassword(User user)
        {
            _context.Update(user).Property(_ => _.Number).IsModified = false;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> Remove(Guid userId)
        {
            var user = await GetById(userId);
            _context.Users.Remove(user);
            var entriesCount = await _context.SaveChangesAsync();
            return entriesCount != 0;
        }

        public async Task<bool> Remove(User user)
        {
            _context.Attach(user);
            _context.Entry(user).State = EntityState.Deleted;
            var entriesCount = await _context.SaveChangesAsync();
            return entriesCount != 0;
        }

        public Task<User> GetByLoginOrEmail(string loginOrEmail)
        {
            return _context.Users.SingleOrDefaultAsync(_ => _.Login == loginOrEmail || _.Email == loginOrEmail);
        }

        public async Task<Role[]> GetRoles(Guid userId)
        {
            var user = await _context.Users.Include(_ => _.Roles).SingleOrDefaultAsync(_ => _.Id == userId);
            return user.Roles.ToArray();
        }

        public async Task<Role[]> GetRoles(string loginOrEmail)
        {
            var user = await _context.Users.Include(_ => _.Roles)
                .SingleOrDefaultAsync(_ => _.Login == loginOrEmail || _.Email == loginOrEmail);
            return user.Roles.ToArray();
        }

        public Task<User[]> GetByIds(Guid[] ids)
        {
            return _context.Users.Where(_ => ids.Contains(_.Id)).ToArrayAsync();
        }

        public Task<User[]> GetPage(int pageNumber, int pageSize, bool? isActive)
        {
            var query = _context.Users.Where(_ => _.IsOperator);
            if (isActive.HasValue)
                query = query.Where(_ => _.IsActive == isActive);

            return query.Skip(pageNumber * pageSize - pageSize).Take(pageSize).ToArrayAsync();
        }

        public Task<long> GetTotalCount(bool? isActive)
        {
            var query = _context.Users.Where(_ => _.IsOperator);
            if (isActive.HasValue)
                query = query.Where(_ => _.IsActive == isActive);

            return query.LongCountAsync();
        }
    }
}