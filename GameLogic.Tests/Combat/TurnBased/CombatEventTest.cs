namespace GameLogic.TurnBasedTests;

using GameLogic.Combat.TurnBased;
using GameLogic.Entities.Characters;
using GameLogic.Events;
using GameLogic.Events.Categories;
using GameLogic.Registry;
using GameLogic.Tests;
using Xunit;

public class CombatEventTest : IClassFixture<CharacterTestFixture>
{
    private readonly CharacterTestFixture _fixture;
    private readonly CharacterTemplateFactory _factory = new();

    public CombatEventTest(CharacterTestFixture fixture)
    {
        _fixture = fixture;
    }

    private Character MakeCharacter(CharacterTemplateData data)
    {
        CharacterTemplate template = _factory.Create(ReferenceId.New(), data);
        return new Character(template, new CharacterInstanceData { TemplateId = data.Id });
    }

    [Fact]
    public void Play_CombatStart_PublishesCombatPhaseChangeEvent()
    {
        Character ash = MakeCharacter(_fixture.AshRecord);
        Character goblin = MakeCharacter(_fixture.GoblinRecord);
        Combat combat = new([ash], [goblin]);

        CombatPhaseChangeEvent? received = null;
        combat.State.EventBus.Subscribe<CombatPhaseChangeEvent, BuiltInEventCategory>(
            e => { received = e; return Task.CompletedTask; });

        combat.Play(); // CombatStart → TurnStart

        Assert.NotNull(received);
        Assert.Equal("CombatStart", received.Action);
        Assert.Equal(EPhase.TurnStart.ToString(), received.Phase);
    }

    [Fact]
    public void PlayerSelectCommand_PublishesSkillUseEvent()
    {
        Character ash = MakeCharacter(_fixture.AshRecord);
        Character goblin = MakeCharacter(_fixture.GoblinRecord);
        Combat combat = new([ash], [goblin]);

        SkillUseEvent? received = null;
        combat.State.EventBus.Subscribe<SkillUseEvent, BuiltInEventCategory>(
            e => { received = e; return Task.CompletedTask; });

        combat.Play(); // CombatStart → TurnStart
        combat.Play(); // TurnStart → PlayerTurn
        combat.Play(); // PlayerTurn → AwaitPlayerSelectCommand
        combat.PlayerSelectCommand(ash.Skills[0], [goblin]);
        combat.Play(); // ExecuteCommand → TurnEnd

        Assert.NotNull(received);
        Assert.Equal("Ignite", received.Skill);
        Assert.Equal("Ash", received.User);
    }

    [Fact]
    public void ExecuteCommand_PublishesCombatPhaseChangeEvent_WithTargets()
    {
        Character ash = MakeCharacter(_fixture.AshRecord);
        Character goblin = MakeCharacter(_fixture.GoblinRecord);
        Combat combat = new([ash], [goblin]);

        CombatPhaseChangeEvent? received = null;
        combat.State.EventBus.Subscribe<CombatPhaseChangeEvent, BuiltInEventCategory>(
            e => { received = e; return Task.CompletedTask; });

        combat.Play(); // CombatStart → TurnStart
        combat.Play(); // TurnStart → PlayerTurn
        combat.Play(); // PlayerTurn → AwaitPlayerSelectCommand
        combat.PlayerSelectCommand(ash.Skills[0], [goblin]);
        combat.Play(); // ExecuteCommand → TurnEnd (last phase change event wins)

        Assert.NotNull(received);
        Assert.Equal("ExecuteCommand", received.Action);
        Assert.Single(received.Targets);
        Assert.Equal("Goblin", received.Targets[0].Name);
    }
}
