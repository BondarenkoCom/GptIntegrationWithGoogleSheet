using Google.Apis.Sheets.v4;
using Google.Apis.Auth.OAuth2;
using System.Reflection;

namespace GptIntegrationWithGoogleSheet.Helpers
{
    public class InitGoogleSheet
    {
        public SheetsService InitializeSheetsService()
        {
            string KeyName = JsonReader.GetValues().PathToKey;
            string basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string pathToKey = Path.Combine(basePath, "keys", $"{KeyName}");

            var credential = GoogleCredential.FromFile(pathToKey);
            return new SheetsService(new Google.Apis.Services.BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = JsonReader.GetValues().ApplicationName
            });
        }
    }
}
