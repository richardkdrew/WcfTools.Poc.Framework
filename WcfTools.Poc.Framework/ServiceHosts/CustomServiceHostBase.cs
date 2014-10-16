using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using WcfTools.Poc.Framework.Behaviors;

namespace WcfTools.Poc.Framework.ServiceHosts
{
    /// <summary>
    ///     Custom service host class used as a base to provide a method for quickly creating service instances pre-configured
    ///     with endpoints, behaviors, etc...
    /// </summary>
    [IntranetServiceBehavior]
    public abstract class CustomServiceHostBase : ServiceHost
    {
        protected CustomServiceHostBase(Type serviceType, Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            AddMetadataEndpoints();
        }

        /// <summary>
        ///     Abstract method allowing derived types to specify their own convention for endpoint addresses
        /// </summary>
        /// <param name="contractType">The contract to use</param>
        /// <returns>A valid string representing the result of the endpoint address check</returns>
        protected abstract string EnforceEndpointAddress(Type contractType);

        /// <summary>
        ///     Abstract method allowing derived types to specify their own default binding
        /// </summary>
        /// <returns>A valid binding (pre-configured based on the rules described in the BindingHelper)</returns>
        protected abstract Binding DefaultBinding();

        /// <summary>
        ///     Method used to ensure a metadata behavior is present
        /// </summary>
        private void EnforceMetadataBehavior()
        {
            var metadataBehavior = Description.Behaviors.Find<ServiceMetadataBehavior>();

            if (metadataBehavior != null) return;
            metadataBehavior = new ServiceMetadataBehavior();
            Description.Behaviors.Add(metadataBehavior);
        }

        /// <summary>
        ///     Method used to add metadata endpoints for the base addresses associated with the service host
        /// </summary>
        private void AddMetadataEndpoints()
        {
            EnforceMetadataBehavior();

            if (Description.Endpoints.Any(endpoint => endpoint.Contract.ContractType == typeof (IMetadataExchange)))
                return;
            // Add a metadata endpoint at each base address using the "/MEX" addressing convention
            foreach (Uri baseAddress in BaseAddresses)
            {
                Binding binding = null;
                switch (baseAddress.Scheme)
                {
                    case "net.tcp":
                    {
                        binding = MetadataExchangeBindings.CreateMexTcpBinding();
                        break;
                    }
                }
                if (binding != null) AddServiceEndpoint(typeof (IMetadataExchange), binding, "MEX");
            }
        }

        /// <summary>
        ///     Method used to apply endpoints for all service contracts implemented by the derived service (based on the default
        ///     binding and the EnforcedEndpointAddress)
        /// </summary>
        protected void ApplyEndpoints()
        {
            foreach (Type contractType in GetContracts())
            {
                AddServiceEndpoint(contractType, DefaultBinding(), EnforceEndpointAddress(contractType));
            }
        }

        /// <summary>
        ///     Method used to retireve a list of the contracts (marked as service contracts) implemented by the associated service
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<Type> GetContracts()
        {
            return (from contract in ImplementedContracts
                where
                    contract.Value.ContractType.GetCustomAttributes(typeof (ServiceContractAttribute), false).Any()
                select contract.Value.ContractType).ToList();
        }
    }
}