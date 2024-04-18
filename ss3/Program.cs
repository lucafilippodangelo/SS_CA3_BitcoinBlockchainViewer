using ss3.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR(); //LD Adding SignalR to the builder

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
        //LD below default to home controller. Commenting
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapHub<BitcoinHub>("/bitcoinHub"); //LD Map the SignalR hub endpoint

app.Run();
