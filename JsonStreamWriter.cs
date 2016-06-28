using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DiabloInterfaceAPI
{
    internal class JsonStreamWriter
    {
        BinaryWriter writer;
        Encoding encoding;

        public JsonStreamWriter(Stream stream, Encoding encoding)
        {
            writer = new BinaryWriter(stream);
            this.encoding = encoding;
        }

        public void WriteJsonString(string json)
        {
            var buffer = encoding.GetBytes(json);
            writer.Write(buffer.Length);
            writer.Write(buffer);
        }

        public async Task WriteJsonStringAsync(string json)
        {
            var buffer = encoding.GetBytes(json);
            var lengthBuffer = BitConverter.GetBytes(buffer.Length);

            await writer.BaseStream.WriteAsync(lengthBuffer, 0, lengthBuffer.Length);
            await writer.BaseStream.WriteAsync(buffer, 0, buffer.Length);

        }

        public void Flush() => writer.Flush();
        public async Task FlushAsync() => await writer.BaseStream.FlushAsync();

        public void WriteJson(object json)
        {
            string jsonData = JsonConvert.SerializeObject(json);
            WriteJsonString(jsonData);
        }

        public async Task WriteJsonAsync(object json)
        {
            string jsonData = JsonConvert.SerializeObject(json);
            await WriteJsonStringAsync(jsonData);
        }
    }
}
