using System;
using System.Diagnostics;

namespace GamerParadise
{
    public static class ProcessHelper
    {
        // Lanza una aplicación con la ruta y parámetros opcionales, iniciándola minimizada.
        public static void LaunchApplication(string appPath, string arguments = "")
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(appPath, arguments)
                {
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Minimized  // Se inicia minimizada
                };
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al lanzar la aplicación: " + ex.Message);
            }
        }

        // Método para cerrar una aplicación, si se conoce el nombre del proceso
        public static void CloseApplication(string processName)
        {
            try
            {
                foreach (Process proc in Process.GetProcessesByName(processName))
                {
                    proc.CloseMainWindow();
                    proc.WaitForExit(2000);
                    if (!proc.HasExited)
                        proc.Kill();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cerrar la aplicación: " + ex.Message);
            }
        }
    }
}
