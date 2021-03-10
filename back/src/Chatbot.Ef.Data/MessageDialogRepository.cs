using System;
using System.Linq;
using System.Threading.Tasks;
using Chatbot.Abstractions.Repositories;
using Chatbot.Common;
using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chatbot.Ef.Data
{
    public class MessageDialogRepository: IMessageDialogRepository
    {
        private readonly ChatbotContext _context;

        public MessageDialogRepository(ChatbotContext context)
        {
            _context = context;
        }

        public Task<MessageDialog[]> GetAll()
        {
            return _context.Dialogs.OrderByDescending(_ => _.DateCreated).ToArrayAsync();
        }

        public Task<MessageDialog[]> GetPage(int pageNumber, int pageSize)
        {
            return _context.Dialogs
                .OrderByDescending(_ => _.DateCreated)
                .Skip(pageNumber * pageSize - pageSize)
                .Take(pageSize).ToArrayAsync();
        }

        public Task<MessageDialog[]> GetPage(DialogStatus status, int number, int size)
        {
            IQueryable<MessageDialog> dialogs = _context.Dialogs.Where(_ => _.Id == Guid.Empty);
            foreach (var flag in Helper.GetFlags(status))
            {
                dialogs = dialogs.Concat(_context.Dialogs.Where(_ => _.DialogStatus == flag));
            }
            
            return dialogs
                .Include(_ => _.Operator)
                .Include(_ => _.Client)
                .OrderByDescending(_ => _.DateCreated)
                .Skip(number * size - size)
                .Take(size)
                .ToArrayAsync();
        }
        
        public Task<MessageDialog[]> GetOffline(int number, int size)
        {
            var query = _context.Dialogs.Where(_ => _.Offline);
            return query
                .Include(_ => _.Operator)
                .Include(_ => _.Client)
                .OrderByDescending(_ => _.DateCreated)
                .Skip(number * size - size)
                .Take(size)
                .ToArrayAsync();
        }

        public Task<MessageDialog[]> GetByFilter(DialogStatus? linkType, string @operator, string client,
            DateTime? startDate, DateTime? closeDate, int? dialogNumber, int pageNumber, int pageSize)
        {
            var query = GetFilterQuery(linkType, @operator, client, startDate, closeDate, dialogNumber);

            return query
                .Include(_ => _.Operator)
                .Include(_ => _.Client)
                .OrderByDescending(_ => _.DateCreated)
                .Skip(pageNumber * pageSize - pageSize)
                .Take(pageSize)
                .ToArrayAsync();
        }

        public Task<long> GetTotalCountByFilter(DialogStatus? linkType, string @operator, string client, DateTime? startDate,
            DateTime? closeDate, int? dialogNumber, int pageNumber, int pageSize)
        {
            var query = GetFilterQuery(linkType, @operator, client, startDate, closeDate, dialogNumber);
            return query.LongCountAsync();
        }

        public Task<MessageDialog[]> GetDialogsByPeriod(DateTime startDate, DateTime endDate)
        {
            return _context.Dialogs
                .Include(_ => _.Operator)
                .Include(_ => _.Client)
                .Where(_ => _.DateCreated >= startDate && _.DateCreated <= endDate)
                .ToArrayAsync();
        }

        public Task<long> GetTotalCount()
        {
            return _context.Dialogs.LongCountAsync();
        }

        public Task<long> GetTotalCount(DialogStatus status)
        {
            IQueryable<MessageDialog> dialogs = _context.Dialogs.Where(_ => _.Id == Guid.Empty);
            foreach (var flag in Helper.GetFlags(status))
            {
                dialogs = dialogs.Concat(_context.Dialogs.Where(_ => _.DialogStatus == flag));
            }
            
            return dialogs.LongCountAsync();
        }

        public Task<long> GetOfflineTotalCount()
        {
            return _context.Dialogs.Where(_ => _.Offline).LongCountAsync();
        }

        public Task<MessageDialog[]> GetByStatus(DialogStatus status)
        {
            return _context.Dialogs
                .Where(_ => _.DialogStatus == status)
                .OrderByDescending(_ => _.DateCreated)
                .ToArrayAsync();
        }

        public async Task<MessageDialog> Upsert(MessageDialog dialog)
        {
            if (dialog.Id == Guid.Empty)
            {
                dialog.Id = Guid.NewGuid();
                _context.Dialogs.Add(dialog);
            }
            else
            {
                _context.Update(dialog).Property(_ => _.Number).IsModified = false;
            }
            
            await _context.SaveChangesAsync();
            _context.Entry(dialog).State = EntityState.Detached;
            return dialog;
        }

        public async Task<bool> Delete(MessageDialog dialog)
        {
            _context.Dialogs.Remove(dialog);
            return await _context.SaveChangesAsync() > 0;
        }

        public Task<MessageDialog> GetById(Guid messageDialogId)
        {
            return _context.Dialogs.AsNoTracking().SingleOrDefaultAsync(_ => _.Id == messageDialogId);
        }

        private IQueryable<MessageDialog> GetFilterQuery(DialogStatus? linkType, string @operator, string client, DateTime? startDate,
            DateTime? closeDate, int? dialogNumber)
        {
            IQueryable<MessageDialog> query = null;
            if (linkType.HasValue)
            {
                foreach (var flag in Helper.GetFlags(linkType.Value))
                {
                    if (query == null)
                    {
                        query = _context.Dialogs.Where(_ => _.DialogStatus == flag);
                        continue;
                    }
                    
                    query = query.Concat(_context.Dialogs.Where(_ => _.DialogStatus == flag));
                }
            }

            if (query == null)
            {
                query = _context.Dialogs;
            }
                
            if (@operator != null)
            {
                query = query.Where(_ => _.Operator.Fio.Contains(@operator));
            }

            if (client != null)
            {
                query = query.Where(_ => _.Client.Fio.Contains(client));
            }

            if (startDate.HasValue)
            {
                var nextDay = startDate.Value.AddDays(1);
                query = query.Where(_ => _.DateCreated >= startDate && _.DateCreated < nextDay);
            }

            if (closeDate.HasValue)
            {
                var nextDay = closeDate.Value.AddDays(1);
                query = query.Where(_ => _.DateCompleted >= closeDate && _.DateCompleted < nextDay);
            }

            if (dialogNumber.HasValue)
            {
                query = query.Where(_ => _.Number == dialogNumber);
            }

            return query;
        }
    }
}