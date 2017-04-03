using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;


namespace PP.Api.Helpers
{
    public class Replacement
    {
        private string key;

        public string Key
        {
            get { return key; }
            set { key = value; }
        }
        private string value;

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public Replacement(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TextReplace : List<Replacement>, ITextReplace
    {
        public void Add(string key, string value)
        {
            Replacement replacement = new Replacement(key, value);
            this.Add(replacement);
        }

        public string ReplaceFromText(string text)
        {
            foreach (Replacement r in this)
                text = RegExReplace(text, r.Key, r.Value);

            return text;
        }

        public string ReplaceFromFile(string file)
        {
            string text = ReadFile(file);

            foreach (Replacement r in this)
                text = RegExReplace(text, r.Key, r.Value);

            return text;
        }

        private string ReadFile(string file)
        {
            StreamReader fp = new StreamReader(file, Encoding.Default);
            string filecontent = fp.ReadToEnd();
            fp.Close();
            return filecontent;
        }

        private string RegExReplace(string stringToReplace, string patternToReplace, string patternToReplaceWith)
        {
            if (string.IsNullOrEmpty(patternToReplaceWith))
                patternToReplaceWith = string.Empty;

            return Regex.Replace(stringToReplace, patternToReplace, patternToReplaceWith, RegexOptions.CultureInvariant);
        }
    }
}