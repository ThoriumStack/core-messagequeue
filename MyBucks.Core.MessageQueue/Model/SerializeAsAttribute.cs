using System;

namespace MyBucks.Core.MessageQueue.Model
{
    internal class SerializeAsAttribute : Attribute
    {
        public string Name { get; set; }
    }
}