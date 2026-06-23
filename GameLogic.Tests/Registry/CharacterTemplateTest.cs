namespace GameLogic.Tests.Registry;

using GameLogic.Config;
using GameLogic.Entities.Characters;
using GameLogic.Registry;
using Xunit;

public class CharacterTemplateTest
{
    private readonly GameRegistry _registry = new();
    private readonly CharacterTemplateFactory _factory = new();

    [Fact]
    public void CharacterTemplate_LoadRegisterRetrieve()
    {
        CharacterTemplateData data = JsonConfigLoader.LoadFromFile<CharacterTemplateData>(
            ConfigPaths.Character.Ash
        );
        CharacterTemplate template = _factory.Create(ReferenceId.From("char-ash"), data);
        _registry.Characters.TryAdd(template.Id, template);

        bool found = _registry.Characters.TryGet(template.Id, out CharacterTemplate? retrieved);

        Assert.True(found);
        Assert.NotNull(retrieved);
        Assert.Equal("char_pc_ash", retrieved!.ToData().Id);
        Assert.Single(retrieved.Skills);
    }

    [Fact]
    public void CharacterTemplate_Skills_HaveCorrectUsables()
    {
        CharacterTemplateData data = JsonConfigLoader.LoadFromFile<CharacterTemplateData>(
            ConfigPaths.Character.Ash
        );
        CharacterTemplate template = _factory.Create(ReferenceId.From("char-ash"), data);

        Assert.Equal("skill_ignite", template.Skills[0].ToData().Id);
        Assert.Single(template.Skills[0].Usables);
        Assert.Equal("usable_ignite", template.Skills[0].Usables[0].ToData().Id);
    }

    [Fact]
    public void CharacterTemplate_ToData_RoundTrips()
    {
        CharacterTemplateData original = JsonConfigLoader.LoadFromFile<CharacterTemplateData>(
            ConfigPaths.Character.Ash
        );
        CharacterTemplate template = _factory.Create(ReferenceId.From("char-ash"), original);

        CharacterTemplateData roundTripped = template.ToData();

        Assert.Equal(original.Id, roundTripped.Id);
        Assert.Equal(original.Name, roundTripped.Name);
        Assert.Equal(original.Stats.Count, roundTripped.Stats.Count);
        Assert.Equal(original.Skills.Count, roundTripped.Skills.Count);
    }
}
