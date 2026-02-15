//using Microsoft.Extensions.VectorData;

using Microsoft.Extensions.VectorData;

namespace RAGMovieApp.Models
{
  public class Movie
  {
    [VectorStoreRecordKey]
    public Guid Key { get; set; } = Guid.NewGuid();
    [VectorStoreRecordData]
    public string Title { get; set; } = string.Empty;

    [VectorStoreRecordData] 
    public string Reference { get; set; } = string.Empty;

    [VectorStoreRecordData]
    public string Description { get; set; } = string.Empty;

    [VectorStoreRecordVector(768,DistanceFunction=DistanceFunction.CosineSimilarity)]
    public ReadOnlyMemory<float> DescriptionEmbedding { get; set; } = Array.Empty<float>();
  }
}
