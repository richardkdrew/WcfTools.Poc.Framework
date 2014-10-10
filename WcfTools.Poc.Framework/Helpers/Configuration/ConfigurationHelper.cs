using System;
using System.Configuration;

namespace WcfTools.Poc.Framework.Helpers.Configuration
{
    internal class ConfigurationHelper
    {
        /// <summary>
        ///     The host name
        /// </summary>
        private static string _hostName;

        /// <summary>
        ///     The host port
        /// </summary>
        private static string _hostPort;

        /// <summary>
        ///     Method used to retrieve the host name from the config (or substitue with a default if no config value is found)
        /// </summary>
        /// <returns>The host name</returns>
        public static string GetHostName()
        {
            // If the host name has already been set, use that
            if (_hostName != null) return _hostName;
            try
            {
                // Try the config
                _hostName = ConfigurationManager.AppSettings["HostName"];
            }
            catch
            {
                _hostName = null;
            }
            // Otherwise return a default value
            return _hostName ?? Environment.MachineName;
        }

        /// <summary>
        ///     Method used to retrieve the host port from the config (or substitue with a default if no config value is found)
        /// </summary>
        /// <returns>The host port</returns>
        public static string GetHostPort()
        {
            // If the host port has already been set, use that
            if (_hostPort != null) return _hostPort;
            try
            {
                // Try the config
                _hostPort = ConfigurationManager.AppSettings["HostPort"];
            }
            catch
            {
                _hostPort = null;
            }
            // Otherwise return a default value
            return _hostPort ?? Utilities.FindAvailablePort().ToString();
        }
    }
}