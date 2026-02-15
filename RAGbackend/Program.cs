
using Microsoft.AspNetCore.Http.Features;
using RAGbackend.Services;

namespace RAGbackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
           
            //Register Services
            builder.Services.AddScoped<IPdfProcessingService, PdfProcessingService>();
            builder.Services.AddScoped<IVectorStoreService, QdrantVectorStoreService>();
            builder.Services.AddScoped<ILlmService, OllamaService>();
            builder.Services.AddScoped<IRagService, RagService>();

            //Configure CORS to allow requests from Angular frontend
            builder.Services.AddCors(options=>options
                .AddPolicy("AllowAngular",
                    policy=>policy.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()));

            // Configure file upload size   
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 100 * 1024 * 1024; // 100 MB
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Enable CORS
            app.UseCors("AllowAngular");

            app.UseHttpsRedirection();

            app.UseAuthorization();
            
            app.MapControllers();

            app.Run();
        }
    }
}
