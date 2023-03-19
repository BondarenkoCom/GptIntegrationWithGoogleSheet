using GptIntegrationWithGoogleSheet.Helpers;
using GptIntegrationWithGoogleSheet.Models;

namespace GptIntegrationWithGoogleSheet.GoogleSheetsIntegration
{
    public class HelperSheet
    {
        public async Task<string> GetNumberOfFilesToUpload()
        {

            var _readGoogle = new InitGoogleSheet();
            var resultAuth = _readGoogle.InitializeSheetsService();
            var spreadsheetId = JsonReader.GetValues().SpreadsheetId;

            try
            {
                string range = "E1";
                var request = resultAuth.Spreadsheets.Values.Get(spreadsheetId, range);
                var responseFileCount = request.Execute();

                foreach (var item in responseFileCount.Values)
                {
                    if (item[0] != null)
                    {
                        int numbersOfFiles = int.Parse(item[0].ToString());

                        for (int rangeCount = 1; rangeCount <= numbersOfFiles; rangeCount++)
                        {
                            try
                            {
                                string currentRange = $"E{rangeCount}";
                                var currentRequest = resultAuth.Spreadsheets.Values.Get(spreadsheetId, currentRange);
                                var currentResponse = currentRequest.Execute();

                                if (currentResponse.Values != null)
                                {
                                    foreach (var itemFile in currentResponse.Values)
                                    {
                                        Console.WriteLine(itemFile[0]);

                                        var url = itemFile[0].ToString();
                                        if (url.Contains("https:"))
                                        {
                                            try
                                            {
                                                var DReader = new DocsReader();
                                                var txtFileForGPT = await DReader.ReadDocsByLink(url);
                                                Thread.Sleep(30000);
                                                return txtFileForGPT.ToString();
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine($"invalid google docs url - {ex.Message}");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"No values found in the current range: {currentRange}");
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error this E ranges - {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return null;
        }

        public async Task<GPTSettingsModel> GetGPTSettngs()
        {
            var _readGoogle = new InitGoogleSheet();
            var resultAuth = _readGoogle.InitializeSheetsService();
            var spreadsheetId = JsonReader.GetValues().SpreadsheetId;
            var settingLetterRange = "A1";
            var settingValueRange = "B1";
            var gptSettings = new GPTSettingsModel();

            for (int rangeSettingsCount = 1; rangeSettingsCount <= 10; rangeSettingsCount++)
            {
                try
                {
                    settingLetterRange = $"A{rangeSettingsCount}";
                    settingValueRange = $"B{rangeSettingsCount}";

                    var requestSettings = resultAuth.Spreadsheets.Values.Get(spreadsheetId, settingLetterRange);
                    var responseSettings = requestSettings.Execute();

                    var requestSettingsValue = resultAuth.Spreadsheets.Values.Get(spreadsheetId, settingValueRange);
                    var responseSettingsValue = requestSettingsValue.Execute();

                    if (responseSettings.Values != null && responseSettingsValue.Values != null && responseSettings.Values.Count == responseSettingsValue.Values.Count)
                    {
                        for (int i = 0; i < responseSettings.Values.Count; i++)
                        {

                            var SetttignsName = responseSettings.Values[i][0].ToString();
                            var SetttignsValue = responseSettingsValue.Values[i][0].ToString();

                            switch (SetttignsName)
                            {
                                case "API":
                                    gptSettings.API = SetttignsValue;
                                    break;
                                case "FINETUNEID":
                                    gptSettings.FinetuneID = SetttignsValue;
                                    break;
                                case "MODELID":
                                    gptSettings.ModelID = SetttignsValue;
                                    break;
                                case "max temp":
                                    gptSettings.MaxTemp = double.Parse(SetttignsValue);
                                    break;
                                case "max tokens":
                                    gptSettings.MaxTokens = int.Parse(SetttignsValue);
                                    break;
                                case "Top P":
                                    gptSettings.TopP = double.Parse(SetttignsValue);
                                    break;
                                case "Best Of":
                                    gptSettings.BestOf = int.Parse(SetttignsValue);
                                    break;
                                case "engine":
                                    gptSettings.Engine = SetttignsValue;
                                    break;
                                case "model":
                                    gptSettings.Model = SetttignsValue;
                                    break;  
                                case "OpenApIKey":
                                    gptSettings.OpenApiKey = SetttignsValue;
                                    break;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return gptSettings;
        }

        public async Task<string> GetPropmtForGPT()
        {
            var _readGoogle = new InitGoogleSheet();
            var resultAuth = _readGoogle.InitializeSheetsService();
            var spreadsheetId = JsonReader.GetValues().SpreadsheetId;
            string promptRange = "B14";
            string outputRange = "C14";

            for (int rangePromptCount = 14; rangePromptCount <= 100; rangePromptCount++)
            {
                try
                {
                    promptRange = $"B{rangePromptCount}";
                    outputRange = $"C{rangePromptCount}";

                    var requestPrompt = resultAuth.Spreadsheets.Values.Get(spreadsheetId, promptRange);
                    var responsePrompt = requestPrompt.Execute();
                    var promptText = "";
                    if (responsePrompt.Values == null)
                    {
                        Console.WriteLine("Stop loop");
                        break;
                    }

                    foreach (var itemPrompt in responsePrompt.Values)
                    {
                         promptText = itemPrompt[0].ToString();

                        if(promptText != null && !promptText.Contains("PROMPT"))
                        {
                            Console.WriteLine($"This command for GPT - {promptText}");
                            return promptText;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                } 
            }

            return null;
        }
    }
}
