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

// get culture names
var list = new Dictionary<string, string>();
foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
{
    string specName = CultureInfo.CreateSpecificCulture(ci.TwoLetterISOLanguageName).TwoLetterISOLanguageName;
    if (specName == "iv")
        continue;
    list.TryAdd(specName, ci.EnglishName);
}

movieService.Translate(new List<string>() { }, "", "");
movieService.GetInfo();
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

        Console.Write("Введите текст для перевода:\n");
        var text = new List<string>();
        while (true)
        {
            var textChunk = Console.ReadLine();
            if (textChunk == "")
                break;
            text.Add(textChunk);
        }

        Console.Write("Введите язык предложений:\n");
        string langFrom = Console.ReadLine();

        Console.Write("Введите язык для перевода:\n");
        string langTo = Console.ReadLine();

        movieService.Translate(text, langFrom, langTo);
    }
    else if (command == "info")
    {
        Console.WriteLine(movieService.GetInfo());
    }

}