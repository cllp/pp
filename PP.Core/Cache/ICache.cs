using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Cache
{
    public interface ICache
    {
        T Get<T>(string key) where T : class;

        void Add<T>(T objectToCache, string key) where T : class;

        void Add(object objectToCache, string key);

        void Clear(string key);

        bool Exists(string key);

        List<string> GetAll();
    }
}
