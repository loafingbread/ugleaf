namespace GameLogic.Tests.Registry;

using GameLogic.Registry;
using Xunit;

public class RegistryTest
{
    [Fact]
    public void TryAdd_AddsEntry()
    {
        var registry = new Registry<string>();
        var id = ReferenceId.From("test");

        bool added = registry.TryAdd(id, "hello");

        Assert.True(added);
        Assert.True(registry.Contains(id));
    }

    [Fact]
    public void TryAdd_ReturnsFalse_WhenDuplicateId()
    {
        var registry = new Registry<string>();
        var id = ReferenceId.From("test");
        registry.TryAdd(id, "first");

        bool added = registry.TryAdd(id, "second");

        Assert.False(added);
        Assert.Equal("first", registry.Get(id));
    }

    [Fact]
    public void TryGet_ReturnsValue_WhenFound()
    {
        var registry = new Registry<string>();
        var id = ReferenceId.From("test");
        registry.TryAdd(id, "hello");

        bool found = registry.TryGet(id, out string? value);

        Assert.True(found);
        Assert.Equal("hello", value);
    }

    [Fact]
    public void TryGet_ReturnsFalse_WhenNotFound()
    {
        var registry = new Registry<string>();
        var id = ReferenceId.From("missing");

        bool found = registry.TryGet(id, out string? value);

        Assert.False(found);
        Assert.Null(value);
    }

    [Fact]
    public void Get_Throws_WhenNotFound()
    {
        var registry = new Registry<string>();
        var id = ReferenceId.From("missing");

        Assert.Throws<KeyNotFoundException>(() => registry.Get(id));
    }

    [Fact]
    public void ReferenceId_Equality_ByValue()
    {
        var a = ReferenceId.From("fireball");
        var b = ReferenceId.From("fireball");

        Assert.Equal(a, b);
    }

    [Fact]
    public void ReferenceId_New_IsUnique()
    {
        var a = ReferenceId.New();
        var b = ReferenceId.New();

        Assert.NotEqual(a, b);
    }
}
