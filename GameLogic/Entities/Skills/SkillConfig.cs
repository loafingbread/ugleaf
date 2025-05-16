namespace GameLogic.Entities.Skills;

using System.Diagnostics.CodeAnalysis;
using GameLogic.Targeting;
using GameLogic.Usables;

public interface ISkillRecord
{
    public string Id { get; }
    public string Name { get; }
    public TargeterRecord? Targeter { get; }
    public UsableRecord? Usable { get; }
}

public record SkillRecord : ISkillRecord
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public TargeterRecord? Targeter { get; init; }
    public UsableRecord? Usable { get; init; }
}

public class SkillConfig
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public TargeterConfig? Targeter { get; init; }
    public UsableConfig? Usable { get; init; }

    [SetsRequiredMembers]
    public SkillConfig(ISkillRecord record)
    {
        this.Id = record.Id;
        this.Name = record.Name;

        if (record.Targeter != null)
        {
            this.Targeter = new TargeterConfig(record.Targeter);
        }

        if (record.Usable != null)
        {
            this.Usable = new UsableConfig(record.Usable);
        }
    }
}
