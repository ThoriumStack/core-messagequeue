using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBucks.Core.MessageQueue
{
    public class RabbitMqConnector
    {
        private static readonly Lazy<IConnection> _instance= new Lazy<IConnection>(StartConnection);
        private static string _username;
        private static string _password;
        private static string _hostname;

        private static IConnection StartConnection()
        {
            var factory = new ConnectionFactory() { HostName = _hostname, UserName = _username, Password = _password };
            var connection = factory.CreateConnection();
            return connection;
        }

        internal static IConnection GetConnection()
        {
            
            return _instance.Value;
        }

        public static void SetConnectionSettings(string hostName, string username, string password)
        {
            _username = username;
            _password = password;
            _hostname = hostName;
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
