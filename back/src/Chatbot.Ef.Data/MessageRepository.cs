using System;
using System.Linq;
using System.Threading.Tasks;
using Chatbot.Abstractions.Repositories;
using Chatbot.Model.DataModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Chatbot.Ef.Data
{
    public class MessageRepository: IMessageRepository
    {
        private readonly ChatbotContext _context;

        public MessageRepository(ChatbotContext context)
        {
            _context = context;
        }

        public async Task<Message> Add(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<bool> Update(Message message)
        {
            _context.Update(message);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public Task<Message> GetMessage(Guid messageId)
        {
            return _context.Messages.SingleOrDefaultAsync(_ => _.Id == messageId);
        }

        public Task<Message[]> GetDialogMessages(Guid dialogId)
        {
            return _context.Messages
                .Where(_ => _.MessageDialogId == dialogId)
                .OrderBy(_ => _.Time)
                .ToArrayAsync();
        }

        public Task<Message[]> GetFirstMessages(Guid[] dialogIds)
        {
            return GetSingleWndByTimeMessages(dialogIds);
        }

        public Task<Message[]> GetLastMessages(Guid[] dialogIds)
        {
            return GetSingleWndByTimeMessages(dialogIds, false);
        }

        public Task<Message[]> GetAllMessagesInLinkedDialogsByNumber(int dialogNumber)
        {
            const string sql = @"
with q1 (main_number, message_dialog_id, number, based_on, level)
as ( 
    select number as main_number, message_dialog_id, number, based_id, 1 as level from message_dialog where based_id is null
    union all
    select q1.main_number, md.message_dialog_id, md.number, md.based_id, q1.level + 1 as level from message_dialog md
        inner join q1 on md.based_id = q1.message_dialog_id
    )
select * from message where message_dialog_id in (
    select message_dialog_id from q1 where main_number = (select main_number from q1 where number = @dialogNumber)
) order by time";

            return _context.Messages.FromSqlRaw(sql, new SqlParameter("dialogNumber", dialogNumber)).ToArrayAsync();
        }

        private Task<Message[]> GetSingleWndByTimeMessages(Guid[] dialogIds, bool orderAsc = true)
        {
            const string sql = @"with q1
    as (
        select
            ROW_NUMBER() over (partition by message_dialog_id order by time {0}) as row_number,
            *
        from message
    ),
     q2 
as ( 
    select * from q1 where q1.row_number = 1
     )

select
    message_id,
    content,
    type,
   owner,
   status,
   time,
   message_dialog_id,
   sender
from q2
where message_dialog_id in ({1})";
            var @params = dialogIds.Select((id, index) => new SqlParameter($@"@id{index}", id)).ToArray();
            var paramNames = string.Join(", ", @params.Select(_ => _.ParameterName));
            var query = string.Format(sql, orderAsc ? "asc" : "desc", paramNames);
            return _context.Messages.FromSqlRaw(query, @params).ToArrayAsync();
        }
    }
}