namespace RAGbackend.Models;

public class DocumentChunk
{
  public string Id { get; set; } = Guid.NewGuid().ToString();
  public string DocumentId { get; set; } = string.Empty;
  public string FileName { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;
  public string PageNumber { get; set; } =String.Empty;
  public string ChunkIndex { get; set; } =string.Empty;
  public float[] Embedding { get; set; } = Array.Empty<float>();
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}