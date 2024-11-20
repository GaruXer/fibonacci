using Leonardo;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/fibonacci", async () => await Fibonacci.RunAsync(new []{"40", "12"}));

app.Run();
