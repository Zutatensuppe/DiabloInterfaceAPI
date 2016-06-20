using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace DiabloInterfaceAPI
{
    public class DiabloInterfaceConnection : IDisposable
    {
        NamedPipeClientStream client;

        /// <summary>
        /// Creates a new connection to the pipe server with a default timeout of 5 seconds.
        /// </summary>
        /// <param name="pipeName">Name of the server to connect to.</param>
        /// <exception cref="ArgumentNullException">pipeName is null.</exception>
        /// <exception cref="ArgumentException">pipeName is a zero length string.</exception>
        /// <exception cref="ArgumentOutOfRangeException">pipeName is set to "anonymous".</exception>
        /// <exception cref="TimeoutException">Could not connecting without timeout period.</exception>
        public DiabloInterfaceConnection(string pipeName)
        {
            InitializeConnection(pipeName, 5000);
        }

        /// <summary>
        /// Creates a new connection to the pipe server with a specified timeout.
        /// </summary>
        /// <param name="pipeName">Name of the server to connect to.</param>
        /// <param name="timeout">Timeout in milliseconds.</param>
        /// <exception cref="ArgumentNullException">pipeName is null.</exception>
        /// <exception cref="ArgumentException">pipeName is a zero length string.</exception>
        /// <exception cref="ArgumentOutOfRangeException">pipeName is set to "anonymous".</exception>
        /// <exception cref="TimeoutException">Could not connecting without timeout period.</exception>
        public DiabloInterfaceConnection(string pipeName, int timeout)
        {
            InitializeConnection(pipeName, timeout);
        }

        void InitializeConnection(string pipeName, int timeout)
        {
            timeout = Math.Max(timeout, 0);
            client = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
            try
            {
                client.Connect(timeout);
            }
            // An IOException is thrown when the server is busy handling other requests and
            // the time runs out. It makes more sense for it to be a TimeoutException in this case.
            catch (IOException e)
            {
                throw new TimeoutException("Could not connect to server.", e);
            }
        }

        /// <summary>
        /// Clean up any resources associated with this object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (client != null)
                {
                    client.Close();
                    client = null;
                }
            }
        }

        /// <summary>
        /// Make an item request to the interface server.
        /// </summary>
        /// <param name="request">Item request to make.</param>
        /// <returns></returns>
        public ItemResponse Request(ItemRequest request)
        {
            try
            {
                var writer = new JsonStreamWriter(client, Encoding.UTF8);
                writer.WriteJson(request);
                writer.Flush();

                client.WaitForPipeDrain();

                var reader = new JsonStreamReader(client, Encoding.UTF8);
                var response = reader.ReadJson<ItemResponse>();

                return response;
            }
            catch (EndOfStreamException)
            {
                return null;
            }
            catch (JsonException)
            {
                return null;
            }
        }
    }
}
