namespace Interfaces
{
    public class RemotingUrlBuilder
    {
        public static string GetUrl(int port, string name) =>
            $"tcp://localhost:{port}/{name}";
    }
}