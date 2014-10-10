using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WcfTools.Poc.Framework
{
    public static class Utilities
    {
        /// <summary>
        /// Method used to find availabel TCP ports
        /// </summary>
        /// <returns>A valid TCP port number</returns>
        public static int FindAvailablePort()
        {
            var mutex = new Mutex(false, "WcfExamples.Tools.Utilities.FindAvailablePort");
            try
            {
                mutex.WaitOne();
                var endPoint = new IPEndPoint(IPAddress.Any, 0);
                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.Bind(endPoint);
                    var local = (IPEndPoint) socket.LocalEndPoint;
                    return local.Port;
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
    }
}