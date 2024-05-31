using Newtonsoft.Json.Linq;

namespace ClientNotifierApi.Services
{
    public interface IRelaySender
    {
        public Task Send(JArray? events);
    }
}
