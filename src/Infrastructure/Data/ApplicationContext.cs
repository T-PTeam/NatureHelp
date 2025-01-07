using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;
public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        :base(options) { }
}
