using System;

namespace Thorium.Core.MessageQueue.Model
{
    internal class SerializeAsAttribute : Attribute
    {
        public string Name { get; set; }
    }
}