using System.Net.Http.Json;

namespace FM_McpServer
{
    /// <summary>
    /// TCP/Http客户端，用于与Rhino插件进行通信，发送命令和接收响应。
    /// </summary>
    public class RhinoClient
    {
        private readonly HttpClient _http;

        public RhinoClient(int port = 5000)
        {
            _http = new HttpClient
            {
                // 插件监听地址与端口
                BaseAddress = new Uri($"http://127.0.0.1:{port}")
            };
        }

        public async Task<string> CreateSphere(double x, double y, double z, double radius)
        {
            var payload = new
            {
                type = "create_sphere",
                center = new[] { x, y, z },
                radius = radius
            };

            var res = await _http.PostAsJsonAsync("/command", payload);
            return await res.Content.ReadAsStringAsync();
        }
        
        public async Task<string> CreateCircle(double x, double y, double z, double radius)
        {
            var payload = new
            {
                type = "create_circle",
                center = new[] { x, y, z },
                radius = radius
            };

            var res = await _http.PostAsJsonAsync("/command", payload);
            return await res.Content.ReadAsStringAsync();
        }

    }
}
