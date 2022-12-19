using GraphQLLearning.DataLoaders;
using GraphQLLearning.Models;

namespace GraphQLLearning.Schema.Queries
{
    public class CourseType: ISearchResultType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Subject Subject { get; set; }
        public IEnumerable<StudentType> Students { get; set; }

        [IsProjected(true)]
        public Guid InstructorId { get; set; }

        [GraphQLNonNullType]
        public async Task<InstructorType> Instructor([Service] InstructorDataLoader instructorDataLoader)
        {
            var instructor = await instructorDataLoader.LoadAsync(InstructorId, CancellationToken.None);
            if (instructor == null) return null;
            return new InstructorType
            {
                Id = instructor.Id,
                FirstName = instructor.FirstName,
                LastName = instructor.LastName,
                Salary = instructor.Salary
            };
        }
    }
}
