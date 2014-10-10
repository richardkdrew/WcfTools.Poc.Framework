using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Text;

namespace WcfTools.Poc.Framework.Helpers.Hosting
{
    /// <summary>
    /// Class used to enable hosting in a console application process
    /// </summary>
    /// <typeparam name="T">The type of the service host i.e. IntranetServiceHost</typeparam>
    public class HostHelper<T> where T : ServiceHost
    {
        private const string LineBreak = "\n";
        private readonly T _service;

        public HostHelper(T service)
        {
            _service = service;
        }

        /// <summary>
        /// Method used to launch the hosting process
        /// </summary>
        public void Launch()
        {
            Debug.Assert(_service != null);

            try
            {
                // Open the ServiceHost (start listening for messages)
                _service.Open();

                // Show some service messages in the console
                Console.WriteLine("{0} : Service Host Running...", _service.Description.Name);
                Console.WriteLine("Press <ENTER> key to terminate service.");
                Console.WriteLine();

                // Wait for enter to be pressed
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                var errorStringBuilder = new StringBuilder();
                errorStringBuilder.Append(LineBreak);
                errorStringBuilder.AppendLine(string.Format("ERROR: {0}", ex.Message));
                errorStringBuilder.Append(LineBreak);
                errorStringBuilder.Append("Press <ENTER> key to exit.");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(errorStringBuilder.ToString());

                // Wait for enter to be pressed
                Console.ReadLine();
            }
            finally
            {
                // Close the ServiceHost (and shut down the service)
                if (_service.State != CommunicationState.Closed &&
                    _service.State != CommunicationState.Faulted)
                    _service.Close();
            }
        }
    }
}