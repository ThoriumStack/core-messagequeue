using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue
{
    public class RabbitMqConnector
    {
        private static readonly Lazy<IConnection> _instance= new Lazy<IConnection>(StartConnection);

        private static IConnection StartConnection()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            return connection;
        }

        internal static IConnection GetConnection()
        {
            return _instance.Value;
        }

        public static void Disconnect()
        {
            if (_instance.IsValueCreated && _instance.Value.IsOpen)
            {
                _instance.Value.Close();
            }
        }
    }
}
