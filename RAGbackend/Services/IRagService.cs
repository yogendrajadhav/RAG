using RAGbackend.Models;

namespace RAGbackend.Services;

public interface IRagService
{
  Task<UploadResponse> ProcessAndStoreDocumentAsync(Stream pdfStream, string fileName);
  Task<QueryResponse> QueryAsync(string question, int topK = 5);
}