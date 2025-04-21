using System.Diagnostics;
using GameLogic.Events;
using GameLogic.Events.SkillEvents;
using Xunit;

namespace GameLogic.EventsTests
{
    class EventBusTestData
    {
        public SkillEvent FireballEvent = new("Fireball", "Alice");
        public SkillEvent? ReceivedSkillEvent { get; set; }

        public EventBusTestData() { }
    }

    public class EventBusTest
    {
        [Fact]
        public void PublishEvent()
        {
            EventBusTestData td = new EventBusTestData();
            EventBus eventBus = new EventBus();

            eventBus.Subscribe<SkillEvent, BuiltInEventCategory>(
                (SkillEvent receivedEvent) =>
                {
                    td.ReceivedSkillEvent = receivedEvent;
                    return Task.CompletedTask;
                }
            );
            eventBus.Publish<SkillEvent, BuiltInEventCategory>(td.FireballEvent);

            Assert.Equal(td.FireballEvent, td.ReceivedSkillEvent);
        }

        [Fact]
        public void UnsubscribeEvent()
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
            eventBus.Publish<SkillEvent, BuiltInEventCategory>(td.FireballEvent);

            Assert.False(handlerWasCalled);
        }
    }
}
