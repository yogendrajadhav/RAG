namespace RAGbackend.Models;

public class QueryResponse
{
  public string Answer { get; set; } = string.Empty;
  public List<SourceDocument> SourceDocuments { get; set; } = new();
  public double ProcessingTimeMs { get; set; }
}