namespace GameLogic.Tests.Registry;

using GameLogic.Registry;
using Xunit;

public class TemplateSpecTest
{
    [Fact]
    public void Inline_HasValue_NoPatch_NoDependency()
    {
        var spec = new TemplateSpec<string, string>
        {
            Kind = ETemplateKind.Inline,
            Value = "my-spec",
        };

        Assert.Equal(ETemplateKind.Inline, spec.Kind);
        Assert.Equal("my-spec", spec.Value);
        Assert.Null(spec.Patch);
        Assert.Null(spec.DependencyId);
    }

    [Fact]
    public void Ref_HasDependencyId_NoValueOrPatch()
    {
        var id = ReferenceId.From("fireball");
        var spec = new TemplateSpec<string, string>
        {
            Kind = ETemplateKind.Ref,
            DependencyId = id,
        };

        Assert.Equal(ETemplateKind.Ref, spec.Kind);
        Assert.Equal(id, spec.DependencyId);
        Assert.Null(spec.Value);
        Assert.Null(spec.Patch);
    }

    [Fact]
    public void Override_HasDependencyIdAndPatch_NoValue()
    {
        var id = ReferenceId.From("fireball");
        var spec = new TemplateSpec<string, string>
        {
            Kind = ETemplateKind.Override,
            DependencyId = id,
            Patch = "my-patch",
        };

        Assert.Equal(ETemplateKind.Override, spec.Kind);
        Assert.Equal(id, spec.DependencyId);
        Assert.Equal("my-patch", spec.Patch);
        Assert.Null(spec.Value);
    }
}
