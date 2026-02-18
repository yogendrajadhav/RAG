namespace RAGbackend.Models;

public class SourceDocument
{
  public string FileName { get; set; } = string.Empty;
  public string PageNumber { get; set; }
  public string Content { get; set; } = string.Empty;
  public double Score { get; set; }
}