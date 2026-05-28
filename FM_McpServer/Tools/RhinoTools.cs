using ModelContextProtocol.Server;
using System.ComponentModel;

namespace FM_McpServer.Tools
{
    public class RhinoTools
    {
        private readonly RhinoClient _client;

        public RhinoTools(RhinoClient client)
        {
            _client = client;
        }

        [McpServerTool]
        [Description("Generates a sphere at the specified location with the given radius.")]
        public async Task<string> CreateSphere(double x, double y, double z, double radius)
        {
            return await _client.CreateSphere(x, y, z, radius);
        }

        [McpServerTool]
        [Description("Generates a circle at the specified location with the given radius.")]
        public async Task<string> CreateCircle(double x, double y, double z, double radius)
        {
            return await _client.CreateCircle(x, y, z, radius);
        }
    }
}
