using PP.Core.Interfaces;
using System;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using PP.Core.Interfaces;

namespace PP.Core.Helpers
{
    public sealed class SerializeXml : ISerializer
    {
        private static Hashtable mSerializers = Hashtable.Synchronized(new Hashtable());

        public T Deserialize<T>(string data)// where T : new()
        {
            T obj;
            using (StringReader dataReader = new StringReader(data))
            {
                obj = (T)GetSerializer(typeof(T)).Deserialize(dataReader);
            }
            return obj;
        }

        public string Serialize(object instance)
        {
            string s;
            using (StringWriter writer = new StringWriter())
            {
                GetSerializer(instance.GetType()).Serialize(writer, instance);
                s = writer.ToString();
            }

            return s;
        }

        private XmlSerializer GetSerializer(Type type)
        {
            if (mSerializers.ContainsKey(type))
            {
                return (XmlSerializer)mSerializers[type];
            }
            else
            {
                XmlSerializer serializer = new XmlSerializer(type);
                mSerializers[type] = serializer;
                return serializer;
            }
        }

    }
}
