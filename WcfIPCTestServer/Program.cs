using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace WcfIPCTestServer
{
    /// <summary>
    /// Example from https://gorillacoding.wordpress.com/2013/02/03/using-wcf-for-inter-process-communication/
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {

            string address = "net.pipe://localhost/gorillacoding/IPCTest";

            ServiceHost serviceHost = new ServiceHost(typeof(IPCTestServer));
            NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            serviceHost.AddServiceEndpoint(typeof(ITestContract), binding, address);
            serviceHost.Open();

            Console.WriteLine("ServiceHost running. Press Return to Exit");
            Console.ReadLine();

            serviceHost.Close();

        }
    }
}
