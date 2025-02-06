using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media;
using Windows.Gaming.Input; // Las APIs de gamepad

namespace GamerParadise
{
    public partial class MainWindow : Window
    {
        RemoteControlServer remoteServer;
        DispatcherTimer gamepadTimer;
        // Para detectar cambios en la lectura del gamepad
        private GamepadReading previousReading;

        public MainWindow()
        {
            InitializeComponent();

            // Inicia el servidor remoto
            remoteServer = new RemoteControlServer("http://localhost:8080/");
            remoteServer.Start();

            // Carga la lista de aplicaciones (método de ejemplo)
            RefreshGameButtons();

            // Configura el timer para leer el estado del gamepad cada 100 ms
            gamepadTimer = new DispatcherTimer();
            gamepadTimer.Interval = TimeSpan.FromMilliseconds(100);
            gamepadTimer.Tick += GamepadTimer_Tick;
            gamepadTimer.Start();

            previousReading = new GamepadReading();
        }

        private void GamepadTimer_Tick(object sender, EventArgs e)
        {
            if (Gamepad.Gamepads.Count > 0)
            {
                // Usa el primer gamepad conectado
                Gamepad gamepad = Gamepad.Gamepads.First();
                GamepadReading reading = gamepad.GetCurrentReading();

                // Detecta transición en DPad Izquierda
                if ((reading.Buttons & GamepadButtons.DPadLeft) == GamepadButtons.DPadLeft &&
                    (previousReading.Buttons & GamepadButtons.DPadLeft) != GamepadButtons.DPadLeft)
                {
                    NavigateFocus(-1);
                }
                // Detecta transición en DPad Derecha
                if ((reading.Buttons & GamepadButtons.DPadRight) == GamepadButtons.DPadRight &&
                    (previousReading.Buttons & GamepadButtons.DPadRight) != GamepadButtons.DPadRight)
                {
                    NavigateFocus(1);
                }
                // Detecta que se presiona el botón A para activar el botón seleccionado
                if ((reading.Buttons & GamepadButtons.A) == GamepadButtons.A &&
                    (previousReading.Buttons & GamepadButtons.A) != GamepadButtons.A)
                {
                    ActivateCurrentButton();
                }

                previousReading = reading;
            }
        }

        // Cambia el foco entre elementos del ListBox de forma cíclica
        private void NavigateFocus(int direction)
        {
            int count = lbGameButtons.Items.Count;
            if (count == 0)
                return;
            int newIndex = (lbGameButtons.SelectedIndex + direction + count) % count;
            lbGameButtons.SelectedIndex = newIndex;
            ListBoxItem item = lbGameButtons.ItemContainerGenerator.ContainerFromIndex(newIndex) as ListBoxItem;
            if (item != null)
                item.Focus();
        }

        // Simula el clic en el botón actualmente seleccionado
        private void ActivateCurrentButton()
        {
            int index = lbGameButtons.SelectedIndex;
            if (index < 0)
                return;
            ListBoxItem item = lbGameButtons.ItemContainerGenerator.ContainerFromIndex(index) as ListBoxItem;
            if (item != null)
            {
                Button btn = FindVisualChild<Button>(item);
                if (btn != null)
                    btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        // Helper para buscar un hijo visual de un tipo específico
        private T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            if (obj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child is T tChild)
                        return tChild;
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        // Método de ejemplo para cargar la lista de aplicaciones.
        // Reemplaza este método con tu carga real (por ejemplo, desde un archivo JSON)
        private void RefreshGameButtons()
        {
            // Carga las aplicaciones desde el archivo JSON
            var config = AppConfig.Load();
            lbGameButtons.ItemsSource = config.Applications;

            if (config.Applications.Count > 0)
                lbGameButtons.SelectedIndex = 0;
        }

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            // Aquí se abriría la ventana de administración (usa tu AdminWindow existente)
            AdminWindow adminWindow = new AdminWindow();
            if (adminWindow.ShowDialog() == true)
            {
                RefreshGameButtons();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Métodos para el control remoto (streaming)
        public void IniciarSesionStreaming()
        {
            Console.WriteLine("Sesión de streaming iniciada.");
        }

        public void FinalizarSesionStreaming()
        {
            Console.WriteLine("Sesión de streaming finalizada. Cerrando aplicación...");
            Application.Current.Shutdown();
        }

        // Evento que se dispara al hacer clic en un botón de aplicación (del DataTemplate)
        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            // Aquí se puede llamar a la función que lance la app; por ejemplo:
            Button btn = sender as Button;
            if (btn?.DataContext is ApplicationItem app)
            {
                // Llama a tu método para lanzar la aplicación (por ejemplo, usando Process.Start)
                try
                {
                    System.Diagnostics.Process.Start(app.Path, app.Arguments);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al lanzar la aplicación: " + ex.Message);
                }
            }
        }
    }

    // Clase para representar una aplicación configurada.
    // Si ya la tienes en otro archivo, elimina esta definición.
    public class ApplicationItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Arguments { get; set; }
    }
}
