var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole();
// Add services to the container.
builder.Services.AddControllers();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
