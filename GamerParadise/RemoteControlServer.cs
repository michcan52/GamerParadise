using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GamerParadise
{
    public class RemoteControlServer
    {
        private readonly HttpListener listener;
        private CancellationTokenSource cts;

        // Se escucha en http://localhost:8080/
        public RemoteControlServer(string prefix = "http://localhost:8080/")
        {
            listener = new HttpListener();
            listener.Prefixes.Add(prefix);
        }

        // Inicia el servidor
        public void Start()
        {
            cts = new CancellationTokenSource();
            listener.Start();
            Task.Run(() => ListenAsync(cts.Token));
        }

        // Detiene el servidor
        public void Stop()
        {
            cts.Cancel();
            listener.Stop();
        }

        private async Task ListenAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var context = await listener.GetContextAsync();
                    string responseString = ProcessRequest(context.Request);
                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                    context.Response.ContentLength64 = buffer.Length;
                    await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                    context.Response.Close();
                }
                catch (HttpListenerException)
                {
                    // Ocurre al detener el listener
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error en el servidor HTTP: " + ex.Message);
                }
            }
        }

        // Procesa la solicitud HTTP entrante
        private string ProcessRequest(HttpListenerRequest request)
        {
            if (request.Url.AbsolutePath.Equals("/launch", StringComparison.OrdinalIgnoreCase))
            {
                string app = request.QueryString["app"];
                if (!string.IsNullOrEmpty(app))
                {
                    Console.WriteLine("Comando recibido para lanzar: " + app);
                    // Aquí se podría buscar la app en la configuración y lanzarla.
                    return "{\"status\":\"Aplicación lanzada (simulación)\"}";
                }
            }
            else if (request.Url.AbsolutePath.Equals("/stream/start", StringComparison.OrdinalIgnoreCase))
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ((MainWindow)Application.Current.MainWindow).IniciarSesionStreaming();
                });
                return "{\"status\":\"Sesión de streaming iniciada\"}";
            }
            else if (request.Url.AbsolutePath.Equals("/stream/stop", StringComparison.OrdinalIgnoreCase))
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ((MainWindow)Application.Current.MainWindow).FinalizarSesionStreaming();
                });
                return "{\"status\":\"Sesión de streaming finalizada\"}";
            }
            else if (request.Url.AbsolutePath.Equals("/status", StringComparison.OrdinalIgnoreCase))
            {
                return "{\"status\":\"OK\"}";
            }

            return "{\"error\":\"Comando no reconocido\"}";
        }
    }
}
