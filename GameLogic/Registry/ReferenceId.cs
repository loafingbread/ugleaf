namespace GameLogic.Registry;

public readonly record struct ReferenceId(string Value)
{
    public override string ToString() => Value;

    public static ReferenceId New() => new(Guid.NewGuid().ToString());
    public static ReferenceId From(string value) => new(value);
}
