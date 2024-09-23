using EmojiApp.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddSignalR(); // Ajout de la librairie SignalR
builder.Services.AddControllers();  // Ajout des contrôleurs

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(b =>
    {
        b.AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed((host) => true)
            .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors();
app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();  // Permet de mapper les contrôleurs

app.UseCors("CorsPolicy");

app.MapHub<EmojiHub>("/emojiHub"); // Permet de mapper les requêtes vers SignalR

app.MapFallbackToFile("/index.html");

app.Run();
