using Microsoft.Extensions.AI;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using RAGbackend.Models;
using System.Numerics;
using QueryResponse = RAGbackend.Models.QueryResponse;

namespace RAGbackend.Services;

public class QdrantVectorStoreService : IVectorStoreService
{
  private readonly QdrantClient _client;
  string collectionName = "documents";
  int VectorSize = 768; // Example vector size, should match the embedding size used

  public QdrantVectorStoreService()
  {
    _client = new QdrantClient("localhost",6334);
  }

  public async Task InitializeAsync()
  {
   
   try
   {
     // 1. Connect to Qdrant
     var qdrant = new QdrantClient("localhost", 6334); // gRPC port

     // 2. Create collection (if not exists)
     await qdrant.CreateCollectionAsync(collectionName, new VectorParams
     {
       Size = (ulong)VectorSize, // embedding size (text-embedding-3-small = 1536)
       Distance = Distance.Cosine
     });

     var collections = await _client.ListCollectionsAsync();
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
          ["documentId"] = chunk.DocumentId,
          ["fileName"] = chunk.FileName,
          ["content"] = chunk.Content,
          ["pageNumber"] = chunk.PageNumber,
          ["chunkIndex"] = chunk.ChunkIndex,
          ["createdAt"] = chunk.CreatedAt.ToString("o")
        }
      }).ToList();

      await _client.UpsertAsync(collectionName, points);
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
      var searchResult = await _client.SearchAsync(collectionName, queryEmbedding, limit: (ulong)topK);
      var chunks = searchResult.Select(result => new DocumentChunk
      {
        Id = result.Id.Uuid,
        DocumentId = result.Payload["documentId"].ToString(),
        FileName = result.Payload["fileName"].ToString(),
        Content = result.Payload["content"].ToString(),
        PageNumber = int.Parse(result.Payload["pageNumber"].ToString()),
        ChunkIndex = int.Parse(result.Payload["chunkIndex"].ToString()),
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
      await _client.DeleteAsync(collectionName, new Filter
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

