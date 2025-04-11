using Godot;

namespace GodotDriver.Resources
{
    public partial class CharacterResource : Resource
    {

        [Export]
        public string Name { get; set; } = "";

        public CharacterResource(string name): base()
        {
            this.Name = name;
        }
    }
}
