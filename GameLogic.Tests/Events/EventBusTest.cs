using GameLogic.Events;
using Xunit;

namespace GameLogic.EventsTests
{
    class EventBusTestData
    {
        public SkillUseEvent FireballSkillUseEvent = new("Fireball", "Alice");
        public SkillEvent? ReceivedFireballSkillEvent { get; set; }

        public CombatPhaseChangeEvent StartCombatPhaseChangedEvent = new(
            new Entities.Character("Alice"),
            new Entities.Character("Alice"),
            new List<Entities.Character>() { new Entities.Character("Jack") },
            "Skill use",
            "Player turn"
        );
        public CombatEvent? ReceivedCombatPhaseChangedEvent { get; set; }

        public EventBusTestData() { }
    }

    public class EventBusTest
    {
        [Fact]
        public void Publish_CallsTwoDifferentEventCategories()
        {
            EventBusTestData td = new EventBusTestData();
            EventBus eventBus = new EventBus();

            eventBus.Subscribe<SkillEvent, BuiltInEventCategory>(
                (SkillEvent receivedEvent) =>
                {
                    td.ReceivedFireballSkillEvent = receivedEvent;
                    return Task.CompletedTask;
                }
            );
            eventBus.Subscribe<CombatEvent, BuiltInEventCategory>(
                (CombatEvent receivedEvent) =>
                {
                    td.ReceivedCombatPhaseChangedEvent = receivedEvent;
                    return Task.CompletedTask;
                }
            );

            eventBus.Publish<SkillEvent, BuiltInEventCategory>(td.FireballSkillUseEvent);
            eventBus.Publish<CombatEvent, BuiltInEventCategory>(td.StartCombatPhaseChangedEvent);

            Assert.Equal(td.FireballSkillUseEvent, td.ReceivedFireballSkillEvent);
            Assert.Equal(td.StartCombatPhaseChangedEvent, td.ReceivedCombatPhaseChangedEvent);
        }

        [Fact]
        public void Publish_DoNotCallHandlerAfterUnsubscribe()
        {
            EventBusTestData td = new EventBusTestData();
            EventBus eventBus = new EventBus();
            bool handlerWasCalled = false;
            Func<SkillEvent, Task> handler = (SkillEvent receivedEvent) =>
            {
                handlerWasCalled = true;
                return Task.CompletedTask;
            };

            eventBus.Subscribe<SkillEvent, BuiltInEventCategory>(handler);
            eventBus.Unsubscribe<SkillEvent, BuiltInEventCategory>(handler);
            eventBus.Publish<SkillEvent, BuiltInEventCategory>(td.FireballSkillUseEvent);

            Assert.False(handlerWasCalled);
        }
    }
}
