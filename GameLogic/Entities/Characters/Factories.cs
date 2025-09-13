namespace GameLogic.Entities.Characters;

using System.Linq;
using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;
using GameLogic.Registry;
using GameLogic.Utils;

public static class CharacterFactory
{
    public static Reference<CharacterTemplate, Character> CreateCharacterFromReference(
        ReferenceUnionSpec record
    )
    {
        switch (record.Metadata.Kind)
        {
            case EReferenceKind.Ref:
                return CreateCharacterTemplateFromReference(record);
            case EReferenceKind.Inline:
                return CreateCharacterTemplateFromInline(record);
            case EReferenceKind.Override:
                return CreateCharacterTemplateFromOverride(record);
            case EReferenceKind.Instance:
                return CreateInstanceFromReference(record);
        }
        throw new NotImplementedException();
    }

    public static Reference<CharacterTemplate, Character> CreateCharacterTemplateFromReference(
        ReferenceUnionSpec record
    )
    {
        var refSpec = record as RefSpec;
        if (refSpec is null)
        {
            throw new InvalidOperationException("Ref spec is not a character template");
        }

        return new Reference<CharacterTemplate, Character>(
            refSpec.Metadata,
            new CharacterTemplate(refSpec.Metadata, null, null),
            null
        );
    }

    public static Reference<CharacterTemplate, Character> CreateCharacterTemplateFromInline(
        ReferenceUnionSpec record
    )
    {
        var inlineSpec = record as InlineSpec<CharacterTemplateRecord>;
        if (inlineSpec is null)
        {
            throw new InvalidOperationException("Inline spec is not a character template");
        }

        return new Reference<CharacterTemplate, Character>(
            inlineSpec.Metadata,
            new CharacterTemplate(inlineSpec.Metadata, inlineSpec.Template, null),
            null
        );
    }

    public static Reference<CharacterTemplate, Character> CreateCharacterTemplateFromOverride(
        ReferenceUnionSpec record
    )
    {
        var overrideSpec = record as OverrideSpec<CharacterTemplateRecord, CharacterOverrideRecord>;
        if (overrideSpec is null)
        {
            throw new InvalidOperationException("Override spec is not a character template");
        }

        return new Reference<CharacterTemplate, Character>(
            overrideSpec.Metadata,
            new CharacterTemplate(overrideSpec.Metadata, null, overrideSpec.Override),
            null
        );
    }

    public static Reference<CharacterTemplate, Character> CreateInstanceFromReference(
        ReferenceUnionSpec record
    )
    {
        var instanceSpec = record as InstanceSpec<CharacterTemplateRecord, CharacterRecord>;
        if (instanceSpec is null)
        {
            throw new InvalidOperationException("Instance spec is not a character template");
        }

        return new Reference<CharacterTemplate, Character>(
            instanceSpec.Metadata,
            null,
            new Character(instanceSpec.Metadata, instanceSpec.InstanceId, instanceSpec.Instance)
        );
    }

    // public static Character CreateCharacterFromRecord(CharacterRecord record)
    // {
    //     return new Character(
    //         GameLogic.Registry.Ids.Instance(record.InstanceId),
    //         record.TemplateIdentifier,
    //         record.Name,
    //         record.Description,
    //         record.Tags,
    //         StatFactory.CreateStatBlockFromRecord(record),
    //         record
    //             .Skills.Select((SkillRecord skill) => SkillFactory.CreateSkillFromRecord(skill))
    //             .ToList()
    //     );
    // }

    public static Character CreateCharacterFromTemplate(CharacterTemplate template)
    {
        return template.Instantiate();
    }

    // public static CharacterTemplate CreateCharacterTemplateFromRecord(
    //     CharacterTemplateRecord record
    // )
    // {
    //     return new CharacterTemplate(
    //         record.TemplateIdentifier,
    //         record.Name,
    //         record.Description,
    //         record.Tags,
    //         StatFactory.CreateStatBlockFromRecord(record),
    //         record.Skills.Select(SkillFactory.CreateSkillFromRecord).ToList()
    //     );
    // }
}
