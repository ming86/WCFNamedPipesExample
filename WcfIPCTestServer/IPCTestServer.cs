using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace WcfIPCTestServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class IPCTestServer : ITestContract
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
        
        public int Subtract(int a, int b)
        {
            return a - b;
        }

    }
}
