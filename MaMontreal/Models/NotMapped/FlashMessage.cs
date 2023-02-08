using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MaMontreal.Models.NotMapped
{
    public class FlashMessage : ISerializable
    {
        public string Message { get; set; } = null!;
        public string Type { get; set; } = null!;

        public FlashMessage()
        {
        }

        public FlashMessage(string message, string type)
        {
            Message = message;
            Type = type;
        }

        // public FlashMessage(SerializationInfo info, StreamingContext context)
        // {
        //     Message = info.GetString("Message");
        //     Type = info.GetString("Type");
        // }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Message", Message);
            info.AddValue("Type", Type);
        }
    }
}