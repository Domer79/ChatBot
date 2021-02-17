using System;
using System.Threading.Tasks;
using Chatbot.Abstractions.Pipe;
using Chatbot.Core.Pipe;
using NUnit.Framework;

namespace Chatbot.Core.Tests
{
    [TestFixture]
    public class PipeTests
    {
        [Test]
        public async Task RunPipeTest()
        {
            var pipe = new Pipe.Pipe(new TextPipeContext() {Text = "Hello World!"}, new PipeConfigurator()
                .RegisterHandler(new OutText1PipeHandler())
                .RegisterHandler(new OutText2PipeHandler()));

            await pipe.Start();
        }
    }
    
    public class OutText1PipeHandler: PipeHandler<ITextPipeContext>
    {
        protected override async Task InvokeAsync(ITextPipeContext context, Func<Task> next)
        {
            await Console.Out.WriteLineAsync(context.Text);
            await next();
        }
    }
    
    public class OutText2PipeHandler: PipeHandler<ITextPipeContext>
    {
        protected override async Task InvokeAsync(ITextPipeContext context, Func<Task> next)
        {
            await Console.Out.WriteLineAsync("А это второй обработчик");
            await next();
        }
    }

    public interface ITextPipeContext: IPipeContext
    {
        string Text { get; }
    }

    public class TextPipeContext : ITextPipeContext
    {
        public string Text { get; set; }
    }
}