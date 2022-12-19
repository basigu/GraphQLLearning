using FluentValidation;

namespace GraphQLLearning.Schema.Mutations.Validators
{
    public class CourseInputValidator : AbstractValidator<CourseInput>
    {
        public CourseInputValidator()
        {
            RuleFor(c => c.Name)
                .MinimumLength(3)
                .MaximumLength(50)
                .WithMessage("Course name must be between 3 and 50 characters long")
                .WithErrorCode("COURSE_NAME_LENGTH");
        }
    }
}
