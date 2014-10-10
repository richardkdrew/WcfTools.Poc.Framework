using System;
using System.ServiceModel;
using System.Xml;

namespace WcfTools.Poc.Framework.Helpers.Binding
{
    public class BindingHelper
    {
        /// <summary>
        ///     Gets and sets the maximum size of the buffer to use. See
        ///     http://msdn.microsoft.com/en-us/library/system.servicemodel.channels.httptransportbindingelement.maxbuffersize.aspx
        ///     for more information.
        /// </summary>
        private const int DefaultMaxBufferSize = (16*1024*1024); //16MB;

        /// <summary>
        ///     Gets and sets the maximum allowable message size, in bytes, that can be received. See
        ///     http://msdn.microsoft.com/en-us/library/system.servicemodel.channels.transportbindingelement.maxreceivedmessagesize.aspx
        ///     for more information.
        /// </summary>
        private const int DefaultMaxReceivedMessageSize = (16*1024*1024); //16MB;

        /// <summary>
        ///     This quota limits the maximum string size that the XML reader returns. See
        ///     http://msdn.microsoft.com/en-us/library/system.xml.xmldictionaryreaderquotas.maxstringcontentlength.aspx for more
        ///     information.
        /// </summary>
        private const int DefaultMaxStringContentLength = (16*1024*1024); //16MB;

        /// <summary>
        ///     Gets and sets the maximum allowed array length. See
        ///     http://msdn.microsoft.com/en-us/library/system.xml.xmldictionaryreaderquotas.maxarraylength(v=vs.110).aspx for more
        ///     information.
        /// </summary>
        private const int DefaultMaxArrayLength = 65535; //16bit

        /// <summary>
        ///     Gets and sets the maximum allowed bytes returned for each read. See
        ///     http://msdn.microsoft.com/en-us/library/system.xml.xmldictionaryreaderquotas.maxbytesperread.aspx for more
        ///     information.
        /// </summary>
        private const int DefaultMaxBytesPerRead = (16*1024*1024); //16MB

        /// <summary>
        ///     This quota limits the maximum nesting depth of XML elements. See
        ///     http://msdn.microsoft.com/en-us/library/system.xml.xmldictionaryreaderquotas.maxdepth(v=vs.110).aspx for more
        ///     information.
        /// </summary>
        private const int DefaultMaxDepth = 65535; //16bit;

        /// <summary>
        ///     This quota limits the maximum number of characters allowed in a nametable. See
        ///     http://msdn.microsoft.com/en-us/library/system.xml.xmldictionaryreaderquotas.maxnametablecharcount.aspx for more
        ///     information.
        /// </summary>
        private const int DefaultMaxNameTableCharCount = 16384; //default 16K

        private const string BrokenBindingPolicyMessage =
            "The {0} value you provided exceeds the custom binding policy applied for this type of binding";

        private const string Namespace = "http://WcfTools.Poc.Framework";

        /// <summary>
        ///     Gets or sets the interval of time provided for a connection to close before the transport raises an exception. See
        ///     http://msdn.microsoft.com/en-us/library/system.servicemodel.channels.binding.closetimeout(v=vs.110).aspx for more
        ///     information.
        /// </summary>
        private static readonly TimeSpan DefaultCloseTimeout = new TimeSpan(0, 0, 0, 10);

        /// <summary>
        ///     Gets or sets the interval of time provided for a connection to open before the transport raises an exception. See
        ///     http://msdn.microsoft.com/en-us/library/system.servicemodel.channels.binding.opentimeout(v=vs.110).aspx for more
        ///     information.
        /// </summary>
        private static readonly TimeSpan DefaultOpenTimeout = new TimeSpan(0, 0, 0, 10);

        /// <summary>
        ///     Gets or sets the interval of time provided for a connection to receive a message before the transport raises an
        ///     exception. See
        ///     http://msdn.microsoft.com/en-us/library/system.servicemodel.channels.binding.receivetimeout(v=vs.110).aspx for more
        ///     information.
        /// </summary>
        private static readonly TimeSpan DefaultReceiveTimeout = new TimeSpan(0, 0, 0, 30);

        /// <summary>
        ///     Class used to set the defaults for an intranet based (TCP) endpoint binding
        /// </summary>
        public static class Intranet
        {
            private const string DefaultBindingName = "Default";

            private static NetTcpBinding _defaultBinding;

