using GptIntegrationWithGoogleSheet.Models;
using Newtonsoft.Json;
using System.Reflection;

namespace GptIntegrationWithGoogleSheet.Helpers
{
    public static class JsonReader
    {
        public static GoogleSheetSettingsModel? GetValues()
        {
            try
            {
                string basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string jsonFilePath = Path.Combine(basePath, "Jsons", "JsonSettings.json");

                if (!File.Exists(jsonFilePath))
                    return null;

                string json = File.ReadAllText(jsonFilePath);

                return JsonConvert.DeserializeObject<GoogleSheetSettingsModel>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing element at path {ex.ToString()}");
                return null;
            }
        }
    }
}
