using GraphQLLearning.DTOs;

namespace GraphQLLearning.Services.Courses
{
    public interface ICoursesRepository
    {
        Task<CourseDTO> Create(CourseDTO course);
        Task<bool> Delete(Guid id);
        Task<CourseDTO> Update(CourseDTO course);
        Task<IEnumerable<CourseDTO>> GetAll();
        Task<CourseDTO?> GetById(Guid courseId);
    }
}