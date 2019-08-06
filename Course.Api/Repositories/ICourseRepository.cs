using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Course.Api.Repositories
{
    public interface ICourseRepository
    {
        Task<Models.Course> GetAsync(Guid id);
        Task<IEnumerable<Models.Course>> BrowseAsync(Guid userId);
        Task AddAsync(Models.Course course);
        Task PublishCourse(Guid courseId);
    }
}