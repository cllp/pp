using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using PP.Core.Helpers;
using PP.Core.Interfaces;


namespace PP.Core.Model
{
    [Serializable]
    public class ModelBase : IModel
    {
        private bool deleted = false;

        [XmlIgnore]
        public bool Deleted
        {
            get { return deleted; }
            set { deleted = value; }
        }

        private bool modified = false;

        [XmlIgnore]
        public bool Modified
        {
            get { return modified; }
            set { modified = value; }
        }

        public IDictionary<string, object> Properties
        {
            get
            {
                var properties = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Where(p => p.PropertyType.IsPrimitive && p.PropertyType.IsValueType || p.PropertyType == typeof(string));

                var dictionary = new SerializableDictionary<string, object>();

                foreach (var property in properties)
                {
                    dictionary.Add(property.Name, GetPropertyValue(property.Name));
                }

                return dictionary;
            }
        }

        public IDictionary<string, object> Models
        {
            get
            {
                var dictionary = new SerializableDictionary<string, object>();
                return dictionary;
            }
        }

 

        public List<Varience> Compare(IDictionary<string, object> second)
        {
            var variences = new List<Varience>();
            var comparer = EqualityComparer<object>.Default;

            foreach (KeyValuePair<string, object> kvp in Properties)
            {
                object secondValue;

                if (second.TryGetValue(kvp.Key, out secondValue) && comparer.Equals(kvp.Value, secondValue)) continue;
                var varience = new Varience
                {
                    Prop = kvp.Key,
                    NewValue = kvp.Value,
                    Type = this.GetType().Name,
                    OldValue = secondValue
                };
                variences.Add(varience);
            }
            return variences;
        }

        private object GetPropertyValue(string propertyName)
        {
            return this.GetType().GetProperty(propertyName).GetValue(this);
        }
    }
}
