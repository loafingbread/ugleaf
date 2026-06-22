namespace GameLogic.Entities.Characters;

using System.Diagnostics.CodeAnalysis;
using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;
using GameLogic.Registry;

public interface ICharacterRecord
{
    public string Id { get; }
    public string Name { get; }

    public int Health { get; }
    public int Attack { get; }
    public int Defense { get; }
    public List<StatRecord> Stats { get; }
    public List<SkillTemplateData> Skills { get; }
}

public record CharacterRecord : ICharacterRecord, IStatBlockRecord
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public int Health { get; init; }
    public int Attack { get; init; }
    public int Defense { get; init; }
    public List<StatRecord> Stats { get; init; } = new();
    public List<SkillTemplateData> Skills { get; init; } = new();
}

public class CharacterConfig : IStatBlockRecord
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public int Health { get; init; }
    public int Attack { get; init; }
    public int Defense { get; init; }
    public List<StatRecord> Stats { get; init; } = new();
    public List<Skill> Skills { get; init; } = new();

    private static readonly SkillTemplateFactory _skillFactory = new();

    [SetsRequiredMembers]
    public CharacterConfig(ICharacterRecord record)
    {
        this.Id = record.Id;
        this.Name = record.Name;
        this.Health = record.Health;
        this.Attack = record.Attack;
        this.Defense = record.Defense;

        this.Stats = record.Stats;

        foreach (SkillTemplateData skillData in record.Skills)
        {
            SkillTemplate template = _skillFactory.Create(ReferenceId.New(), skillData);
            this.Skills.Add(new Skill(template, new SkillInstanceData { TemplateId = skillData.Id }));
        }
    }
}
