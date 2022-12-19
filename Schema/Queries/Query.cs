using GraphQLLearning.Services;
using GraphQLLearning.Services.Courses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GraphQLLearning.Schema.Queries
{
    public class Query
    {
        private readonly ICoursesRepository _coursesRepository;
        public Query(ICoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }
        
        [UseDbContext(typeof(SchoolDbContext))]
        public async Task<IEnumerable<ISearchResultType>> Search(string term, [ScopedService] SchoolDbContext context)
        {
            var courses = await context.Courses
                .Where(c => c.Name.Contains(term))
                .Select(c => new CourseType
                {
                    Id = c.Id,
                    Name = c.Name,
                    Subject = c.Subject,
                    InstructorId = c.InstructorId
                }).ToListAsync();

            var instructors = await context.Instructors
                .Where(i => i.FirstName.Contains(term) || i.LastName.Contains(term))
                .Select(i => new InstructorType
            {
                Id = i.Id,
                FirstName = i.FirstName,
                LastName = i.LastName,
                Salary = i.Salary
            }).ToListAsync();

            var searchResults = new List<ISearchResultType>();
            searchResults.AddRange(courses);
            searchResults.AddRange(instructors);
            return searchResults;
        }

        [GraphQLDeprecated("The query is deprecated.")]
        public string Instructions => "Smash that like button and subscribe!";
    }
}
