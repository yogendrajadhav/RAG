using RAGbackend.Models;

namespace RAGbackend.Services;

public class OllamaService : ILlmService
{
  private readonly HttpClient _httpClient;
  private readonly string _embeddingApiUrl;
  private readonly string _embeddingModel;
  private readonly string _chatModel;

  public OllamaService() 
  {
    _httpClient = new HttpClient();
    _embeddingApiUrl = "localhost:11434"; // Placeholder URL
    _embeddingModel = "nomic-embed-text"; // Placeholder model name
    _chatModel = "llama3"; // Placeholder model name
  }
  public async Task<float[]> GenerateEmbeddingAsync(string text)
  {
    // Placeholder for embedding generation logic
    // In a real implementation, this would call an external API or use a local model to generate embeddings
    var request= new
    {
      model = _embeddingModel,
      prompt = text 
    };
    var content= new StringContent(System.Text.Json.JsonSerializer.Serialize(request), System.Text.Encoding.UTF8, "application/json");
    var response = await _httpClient.PostAsync($"{_embeddingApiUrl}/api/embeddings", content);

    response.EnsureSuccessStatusCode();
    var jsonResponse = await response.Content.ReadAsStringAsync();
    var result = System.Text.Json.JsonSerializer.Deserialize<OllamaEmbeddingResponse>(jsonResponse);

    // Use the deserialized embedding if available, otherwise return a default array
    return result?.Embedding?.Select(d => (float)d).ToArray() ?? Array.Empty<float>();// Example embedding size
  }

  public async Task<string> GenerateResponseAsync(string prompt, string context)
  {
    // Placeholder for response generation logic
    // In a real implementation, this would call an external API or use a local model to generate a response based on the prompt and context
    var fullPrompt = $@"Answer the question based on the following context. If the answer cannot be found in the context, say 'I don't have enough information to answer this question.'
                                Context: {context}
                                Question: {prompt}
                                Answer:";

    var request = new
    {
      model = _chatModel,
      prompt = fullPrompt,
      stream = false 
    };
    var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(request), System.Text.Encoding.UTF8, "application/json");
    var response = await _httpClient.PostAsync($"{_embeddingApiUrl}/api/generate", content);
    response.EnsureSuccessStatusCode();
    var jsonResponse = await response.Content.ReadAsStringAsync();
    var result = System.Text.Json.JsonSerializer.Deserialize<OllamaGenerateResponse>(jsonResponse);
    return result?.Response ?? "This is a generated response based on the provided prompt and context.";
  }
}