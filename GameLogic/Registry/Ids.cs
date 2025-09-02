namespace GameLogic.Registry;

public readonly record struct TemplateId(string value)
{
    public override string ToString() => value;
}

public readonly record struct InstanceId(string value)
{
    public override string ToString() => value;
}

public static class Ids
{
    public static TemplateId Template(string value) => new(value);

    public static InstanceId Instance() => new(System.Guid.NewGuid().ToString());
}
