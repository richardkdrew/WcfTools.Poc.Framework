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
                return new Uri(string.Format("net.tcp://{0}:{1}/", ConfigurationHelper.GetHostName(),
                    ConfigurationHelper.GetHostPort()));
            }

            //public static Uri BaseAddress(string host, int port)
            //{
            //    return new Uri(string.Format("net.tcp://{0}:{1}/", host, port));
            //}

            //public static Uri Address<T>(string host, int port) where T : class
            //{
            //    string name = typeof (T).FullName.Replace("Contracts", "Services");
            //    return new Uri(string.Format("{0}{1}", BaseAddress(host, port), name));
            //}
        }

        //public static class Queue
        //{
        //    public static Uri BaseAddress()
        //    {
        //        return new Uri(string.Format("net.msmq://{0}/private/", ConfigurationHelper.GetHostName()));
        //    }

        //    public static Uri Address(string queueName)
        //    {
        //        return new Uri(string.Format("{0}{1}", BaseAddress(), queueName));
        //    }
        //}
    }
}