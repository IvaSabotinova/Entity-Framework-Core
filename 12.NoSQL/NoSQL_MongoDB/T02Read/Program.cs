using MongoDB.Bson;
using MongoDB.Driver;



MongoClient client = new MongoClient(
    "mongodb://localhost:27017");


IMongoDatabase database = client.GetDatabase("NoSQL");

IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("softuniArticles");


List<BsonDocument> allArticles = collection.Find(new BsonDocument{}).ToList();

foreach (BsonDocument article in allArticles)
{
    string articleName = article.GetElement("name").Value.AsString;
    Console.WriteLine(articleName);

}

