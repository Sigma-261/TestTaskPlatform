using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
HttpClient httpClient = new HttpClient();


string url = $"http://api.mymemory.translated.net/get?q={Uri.EscapeDataString("Привет мир!")}&langpair={"ru"}|{"en"}";

HttpResponseMessage response = await httpClient.GetAsync(url);
response.EnsureSuccessStatusCode();

string responseJson = await response.Content.ReadAsStringAsync();
var translationResult = JsonConvert.DeserializeObject<TranslationResponse>(responseJson);


public class TranslationResponse
{
    [JsonProperty("responseStatus")]
    public int ResponseStatus { get; set; }

    [JsonProperty("responseData")]
    public TranslationData ResponseData { get; set; }

    public string TranslatedText => ResponseData?.TranslatedText;
}

public class TranslationData
{
    [JsonProperty("translatedText")]
    public string TranslatedText { get; set; }
}
