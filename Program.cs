using System.Diagnostics;
using System.Text;
using LuaRules;
using NLua;

var foos =
    Enumerable
        .Range(0, 100000)
        .Select(i => new Foo
        {
            Name = (i % 100 == 0 ? null : $"Name {i}")!,
            UniqueCustomerId = $"id_{i.ToString().PadLeft(5, '0')}",
            Id = i
        })
        .ToList();

using (var lua = new Lua())
{
    lua.State.Encoding = Encoding.UTF8;
    lua.LoadCLRPackage ();
    lua.DoString(@"import ('LuaRules', 'LuaRules')");
    lua.DoFile("Rules.lua");
    var validateFoo = lua["ValidateFoo"] as LuaFunction;
    var errors = new List<string>(10000);

    var stopwatch = new Stopwatch();
    stopwatch.Start();

    foos.ForEach(foo =>
    {
        // ReSharper disable once SuspiciousTypeConversion.Global
        var table = (LuaTable)validateFoo!.Call(foo).First();
        errors.AddRange(table.Values.Cast<string>());
    });
    
    stopwatch.Stop();
    Console.WriteLine($"Execution time: {stopwatch.Elapsed.TotalSeconds}");
    Console.WriteLine($"Found {errors.Count} errors");

    var transformFoo = lua["Transform"] as LuaFunction;
    var transformedFoo = (TransformedFoo)transformFoo.Call(foos.Skip(1).First()).First();
}

Console.WriteLine("Execution finished");
Console.ReadKey();