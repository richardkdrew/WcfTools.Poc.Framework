using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using WcfTools.Poc.Framework.Helpers.Binding;

namespace WcfTools.Poc.Framework.Clients
{
    public class IntranetClient<T> : CustomClientBase<T> where T : class
    {
        public IntranetClient(Uri baseAddress) : base(baseAddress)
        {
        }

        protected override Binding Binding()
        {
            return BindingHelper.Intranet.Binding();
        }

        protected override EndpointAddress EnforceEndpointAddress(Uri baseAddress)
        {
            string serviceName = typeof (T).FullName.Replace("Contracts", "Services");
            return new EndpointAddress(string.Format("{0}{1}", baseAddress, serviceName));
        }
    }
}