using System;

namespace Chatbot.Common
{
    public class EnumInfo
    {
        public string Name { get; set; }
        public Enum Enum { get; set; }
        public string Description { get; set; }
    }
    
    public class EnumInfo<T>: EnumInfo where T: Enum
    {
        public EnumInfo()
        {
        }

        public EnumInfo(EnumInfo enumInfo)
        {
            Name = enumInfo.Name;
            Description = enumInfo.Description;
            Enum = enumInfo.Enum;
        }

        public T Value { get; set; }
    }
}