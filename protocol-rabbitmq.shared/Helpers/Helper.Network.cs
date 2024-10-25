using System.Net.Sockets;

namespace protocol.rabbitmq.shared.Helpers
{
    public static partial class Helper
    {
        public static class NetWork
        {
            /// <summary>
            /// Verifica se uma determinada porta esta aberta
            /// </summary>
            /// <param name="ip"></param>
            /// <param name="port"></param>
            /// <param name="timeout"></param>
            /// <returns></returns>
            public static async Task<bool> PortIsOpenAsync(string ip, int port, int timeout = 3000)
            {
                using (TcpClient client = new TcpClient())
                {
                    using (var cancellationTokenSource = new CancellationTokenSource(timeout))
                    {
                        try
                        {
                            await client.ConnectAsync(ip, port).WaitAsync(cancellationTokenSource.Token);
                            return true; // Conexão bem sucedida, port aberta
                        }
                        catch (Exception)
                        {
                            return false; // Conexão falhou, port fechada ou inacessível
                        }
                    }
                }
            }

            public static async Task<bool> PortIsOpenByUrlAsync(string url, int timeout = 3000)
            {
                Uri uri;
                try
                {
                    uri = new Uri(url);
                }
                catch (UriFormatException)
                {
                    return false; // URL inválida
                }

                string ip = uri.Host;
                int port = uri.Port;

                // Se a porta não for especificada na URL, use a porta padrão para o protocolo
                if (port == -1)
                {
                    switch (uri.Scheme.ToLower())
                    {
                        case "http":
                            port = 80;
                            break;
                        case "https":
                            port = 443;
                            break;
                        case "ftp":
                            port = 21;
                            break;
                        default:
                            return false; // Protocolo não suportado
                    }
                }

                using (TcpClient client = new TcpClient())
                {
                    using (var cancellationTokenSource = new CancellationTokenSource(timeout))
                    {
                        try
                        {
                            await client.ConnectAsync(ip, port).WaitAsync(cancellationTokenSource.Token);
                            return true; // Conexão bem sucedida, porta aberta
                        }
                        catch (Exception)
                        {
                            return false; // Conexão falhou, porta fechada ou inacessível
                        }
                    }
                }
            }

            public static bool PortIsOpenByUrl(string url, int timeout = 3000)
            {
                var task = Task.Run(() => PortIsOpenByUrlAsync(url, timeout)); // Usar Task.Run para evitar bloqueio da thread principal
                task.Wait();
                return task.Result;
            }


        }
    }
}
