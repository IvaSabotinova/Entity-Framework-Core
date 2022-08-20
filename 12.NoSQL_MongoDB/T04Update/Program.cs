
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

string connectionString = "mongodb://localhost:27017";

IServiceProvider serviceProvider = new ServiceCollection()
    .AddSingleton<IMongoClient>(s=> new MongoClient(connectionString))
    .BuildServiceProvider();

IMongoClient client = serviceProvider.GetRequiredService<IMongoClient>();

IMongoDatabase database = client.GetDatabase("NoSQL");

IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("softuniArticles");

List<BsonDocument> articles = collection.Find(new BsonDocument { }).ToList();

foreach (BsonDocument article in articles)
{
    int updatedRating = int.Parse(article.GetElement("rating").Value.AsString) + 10;

    FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", article.GetElement("_id").Value);
    UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Set("rating", updatedRating.ToString());
    collection.UpdateOne(filter, update);

    string articleName = article.GetElement("name").Value.AsString;
    Console.WriteLine($"{articleName} - rating: {article.GetElement("rating").Value}");


}
