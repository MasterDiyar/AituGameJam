using Godot;

namespace AITUgameJam.scripts.shop
{
    public partial class SeedButton : Button
    {
        // Путь к сцене БЕЗ "2", например: res://Semena/wheat.tscn
        [Export] public string BaseScenePath = "";

        [Export] public int Price = 1;
        [Export] public bool IsSell = true;

        public PackedScene GetBaseScene()
        {
            if (string.IsNullOrEmpty(BaseScenePath)) return null;
            return GD.Load<PackedScene>(BaseScenePath);
        }

        public override void _Ready()
        {
            FocusMode = FocusModeEnum.None;
        }
    }
}