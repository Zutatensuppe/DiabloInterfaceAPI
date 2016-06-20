using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace DiabloInterfaceAPI
{
    internal class JsonStreamReader
    {
        BinaryReader reader;
        Encoding encoding;

        public JsonStreamReader(Stream stream, Encoding encoding)
        {
            reader = new BinaryReader(stream);
            this.encoding = encoding;
        }

        string ReadJsonString()
        {
            int length = reader.ReadInt32();
            byte[] buffer = new byte[length];
            int read = reader.Read(buffer, 0, length);
            if (read != length) return null;

            return encoding.GetString(buffer);
        }

        public object ReadJson()
        {
            string jsonData = ReadJsonString();
            if (jsonData == null) return null;
            return JsonConvert.DeserializeObject(jsonData);
        }

        public T ReadJson<T>() where T : class
        {
            string jsonData = ReadJsonString();
            if (jsonData == null) return null;
            return JsonConvert.DeserializeObject<T>(jsonData);
        }
    }
}
