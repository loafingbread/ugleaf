namespace GameLogic.EventsTests;

using GameLogic.Config;
using GameLogic.Entities.Characters;
using GameLogic.Tests;
using GameLogic.Events;
using Xunit;

class EventBusTestData
{
    private readonly CharacterTestFixture _characters;

    public Character Alice;
    public Character Brock;
    public Events.Categories.SkillUseEvent FireballSkillUseEvent;
    public Events.Categories.SkillEvent? ReceivedFireballSkillEvent { get; set; }

    public Events.Categories.CombatPhaseChangeEvent StartCombatPhaseChangedEvent;
    public Events.Categories.CombatEvent? ReceivedCombatPhaseChangedEvent { get; set; }

    public EventBusTestData(CharacterTestFixture characters)
    {
        this._characters = characters;
        this.Alice = GameFactory.CreateFromConfig<Character, ICharacterData>(
            this._characters.AliceConfig
        );
        this.Brock = GameFactory.CreateFromConfig<Character, ICharacterData>(
            this._characters.BrockConfig
        );

        this.FireballSkillUseEvent = new("Fireball", this.Alice.Name);
        this.StartCombatPhaseChangedEvent = new(
            this.Alice,
            this.Alice,
            [this.Brock],
            "Skill use",
            "Player turn"
        );
    }
}

public class EventBusTest : IClassFixture<CharacterTestFixture>
{
    private readonly CharacterTestFixture _characters;

    public EventBusTest(CharacterTestFixture characters)
    {
        this._characters = characters;
    }

    [Fact]
    public void Publish_CallsTwoDifferentEventCategories()
    {
        EventBusTestData td = new EventBusTestData(this._characters);
        EventBus eventBus = new EventBus();

        eventBus.Subscribe<Events.Categories.SkillEvent, BuiltInEventCategory>(
            (Events.Categories.SkillEvent receivedEvent) =>
            {
                td.ReceivedFireballSkillEvent = receivedEvent;
                return Task.CompletedTask;
            }
        );
        eventBus.Subscribe<Events.Categories.CombatEvent, BuiltInEventCategory>(
            (Events.Categories.CombatEvent receivedEvent) =>
            {
                td.ReceivedCombatPhaseChangedEvent = receivedEvent;
                return Task.CompletedTask;
            }
        );

        eventBus.Publish<Events.Categories.SkillEvent, BuiltInEventCategory>(
            td.FireballSkillUseEvent
        );
        eventBus.Publish<Events.Categories.CombatEvent, BuiltInEventCategory>(
            td.StartCombatPhaseChangedEvent
        );

        Assert.Equal(td.FireballSkillUseEvent, td.ReceivedFireballSkillEvent);
        Assert.Equal(td.StartCombatPhaseChangedEvent, td.ReceivedCombatPhaseChangedEvent);
    }

    [Fact]
    public void Publish_DoNotCallHandlerAfterUnsubscribe()
    {
        EventBusTestData td = new EventBusTestData(this._characters);
        EventBus eventBus = new EventBus();
        bool handlerWasCalled = false;
        Func<Events.Categories.SkillEvent, Task> handler = (
            Events.Categories.SkillEvent receivedEvent
        ) =>
        {
            handlerWasCalled = true;
            return Task.CompletedTask;
        };

        eventBus.Subscribe<Events.Categories.SkillEvent, BuiltInEventCategory>(handler);
        eventBus.Unsubscribe<Events.Categories.SkillEvent, BuiltInEventCategory>(handler);
        eventBus.Publish<Events.Categories.SkillEvent, BuiltInEventCategory>(
            td.FireballSkillUseEvent
        );

        Assert.False(handlerWasCalled);
    }
}
