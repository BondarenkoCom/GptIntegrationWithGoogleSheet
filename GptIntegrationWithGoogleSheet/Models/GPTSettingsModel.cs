namespace GptIntegrationWithGoogleSheet.Models
{
    public class GPTSettingsModel
    {
        public string API { get; set; }
        public string FinetuneID { get; set; }
        public string ModelID { get; set; }
        public double MaxTemp { get; set; }
        public int MaxTokens { get; set; }
        public double TopP { get; set; }
        public int BestOf { get; set; }
        public string Engine { get; set; }
        public string Model { get; set; }
        public string OpenApiKey { get; set; }
        public string endPoint { get; set; }
    }
}
