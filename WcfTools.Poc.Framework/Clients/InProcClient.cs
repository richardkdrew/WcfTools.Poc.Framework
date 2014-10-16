using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using WcfTools.Poc.Framework.Helpers.Address;
using WcfTools.Poc.Framework.Helpers.Binding;
using WcfTools.Poc.Framework.ServiceHosts;

namespace WcfTools.Poc.Framework.Clients
{
    public class InProcClient<TImplementation, TInterface> : CustomClientBase<TInterface> where TImplementation : class
        where TInterface : class
    {
        private Guid _id;
        private InProcServiceHost _serviceHost;

        public InProcClient() : base(AddressHelper.InProc.BaseAddress())
        {
            StartService(AddressHelper.InProc.BaseAddress());
        }

        protected override Binding Binding()
        {
            return BindingHelper.InProc.Binding();
        }

        protected override EndpointAddress EnforceEndpointAddress(Uri baseAddress)
        {
            _id = Guid.NewGuid();
            string serviceName = typeof (TInterface).Name;
            return new EndpointAddress(string.Format("{0}{1}/{2}", baseAddress, _id, serviceName));
        }

        private void StartService(Uri baseAddress)
        {
            _serviceHost = new InProcServiceHost(typeof (TImplementation), new[] {baseAddress},
                _id);

            _serviceHost.Open();
        }

        public void Close()
        {
            if (_serviceHost.State != CommunicationState.Closed) _serviceHost.Close();
        }
    }
}