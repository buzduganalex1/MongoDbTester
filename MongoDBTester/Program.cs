using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBTester
{
	class Program
	{
		private static IMongoCollection<EducatedRealityRoom> collection;

		static void Main(string[] args)
		{
			try
			{
				Console.WriteLine("Connecting...");

				var client = new MongoClient("mongodb+srv://abuzduga:Himnituterugi1@cluster0.htavf.mongodb.net/EducatedReality?retryWrites=true&w=majority");
				var database = client.GetDatabase("EducatedReality");
				collection = database.GetCollection<EducatedRealityRoom>("Rooms");

				var rooms = GetAll();

				foreach (var room in rooms)
				{
					Console.WriteLine($"Roomname: {room.Name}");
					Console.WriteLine($"Roompassword: {room.Password}");
				}

				Console.ReadLine();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		public static IEnumerable<EducatedRealityRoom> GetAll()
		{
			var result = collection.Find(new BsonDocument())
				.Skip(0)
				.Limit(100)
				.ToListAsync().Result;

			if (result.Any())
			{
				return result.Select(x => new EducatedRealityRoom()
				{
					Name = x.Name,
					Password = x.Password,
					Type = x.Type
				});
			}

			return new List<EducatedRealityRoom>();
		}

		public static void Insert(EducatedRealityRoom entity)
		{
			collection.InsertOne(entity);
		}

		public static void InsertMany(IEnumerable<EducatedRealityRoom> entities)
		{
			collection.InsertMany(entities);
		}
	}

	public class EducatedRealityRoom
	{
		[BsonId]
		public ObjectId Id { get; set; }

		[BsonElement("name")]
		public string Name { get; set; }

		[BsonElement("password")]
		public string Password { get; set; }

		[BsonElement("type")]
		public string Type { get; set; }
	}
}
