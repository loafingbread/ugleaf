using Godot;

namespace GameLogic.Entities
{
    public partial class Character : GodotObject
    {
        public string Name { get; set; } = "";

        public Character(string name) {
            this.Name = name;
        } 
    }
}
