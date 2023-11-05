using Microsoft.Extensions.Logging;

namespace Application.Extensions
{
    public static class TastySerilog
    {
        public static void AddInfoWithTastyName(this ILogger logger,string message)
        {
            logger.LogInformation($"[TASTY BITS]{message}");
        }
    }
}
