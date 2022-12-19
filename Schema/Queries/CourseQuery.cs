using GraphQLLearning.Services.Courses;
using GraphQLLearning.Services;

namespace GraphQLLearning.Schema.Queries
{
    [ExtendObjectType(typeof(Query))]
    public class CourseQuery
    {
        private readonly ICoursesRepository _coursesRepository;
        public CourseQuery(ICoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }

        public async Task<IEnumerable<CourseType>> GetCourses()
        {
            var courses = await _coursesRepository.GetAll();
            return courses.Select(c => new CourseType
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId
            });
        }

        [UseDbContext(typeof(SchoolDbContext))]
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)]
        [UseProjection]
        [UseFiltering]
        public IQueryable<CourseType> GetPaginatedCourses([ScopedService] SchoolDbContext context)
        {
            return context.Courses.Select(c => new CourseType
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId
            });
        }
        public async Task<CourseType?> GetCourse(Guid id)
        {
            var course = await _coursesRepository.GetById(id);
            if (course == null)
            {
                return null;
            }
            return new CourseType
            {
                Id = course.Id,
                Name = course.Name,
                Subject = course.Subject,
                InstructorId = course.InstructorId
            };
        }
    }
}
