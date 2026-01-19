using AITUgameJam.scripts.mapEditor;
using AITUgameJam.scripts.mobs;
using Godot;

namespace AITUgameJam.scripts.actions;

public partial class MossClick : ActionNode
{
    private MossDrawer drawerLayel;
    [Export] Timer timer;
    bool executed = false;

    private Cat _cat;

    public override void _Ready()
    {
        drawerLayel = GetTree().GetFirstNodeInGroup("map").GetNode<MossDrawer>("moss");

        // игрок по группе
        _cat = GetTree().GetFirstNodeInGroup("player") as Cat;

        base._Ready();

        if (Item.Type == "hoe")
            Item.MainHandAction += SetMoss;

        timer.Timeout += () =>
        {
            executed = false;
            timer.Stop();
        };
    }

    void SetMoss()
    {
        // Блокируем действие, если открыт магазин/UI
        if (_cat != null && _cat.UiBlocked) return;

        if (executed) return;
        executed = true;

        Tween tween = GetTree().CreateTween();

        tween.TweenProperty(Item, "rotation_degrees", 90f, 0.15f)
            .SetTrans(Tween.TransitionType.Quint)
            .SetEase(Tween.EaseType.Out);

        tween.TweenProperty(Item, "rotation_degrees", 0f, 0.45f)
            .SetTrans(Tween.TransitionType.Back)
            .SetEase(Tween.EaseType.InOut);

        timer.Start();
        drawerLayel.SetCellAtMouse(GlobalPosition);
    }

    public override void _ExitTree()
    {
        if (Item != null)
            Item.MainHandAction -= SetMoss;
    }
}