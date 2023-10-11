using Persistance.Contexts;

namespace Persistance.Seeders
{
    public class SeedDB
    {
        private readonly ApplicationDbContext _context;

        public SeedDB(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            //await CheckParameters();
            //await CheckImages();
        }
    }
}
