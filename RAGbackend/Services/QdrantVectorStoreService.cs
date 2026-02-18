using Microsoft.Extensions.AI;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using RAGbackend.Models;
using System.Numerics;
using QueryResponse = RAGbackend.Models.QueryResponse;

namespace RAGbackend.Services;

public class QdrantVectorStoreService : IVectorStoreService
{
  private readonly QdrantClient _qdrantClient;
  string collectionName = "documents";
  int VectorSize = 768; // Example vector size, should match the embedding size used

  public QdrantVectorStoreService()
  {
    _qdrantClient = new QdrantClient(new Uri("http://localhost:6334"));// gRPC port
  }

  public async Task InitializeAsync()
  {
   
   try
   {
     // 1. Connect to Qdrant
   //  var _qdrantClient = new QdrantClient("localhost", 6334); 

      // 2. Create collection (if not exists)
      var isCollectionExists= await _qdrantClient.CollectionExistsAsync(collectionName);
      if (!isCollectionExists)
      {
        await _qdrantClient.CreateCollectionAsync(collectionName, new VectorParams
        {
          Size = (ulong)VectorSize, // embedding size (text-embedding-3-small = 1536)
          Distance = Distance.Cosine
        });
      }

     var collections = await _qdrantClient.ListCollectionsAsync();
     foreach (var collection in collections)
     {
       Console.WriteLine($"Collection: {collection}");
     }
   }
   catch (Exception ex)
   {
     Console.WriteLine($"Error initializing Qdrant collection: {ex.Message}");
     throw;
   }
  }
  
  public async Task<bool> UpsertChunksAsync(List<DocumentChunk> chunks)
  {
    try
    {
      var points = chunks.Select(chunk => new PointStruct
      {
        Id = new PointId { Uuid = chunk.Id },
        Vectors = chunk.Embedding,
        Payload =
        {
          ["documentId"] = chunk.DocumentId.ToString(),
          ["fileName"] = chunk.FileName.ToString(),
          ["content"] = chunk.Content.ToString(),
          ["pageNumber"] = chunk.PageNumber.ToString(),
          ["chunkIndex"] = chunk.ChunkIndex.ToString(),
          ["createdAt"] = chunk.CreatedAt.ToString("o")
        }
      }).ToList();

      await _qdrantClient.UpsertAsync(collectionName, points);
      return true;
    }
    catch (Exception exception)
    {
      Console.WriteLine(exception);
      return false;
    }
  }

  public async Task<List<DocumentChunk>> SearchAsync(float[] queryEmbedding, int topK = 5)
  {
    try
    {
      var searchResult = await _qdrantClient.SearchAsync(collectionName, queryEmbedding, limit: (ulong)topK);
      var chunks = searchResult.Select(result => new DocumentChunk
      {
        Id = result.Id.Uuid,
        DocumentId = result.Payload["documentId"].ToString(),
        FileName = result.Payload["fileName"].ToString(),
        Content = result.Payload["content"].ToString(),
        PageNumber = result.Payload["pageNumber"].ToString(),
        ChunkIndex = result.Payload["chunkIndex"].ToString(),
      //  CreatedAt = DateTime.Parse(result.Payload["createdAt"].ToString())
      }).ToList();
      return chunks;
    }
    catch (Exception exception)
    {
      Console.WriteLine(exception);
      return new List<DocumentChunk>();
    }
  }

  public async Task<bool> DeleteDocumentAsync(string documentId)
  {
    try
    {
      await _qdrantClient.DeleteAsync(collectionName, new Filter
      {
        Must =
        {
          new Condition
          {
            Field = new FieldCondition
            {
              Key = "documentId",
              Match = new Match { Keyword = documentId }
            }
          }
        }
      });
      return true;
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }
}

