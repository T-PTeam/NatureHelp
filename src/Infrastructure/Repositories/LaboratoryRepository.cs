using Domain.Models.Organization;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class LaboratoryRepository : BaseRepository<Laboratory>, ILaboratoryRepository
{
    public LaboratoryRepository(IDbContextFactory<ApplicationContext> contextFactory)
        : base(contextFactory) { }
}
