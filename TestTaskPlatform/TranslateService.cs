using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskPlatform;

/// <summary>
/// Сервис для перевода
/// </summary>
public class TranslateService : ITranslateService
{
    private readonly IMemoryCache _cache;
    private readonly HttpClient _httpClient;
    public TranslateService(IMemoryCache cache, HttpClient httpClient)
    {
        _cache = cache;
        _httpClient = httpClient;
    }

    /// <summary>
    /// Перевод текста
    /// </summary>
    /// <param name="text"></param>
    /// <param name="langFrom"></param>
    /// <param name="langTo"></param>
    public List<string>? Translate(List<string> text, string langFrom, string langTo)
    {
        var translatedText = new List<string>();

        foreach (var textChunk in text)
        {
            string url = $"http://api.mymemory.translated.net/get?q={Uri.EscapeDataString(textChunk)}&langpair={langFrom}|{langTo}";

            HttpResponseMessage response = _httpClient.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();

            string responseJson = response.Content.ReadAsStringAsync().Result;
            var translationResult = JsonConvert.DeserializeObject<TranslationResponse>(responseJson);

            if(translationResult.ResponseStatus != 200)
            {
                Console.WriteLine($"\nОШИБКА: {translationResult.TranslatedText}");
                return null;
            }

            if (!_cache.TryGetValue(textChunk, out string cacheValue))
            {
                cacheValue = translationResult.TranslatedText;

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(2))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(24));

                _cache.Set(textChunk, cacheValue, cacheEntryOptions);

                //Сделано для того, чтобы апи не выдавала ошибки при частых запросах
                Thread.Sleep(5000);
            }

            translatedText.Add(cacheValue);
        }
        return translatedText;
    }

    public string GetInfo()
    {
        return
            """

            Сервис: http://api.mymemory.translated.net
            Тип кэша: дисковый/буферный
            Объем: реализация не подразумевает механизм измерения размера записей
            """;
    }

    private class TranslationResponse
    {
        [JsonProperty("responseStatus")]
        public int ResponseStatus { get; set; }

        [JsonProperty("responseData")]
        public TranslationValue ResponseData { get; set; }

        public string TranslatedText => ResponseData?.TranslatedText;
    }

    private class TranslationValue
    {
        [JsonProperty("translatedText")]
        public string TranslatedText { get; set; }
    }
}
