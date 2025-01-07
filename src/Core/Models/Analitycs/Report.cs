using Core.Enums;
using Core.Models.Organization;

namespace Core.Models.Analitycs;
public class Report
{
    public string? Title { get; set; }
    public EReportTopic Topic { get; set; }
    public string Data { get; set; } = null!;
    public User Reporter { get; set; } = null!;
}
