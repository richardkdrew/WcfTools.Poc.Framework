using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace WcfTools.Poc.Framework.Behaviors
{
    /// <summary>
    /// Custom service attribute used to ensure that the service instance has the preffered settings associated
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class IntranetServiceBehavior : Attribute, IServiceBehavior
    {
        //OperationBehaviorAttribute 
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            EnforceServiceBehavior(serviceDescription, serviceHostBase);
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        /// <summary>
        /// Method used to enforce that the service instance has the preffered settings associated
        /// </summary>
        internal virtual void EnforceServiceBehavior(ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase)
        {
            var serviceBehaviorAttribute = serviceDescription.Behaviors.Find<ServiceBehaviorAttribute>();
            if (serviceBehaviorAttribute == null) return;
            serviceBehaviorAttribute.InstanceContextMode = InstanceContextMode.PerCall;
            serviceBehaviorAttribute.ConcurrencyMode = ConcurrencyMode.Multiple;
            serviceBehaviorAttribute.UseSynchronizationContext = false;
        }
    }
}