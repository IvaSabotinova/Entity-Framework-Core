

using MongoDB.Bson;
using MongoDB.Driver;
using System.Globalization;

MongoClient client = new MongoClient("mongodb://localhost:27017");

IMongoDatabase database = client.GetDatabase("NoSQL");

IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("softuniArticles");

collection.InsertOne(new BsonDocument
{
    {"author", "Steve Jobs" } ,
    {"date", "05-05-2005" } ,
    {"name", "The story of Apple" } ,
    {"rating", "60" }

});

List<BsonDocument> allArticles = collection.Find(new BsonDocument { }).ToList();



foreach (BsonDocument article in allArticles)
{

    string articleName = article.GetElement("name").Value.AsString;

    DateTime date = DateTime.ParseExact(article.GetElement("date").Value.AsString, "dd-MM-yyyy", 
        CultureInfo.InvariantCulture, DateTimeStyles.None);

    FilterDefinition<BsonDocument> filterQuery = Builders<BsonDocument>.Filter.Eq("_id", article.GetElement("_id").Value);

    List<BsonDocument> item = collection.Find(filterQuery).ToList();

    int newRating = int.Parse(article.GetElement("rating").Value.AsString) + 10;

    UpdateDefinition<BsonDocument> updatedQuery = Builders<BsonDocument>.Update.Set("rating", newRating.ToString());

    

    Console.WriteLine(articleName);
    Console.WriteLine(date);

    Console.WriteLine(article.ToJson());

    
}


