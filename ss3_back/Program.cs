using ss3_back.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR(); //LD Adding SignalR to the builder

var app = builder.Build();


app.UseCors(options =>
{
    options.WithOrigins("http://localhost:3000") //LD react is running localhost:3000
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials(); //LD should solve credentials errors
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<BitcoinHub>("/bitcoinHub"); //LD Map the SignalR hub endpoint

app.Run();
