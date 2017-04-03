using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Api.Helpers
{
    public interface ITextReplace
    {
        void Add(string key, string value);

        string ReplaceFromText(string text);

        string ReplaceFromFile(string file);
    }
}
