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

    public static InstanceId Instance(string? value = null)
    {
        if (value is null)
        {
            return new(System.Guid.NewGuid().ToString());
        }
        else if (value == "")
        {
            return new(System.Guid.NewGuid().ToString());
        }

        return new(value);
    }
}