            /// <summary>
            /// Method used to retrieve a TCP binding pre-configured with some default values
            /// </summary>
            /// <returns>A valid (configured TCP) binding</returns>
            private static NetTcpBinding DefaultBinding()
            {
                return new NetTcpBinding(SecurityMode.Transport)
                {
                    Name = DefaultBindingName,
                    Namespace = Namespace,
                    MaxBufferSize = DefaultMaxBufferSize,
                    MaxReceivedMessageSize = DefaultMaxReceivedMessageSize,
                    ReaderQuotas = new XmlDictionaryReaderQuotas
                    {
                        MaxArrayLength = DefaultMaxArrayLength,
                        MaxBytesPerRead = DefaultMaxBytesPerRead,
                        MaxDepth = DefaultMaxDepth,
                        MaxNameTableCharCount = DefaultMaxNameTableCharCount,
                        MaxStringContentLength = DefaultMaxStringContentLength
                    },
                    CloseTimeout = DefaultCloseTimeout,
                    OpenTimeout = DefaultOpenTimeout,
                    ReceiveTimeout = DefaultReceiveTimeout
                };
            }

            /// <summary>
            ///     Method used to set the binding
            /// </summary>
            /// <param name="binding">The binding to set</param>
            public static void SetBinding(NetTcpBinding binding)
            {
                _defaultBinding = binding;
            }

            /// <summary>
            ///     Method used to get the binding, check the config for a configuration setting for the binding, checks what it finds
            ///     against some custom policies (EnforceBindingPolicies) and substitutes with a default binding if there is no config.
            /// </summary>
            /// <returns>A valid (configured TCP) binding</returns>
            public static System.ServiceModel.Channels.Binding Binding()
            {
                if (_defaultBinding != null) return _defaultBinding;
                try
                {
                    _defaultBinding = new NetTcpBinding(DefaultBindingName);
                }
                catch
                {
                    _defaultBinding = DefaultBinding();
                }

                EnforceBindingPolicies(_defaultBinding);
                return _defaultBinding;
            }

            /// <summary>
            /// Method used to check that any TCP binding being used complies with a set of custom policies
            /// </summary>
            /// <param name="binding">The binding to check</param>
            public static void EnforceBindingPolicies(NetTcpBinding binding)
            {
                if (binding.OpenTimeout > DefaultOpenTimeout)
                    throw new Exception(string.Format(BrokenBindingPolicyMessage, "OpenTimeout"));

                if (binding.CloseTimeout > DefaultCloseTimeout)
                    throw new Exception(string.Format(BrokenBindingPolicyMessage, "CloseTimeout"));

                if (binding.ReceiveTimeout > DefaultReceiveTimeout)
                    throw new Exception(string.Format(BrokenBindingPolicyMessage, "ReceiveTimeout"));

                if (binding.MaxBufferSize > DefaultMaxBufferSize)
                    throw new Exception(string.Format(BrokenBindingPolicyMessage, "MaxBufferSize"));

                if (binding.MaxReceivedMessageSize > DefaultMaxReceivedMessageSize)
                    throw new Exception(string.Format(BrokenBindingPolicyMessage, "MaxReceivedMessageSize"));

                if (binding.ReaderQuotas.MaxStringContentLength > DefaultMaxStringContentLength)
                    throw new Exception(string.Format(BrokenBindingPolicyMessage, "MaxStringContentLength"));

                if (binding.ReaderQuotas.MaxArrayLength > DefaultMaxArrayLength)
                    throw new Exception(string.Format(BrokenBindingPolicyMessage, "MaxArrayLength"));

                if (binding.ReaderQuotas.MaxBytesPerRead > DefaultMaxBytesPerRead)
                    throw new Exception(string.Format(BrokenBindingPolicyMessage, "MaxBytesPerRead"));

                if (binding.ReaderQuotas.MaxDepth > DefaultMaxDepth)
                    throw new Exception(string.Format(BrokenBindingPolicyMessage, "MaxDepth"));

                if (binding.ReaderQuotas.MaxNameTableCharCount > DefaultMaxNameTableCharCount)
                    throw new Exception(string.Format(BrokenBindingPolicyMessage, "MaxNameTableCharCount"));
            }
        }

        /// <summary>
        ///     Class used to set the defaults for an queue based (MSMQ) endpoint binding
        /// </summary>
        public static class Queue
        {
            private const string DefaultBindingName = "DefaultMsmq";

