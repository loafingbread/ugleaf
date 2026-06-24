namespace GameLogic.TurnBasedTests;

using GameLogic.Combat.TurnBased;
using GameLogic.Entities.Characters;
using GameLogic.Entities.Skills;
using GameLogic.Registry;
using GameLogic.Tests;
using Xunit;

public class CombatTest : IClassFixture<CharacterTestFixture>
{
    private readonly CharacterTestFixture _fixture;
    private readonly CharacterTemplateFactory _factory = new();

    public CombatTest(CharacterTestFixture fixture)
    {
        _fixture = fixture;
    }

    private Character MakeCharacter(CharacterTemplateData data)
    {
        CharacterTemplate template = _factory.Create(ReferenceId.New(), data);
        return new Character(template, new CharacterInstanceData { TemplateId = data.Id });
    }

    [Fact]
    public void Combat_StartsInCombatStartPhase()
    {
        Character ash = MakeCharacter(_fixture.AshRecord);
        Character goblin = MakeCharacter(_fixture.GoblinRecord);
        Combat combat = new([ash], [goblin]);

        Assert.Equal(EPhase.CombatStart, combat.State.Phase);
    }

    [Fact]
    public void Play_AdvancesToAwaitPlayerSelectCommand_AfterThreeSteps()
    {
        Character ash = MakeCharacter(_fixture.AshRecord);
        Character goblin = MakeCharacter(_fixture.GoblinRecord);
        Combat combat = new([ash], [goblin]);

        combat.Play(); // CombatStart → TurnStart
        combat.Play(); // TurnStart → PlayerTurn
        combat.Play(); // PlayerTurn → AwaitPlayerSelectCommand

        Assert.Equal(EPhase.AwaitPlayerSelectCommand, combat.State.Phase);
    }

    [Fact]
    public void PlayerSelectCommand_ExecutesIgnite_AppliesStatusToTarget()
    {
        Character ash = MakeCharacter(_fixture.AshRecord);
        Character goblin = MakeCharacter(_fixture.GoblinRecord);
        Combat combat = new([ash], [goblin]);

        combat.Play(); // CombatStart → TurnStart
        combat.Play(); // TurnStart → PlayerTurn
        combat.Play(); // PlayerTurn → AwaitPlayerSelectCommand

        Skill ignite = ash.Skills[0];
        combat.PlayerSelectCommand(ignite, [goblin]); // → ExecuteCommand
        combat.Play(); // ExecuteCommand → applies Burn → TurnEnd

        Assert.Single(goblin.ActiveStatusEffects);
        Assert.Equal("BurnStatusVariant", goblin.ActiveStatusEffects[0].VariantName);
    }

    [Fact]
    public void TurnEnd_RemovesDefeatedCharacter_TransitionsToCombatEnd()
    {
        Character ash = MakeCharacter(_fixture.AshRecord);
        Character goblin = MakeCharacter(_fixture.GoblinRecord);
        goblin.Stats.ModifyResourceStat("resource_stat_health", -9999);
        Combat combat = new([ash], [goblin]);

        combat.Play(); // CombatStart → TurnStart
        combat.Play(); // TurnStart → PlayerTurn
        combat.Play(); // PlayerTurn → AwaitPlayerSelectCommand
        combat.PlayerSelectCommand(ash.Skills[0], [goblin]); // → ExecuteCommand
        combat.Play(); // ExecuteCommand → TurnEnd
        combat.Play(); // TurnEnd → removes goblin → CombatEnd

        Assert.True(combat.IsCombatOver());
    }
}
