using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Repositories;
using Chatbot.Model.DataModel;

namespace Chatbot.Ef.Data
{
    public class UserRoleRepository: IUserRoleRepository
    {
        private readonly ChatbotContext _context;

        public UserRoleRepository(ChatbotContext context)
        {
            _context = context;
        }

        public Task Add(Guid userId, Guid roleId)
        {
            var entity = new UserRole()
            {
                UserId = userId,
                RoleId = roleId,
            };

            _context.UserRoles.Add(entity);
            return _context.SaveChangesAsync();
        }

        public async Task<bool> Delete(Guid userId, Guid roleId)
        {
            try
            {
                var entity = await _context.UserRoles.FindAsync(userId, roleId);
                _context.UserRoles.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}