using GraphQLLearning.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GraphQLLearning.Services.Instructors
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly IDbContextFactory<SchoolDbContext> _contextFactory;

        public InstructorRepository(IDbContextFactory<SchoolDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<InstructorDTO?> GetById(Guid instructorId)
        {
            using SchoolDbContext context = _contextFactory.CreateDbContext();
            return await context.Instructors
               .FirstOrDefaultAsync(_ => _.Id == instructorId);
        }

        public async Task<IEnumerable<InstructorDTO>> GetManyByIds(IReadOnlyList<Guid> instructorIds)
        {
            using SchoolDbContext context = _contextFactory.CreateDbContext();
            return await context.Instructors
                .Where(i => instructorIds.Contains(i.Id))
                .ToListAsync();
        }
    }
}
