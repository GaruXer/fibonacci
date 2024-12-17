using Microsoft.EntityFrameworkCore;

namespace Leonardo;

public record FibonacciResult(int Input, long Result);

public class Fibonacci
{
    private readonly FibonacciDataContext _context;

    public Fibonacci(FibonacciDataContext context)
    {
        _context = context;
    }
    
    private int Run(int n)
    {
        if (n < 2) return n;

        return Run(n - 1) + Run(n - 2);
    }
    
    public async Task<List<FibonacciResult>> RunAsync(string[] strings)
    {
        var tasks = new List<Task<FibonacciResult>>();

        foreach (var input in strings)
        {
            var int32 = Convert.ToInt32(input);
            var fibo = await _context.TFibonaccis.Where(f => f.FibInput == int32).FirstOrDefaultAsync();

            if (fibo != null)
            {
                var t = Task.Run(() =>
                {
                    return new FibonacciResult(fibo.FibInput, fibo.FibOutput);
                });
                
                tasks.Add(t);
            }
            else
            {
                var r = Task.Run(() =>
                {
                    var result = Run(int32);
                    return new FibonacciResult(int32, result);
                });
    
                tasks.Add(r);
            }
        }
        
        var results = new List<FibonacciResult>();
        
        foreach (var task in tasks)
        {
            var r = await task;
            var fibo = await _context.TFibonaccis.Where(f => f.FibInput == r.Input).FirstOrDefaultAsync();

            if (fibo == null)
            {
                _context.TFibonaccis.Add(new TFibonacci()
                {
                    FibInput = r.Input,
                    FibOutput = r.Result,
                });
            }

            results.Add(r);
        }

        await _context.SaveChangesAsync();
        
        return results;
    }
}