using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RAGbackend.Models;
using RAGbackend.Services;

namespace RAGbackend.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class DocumentController : ControllerBase
  {
    private readonly IRagService _ragService;
    private readonly IVectorStoreService _vectorStoreService;

    public DocumentController(IRagService ragService, IVectorStoreService vectorStoreService)
    {
      _ragService = ragService;
      _vectorStoreService = vectorStoreService;
      // Ensure the vector store is initialized (e.g., collection created) when the controller is instantiated
      _vectorStoreService.InitializeAsync().Wait();
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadDocument(IFormFile file)
    {
      if (file == null || file.Length == 0)
      {
        return BadRequest(new UploadResponse{Success = false,Message = "No file uploaded."});
      }

      if (!file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
      {
        return BadRequest(new UploadResponse{Success = false,Message = "Only PDF files are supported."});
      }

      try
      {
        await using var stream = file.OpenReadStream();
        var response = await _ragService.ProcessAndStoreDocumentAsync(stream, file.FileName);
        if (response.Success)
        {
          return Ok(response);
        }
        else
        {
          return StatusCode(StatusCodes.Status500InternalServerError, new UploadResponse{Success = false,Message = response.Message});
        }
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new UploadResponse{Success = false,Message = $"Error processing document: {ex.Message}"});
      }
    }

    [HttpPost("query")]
    public async Task<IActionResult> Query([FromBody] QueryRequest request)
    {
      if (string.IsNullOrWhiteSpace(request.Question))
      {
        return BadRequest("Question cannot be empty.");
      }

      try
      {
        var response = await _ragService.QueryAsync(request.Question, request.TopK);
        return Ok(response);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
        return StatusCode(StatusCodes.Status500InternalServerError,new {message="Error processing query." });
      }
    }

    [HttpDelete("{documentId}")]
    public async Task<ActionResult> DeleteDocument(string documentId) 
    { 
      var success = await _vectorStoreService.DeleteDocumentAsync(documentId);
      if (success)
        return Ok(new { message = "Document deleted successfully" });
      return NotFound(new { message = "Document not found or deletion failed" });
    }
  }
}
