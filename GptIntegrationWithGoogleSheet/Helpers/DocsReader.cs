using System.Text.RegularExpressions;

namespace GptIntegrationWithGoogleSheet.Helpers
{
    public class DocsReader
    {
        public async Task<string> ReadDocsByLink(string urlFromSheetCell)
        {
            string googleDocsUrl = urlFromSheetCell;
            string fileData = "output.txt";

            await DownloadTextFileFromGoogleDrive(googleDocsUrl, fileData);

            Console.WriteLine($"Downloaded text file as {fileData}");
            string content = "";
            List<string> lines = new List<string>();

            //Try read Google Docs txt
            try
            {
                lines = File.ReadAllLines(fileData).ToList();

                foreach (string line in lines)
                {
                  content += line;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return content;
        }

        private async Task DownloadTextFileFromGoogleDrive(string googleDocsUrl, string fileData)
        {
            using HttpClient httpClient = new HttpClient();

            string fileIdPattern = @"\/document\/d\/([a-zA-Z0-9-_]+)\/";
            Regex fileIdRegex = new Regex(fileIdPattern);
            Match fileIdMatch = fileIdRegex.Match(googleDocsUrl);

            if (!fileIdMatch.Success)
            {
                throw new Exception("File ID not found");
            }

            string fileId = fileIdMatch.Groups[1].Value;
            string downloadLink = $"https://docs.google.com/document/export?format=txt&id={fileId}";

            // Download the file
            HttpResponseMessage downloadResponse = await httpClient.GetAsync(downloadLink);
            downloadResponse.EnsureSuccessStatusCode();

            using Stream responseStream = await downloadResponse.Content.ReadAsStreamAsync();
            using FileStream fileStream = new FileStream(fileData, FileMode.Create, FileAccess.Write, FileShare.None);

            await responseStream.CopyToAsync(fileStream);
        }
    }
}
