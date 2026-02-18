  using System.Diagnostics;
using RAGbackend.Models;

namespace RAGbackend.Services;

public class RagService : IRagService
{
  private readonly IPdfProcessingService _pdfProcessor;
  private readonly IVectorStoreService _vectorStore;
  private readonly ILlmService _llmService;
  public RagService(IPdfProcessingService pdfProcessor, IVectorStoreService vectorStore, ILlmService llmService)
  {
    _pdfProcessor = pdfProcessor;
    _vectorStore = vectorStore;
    _llmService = llmService;
  }

  public async Task<UploadResponse> ProcessAndStoreDocumentAsync(Stream pdfStream, string fileName)
  {
    try
    {
      // Extract text chunks from PDF
      var chunks = await _pdfProcessor.ProcessPdfAsync( fileName, pdfStream);
      if (!chunks.Any())
      {
        return new UploadResponse
        {
          Success = false,
          Message = "No text content found in the PDF."
        };
      } // Generate embeddings for each chunk
      foreach (var chunk in chunks)
      {
        chunk.Embedding = await
          _llmService.GenerateEmbeddingAsync(chunk.Content);
      } // Store in vector database
      var success = await _vectorStore.UpsertChunksAsync(chunks);
      return new UploadResponse
      {
        Success = success,
        Message = success ? "Document processed successfully" : "Failed to store document",
        DocumentId = chunks.First().DocumentId,
        ChunksProcessed = chunks.Count
      };
    } catch(Exception ex)
    {
      return new UploadResponse
      {
        Success = false,
        Message = $"Error processing document: {ex.Message}"
      };
    }
  }
  public async Task<QueryResponse> QueryAsync(string question, int topK = 5)
  {
    var stopwatch = Stopwatch.StartNew();
    try
    {
      // Generate embedding for the question
      var questionEmbedding = await _llmService.GenerateEmbeddingAsync(question);
      // Search for relevant chunks
      var relevantChunks = await _vectorStore.SearchAsync(questionEmbedding, topK);
      if (!relevantChunks.Any())
      {
        return new QueryResponse
        {
          Answer = "I don't have any relevant information to answer this question.",
          SourceDocuments = new List<SourceDocument>(),
          ProcessingTimeMs = stopwatch.ElapsedMilliseconds
        };
      } // Build context from relevant chunks
      var context = string.Join("\n\n", relevantChunks.Select(c => $"[Source: {c.FileName}, Page {c.PageNumber}]\n{c.Content}"));
      // Generate response using LLM
      var answer = await _llmService.GenerateResponseAsync(question, context);
      stopwatch.Stop();
      return new QueryResponse
      {
        Answer = answer,
        SourceDocuments = relevantChunks.Select(c => new SourceDocument
        {
          FileName = c.FileName,
          PageNumber = c.PageNumber,
          Content = c.Content.Substring(0, Math.Min(200, c.Content.Length))
                    + "...",
          Score = 0 // Qdrant returns score but we simplified
        }).ToList(),
        ProcessingTimeMs = stopwatch.ElapsedMilliseconds
      };
    } catch(Exception ex)
    {
      stopwatch.Stop();
      return new QueryResponse
      {
        Answer = $"Error processing query: {ex.Message}",
        SourceDocuments = new List<SourceDocument>(),
        ProcessingTimeMs = stopwatch.ElapsedMilliseconds
      };
    }
  }
}