            private static NetMsmqBinding _defaultBinding;

            /// <summary>
            /// Method used to retrieve a MSMQ binding pre-configured with some default values
            /// </summary>
            /// <returns>A valid (configured MSMQ) binding</returns>
            private static NetMsmqBinding DefaultBinding()
            {
                return new NetMsmqBinding(NetMsmqSecurityMode.None)
                {
                    Name = DefaultBindingName,
                    Namespace = Namespace,
                    ReceiveRetryCount = 3,
                    ReceiveErrorHandling = ReceiveErrorHandling.Move,
                    MaxRetryCycles = 3,
                    RetryCycleDelay = TimeSpan.FromSeconds(5.0),
                    MaxReceivedMessageSize = DefaultMaxReceivedMessageSize,
                    ReaderQuotas = new XmlDictionaryReaderQuotas
                    {
                        MaxArrayLength = DefaultMaxArrayLength,
                        MaxBytesPerRead = DefaultMaxBytesPerRead,
                        MaxDepth = DefaultMaxDepth,
                        MaxNameTableCharCount = DefaultMaxNameTableCharCount,
                        MaxStringContentLength = DefaultMaxStringContentLength
                    },
                    CloseTimeout = DefaultCloseTimeout,
                    OpenTimeout = DefaultOpenTimeout,
                    ReceiveTimeout = DefaultReceiveTimeout
                };
            }

            /// <summary>
            ///     Method used to set the binding
            /// </summary>
            /// <param name="binding">The binding to set</param>
            public static void SetBinding(NetMsmqBinding binding)
            {
                _defaultBinding = binding;
            }

            /// <summary>
            ///     Method used to get the binding, check the config for a configuration setting for the binding, checks what it finds
            ///     against some custom policies (EnforceBindingPolicies) and substitutes with a default binding if there is no config.
            /// </summary>
            /// <returns>A valid (configured MSMQ) binding</returns>
            public static System.ServiceModel.Channels.Binding Binding()
            {
                if (_defaultBinding != null) return _defaultBinding;
                try
                {
                    _defaultBinding = new NetMsmqBinding(DefaultBindingName);
                }
                catch
                {
                    _defaultBinding = DefaultBinding();
                }

                EnforceBindingPolicies(_defaultBinding);
                return _defaultBinding;
            }

            /// <summary>
            /// Method used to check that any MSMQ binding being used complies with a set of custom policies
            /// </summary>
            /// <param name="binding">The binding to check</param>
            public static void EnforceBindingPolicies(NetMsmqBinding binding)
            {
                if (binding.OpenTimeout > DefaultOpenTimeout)
                    throw new Exception(string.Format(BrokenBindingPolicyMessage, "OpenTimeout"));

                if (binding.CloseTimeout > DefaultCloseTimeout)
                    throw new Exception(string.Format(BrokenBindingPolicyMessage, "CloseTimeout"));

                if (binding.ReceiveTimeout > DefaultReceiveTimeout)
                    throw new Exception(string.Format(BrokenBindingPolicyMessage, "ReceiveTimeout"));

                if (binding.MaxReceivedMessageSize > DefaultMaxReceivedMessageSize)
                    throw new Exception(string.Format(BrokenBindingPolicyMessage, "MaxReceivedMessageSize"));

                if (binding.ReaderQuotas.MaxStringContentLength > DefaultMaxStringContentLength)
                    throw new Exception(string.Format(BrokenBindingPolicyMessage, "MaxStringContentLength"));

                if (binding.ReaderQuotas.MaxArrayLength > DefaultMaxArrayLength)
                    throw new Exception(string.Format(BrokenBindingPolicyMessage, "MaxArrayLength"));

                if (binding.ReaderQuotas.MaxBytesPerRead > DefaultMaxBytesPerRead)
                    throw new Exception(string.Format(BrokenBindingPolicyMessage, "MaxBytesPerRead"));

                if (binding.ReaderQuotas.MaxDepth > DefaultMaxDepth)
                    throw new Exception(string.Format(BrokenBindingPolicyMessage, "MaxDepth"));

                if (binding.ReaderQuotas.MaxNameTableCharCount > DefaultMaxNameTableCharCount)
                    throw new Exception(string.Format(BrokenBindingPolicyMessage, "MaxNameTableCharCount"));
            }
        }
    }
}