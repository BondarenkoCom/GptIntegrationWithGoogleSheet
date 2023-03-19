using GptIntegrationWithGoogleSheet.GoogleSheetsIntegration;
using GptIntegrationWithGoogleSheet.GPTIntegration;
using GptIntegrationWithGoogleSheet.Helpers;
using GptIntegrationWithGoogleSheet.Models;

Console.WriteLine("start");

//var ReadGoogleSheets = new HelperSheet();
//await ReadGoogleSheets.GetNumberOfFilesToUpload();

//var DReader = new DocsReader();
//await DReader.ReadDocsByLink();

//var ReadGoogleSheetsPrompt = new HelperSheet();
//await ReadGoogleSheetsPrompt.GetPropmtForGPT();

//var StartGptDriver = new GPTDriver();
//await StartGptDriver.InitGPTDriver();

//GPTDriver.ExecuteGPTDriver();

var InitGpt = new GPTDriver();
await InitGpt.InitGPTAPI();

Console.ReadLine();