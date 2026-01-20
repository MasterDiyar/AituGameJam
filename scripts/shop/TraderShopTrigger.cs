using Godot;
using AITUgameJam.scripts.shop;

public partial class TraderShopTrigger : Area2D
{
    private HBoxContainers _shop;

    public override void _Ready()
    {
        BodyEntered += OnEnter;
        BodyExited += OnExit;
        BodyEntered += (body) => GD.Print("ENTER: ", body.Name);
        BodyExited += (body) => GD.Print("EXIT: ", body.Name);
    }

    private void ResolveShop()
    {
        if (_shop != null) return;
        var n = GetTree().GetFirstNodeInGroup("shop_ui");
        _shop = n as HBoxContainers;
        if (_shop == null) GD.PushError("ShopUI not found. Add group 'shop_ui' to ShopUI node.");
    }

    private void OnEnter(Node2D body)
    {
        if (!body.IsInGroup("player")) return;
        ResolveShop();
        _shop?.Open();
    }

    private void OnExit(Node2D body)
    {
        if (!body.IsInGroup("player")) return;
        ResolveShop();
        _shop?.Close();
    }
   

}