using Microsoft.Azure.Relay;
using Newtonsoft.Json.Linq;

namespace ClientNotifierApi.Services
{
    public class RelaySender : IRelaySender
    {
        private readonly HybridConnectionClient _hybridConnectionClient;

        public RelaySender(IConfiguration configuration)
        {
            var relayNamespace = configuration["RelayNamespace"];
            var connectionName = configuration["ConnectionName"];
            var keyName = configuration["KeyName"];
            var key = configuration["Key"];

             var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(keyName, key);
            _hybridConnectionClient = new HybridConnectionClient(new Uri(String.Format("sb://{0}/{1}", relayNamespace, connectionName)), tokenProvider);
        }

        public async Task Send(JArray? events)
        {
            var relayConnection = await _hybridConnectionClient.CreateConnectionAsync();
            var writer = new StreamWriter(relayConnection) { AutoFlush = true };
            foreach (var gridEvent in events)
            {
                await writer.WriteLineAsync(gridEvent.ToString());
            }
        }
    }
}
