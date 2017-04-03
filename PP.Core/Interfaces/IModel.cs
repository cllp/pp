using System.Collections.Generic;
using PP.Core.Model;

namespace PP.Core.Interfaces
{
    public interface IModel
    {
        List<Varience> Compare(IDictionary<string, object> second);

        IDictionary<string, object> Properties { get; }

        IDictionary<string, object> Models { get; }

        bool Deleted { get; set; }

        bool Modified { get; set; }
    }
}
