using Godot;

namespace AITUgameJam.scripts.shop
{
    public partial class SeedButton : Button
    {
        [Export] public string BaseScenePath = "";
        [Export] public int id = 0;
        [Export] public int Price = 1;
        [Export] public bool IsSell = true;

        public PackedScene GetBaseScene()
        {
            if (string.IsNullOrEmpty(BaseScenePath)) return null;
            return GD.Load<PackedScene>(BaseScenePath);
        }
        bool wool = false;

        public override void _Ready()
        {
            GD.Print("Ready");
            GetParent<Container>().FocusEntered += () => { wool = true; GD.Print("MouseEntered"); };
            GetParent<Container>().FocusExited += () => wool = false;
            
        }

        public override void _Process(double delta)
        {
            if (wool && Input.IsActionJustPressed("lm"))
                EmitSignal("Pressed");
        }
    }
}