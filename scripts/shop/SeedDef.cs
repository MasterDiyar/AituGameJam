using Godot;

namespace AITUgameJam.scripts.shop
{
    [GlobalClass]
    public partial class SeedDef : Resource
    {
        [Export] public PackedScene BaseScene;

        [Export] public int SellPrice = 1;
        [Export] public int BuyPrice = 1;

        [Export] public string SeedsFolder = "res://Semena/";
    }
}