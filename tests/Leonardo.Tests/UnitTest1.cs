using Microsoft.EntityFrameworkCore;

namespace Leonardo.Tests;

public class UnitTest1
{
    [Fact]
    public async void Test1()
    {
        var builder = new DbContextOptionsBuilder<FibonacciDataContext>(); 
        var dataBaseName = Guid.NewGuid().ToString(); 
        builder.UseInMemoryDatabase(dataBaseName);  
        var options = builder.Options; 
        var context = new FibonacciDataContext(options); 
        await context.Database.EnsureCreatedAsync();
        
        var results = await new Fibonacci(context).RunAsync(new string[] {"42"});
        Assert.Equal(42, results[0].Input);
        Assert.Equal(267914296, results[0].Result);
    }
}