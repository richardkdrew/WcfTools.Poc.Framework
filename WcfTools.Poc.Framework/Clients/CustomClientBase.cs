using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace WcfTools.Poc.Framework.Clients
{
    public abstract class CustomClientBase<T> where T : class
    {
        private readonly T _channel;

        protected CustomClientBase(Uri baseAddress)
        {
            _channel = CreateFactory(baseAddress).CreateChannel();
        }

        protected abstract Binding Binding();
        protected abstract EndpointAddress EnforceEndpointAddress(Uri baseAddress);

        private ChannelFactory<T> CreateFactory(Uri baseAddress)
        {
            return new ChannelFactory<T>(Binding(), EnforceEndpointAddress(baseAddress));
        }

        public void CallService(Action<T> action)
        {
            bool success = false;
            try
            {
                action(_channel);
                success = true;
            }
            finally
            {
                CleanUpChannelResources(success);
            }
        }

        public T1 CallService<T1>(Func<T, T1> action)
        {
            T1 result;

            bool success = false;
            try
            {
                result = action(_channel);
                success = true;
            }
            finally
            {
                CleanUpChannelResources(success);
            }
            return result;
        }

        private void CleanUpChannelResources(bool success)
        {
            if (!success)
            {
                ((IClientChannel) _channel).Abort();
            }
            else
            {
                if (_channel != null && ((IClientChannel) _channel).State != CommunicationState.Faulted)
                    ((IClientChannel) _channel).Close();
            }
        }
    }
}