using GraphQLLearning.Models;
using GraphQLLearning.Schema.Queries;

namespace GraphQLLearning.Schema.Mutations
{
    public class CourseInput
    {
        public string Name { get; set; }
        public Subject Subject { get; set; }
        public Guid InstructorId { get; set; }
    }
}