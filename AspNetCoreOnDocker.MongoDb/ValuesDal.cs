using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace AspNetCoreOnDocker.MongodDb
{
    public class ValuesDal
    {
        private const string CONNECTION_STRING = "mongodb://localhost:27017";
        private const string DATABASE_NAME = "AspNetCoreOnDocker";
        private const string COLLECTION_NAME = "Values";
        private readonly IMongoCollection<ValueModel> _valuesCollection;

        public ValuesDal()
        {
            var client = new MongoClient(CONNECTION_STRING);
            IMongoDatabase db = client.GetDatabase(DATABASE_NAME);
            _valuesCollection = db.GetCollection<ValueModel>(COLLECTION_NAME);
        }

        public void AddValue(string value)
        {
            var model = new ValueModel
            {
                Value = value
            };
            _valuesCollection.InsertOne(model);
        }

        public IEnumerable<string> GetAllValues()
        {
            return _valuesCollection.AsQueryable()
                .ToEnumerable()
                .Select(x => x.Value);
        }
    }

    public class ValueModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Value { get; set; }
    }
}