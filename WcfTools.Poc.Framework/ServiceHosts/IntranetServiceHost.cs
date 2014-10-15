using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using WcfTools.Poc.Framework.Helpers.Address;
using WcfTools.Poc.Framework.Helpers.Binding;
using WcfTools.Poc.Framework.Helpers.Queue;

namespace WcfTools.Poc.Framework.ServiceHosts
{
    /// <summary>
    ///     Class used as an intranet based service host with (TCP) endpoints for all implemented contracts
    /// </summary>
    public class IntranetServiceHost : CustomServiceHostBase
    {
        public IntranetServiceHost(Type serviceType)
            : base(serviceType, new[] {AddressHelper.Intranet.BaseAddress(), AddressHelper.Queue.BaseAddress()})
        {
            ApplyIntranetEndpoints();
        }

        /// <summary>
        ///     Override the OnOpening handler and validate any queues based on whether there's an apropriate endpoint associated
        ///     with the service host
        /// </summary>
        protected override void OnOpening()
        {
            foreach (
                ServiceEndpoint endpoint in
                    Description.Endpoints.Where(endpoint => endpoint.Binding.Scheme == "net.msmq"))
            {
                QueueHelper.ValidateQueue(endpoint.Contract.ContractType);
            }
            base.OnOpening();
        }

        /// <summary>
        ///     Method used to retrieve the default binding for an intranet based service host
        /// </summary>
        /// <returns>A valid binding (pre-configured based on the rules described in the BindingHelper)</returns>
        protected override Binding DefaultBinding()
        {
            return BindingHelper.Intranet.Binding();
        }

        /// <summary>
        ///     Method used to retrieve the queue binding for an intranet based service host
        /// </summary>
        /// <returns>A valid binding (pre-configured based on the rules described in the BindingHelper)</returns>
        protected Binding QueueBinding()
        {
            return BindingHelper.Queue.Binding();
        }

        /// <summary>
        ///     Method used to apply the default endpoints for the contracts associated with an intranet based service host
        /// </summary>
        private void ApplyIntranetEndpoints()
        {          
            if (Description.Endpoints.Any(x => x.Binding is NetTcpBinding)) return;
            ApplyEndpoints();
        }

        /// <summary>
        ///     Method used to allow a queued endpoint to be added for a given contract
        /// </summary>
        /// <param name="contractType">The contract type to add the queued endpoint for</param>
        public void AddQueueEndpoint(Type contractType)
        {
            if (GetContracts().Any(x => x == contractType))
            {
                string address = EnforceQueueEndpointAddress(contractType);
                AddServiceEndpoint(contractType, QueueBinding(), address);
            }
            else
                throw new Exception(string.Format("The supplied contract was not implemented by the this service: {0}",
                    contractType));
        }

        /// <summary>
        ///     Method used to ensure that the endpoint address matches the desired (custom) convention
        /// </summary>
        /// <param name="contractType">The contract to be used as part of the convention based check</param>
        /// <returns>A string based endpoint address that corresponds with the convention used for an intranet based service host</returns>
        protected override string EnforceEndpointAddress(Type contractType)
        {
            return contractType.FullName.Replace("Contracts", "Services");
        }

        /// <summary>
        ///     Method used to ensure that the endpoint address matches the desired (custom) convention
        /// </summary>
        /// <param name="contractType">The contract to be used as part of the convention based check</param>
        /// <returns>
        ///     A string based endpoint address that corresponds with the convention used for an queue endpoint for an
        ///     intranet based service host
        /// </returns>
        public static string EnforceQueueEndpointAddress(Type contractType)
        {
            return contractType.Name;
        }
    }
}