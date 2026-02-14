using Qdrant.Client.Grpc;
using RAGbackend.Models;

namespace RAGbackend.Services
{
  public interface ILlmService
    {
        Task<float[]> GenerateEmbeddingAsync(string text);
        Task<string> GenerateResponseAsync(string prompt, string context);
    }
}
