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
		private static IMongoCollection<ParkDesign> parkCollection;


		static void Main(string[] args)
		{
			try
			{
				Console.WriteLine("Connecting...");

				var client = new MongoClient("mongodb+srv://abuzduga:EducatedReality@cluster0.htavf.mongodb.net/EducatedReality?retryWrites=true&w=majority");
				
				var database = client.GetDatabase("EducatedReality");
				parkCollection = database.GetCollection<ParkDesign>("ParkDesigns");

				var parkDesign1 = new ParkDesign() {
					Description = "tets",
					Longitude = "10",
					Latitude = "10",
					VideoUrl = "123"
				};

				Insert(parkDesign1);

				var parkDesigns = GetAll();

				foreach (var parkDesign in parkDesigns)
				{
					Console.WriteLine($"Description: {parkDesign.Description}");
					Console.WriteLine($"VideoUrl: {parkDesign.VideoUrl}");
					Console.WriteLine($"Latitude: {parkDesign.Latitude}");
					Console.WriteLine($"Longitude: {parkDesign.Longitude}");
				}

				Console.ReadLine();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		public static IEnumerable<ParkDesign> GetAll()
		{
			var result = parkCollection.Find(new BsonDocument())
				.Skip(0)
				.Limit(100)
				.ToListAsync().Result;

			if (result.Any())
			{
				return result.Select(x => new ParkDesign()
				{ 
					Id = x.Id,
					Description = x.Description,
					Latitude = x.Latitude,
					Longitude = x.Longitude,
					VideoUrl = x.VideoUrl
				});;
			}

			return new List<ParkDesign>();
		}

		public static void Insert(ParkDesign entity)
		{
			parkCollection.InsertOne(entity);
		}

		public static void InsertMany(IEnumerable<ParkDesign> entities)
		{
			parkCollection.InsertMany(entities);
		}
	}

	public class ParkDesign
	{
		[BsonId]
		public ObjectId Id { get; set; }

		[BsonElement("videoUrl")]
		public string VideoUrl { get; set; }

		[BsonElement("description")]
		public string Description { get; set; }

		[BsonElement("longitude")]
		public string Longitude { get; set; }

		[BsonElement("latitude")]
		public string Latitude { get; set; }
	}
}
