

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
    FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", article.GetElement("_id").Value);
    UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Set("rating", int.Parse(article.GetElement("rating").Value.AsString));
    collection.UpdateOne(filter, update);

}

FilterDefinition<BsonDocument> deleteFilter = Builders<BsonDocument>.Filter.Lte("rating", 50);

collection.DeleteMany(deleteFilter); 


foreach (BsonDocument article in articles)
{
    Console.WriteLine(article.GetElement("name").Value);
}

//Get ratings back to strings from integers

//foreach (BsonDocument article in articles)
//{
//    FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("rating", article.GetElement("rating").Value);
//    UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Set("rating", article.GetElement("rating").Value.ToString());
//    collection.UpdateOne(filter, update);
//    Console.WriteLine(article);

//}