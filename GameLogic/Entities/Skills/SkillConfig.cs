namespace GameLogic.Entities.Skills;

using System.Diagnostics.CodeAnalysis;
using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables;

public interface ISkillRecord
{
    public string Id { get; }
    public string Name { get; }
    public TargeterRecord? Targeter { get; }
    public List<UsableTemplateData> Usables { get; }
}

public record SkillRecord : ISkillRecord
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public TargeterRecord? Targeter { get; init; }
    public List<UsableTemplateData> Usables { get; init; } = new();
}

public class SkillConfig
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public TargeterConfig? Targeter { get; init; }
    public List<UsableTemplate> Usables { get; init; } = new();

    private static readonly UsableTemplateFactory _usableFactory = new();

    [SetsRequiredMembers]
    public SkillConfig(ISkillRecord record)
    {
        this.Id = record.Id;
        this.Name = record.Name;

        if (record.Targeter != null)
            this.Targeter = new TargeterConfig(record.Targeter);

        foreach (UsableTemplateData usableData in record.Usables)
            this.Usables.Add(_usableFactory.Create(ReferenceId.New(), usableData));
    }
}
