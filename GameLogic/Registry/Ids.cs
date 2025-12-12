namespace GameLogic.Registry;

public readonly record struct TemplateId(string value)
{
    public override string ToString() => value;
}

public readonly record struct InstanceId(string value)
{
    public override string ToString() => value;
}

public readonly record struct ReferenceId(string value)
{
    public override string ToString() => value;
}

public static class Ids
{
    public static TemplateId Template(string value) => new(value);

    public static ReferenceId Reference(string value) => new(value);
    public static ReferenceId NewReferenceId() => new(System.Guid.NewGuid().ToString());

    public static TemplateId NewTemplateId() => new(System.Guid.NewGuid().ToString());

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

public enum ETemplateType
{
    Character,
    Item,
    Skill,
    Usable,
    Effect,
    Stat,
}

public enum ETemplateSubType
{
    None,
    StatValue,
    StatResource,
}
