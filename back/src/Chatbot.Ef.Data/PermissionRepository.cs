using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Repositories;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace Chatbot.Ef.Data
{
    public class PermissionRepository: IPermissionRepository
    {
        private readonly ChatbotContext _context;

        public PermissionRepository(ChatbotContext context)
        {
            _context = context;
        }

        public Task<Permission> GetPermission(SecurityPolicy securityPolicy)
        {
            return _context.Permissions.SingleOrDefaultAsync(_ => _.Politic == securityPolicy);
        }

        public Task<Permission[]> GetAll()
        {
            return _context.Permissions.ToArrayAsync();
        }

        public async Task<Permission> Upsert(Permission permission)
        {
            if (permission.Id == Guid.Empty)
            {
                permission.Id = Guid.NewGuid();
                _context.Add(permission);
                await _context.SaveChangesAsync();
            }
            else
            {
                _context.Update(permission);
                await _context.SaveChangesAsync();
            }

            return permission;
        }

        public async Task<bool> Delete(Permission permission)
        {
            _context.Remove(permission);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(SecurityPolicy policy)
        {
            var permission = await _context.Permissions.SingleAsync(_ => _.Politic == policy);
            return await Delete(permission);
        }
    }
}