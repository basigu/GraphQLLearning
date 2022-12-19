using GraphQLLearning.Models;

namespace GraphQLLearning.DTOs
{
    public class CourseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Subject Subject { get; set; }

        public Guid InstructorId { get; set; }
        public IEnumerable<StudentDTO> Students { get; set; }
        public InstructorDTO Instructor { get; set; }
    }
}
