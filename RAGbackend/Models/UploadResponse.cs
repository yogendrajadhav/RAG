namespace RAGbackend.Models;

public class UploadResponse
{
  public bool Success { get; set; }
  public string Message { get; set; } = string.Empty;
  public string DocumentId { get; set; } = string.Empty;
  public int ChunksProcessed { get; set; }
}