namespace GameLogic.Tests;

using GameLogic.Entities.Characters;
using GameLogic.Entities.Stats;
using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables.Effects;
using GameLogic.Usables.Effects.Results;
using GameLogic.Usables.Effects.Variants;
using Xunit;

public class CharacterCombatTest : IClassFixture<CharacterTestFixture>
{
    private readonly CharacterTestFixture _fixture;
    private readonly CharacterTemplateFactory _factory = new();

    public CharacterCombatTest(CharacterTestFixture fixture)
    {
        this._fixture = fixture;
    }

    private Character MakeCharacter(CharacterTemplateData data, EFaction faction = EFaction.Player)
    {
        CharacterTemplate template = _factory.Create(ReferenceId.New(), data);
        var character = new Character(template, new CharacterInstanceData { TemplateId = data.Id });
        character.Faction = faction;
        return character;
    }

    [Fact]
    public void AttackEffectResult_Apply_ReducesTargetHealth()
    {
        Character goblin = MakeCharacter(_fixture.GoblinRecord, EFaction.Enemy);
        int startHp = (goblin.Stats.GetStat("resource_stat_health", StatType.Resource) as ResourceStat)!.CurrentValue;
        var variant = new AttackEffectVariant(new EffectTemplateData { Type = "Attack", Subtype = "SingleHit", Value = 10f });
        var result = new AttackEffectResult(variant, goblin, 10f, false);

        result.Apply();

        int finalHp = (goblin.Stats.GetStat("resource_stat_health", StatType.Resource) as ResourceStat)!.CurrentValue;
        Assert.Equal(startHp - 10, finalHp);
    }

    [Fact]
    public void HealEffectResult_Apply_IncreasesTargetHealth()
    {
        Character goblin = MakeCharacter(_fixture.GoblinRecord, EFaction.Enemy);
        goblin.Stats.ModifyResourceStat("resource_stat_health", -30);
        int damagedHp = (goblin.Stats.GetStat("resource_stat_health", StatType.Resource) as ResourceStat)!.CurrentValue;
        var variant = new HealEffectVariant(new EffectTemplateData { Type = "Heal", Subtype = "Direct", Value = 10f });
        var result = new HealEffectResult(variant, goblin, 10f);

        result.Apply();

        int finalHp = (goblin.Stats.GetStat("resource_stat_health", StatType.Resource) as ResourceStat)!.CurrentValue;
        Assert.Equal(damagedHp + 10, finalHp);
    }

    [Fact]
    public void StatusEffectResult_Apply_AddsToActiveStatusEffects()
    {
        Character goblin = MakeCharacter(_fixture.GoblinRecord, EFaction.Enemy);
        var variant = new BurnStatusVariant(new EffectTemplateData { Type = "Status", Subtype = "Burn", Value = 5f, Duration = 3f });
        var result = new StatusEffectResult(variant, goblin, 5f, 3f);

        result.Apply();

        Assert.Single(goblin.ActiveStatusEffects);
        Assert.Equal(5f, goblin.ActiveStatusEffects[0].Value);
        Assert.Equal(3f, goblin.ActiveStatusEffects[0].Duration);
    }

    [Fact]
    public void AddStatusEffect_AddsToList()
    {
        Character goblin = MakeCharacter(_fixture.GoblinRecord, EFaction.Enemy);

        goblin.AddStatusEffect(new ActiveStatusEffect
        {
            VariantName = "BurnStatusVariant",
            Value = 5f,
            Duration = 3f,
            RemainingTurns = 3f,
        });

        Assert.Single(goblin.ActiveStatusEffects);
        Assert.Equal("BurnStatusVariant", goblin.ActiveStatusEffects[0].VariantName);
    }

    [Fact]
    public void Character_Faction_DefaultsToNeutral()
    {
        CharacterTemplate template = _factory.Create(ReferenceId.New(), _fixture.GoblinRecord);
        var goblin = new Character(template, new CharacterInstanceData { TemplateId = _fixture.GoblinRecord.Id });

        Assert.Equal(EFaction.Neutral, goblin.Faction);
    }
}
