
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;


string connectionString = "mongodb://localhost:27017";

IServiceProvider serviceProvider = new ServiceCollection()
    .AddSingleton<IMongoClient>(s => new MongoClient(connectionString))
    .BuildServiceProvider();

IMongoClient client = serviceProvider.GetRequiredService<IMongoClient>();

IMongoDatabase database = client.GetDatabase("NoSQL");

IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("softuniArticles");

//List<BsonDocument> articles = collection.Find(new BsonDocument { }).ToList()
//              .Where(x => int.Parse(x.GetElement("rating").Value.AsString) < 50).ToList();

//foreach (BsonDocument article in articles)
//{
//    Console.WriteLine(article);
//}


List<BsonDocument> articles = collection.Find(new BsonDocument { }).ToList();

foreach (BsonDocument article in articles)
{

    FilterDefinition<BsonDocument> filterQuery = Builders<BsonDocument>.Filter.Eq("rating", article.GetElement("rating").Value);
    UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Set("rating", int.Parse(article.GetElement("rating").Value.AsString));
    collection.UpdateOne(filterQuery, update);
    Console.WriteLine(article);

}

FilterDefinition<BsonDocument> newFilter = Builders<BsonDocument>.Filter.Lte("rating", 50);
IFindFluent<BsonDocument, BsonDocument> query = collection.Find(newFilter);


foreach (BsonDocument article in query.ToList())
{
    Console.WriteLine(article);
}

//List<BsonDocument> articles = collection.Find(new BsonDocument { }).ToList();

//foreach (BsonDocument article in articles)
//{
//    FilterDefinition<BsonDocument> filterQuery = Builders<BsonDocument>.Filter.Eq("rating", article.GetElement("rating").Value);
//    UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Set("rating", article.GetElement("rating").Value.ToString());
//    collection.UpdateOne(filterQuery, update);
//    Console.WriteLine(article);

//}



