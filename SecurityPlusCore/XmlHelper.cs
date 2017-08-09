using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SecurityPlusCore
{
    public class XmlHelper
    {
        public static T Deserialize<T>(string item)
        {
            T result = default(T);

            if (!string.IsNullOrWhiteSpace(item))
            {
                using (var reader = new StringReader(item))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    result = (T)serializer.Deserialize(reader);
                }
            }

            return result;
        }

        public static string Serialize<T>(T item)
        {
            var stringBuilder = new StringBuilder();

            if (null != item)
            {
                using (var writer = new StringWriter(stringBuilder))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(writer, item);
                }
            }

            return stringBuilder.ToString();
        }
    }
}
