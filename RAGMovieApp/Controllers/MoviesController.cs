using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using OllamaSharp.Models.Chat;
using Qdrant.Client;
using RAGMovieApp.Models;
using ChatRole = Microsoft.Extensions.AI.ChatRole;

namespace RAGMovieApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
  private static readonly Uri OllamaEndpoint = new("http://localhost:11434");
  private static Uri qdrantEndpoint = new("http://localhost:6334");
  private static string chatModelId = "qwen2.5:3b";
  private static string embeddingModelId = "nomic-embed-text";
  private static string qdrantCollectionName = "movies";
  private readonly IChatClient _chatClient = new OllamaChatClient(OllamaEndpoint, chatModelId);

  private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator =
    new OllamaEmbeddingGenerator(OllamaEndpoint, embeddingModelId);

  private readonly QdrantClient _qdrantClient = new(qdrantEndpoint);
  private readonly QdrantVectorStore _vectorStore;
  private IVectorStoreRecordCollection<Guid, Movie> _movies;

  public MoviesController()
  {
    _vectorStore = new QdrantVectorStore(_qdrantClient,
      new QdrantVectorStoreOptions { EmbeddingGenerator = _embeddingGenerator });
    InitializeVectorStoreAsync().GetAwaiter().GetResult();
  }

  private async Task InitializeVectorStoreAsync()
  {
    _movies = _vectorStore.GetCollection<Guid, Movie>(qdrantCollectionName);
    if (_movies == null)
    {
      var movieData = MovieData.GetMovies();
      var collections = await _qdrantClient.ListCollectionsAsync();
      var collectionExists = collections.Any(c => c == qdrantCollectionName);
      if (!collectionExists)
      {
        await _movies.CreateCollectionIfNotExistsAsync();
        foreach (var movie in movieData)
        {
          movie.DescriptionEmbedding = await _embeddingGenerator.GenerateVectorAsync(movie.Description);
          await _movies.UpsertAsync(movie);
        }
      }
    }
  }

  [HttpGet("search")]
  public async Task<IActionResult> Search([FromQuery] string query, [FromQuery] int topK = 5)
  {
    var queryEmbedding = await _embeddingGenerator.GenerateVectorAsync(query);
    // var results = await _vectorStore.GetNearestMatchesAsync<Guid, Movie>(qdrantCollectionName, queryEmbedding, topK);
    // return Ok(results.Select(r => r.Metadata));
    var results = _movies.SearchEmbeddingAsync(queryEmbedding, topK, new VectorSearchOptions<Movie>()
    {
      VectorProperty = movie => movie.DescriptionEmbedding
    });
    var searchResult = new HashSet<string>();
    await foreach (var result in results) searchResult.Add($"[{result.Record.Title}]: {result.Record.Description}");
    var context = string.Join(Environment.NewLine, searchResult);

    var prompt =
      $"Context:{context} based on context above, please answer the following question. If context doesn't provide the answer, say you don't know based on provided information. User Question:{query} Answer:";
    var systemMessage = new ChatMessage(ChatRole.System, "You are a movie assistant.");
    var userMessage = new ChatMessage(ChatRole.User, prompt);
    var chatMessages = new List<ChatMessage> { systemMessage, userMessage };
    var response = _chatClient.GetStreamingResponseAsync(chatMessages);
    var answer = "";
    await foreach (var message in response)
    {
      answer += message.Text;
    }
    return Ok(answer);
  }
}