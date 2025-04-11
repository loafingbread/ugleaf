namespace GameLogic.Entities
{
    public partial class Character
    {
        public string Name { get; set; } = "";

        public Character(string name)
        {
            this.Name = name;
        }
    }
}
