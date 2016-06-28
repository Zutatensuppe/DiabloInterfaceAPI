using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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

        async Task<string> ReadJsonStringAsync()
        {
            // Get string length.
            byte[] buffer = new byte[4];
            int read = await reader.BaseStream.ReadAsync(buffer, 0, 4);
            if (read != 4) return null;
            int length = BitConverter.ToInt32(buffer, 0);

            // Read string bytes.
            buffer = new byte[length];
            read = await reader.BaseStream.ReadAsync(buffer, 0, length);
            if (read != length) return null;

            // Convert to correct encoding.
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

        public async Task<T> ReadJsonAsync<T>() where T : class
        {
            string jsonData = await ReadJsonStringAsync();
            if (jsonData == null) return null;
            return JsonConvert.DeserializeObject<T>(jsonData);
        }
    }
}
