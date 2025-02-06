using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;

namespace GamerParadise
{
    // Representa cada aplicación o juego configurado


    // Clase que contiene la lista de aplicaciones y métodos para persistir la configuración
    public class AppConfig
    {
        public ObservableCollection<ApplicationItem> Applications { get; set; } = new ObservableCollection<ApplicationItem>();

        private static readonly string configFilePath = "config.json";

        // Cargar la configuración desde un archivo JSON
        public static AppConfig Load()
        {
            if (File.Exists(configFilePath))
            {
                string json = File.ReadAllText(configFilePath);
                return JsonConvert.DeserializeObject<AppConfig>(json);
            }
            return new AppConfig();
        }

        // Guardar la configuración en el archivo JSON
        public void Save()
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(configFilePath, json);
        }
    }
}
