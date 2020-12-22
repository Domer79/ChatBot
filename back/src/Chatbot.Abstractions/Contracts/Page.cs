namespace Chatbot.Abstractions.Contracts
{
    public class Page<T>
    {
        public T[] Items { get; set; }
        public long TotalCount { get; set; }
    }
}