using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskPlatform;

public interface ITranslateService
{
    public void Translate(List<string> text, string langFrom, string langTo);

    public string GetInfo();
}
