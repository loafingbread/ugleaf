namespace GameLogic.EventsTests;

using GameLogic.Config;
using GameLogic.Entities.Characters;
using GameLogic.Events;
using GameLogic.Tests;
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
        Character? AliceInstance = CharacterFactory
            .CreateCharacterReferenceFromRecord(this._characters.AliceRecord)
            .Instance;
        if (AliceInstance is null)
        {
            throw new InvalidOperationException("Alice instance is null");
        }
        this.Alice = AliceInstance;

        Character? BrockInstance = CharacterFactory
            .CreateCharacterReferenceFromRecord(this._characters.BrockRecord)
            .Instance;
        if (BrockInstance is null)
        {
            throw new InvalidOperationException("Brock instance is null");
        }
        this.Brock = BrockInstance;

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
