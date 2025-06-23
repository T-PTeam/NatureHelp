using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

[Table("obj_attachments")]
public class Attachment : BaseModel
{
    public string FileName { get; set; } = null!;

    public long FileSize { get; set; }

    public string MimeType { get; set; } = null!;

    public string StoragePath { get; set; } = null!;

    public string PreviewUrl { get; set; } = null!;
}