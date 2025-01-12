using Domain.Enums;
using Domain.Models.Organization;

namespace Domain.Models.Analitycs;
public class Report : BaseEntity
{
    public string? Title { get; set; }
    public EReportTopic Topic { get; set; }
    public string Data { get; set; } = null!;
    public User Reporter { get; set; } = null!;
}
