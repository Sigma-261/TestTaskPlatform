using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using TestTaskPlatform;
using static System.Net.Mime.MediaTypeNames;

var services = new ServiceCollection();

services.AddMemoryCache();
services.AddTransient<ITranslateService, TranslateService>();
services.AddHttpClient();

var serviceProvider = services.BuildServiceProvider();

ITranslateService movieService = serviceProvider.GetRequiredService<ITranslateService>();

while (true)
{
    Console.Write(
        """

        Введите команду:
        translate - перевод текста
        info - информация о тексте

        """);
    var command = Console.ReadLine();

    if (command == "translate")
    {
        Console.Write("Список языков:\n");
        foreach (var code in GetLangCodes())
        {
            Console.WriteLine($"{code.Key} - {code.Value}");
        }

        Console.Write("\nВведите текст для перевода:\n");
        var text = new List<string>();
        while (true)
        {
            var textChunk = Console.ReadLine();
            if (textChunk == "")
                break;
            text.Add(textChunk);
        }

        Console.Write("\nВведите язык предложений:\n");
        string langFrom = Console.ReadLine();

        Console.Write("\nВведите язык для перевода:\n");
        string langTo = Console.ReadLine();

        var translatedText = movieService.Translate(text, langFrom, langTo);

        if (translatedText == null)
            continue;

        Console.Write("\nПеревод:\n");

        foreach (var textChunk in translatedText)
        {
            Console.WriteLine(textChunk);
        }

    }
    else if (command == "info")
    {
        Console.WriteLine(movieService.GetInfo());
    }

}


static Dictionary<string, string> GetLangCodes()
{
    var langCodes = new Dictionary<string, string>();
    foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
    {
        string specName = CultureInfo.CreateSpecificCulture(ci.TwoLetterISOLanguageName).TwoLetterISOLanguageName;
        if (specName == "iv")
            continue;
        langCodes.TryAdd(specName, ci.EnglishName);
    }

    return langCodes;
}