using RAGbackend.Models;

namespace RAGbackend.Services;

public interface IVectorStoreService
{
  Task InitializeAsync();
  Task<bool> UpsertChunksAsync(List<DocumentChunk> chunks);
  Task<List<DocumentChunk>> SearchAsync(float[] queryEmbedding, int topK = 5);
  Task<bool> DeleteDocumentAsync(string documentId);
}