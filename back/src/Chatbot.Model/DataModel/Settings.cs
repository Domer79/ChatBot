using System;

namespace Chatbot.Model.DataModel
{
    public class Settings
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public DateTime DateCreated { get; set; }
    }
}