

using System.Threading.Tasks;
using Orleans;

namespace OrleansBasic
{
    public interface IHello : IGrainWithIntegerKey
    {
        Task<string> SayHello(string greeting);
    }
}