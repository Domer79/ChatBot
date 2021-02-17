using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Pipe;

namespace Chatbot.Hosting.Hubs.MessageHandlers
{
    public class ValidateDialogHandler: PipeHandler<IMessagePipeContext>
    {
        protected override Task InvokeAsync(IMessagePipeContext context, Func<IPipeContext, Task> next)
        {
            if (context.DialogGroup == null)
                throw new InvalidOperationException(
                    $"Group by message dialog id '{context.Message.MessageDialogId}' not found");
            if (!context.DialogGroup.UserExist(context.User))
            {
                throw new InvalidOperationException($"User {context.User.Login} not connected in chat room");
            }

            return next(context);
        }
    }
}