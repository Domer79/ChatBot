using System;
using System.Linq;
using System.Threading.Tasks;
using Chatbot.Abstractions.Core.Services;
using Chatbot.Abstractions.Repositories;
using Chatbot.Model.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Chatbot.Ef.Data
{
    public class RoleRepository: IRoleRepository
    {
        private readonly ChatbotContext _context;

        public RoleRepository(ChatbotContext context)
        {
            _context = context;
        }

        public Task<Permission[]> GetPermissions(Guid[] roleIds)
        {
            return _context.Roles
                .Include(_ => _.Permissions)
                .Where(_ => roleIds.Contains(_.Id))
                .SelectMany(_ => _.Permissions)
                .ToArrayAsync();
        }

        public Task<Role[]> GetAll()
        {
            return _context.Roles.ToArrayAsync();
        }
    }
}