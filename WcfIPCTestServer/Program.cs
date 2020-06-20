using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;

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
            var behavior = serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>();
            behavior.IncludeExceptionDetailInFaults = true;

            foreach (var endpoint in serviceHost.Description.Endpoints)
            {
                endpoint.Behaviors.Add(new ConsoleOutputBehavior());
            }

            // add a meta-data end - point
            //wcftestclient consumes this MEx and allow to invoke methods on net.pipe://localhost/gorillacoding/IPCTest
            ServiceMetadataBehavior serviceMetadataBehavior = new ServiceMetadataBehavior();
            serviceMetadataBehavior.HttpGetEnabled = true;
            serviceMetadataBehavior.HttpGetUrl = new Uri("http://localhost:8002/gorillacoding/mex");
            serviceHost.Description.Behaviors.Add(serviceMetadataBehavior);

            serviceHost.Open();

            Console.WriteLine("ServiceHost running. Press Return to Exit");
            Console.ReadLine();

            serviceHost.Close();

        }
    }
    public class ConsoleOutputHeadersMessageInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
          

            MessageBuffer buffer = request.CreateBufferedCopy(Int32.MaxValue);

            Console.WriteLine("Message:");
            Console.WriteLine(request);
            Console.WriteLine();

            request = buffer.CreateMessage();

            Message originalMessage = buffer.CreateMessage();

            //if use this cycle request gets consumed
            //foreach (MessageHeader h in originalMessage.Headers)
            //{
            //    //Console.WriteLine("\n{0}\n", h);
            //}

            
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
           
            MessageBuffer buffer = reply.CreateBufferedCopy(Int32.MaxValue);
            reply = buffer.CreateMessage();
            Message originalMessage = buffer.CreateMessage();

            Console.WriteLine("Message:");
            Console.WriteLine(reply);
            Console.WriteLine();

            //if use this cycle reply gets consumed
            //foreach (MessageHeader h in originalMessage.Headers)
            //{
            //    Console.WriteLine("\n{0}\n", h);
            //}
        }
    }
    public class ConsoleOutputBehavior : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            throw new Exception("Behavior not supported on the consumer side!");
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            ConsoleOutputHeadersMessageInspector inspector = new ConsoleOutputHeadersMessageInspector();
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector);
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }

    //Нужен, только если, использовать xml для добавления 
    public class ConsoleOutputBehaviorExtensionElement : BehaviorExtensionElement
    {
        protected override object CreateBehavior()
        {
            return new ConsoleOutputHeadersMessageInspector();
        }

        public override Type BehaviorType
        {
            get
            {
                return typeof(ConsoleOutputHeadersMessageInspector);
            }
        }
    }
}
