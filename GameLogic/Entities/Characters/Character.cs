namespace GameLogic.Entities.Characters;

using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;
using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Utils;

public class CharacterTemplate : IDeepCopyable<CharacterTemplate>
{
    public ReferenceId ReferenceId { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<string> Tags { get; set; } = new();
    public StatBlock Stats { get; set; }
    public List<Skill> Skills { get; set; } = new();

    public CharacterTemplate(CharacterData data)
    {
        this.ReferenceId = data.ReferenceId;

        this.Name = data.Name;
        this.Description = data.Description;
        this.Tags = [.. data.Tags];

        this.Stats = StatFactory.CreateStatBlockFromRecord(data);
        this.Skills = data.Skills.Select(skillData => new Skill(skillData)).ToList();
    }

    // Copy constructor
    public CharacterTemplate(CharacterTemplate template)
    {
        this.ReferenceId = Ids.NewReferenceId();

        this.Name = template.Name;
        this.Description = template.Description;
        this.Tags = [.. template.Tags];

        this.Stats = template.Stats.DeepCopy();
        this.Skills = template.Skills.DeepCopyList();
    }

    public Character Instantiate()
    {
        return CharacterFactory.CreateCharacterFromTemplate(this);
    }

    public CharacterTemplate DeepCopy()
    {
        return new CharacterTemplate(this);
    }
}

public class Character : CharacterTemplate, IDeepCopyable<Character>
{
    public Character(CharacterData data)
        : base(data) { }

    public Character(Character character)
        : base((character as CharacterTemplate).DeepCopy()) { }

    public Character(CharacterTemplate template)
        : base(template) { }

    public new Character DeepCopy()
    {
        return new Character(this);
    }
}
