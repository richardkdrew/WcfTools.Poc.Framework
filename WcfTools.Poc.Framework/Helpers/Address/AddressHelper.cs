using System;
using WcfTools.Poc.Framework.Helpers.Configuration;

namespace WcfTools.Poc.Framework.Helpers.Address
{
    public sealed class AddressHelper
    {
        public static class Intranet
        {
            public static Uri BaseAddress()
            {
                return BaseAddress(ConfigurationHelper.GetHostName(),
                    int.Parse(ConfigurationHelper.GetHostPort()));
            }

            public static Uri BaseAddress(string host, int port)
            {
                return new Uri(string.Format("net.tcp://{0}:{1}/", host, port));
            }
        }

        public static class InProc
        {
            public static Uri BaseAddress()
            {
                return new Uri("net.pipe://localhost/");
            }
        }

        public static class Queue
        {
            public static Uri BaseAddress()
            {
                return new Uri(string.Format("net.msmq://{0}/private/", ConfigurationHelper.GetHostName()));
            }

            public static Uri Address(string queueName)
            {
                return new Uri(string.Format("{0}{1}", BaseAddress(), queueName));
            }
        }
    }
}