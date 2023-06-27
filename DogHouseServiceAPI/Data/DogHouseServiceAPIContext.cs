using Microsoft.EntityFrameworkCore;

namespace DogHouseServiceAPI.Data
{
    public class DogHouseServiceAPIContext : DbContext
    {
        public virtual DbSet<DogHouseServiceAPI.Models.Dog> Dog { get; set; } = default!;

        public DogHouseServiceAPIContext(DbContextOptions<DogHouseServiceAPIContext> options)
            : base(options)
        {
        }
        public DogHouseServiceAPIContext()
        {
        }
    }
}
