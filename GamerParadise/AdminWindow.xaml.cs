using System;
using System.Windows;
using System.Xml.Linq;
using Microsoft.Win32;

namespace GamerParadise
{
    public partial class AdminWindow : Window
    {
        private AppConfig config;
        // Variable para saber si se está editando un ítem
        private ApplicationItem currentEditingItem = null;

        public AdminWindow()
        {
            InitializeComponent();
            // Cargar la configuración actual
            config = AppConfig.Load();
            lbApps.ItemsSource = config.Applications;
        }

        // Al pulsar "Examinar" se abre el OpenFileDialog
        private void btnExaminar_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Archivos ejecutables (*.exe)|*.exe";
            if (dlg.ShowDialog() == true)
            {
                txtPath.Text = dlg.FileName;
                // Si el campo Nombre está vacío, se completa automáticamente
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    txtName.Text = System.IO.Path.GetFileNameWithoutExtension(dlg.FileName);
                }
            }
        }

        // Agregar una nueva aplicación (modo Agregar)
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPath.Text))
            {
                MessageBox.Show("Debe ingresar al menos el nombre y la ruta.");
                return;
            }

            // Agregar la nueva aplicación
            config.Applications.Add(new ApplicationItem
            {
                Name = txtName.Text.Trim(),
                Path = txtPath.Text.Trim(),
                Arguments = txtArguments.Text.Trim()
            });
            config.Save();
            lbApps.Items.Refresh();
            ClearFields();
            // Opcional: puedes notificar al usuario que la app se agregó correctamente.
            MessageBox.Show("Aplicación agregada correctamente.");
        }

        // Actualizar la aplicación en edición (modo Actualizar)
        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (currentEditingItem == null)
                return;

            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPath.Text))
            {
                MessageBox.Show("Debe ingresar al menos el nombre y la ruta.");
                return;
            }

            // Actualizar los valores del item
            currentEditingItem.Name = txtName.Text.Trim();
            currentEditingItem.Path = txtPath.Text.Trim();
            currentEditingItem.Arguments = txtArguments.Text.Trim();
            config.Save();
            lbApps.Items.Refresh();

            // Restaurar el modo Agregar
            currentEditingItem = null;
            btnActualizar.Visibility = Visibility.Collapsed;
            btnAgregar.Visibility = Visibility.Visible;
            ClearFields();
            MessageBox.Show("Aplicación actualizada correctamente.");
        }

        // Eliminar la aplicación seleccionada
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as FrameworkElement;
            var item = btn.Tag as ApplicationItem;
            if (item != null)
            {
                var result = MessageBox.Show($"¿Está seguro de eliminar la aplicación \"{item.Name}\"?", "Confirmar eliminación", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    config.Applications.Remove(item);
                    config.Save();
                    lbApps.Items.Refresh();
                }
            }
        }

        // Inicia el modo de edición al pulsar el botón "Editar"
        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as FrameworkElement;
            var item = btn.Tag as ApplicationItem;
            if (item != null)
            {
                currentEditingItem = item;
                // Rellenar los campos con los datos del ítem a editar
                txtName.Text = item.Name;
                txtPath.Text = item.Path;
                txtArguments.Text = item.Arguments;
                // Cambiar botones: ocultar Agregar y mostrar Actualizar
                btnAgregar.Visibility = Visibility.Collapsed;
                btnActualizar.Visibility = Visibility.Visible;
            }
        }

        // Limpia los campos de entrada
        private void ClearFields()
        {
            txtName.Clear();
            txtPath.Clear();
            txtArguments.Clear();
        }

        // Evento para cerrar la ventana de forma controlada
        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
