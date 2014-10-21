using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using WcfTools.Poc.Framework.Helpers.Binding;
using WcfTools.Poc.Framework.ServiceHosts;

namespace WcfTools.Poc.Framework.Clients
{
    public class InProcClient<TImplementation, TInterface> : CustomClientBase<TInterface> where TImplementation : class
        where TInterface : class
    {
        private InProcServiceHost _serviceHost;

        public InProcClient(Guid id)
            : base(new Uri("net.pipe://localhost/" + id))
        {
            StartService(new Uri("net.pipe://localhost/" + id));
        }

        protected override Binding Binding()
        {
            return BindingHelper.InProc.Binding();
        }

        protected override EndpointAddress EnforceEndpointAddress(Uri baseAddress)
        {
            string serviceName = typeof (TInterface).Name;
            return new EndpointAddress(string.Format("{0}/{1}", baseAddress, serviceName));
        }

        private void StartService(Uri baseAddress)
        {
            _serviceHost = new InProcServiceHost(typeof (TImplementation), new[] {baseAddress});
            _serviceHost.Open();
        }

        public void Close()
        {
            if (_serviceHost.State != CommunicationState.Closed) _serviceHost.Close();
        }
    }
}