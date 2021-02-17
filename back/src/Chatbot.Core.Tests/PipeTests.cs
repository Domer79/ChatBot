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
            var pipe = new Pipe.Pipe(new PipeConfigurator()
                .AddHandler(new OutText1PipeHandler(new EmptyPipe()))
                .AddHandler(new OutText2PipeHandler()));

            await pipe.Start(new TextPipeContext() {Text = "Hello World!"});
        }
    }
    
    public class OutText1PipeHandler: PipeHandler<ITextPipeContext>
    {
        private readonly EmptyPipe _emptyPipe;

        public OutText1PipeHandler(EmptyPipe emptyPipe)
        {
            _emptyPipe = emptyPipe;
        }

        protected override async Task InvokeAsync(ITextPipeContext context, Func<IPipeContext, Task> next)
        {
            await Console.Out.WriteLineAsync(context.Text);
            await _emptyPipe.Start(context);
        }
    }
    
    public class OutText2PipeHandler: PipeHandler<ITextPipeContext>
    {
        protected override async Task InvokeAsync(ITextPipeContext context, Func<IPipeContext, Task> next)
        {
            await Console.Out.WriteLineAsync("А это второй обработчик");
            await next(context);
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
    
    public class EmptyPipe: IPipe
    {
        public Task Start(IPipeContext context)
        {
            return Task.CompletedTask;
        }
    }
}