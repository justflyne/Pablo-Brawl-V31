namespace Supercell.Laser.Server.Networking
{
    using Supercell.Laser.Server.Networking.Session;
    using System.Collections.Generic;
    using System.Threading;

    public static class Connections
    {
        public static int Count => ActiveConnections.Count;

        private static List<Connection> ActiveConnections;
        private static Thread Thread;
        private static readonly object _lock = new object();

        public static void Init()
        {
            ActiveConnections = new List<Connection>();
            Thread = new Thread(Update);
            Thread.Start();
        }

        private static void Update()
        {
            while (true)
            {
                List<Connection> connectionsCopy;
                
                lock (_lock)
                {
                    connectionsCopy = new List<Connection>(ActiveConnections); // Копируем список перед итерацией
                }

                foreach (Connection connection in connectionsCopy)
                {
                    if (!connection.MessageManager.IsAlive())
                    {
                        lock (_lock)
                        {
                            if (connection.MessageManager.HomeMode != null)
                            {
                                Sessions.Remove(connection.Avatar.AccountId);
                            }
                            connection.Close();
                            ActiveConnections.Remove(connection); // Удаляем безопасно
                        }
                    }
                }
                
                Thread.Sleep(1000);
            }
        }

        public static void AddConnection(Connection connection)
        {
            lock (_lock)
            {
                ActiveConnections.Add(connection);
            }
        }
    }
}