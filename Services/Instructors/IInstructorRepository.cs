using GraphQLLearning.DTOs;

namespace GraphQLLearning.Services.Instructors
{
    public interface IInstructorRepository
    {
        Task<InstructorDTO?> GetById(Guid instructorId);

        Task<IEnumerable<InstructorDTO>> GetManyByIds(IReadOnlyList<Guid> instructorIds);
    }
}