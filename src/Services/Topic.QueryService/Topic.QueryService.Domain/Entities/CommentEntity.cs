using System.ComponentModel.DataAnnotations.Schema;

namespace Topic.QueryService.Domain.Entities;

[Table("Comments", Schema = "public")]
public class CommentEntity
{
    public Guid Id { get; set; }
    public Guid TopicId { get; set; }
    public string MessageText { get; set; } = null!;
    public string AuthorName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public bool IsTextEdited { get; set; }
}