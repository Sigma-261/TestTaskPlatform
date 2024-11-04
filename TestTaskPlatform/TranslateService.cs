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
    public void Translate(List<string> text, string langFrom, string langTo)
    {

        string CurrentDateTime = "Hello";

        if (!_cache.TryGetValue(CurrentDateTime, out string cacheValue))
        {
            cacheValue = CurrentDateTime;

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(23))
                .SetAbsoluteExpiration(TimeSpan.FromHours(23));

            _cache.Set(cacheValue, CurrentDateTime, cacheEntryOptions);
        }

        var test = cacheValue;

        //foreach (var item in text)
        //{
        //    string CurrentDateTime = "Hello";

        //    if (!_cache.TryGetValue(CurrentDateTime, out string cacheValue))
        //    {
        //        cacheValue = CurrentDateTime;

        //        var cacheEntryOptions = new MemoryCacheEntryOptions()
        //            .SetSlidingExpiration(TimeSpan.FromHours(23));

        //        _cache.Set(cacheValue, CurrentDateTime, cacheEntryOptions);
        //    }

        //    var test = cacheValue;

        //    string url = $"http://api.mymemory.translated.net/get?q={Uri.EscapeDataString("Привет мир!")}&langpair={"ru"}|{"en"}";

        //    HttpResponseMessage response = _httpClient.GetAsync(url).Result;
        //    response.EnsureSuccessStatusCode();

        //    //string responseJson = await response.Content.ReadAsStringAsync();
        //    //var translationResult = JsonConvert.DeserializeObject<TranslationResponse>(responseJson);
        //}

    }

    public string GetInfo()
    {
        return
            """
            Сервис: TranslateService
            Тип кэша: дисковый/буферный
            Объем: Реализация не подразумевает механизм измерения размера записей
            """;
    }
}
