using System.Net;
//using System.Reflection.Metadata;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using order.domain.AggregateModels.OrderAggregate;
using order.domain.Contracts;
//using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace order.infrastructure.nosql.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _endpointUri;
        private readonly string _primaryKey;
        private DocumentClient _client;
        private const string _databaseName = "OrderDB";
        private readonly string _collectionName = "OrderCollection";

        public OrderRepository(DataStoreConfiguration dataStoreConfiguration)
        {
            _endpointUri = dataStoreConfiguration.EndPointUri;
            _primaryKey = dataStoreConfiguration.Key;
        }

        public async Task<string> Add(Order entity,
            TelemetryClient _telemetryClient)
        {
            // Telemetry variables
            var success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            var returnValue = "Not Found";

            _client = new DocumentClient(new Uri(_endpointUri), _primaryKey);
            await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = _databaseName });
            DocumentCollection collectionDefinition = new DocumentCollection();
            collectionDefinition.PartitionKey.Paths.Add("/id");
            collectionDefinition.Id = "OrderCollection";
            collectionDefinition.IndexingPolicy = new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 });
            await _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(_databaseName),
                collectionDefinition);



            //// Upsert Code
            ////Yes, Cosmos DB supports upserts. This can be done using the ReplaceDocumentAsync method with the replaceOrInsert parameter set to true.This operation will attempt to replace the existing document with the one you supply.If the document does not exist, it will insert the new document instead.
            //var client = new CosmosClient("cosmosEndpoint", "cosmosKey");
            //var container = client.GetContainer("databaseName", "containerName");
            //var document = new MyDocument { Id = "myid", Prop1 = "value1" };
            //await container.ReplaceItemAsync<MyDocument>(document, document.Id, new PartitionKey(document.prop1), new ItemRequestOptions() { IfMatchEtag = "*" }, new Cosmos.Diagnostics.DiagnosticsContext(default));

            //The above example shows how to use the ReplaceItemAsync method to update or insert a MyDocument instance into the container variable.The IfMatchEtag option is set to *, which means that the request should replace any document with the same ID as document, regardless of whether it has previously been modified or not.


            //return await CreateOrderDocumentIfNotExists(_databaseName, "OrderCollection", entity);

            try
            {
                var response = await _client.CreateDocumentAsync(
                    UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName), entity);

                returnValue = response.Resource.Id;
                success = true;
                //return response.Resource.Id;
            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    // await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), orders);
                    returnValue = "NotFound";
                }
                else
                {
                    _telemetryClient.TrackException(ex);
                    throw;
                }
            }
            finally
            {
                _telemetryClient.TrackDependency("DependencyType", "CosmosInsert", returnValue, startTime, timer.Elapsed, success);
            }

            return returnValue;
        }

        public async Task<dynamic> GetById(string id,
                                   string correlationToken,
                                   TelemetryClient _telemetryClient)
        {
            // Telemetry variables
            var success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            var returnValue = "Not Found";

            dynamic order = null;

            try
            {
                _client = new DocumentClient(new Uri(_endpointUri), _primaryKey);

                var response =
                    await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri(_databaseName, _collectionName,
                        id), new RequestOptions
                        { PartitionKey = new PartitionKey(id) });

                success = true;

                return response.Resource;

            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    // Cannot find specified document
                    order = null;
                }
                else
                {
                    _telemetryClient.TrackException(ex);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                throw;
            }
            finally
            {
                _telemetryClient.TrackDependency("DependencyType", "GetOrderById", returnValue, startTime, timer.Elapsed, success);
            }

            return order;
        }

        public async Task<dynamic> GetAll(string correlationToken,
                                          TelemetryClient _telemetryClient)
        {
            // Telemetry variables
            var success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            var returnValue = "Not Found";

            var orders = new List<dynamic>();
            try
            {
                dynamic order = null;
                _client = new DocumentClient(new Uri(_endpointUri), _primaryKey);
                var documents =
                    await _client.ReadDocumentFeedAsync(
                        UriFactory.CreateDocumentCollectionUri(_databaseName, "OrderCollection"), new FeedOptions { MaxItemCount = 300 });

                foreach (Document document in documents)
                {
                    order = document;
                    orders.Add(order);
                }
            }
            catch (DocumentClientException ex)
            {
                // Cannot find specified document
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    _telemetryClient.TrackException(ex);
                    orders = null;
                }
                else
                {
                    _telemetryClient.TrackException(ex);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                throw;
            }
            finally
            {
                _telemetryClient.TrackDependency("DependencyType", "GetAllOrders", returnValue, startTime, timer.Elapsed, success);
            }

            return orders;
        }

        private async Task<string> CreateOrderDocumentIfNotExists(string databaseName, string collectionName,
            Order orders)
        {
            try
            {
                var response = await _client.CreateDocumentAsync(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    orders);

                return response.Resource.Id;
            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    // await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), orders);
                }
                else
                {
                    throw;
                }
            }

            return null;
        }
    }
}