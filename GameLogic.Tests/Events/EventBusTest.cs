using GameLogic.Events;
using Xunit;

namespace GameLogic.EventsTests
{
    class EventBusTestData 
    {
        public List<Func<IEvent, Task>> Subscribers;
        public StringWriter Output;
        public TextWriter OriginalOutput;

        public EventBusTestData()
        {
           this.Subscribers = new List<Func<IEvent, Task>> {
            (e) => {
                Console.WriteLine("Subscriber 1");
                return Task.CompletedTask;
            },
            (e) => {
                Console.WriteLine("Subscriber 2");
                return Task.CompletedTask;  
            }
           };
           (this.Output, this.OriginalOutput) = setup();
        } 

        private (StringWriter, TextWriter) setup() {
            var output = new StringWriter();
            var originalOutput = Console.Out;
            Console.SetOut(output);

            return (output, originalOutput);
        }

        public void Teardown() {
            Console.SetOut(OriginalOutput);
            Output.Dispose();
        }
    }

    public class TestEvent : IEvent
    {
        public string Name => "TestEvent";
    }

    public class EventBusTest
    {
        [Fact]
        public void PublishEvent() {
            EventBusTestData td = new EventBusTestData();
            EventBus eventBus = new EventBus();

            eventBus.Subscribe<TestEvent>(td.Subscribers[0]);
            eventBus.Subscribe<TestEvent>(td.Subscribers[1]);

            eventBus.Publish(new TestEvent());
            
            Assert.Contains("Subscriber 1", td.Output.ToString());
            Assert.Contains("Subscriber 2", td.Output.ToString());

            td.Teardown();
        }

        [Fact]
        public void UnsubscribeEvent() {
            EventBusTestData td = new EventBusTestData();
            EventBus eventBus = new EventBus();

            eventBus.Subscribe<TestEvent>(td.Subscribers[0]);
            eventBus.Unsubscribe<TestEvent>(td.Subscribers[0]);

            eventBus.Publish(new TestEvent());

            Assert.DoesNotContain("Subscriber 1", td.Output.ToString());

            td.Teardown();
        }

    }
}
