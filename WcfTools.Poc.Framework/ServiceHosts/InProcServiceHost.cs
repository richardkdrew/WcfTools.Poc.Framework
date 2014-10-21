using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using WcfTools.Poc.Framework.Helpers.Binding;

namespace WcfTools.Poc.Framework.ServiceHosts
{
    /// <summary>
    ///     Class used as an in process based service host with (pipe) endpoints for all implemented contracts
    /// </summary>
    public class InProcServiceHost : CustomServiceHostBase
    {
        public InProcServiceHost(Type serviceType, Uri[] baseAddresses) : base(serviceType, baseAddresses)
        {
            ApplyInProcEndpoints();
        }

        /// <summary>
        ///     Method used to retrieve the default binding for an in process based service host
        /// </summary>
        /// <returns>A valid binding (pre-configured based on the rules described in the BindingHelper)</returns>
        protected override Binding DefaultBinding()
        {
            return BindingHelper.InProc.Binding();
        }

        /// <summary>
        ///     Method used to apply the default endpoints for the contracts associated with an in process based service host
        /// </summary>
        private void ApplyInProcEndpoints()
        {
            if (Description.Endpoints.Any(x => x.Binding is NetNamedPipeBinding)) return;
            ApplyEndpoints();
        }

        /// <summary>
        ///     Method used to ensure that the endpoint address matches the desired (custom) convention
        /// </summary>
        /// <param name="contractType">The contract to be used as part of the convention based check</param>
        /// <returns>A string based endpoint address that corresponds with the convention used for an in process based service host</returns>
        protected override string EnforceEndpointAddress(Type contractType)
        {
            return contractType.Name;
        }
    }
}