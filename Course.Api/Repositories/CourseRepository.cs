using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;

namespace Course.Api.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly IMongoDatabase _database;

        public CourseRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Models.Course> GetAsync(Guid id)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<Models.Course>> BrowseAsync(Guid userId)
            => await Collection
                .AsQueryable()
                .Where(x => x.UserId == userId)
                .ToListAsync();

        public async Task AddAsync(Models.Course course)
            => await Collection.InsertOneAsync(course);

        public async Task PublishCourse(Guid courseId)
        {
            var serializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };
            var bson = new BsonDocument() { { "$set", BsonDocument.Parse(JsonConvert.SerializeObject(new { IsCoursePublished = true }, serializerSettings)) } };

           await Collection.UpdateOneAsync(Builders<Models.Course>.Filter.Eq("Id", courseId), bson);
        }

        private IMongoCollection<Models.Course> Collection 
            => _database.GetCollection<Models.Course>("Courses");
    }
}