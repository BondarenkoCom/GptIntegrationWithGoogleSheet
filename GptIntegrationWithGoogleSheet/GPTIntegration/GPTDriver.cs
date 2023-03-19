using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GptIntegrationWithGoogleSheet.GoogleSheetsIntegration;
using GptIntegrationWithGoogleSheet.Models;
using Newtonsoft.Json;

namespace GptIntegrationWithGoogleSheet.GPTIntegration
{
    public class GPTDriver
    {
        public async Task<string> InitGPTAPI()
        {
            var ReadGoogleSheetsSetting = new HelperSheet();
            var ReadGoogleSheets = new HelperSheet();
            var ReadGoogleSheetsPrompt = new HelperSheet();
            var ConnectWithGoogleSheet = new PushToGoogleSheets();

            string apiKey = ReadGoogleSheetsSetting.GetGPTSettngs().Result.FinetuneID;
            string endpoint = ReadGoogleSheetsSetting.GetGPTSettngs().Result.API;
            string modelAI = ReadGoogleSheetsSetting.GetGPTSettngs().Result.Model;

            string contentFromDocs =await ReadGoogleSheets.GetNumberOfFilesToUpload();
            string promptGPT = await ReadGoogleSheetsPrompt.GetPropmtForGPT();

            string promptMessage = $"{promptGPT} -- {contentFromDocs}"; 

            List<Message> messages = new List<Message>();
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            while (true)
            {
                var content = promptMessage;

                if (content is not { Length: > 0 }) break;
                var message = new Message() { Role = "user", Content = content };
                messages.Add(message);

                var requestData = new Request()
                {
                    ModelId = modelAI,
                    Messages = messages
                };

                string jsonRequestData = JsonConvert.SerializeObject(requestData, Formatting.Indented);
                Console.WriteLine("Request Data: \n" + jsonRequestData);


                using var response = await httpClient.PostAsJsonAsync(endpoint, requestData);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"{(int)response.StatusCode} {response.StatusCode}");
                    break;
                }
                ResponseData? responseData = await response.Content.ReadFromJsonAsync<ResponseData>();

                var choices = responseData?.Choices ?? new List<Choice>();
                if (choices.Count == 0)
                {
                    Console.WriteLine("No choices were returned by the API");
                    continue;
                }
                var choice = choices[0];
                var responseMessage = choice.Message;
                messages.Add(responseMessage);
                var responseText = responseMessage.Content.Trim();

                //Console.WriteLine($"ChatGPT: {responseText}");

                ConnectWithGoogleSheet.PushValueToCell("C15", responseText);

                return responseText;
            }

            return null;
        }
    }
}
