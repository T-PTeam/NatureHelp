using Domain.Enums;
using Domain.Models.Organization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Analitycs;
public class Report : BaseModel
{
    public string? Title { get; set; }
    public EReportTopic Topic { get; set; }
    public string Data { get; set; } = null!;
    public User Reporter { get; set; } = null!;

    [ForeignKey(nameof(Reporter))]
    public Guid ReporterId { get; set; }
}
