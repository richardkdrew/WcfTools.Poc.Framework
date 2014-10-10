using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using WcfTools.Poc.Framework.Helpers.Binding;

namespace WcfTools.Poc.Framework.Clients
{
    public class QueueClient<T> : CustomClientBase<T> where T : class
    {
        public QueueClient(Uri baseAddress)
            : base(baseAddress)
        {
        }

        protected override Binding Binding()
        {
            return BindingHelper.Queue.Binding();
        }

        protected override EndpointAddress EnforceEndpointAddress(Uri baseAddress)
        {
            string serviceName = typeof(T).Name;
            return new EndpointAddress(string.Format("{0}{1}", baseAddress, serviceName));
        }
    }
}