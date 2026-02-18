using RAGbackend.Models;
using UglyToad.PdfPig;

namespace RAGbackend.Services;

public class PdfProcessingService : IPdfProcessingService
{
  int chunkSize = 1000; // Number of characters per chunk
  int chunkOverlap = 200; // Number of overlapping characters between chunks

  public async Task<List<DocumentChunk>> ProcessPdfAsync(string fileName, Stream pdfStream)
  {
    var chunks = new List<DocumentChunk>();
    var documentId = Guid.NewGuid().ToString();
    // Extract text from PDF (using a hypothetical PDF text extraction library)
    using (var document = PdfDocument.Open(pdfStream))
    {
      for (int pageNumber = 1; pageNumber <= document.NumberOfPages; pageNumber++)
      {
        var page = document.GetPage(pageNumber);
        var text = page.Text;

        if (string.IsNullOrWhiteSpace(text))
        {
          continue;
        }

        var pageChunks = CreateChunks(text, documentId, fileName, pageNumber.ToString());
        chunks.AddRange(pageChunks);
      }
    }

    return await Task.FromResult(chunks);
  }

  private List<DocumentChunk> CreateChunks(string text, string documentId, string fileName, string pageNumber)
  {
    var chunks = new List<DocumentChunk>();
    var words = text.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
    for (int i = 0; i < words.Length; i += chunkSize - chunkOverlap)
    {
      var chunkWords = words.Skip(i).Take(chunkSize).ToArray();
      var chunkText = string.Join(" ", chunkWords);
      //var chunkIndex = i / (chunkSize - chunkOverlap);
      chunks.Add(new DocumentChunk
      {
        Id = Guid.NewGuid().ToString(),
        DocumentId = documentId,
        FileName = fileName,
        Content = chunkText,
        PageNumber = pageNumber,
        ChunkIndex = chunks.Count.ToString(),
        CreatedAt = DateTime.UtcNow
      });
    }
    return chunks;
  }
}