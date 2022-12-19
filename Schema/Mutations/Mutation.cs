using AppAny.HotChocolate.FluentValidation;
using GraphQLLearning.DTOs;
using GraphQLLearning.Schema.Mutations.Validators;
using GraphQLLearning.Services.Courses;

namespace GraphQLLearning.Schema.Mutations
{
    public class Mutation
    {
        private readonly ICoursesRepository _coursesRepository;
       

        public Mutation(ICoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }

        public async Task<CourseResult> CreateCourseAsync([UseFluentValidation, UseValidator<CourseInputValidator>()] CourseInput input)
        {
            CourseDTO course = new()
            {
                Id = Guid.NewGuid(),
                Name = input.Name,
                Subject = input.Subject,
                InstructorId = input.InstructorId,

            };
            course = await _coursesRepository.Create(course);
            return new CourseResult
            {
                Id = course.Id,
                Name = course.Name,
                InstructorId = course.InstructorId,
                Subject = course.Subject
            };

        }

        public CourseResult UpdateCourse(Guid id, CourseInput input)
        {
            CourseDTO course = new()
            {
                Id = id,
                Name = input.Name,
                Subject = input.Subject,
                InstructorId = input.InstructorId,

            };
            course = _coursesRepository.Update(course).Result;
            return new CourseResult
            {
                Id = course.Id,
                Name = course.Name,
                InstructorId = course.InstructorId,
                Subject = course.Subject
            };
        }

        public async Task<bool> DeleteCourse(Guid id)
        {
            try
            {
                return await _coursesRepository.Delete(id);
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
