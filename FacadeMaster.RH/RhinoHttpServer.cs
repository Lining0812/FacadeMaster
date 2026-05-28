using Newtonsoft.Json;
using Rhino;
using Rhino.Geometry;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FacadeMaster.RH
{
    public class RhinoHttpServer : IDisposable
    {
        private HttpListener _listener;
        private CancellationTokenSource _cts;

        public void Start()
        {
            int port = FindAvailablePort(5000);
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://127.0.0.1:{port}/");
            _listener.Start();
            RhinoApp.WriteLine($"[FacadeMaster] HTTP 服务已启动: http://127.0.0.1:{port}/");

            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    HttpListenerContext ctx;
                    try { ctx = await _listener.GetContextAsync(); }
                    catch { break; }

                    _ = HandleAsync(ctx);
                }
            });
        }

        private async Task HandleAsync(HttpListenerContext context)
        {
            try
            {
                string json;
                using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                    json = await reader.ReadToEndAsync();

                var cmd = JsonConvert.DeserializeObject<HttpCommand>(json);
                if (cmd == null) throw new InvalidOperationException("Empty body");

                if (cmd.Type == "create_sphere")
                {
                    if (cmd.Center == null || cmd.Center.Length < 3)
                        throw new ArgumentException("center must be [x,y,z]");

                    var center = new Point3d(cmd.Center[0], cmd.Center[1], cmd.Center[2]);
                    double r = cmd.Radius;

                    // 关键：切回 Rhino 主线程
                    RhinoApp.InvokeOnUiThread(new Action(() =>
                    {
                        var doc = RhinoDoc.ActiveDoc;
                        if (doc == null) return;
                        doc.Objects.AddSphere(new Sphere(center, r));
                        doc.Views.Redraw();
                    }));
                }

                if (cmd.Type == "create_circle")
                {
                    if (cmd.Center == null || cmd.Center.Length < 3)
                        throw new ArgumentException("center must be [x,y,z]");

                    var center = new Point3d(cmd.Center[0], cmd.Center[1], cmd.Center[2]);
                    double r = cmd.Radius;

                    // 关键：切回 Rhino 主线程
                    RhinoApp.InvokeOnUiThread(new Action(() =>
                    {
                        var doc = RhinoDoc.ActiveDoc;
                        if (doc == null) return;
                        doc.Objects.AddCircle(new Circle(center, r));
                        doc.Views.Redraw();
                    }));
                }

                var ok = Encoding.UTF8.GetBytes("{\"ok\":true,\"type\":\"" + cmd.Type + "\"}");
                context.Response.StatusCode = 200;
                context.Response.ContentType = "application/json";
                context.Response.OutputStream.Write(ok, 0, ok.Length);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                var bytes = Encoding.UTF8.GetBytes(ex.Message);
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
            }
            finally
            {
                context.Response.Close();
            }
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _listener?.Stop();
            _listener?.Close();
        }

        private int FindAvailablePort(int startPort = 5000)
        {
            for (int port = startPort; port < startPort + 100; port++)
            {
                try
                {
                    var l = new HttpListener();
                    l.Prefixes.Add($"http://127.0.0.1:{port}/");
                    l.Start();
                    l.Stop();
                    l.Close();
                    return port;
                }
                catch { /* 端口被占用，试下一个 */ }
            }
            throw new InvalidOperationException("找不到可用端口");
        }
    }

    public class HttpCommand
    {
        public string Type { get; set; }
        public double[] Center { get; set; }
        public double Radius { get; set; }
    }
}
