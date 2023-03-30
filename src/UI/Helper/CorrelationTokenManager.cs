using Snowflake;

namespace MusicStore.Helper
{
    public class CorrelationTokenManager 
    {
        public static string GenerateToken()
        {
            // Attempt to grab operationId for App Insights.
            // If available, assign as correlationToken.
            // If not, generate with Snowflake
            var appInsightOperationKey = System.Diagnostics.Activity.Current.RootId;

            return !string.IsNullOrEmpty(appInsightOperationKey) ? 
                $"{SnowflakeEnum.Correlation}-{appInsightOperationKey}" 
                : SnowflakeIdGenerator.GenerateId(SnowflakeEnum.Correlation);
        }
    }
}