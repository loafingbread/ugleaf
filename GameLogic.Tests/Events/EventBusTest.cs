using GameLogic.Events;
using Xunit;

namespace GameLogic.EventsTests
{
    class EventBusTestData
    {
        public Events.Categories.SkillUseEvent FireballSkillUseEvent = new("Fireball", "Alice");
        public Events.Categories.SkillEvent? ReceivedFireballSkillEvent { get; set; }

        public Events.Categories.CombatPhaseChangeEvent StartCombatPhaseChangedEvent = new(
            new Entities.Character("Alice"),
            new Entities.Character("Alice"),
            new List<Entities.Character>() { new Entities.Character("Jack") },
            "Skill use",
            "Player turn"
        );
        public Events.Categories.CombatEvent? ReceivedCombatPhaseChangedEvent { get; set; }

        public EventBusTestData() { }
    }

    public class EventBusTest
    {
        [Fact]
        public void Publish_CallsTwoDifferentEventCategories()
        {
            EventBusTestData td = new EventBusTestData();
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
            EventBusTestData td = new EventBusTestData();
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
}
