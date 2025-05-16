using System;
using Godot;

namespace Entities
{
    public partial class Character : GodotObject
    {
        public string Name { get; set; } = "";

        public Character(string name)
        {
            this.Name = name;
        }
    }
}
