using System.ServiceModel;

namespace WcfIPCTestClient
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
