using LowCortisol.Platform.API.Shared.Domain.Model;

namespace LowCortisol.Platform.API.Support.Domain.Model.Entities;

public class KnowledgeArticle : IEntity, IAuditableEntity
{
    private KnowledgeArticle()
    {
    }

    public KnowledgeArticle(Guid id, string title, string summary, string category, int helpfulCount, string content)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Article title is required.", nameof(title));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Title = title.Trim();
        Summary = summary?.Trim() ?? string.Empty;
        Category = string.IsNullOrWhiteSpace(category) ? "General" : category.Trim();
        HelpfulCount = Math.Max(0, helpfulCount);
        Content = content?.Trim() ?? string.Empty;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Summary { get; private set; } = string.Empty;
    public string Category { get; private set; } = "General";
    public int HelpfulCount { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
}
