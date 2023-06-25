namespace order.infrastructure.nosql.Configurations;
using System.Collections.Generic;

public class CosmosDbConfiguration
{
    public const string SectionKey = "CosmosDBSettings";
    /// <summary>
    ///     CosmosDb Account - The Azure Cosmos DB endpoint
    /// </summary>
    public string ConnectionString { get; set; }
    /// <summary>
    ///     Database name
    /// </summary>
    public string DatabaseName { get; set; }

    /// <summary>
    ///     List of containers in the database
    /// </summary>
    public List<ContainerInfo> Containers { get; set; }

}
public class ContainerInfo
{
    /// <summary>
    ///     Container Name
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    ///     Container partition Key
    /// </summary>
    public string PartitionKey { get; set; }
}
