namespace GameLogic.Registry;

public enum ETemplateKind
{
    Inline,    // full definition embedded here
    Ref,       // reference to an existing template by id
    Override,  // reference to a base template + a patch applied on top
}

/// <summary>
/// Generic wrapper for how a nested template is specified in JSON.
/// Used anywhere a parent entity embeds or references a child entity.
/// </summary>
public record TemplateSpec<TSpec, TPatch>
{
    public required ETemplateKind Kind { get; init; }

    /// <summary>Present when Kind = Inline. The full spec embedded here.</summary>
    public TSpec? Value { get; init; }

    /// <summary>Present when Kind = Override. Fields to apply on top of the base.</summary>
    public TPatch? Patch { get; init; }

    /// <summary>Present when Kind = Ref or Override. Id of the referenced template.</summary>
    public ReferenceId? DependencyId { get; init; }
}
