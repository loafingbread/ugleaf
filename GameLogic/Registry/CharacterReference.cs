namespace GameLogic.Registry;

using GameLogic.Entities.Characters;
using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;

public class CharacterReference : ReferenceBase<CharacterTemplate, CharacterData>
{
    public CharacterReference(IRegistry registry, ReferenceSpec spec, CharacterTemplate? value)
        : base(registry, spec, value) { }

    protected override CharacterData InitData()
    {
        return new CharacterData(this.Spec);
    }

    public override void ResolveDependencies(IRegistry registry)
    {
        this.Data.Resolve(
            this.Spec,
            this.GetCharacterData(registry),
            this.GetStatData(registry),
            this.GetSkillData(registry)
        );
    }

    public override void Initialize()
    {
        if (this.Data is null)
        {
            throw new InvalidOperationException(
                "Character data should be resolved before initializing the reference"
            );
        }

        Character character = new Character(this.Data);
        this.Registry.TryAdd(
            (IReference<object, ReferenceSpec>)this,
            this.Spec.Metadata.ReferenceId
        );
    }

    protected Func<ReferenceId?, CharacterData> GetCharacterData(IRegistry registry)
    {
        return (ReferenceId? referenceId) =>
        {
            var characterRef =
                registry.GetReference(referenceId) as IReference<CharacterTemplate, CharacterData>
                ?? throw new InvalidOperationException("Character reference not found");

            characterRef.Resolve(registry);
            return characterRef.GetData();
        };
    }

    protected Func<ReferenceId?, StatData> GetStatData(IRegistry registry)
    {
        return (ReferenceId? referenceId) =>
        {
            var statRef =
                registry.GetReference(referenceId) as IReference<Stat, StatData>
                ?? throw new InvalidOperationException("Stat reference not found");

            statRef.Resolve(registry);
            return statRef.GetData();
        };
    }

    protected Func<ReferenceId?, SkillData> GetSkillData(IRegistry registry)
    {
        return (ReferenceId? referenceId) =>
        {
            var skillRef =
                registry.GetReference(referenceId) as IReference<SkillTemplate, SkillData>
                ?? throw new InvalidOperationException("Skill reference not found");

            skillRef.Resolve(registry);
            return skillRef.GetData();
        };
    }
}
