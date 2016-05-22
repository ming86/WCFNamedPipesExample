using System.ServiceModel;

namespace WcfIPCTestServer
{
    [ServiceContract]
    interface ITestContract
    {
        [OperationContract]
        int Add(int a, int b);

        [OperationContract]
        int Subtract(int a, int b);
    }
}
