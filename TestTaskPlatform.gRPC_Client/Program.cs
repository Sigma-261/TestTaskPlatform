// создаем канал для обмена сообщениями с сервером
// параметр - адрес сервера gRPC
using Grpc.Net.Client;
using System.Globalization;
using System.Xml.Linq;
using TestTaskPlatform.gRPC_Client;

using var channel = GrpcChannel.ForAddress("http://localhost:5176");
// создаем клиент
var client = new GrpcTranslate.GrpcTranslateClient(channel);

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

        var textRequest = new ListOfStrings();
        textRequest.Strings.AddRange(text);

        var translatedText = client.GetTranslate(new TranslateRequest { Text = textRequest, LangFrom = langFrom, LangTo = langTo });

        if (translatedText == null)
            continue;

        Console.Write("\nПеревод:\n");

        foreach (var textChunk in translatedText.Strings)
        {
            Console.WriteLine(textChunk);
        }

    }
    else if (command == "info")
    {
        Console.WriteLine(client.GetInfo(new Empty { }));
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