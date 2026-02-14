using System.Runtime.InteropServices.ComTypes;
using RAGbackend.Models;

namespace RAGbackend.Services
{
    public interface IPdfProcessingService
    {
        Task<List<DocumentChunk>> ProcessPdfAsync(string fileName, Stream pdfStream);
    }
}
    