namespace MemoryTests;

using FluentAssertions;
using Oml.Memory;

public class Dto
{
    public string Label { get; set; }
    public string Label2 { get; set; }
}

public class DtoFields
{
    public string Label;
    public string Label2;
}

public class OptimizeStringMemory
{
    [Fact]
    public void OptimizeFullCollection()
    {
        var collection = GetCollection();

        bool isSameValue = Object.ReferenceEquals(collection[0].Label, collection[1].Label);
        isSameValue.Should().BeFalse();

        GC.Collect();
        GC.WaitForPendingFinalizers();

        long memory = GC.GetTotalMemory(false);

        var hash = new HashSet<string>();

        collection.OptimizeStringMemory(hash);

        GC.Collect();
        GC.WaitForPendingFinalizers();
        long memory2 = GC.GetTotalMemory(false);
        long optimisation = memory / memory2;
        memory2.Should().BeLessThan((long)(memory * 0.2));
    }

    [Fact]
    public void OptimizeSelectedPropertiesOnCollection()
    {
        var collection = GetCollection();

        bool isSameValue = Object.ReferenceEquals(collection[0].Label, collection[1].Label);
        isSameValue.Should().BeFalse();

        GC.Collect();
        GC.WaitForPendingFinalizers();

        long memory = GC.GetTotalMemory(false);

        var hash = new HashSet<string>();

        collection.OptimizeStringMemory(hash, p => p.Label);

        GC.Collect();
        GC.WaitForPendingFinalizers();
        long memory2 = GC.GetTotalMemory(false);
        long optimisation = memory / memory2;
        memory2.Should().BeLessThan((long)(memory * 0.6));
    }

    [Fact]
    public void OptimizeSelectedFieldOnCollection()
    {

        List<DtoFields> collection = new List<DtoFields>(1_000_000);
        for (int i = 0; i < 1_000_000; i++)
        {
            char[] txt = "0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789".ToCharArray();
            collection.Add(new DtoFields()
            {
                Label = new string(txt) + new string(txt),
                Label2 = new string(txt) + new string(txt),
            });
        }

        bool isSameValue = Object.ReferenceEquals(collection[0].Label, collection[1].Label);
        isSameValue.Should().BeFalse();

        GC.Collect();
        GC.WaitForPendingFinalizers();

        long memory = GC.GetTotalMemory(false);

        var hash = new HashSet<string>();

        collection.OptimizeStringMemory(hash, p => p.Label);

        GC.Collect();
        GC.WaitForPendingFinalizers();
        long memory2 = GC.GetTotalMemory(false);
        long optimisation = memory / memory2;
        memory2.Should().BeLessThan((long)(memory * 0.6));
    }

    [Fact]
    public void OptimizeObjectWithNullProperty()
    {
        Dto dto = new Dto()
        {
            Label = null,
            Label2 = "data is here",
        };


        var hash = new HashSet<string>();

        dto.InternalizeStringProperties(hash);
    }

    private IReadOnlyList<Dto> GetCollection()
    {
        List<Dto> collection = new List<Dto>(1_000_000);
        for (int i = 0; i < 1_000_000; i++)
        {
           

            char[] txt = "0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789".ToCharArray();
            collection.Add(new Dto()
            {
                Label = new string(txt) + new string(txt),
                Label2 = new string(txt) + new string(txt),
            });
        }

        return collection;
    }
}