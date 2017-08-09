using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SecurityPlusCore
{
    public class Client
    {
        private volatile bool canRun;

        private Protocol protocol;

        private IPEndPoint endpoint;

        private Thread serverThread;

        private TcpClient tcpClient;

        public Client(IPEndPoint endpoint)
        {
            if (null == endpoint)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            this.endpoint = endpoint;

            this.protocol = new Protocol();

            this.tcpClient = new TcpClient();
        }

        public void Start()
        {
            if (!this.canRun)
            {
                this.canRun = true;
                this.serverThread = new Thread(this.ExecuteOnThread);
                this.serverThread.IsBackground = true;
                this.serverThread.Start();
            }
        }

        public void Stop()
        {
            if (this.canRun)
            {
                this.canRun = false;
                this.serverThread.Join();
            }
        }

        public void RegisterValidationCommand(IProcessValidationCommand processValidationCommand)
        {
            this.protocol.RegisterProcessValidationCommand(processValidationCommand);
        }

        public void ClearRegisteredCommands()
        {
            this.protocol.ClearRegisteredProcessValidationCommands();
        }

        public void RegisterPreProcessValidationCallback(EventHandler<ProcessValidationRequest> preProcessValidationCallback)
        {
            this.protocol.PreProcessValidation += preProcessValidationCallback;
        }

        public void RegisterPostProcessValidationCallback(EventHandler<Tuple<ProcessValidationRequest, ProcessValidationResponse>> postProcessValidationCallback)
        {
            this.protocol.PostProcessValidation += postProcessValidationCallback;
        }

        private void ExecuteOnThread()
        {
            try
            {
                using (this.tcpClient)
                {
                    if (!this.tcpClient.Connected)
                    {
                        this.tcpClient.Connect(this.endpoint);
                    }

                    using (var stream = this.tcpClient.GetStream())
                    {
                        this.protocol.SendProtectedProcessId(stream);

                        do
                        {
                            this.protocol.ProcessRequest(stream).ConfigureAwait(true);
                        }
                        while (this.canRun);
                    }
                }
            }
            catch
            {
                if (this.tcpClient.Connected)
                {
                    this.tcpClient.Close();
                }
            }
        }
    }
}
