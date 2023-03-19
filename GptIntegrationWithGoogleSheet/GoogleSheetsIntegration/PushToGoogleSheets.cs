using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using GptIntegrationWithGoogleSheet.Helpers;

namespace GptIntegrationWithGoogleSheet.GoogleSheetsIntegration
{
    public class PushToGoogleSheets
    {
        public async Task PushValueToCell(string cellName, string content)
        {
            var _readGoogle = new InitGoogleSheet();
            var resultAuth = _readGoogle.InitializeSheetsService();
            var spreadsheetId = JsonReader.GetValues().SpreadsheetId;

            var values = new List<IList<object>>
            {
                new List<object> { content}
            };
            try
            {
                foreach (var value in values)
                {
                    ValueRange requestBody = new ValueRange
                    {
                        Values = new List<IList<object>> { value }
                    };

                    var request = resultAuth.Spreadsheets.Values.Update(requestBody, spreadsheetId, cellName);
                    request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                    request.Execute();

                    Thread.Sleep(TimeSpan.FromSeconds(10));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while sending data to Google Sheets: {ex.Message}");
            }
        }
    }
}
