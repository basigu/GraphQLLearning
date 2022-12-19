using GraphQLLearning.DTOs;
using GraphQLLearning.Services.Instructors;

namespace GraphQLLearning.DataLoaders
{
    public class InstructorDataLoader : BatchDataLoader<Guid, InstructorDTO>
    {
        private readonly IInstructorRepository _instructorRepository;

        public InstructorDataLoader(
            IInstructorRepository instructorRepository,
            IBatchScheduler batchScheduler,
            DataLoaderOptions options) : base(batchScheduler, options)
        {
            _instructorRepository = instructorRepository;
        }

        protected override async Task<IReadOnlyDictionary<Guid, InstructorDTO>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            var instructors = await _instructorRepository.GetManyByIds(keys);
            return instructors.ToDictionary(x => x.Id);
        }
    }
}
