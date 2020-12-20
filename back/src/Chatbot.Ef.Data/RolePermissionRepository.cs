using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Repositories;
using Chatbot.Model.DataModel;

namespace Chatbot.Ef.Data
{
    public class RolePermissionRepository: IRolePermissionRepository
    {
        private readonly ChatbotContext _context;

        public RolePermissionRepository(ChatbotContext context)
        {
            _context = context;
        }

        public Task Add(Guid roleId, Guid permissionId)
        {
            var entity = new RolePermission()
            {
                RoleId = roleId,
                PermissionId = permissionId
            };

            _context.RolePermissions.Add(entity);
            return _context.SaveChangesAsync();
        }

        public async Task<bool> Delete(Guid roleId, Guid permissionId)
        {
            try
            {
                var entity = await _context.RolePermissions.FindAsync(roleId, permissionId);
                _context.RolePermissions.Remove(entity);
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