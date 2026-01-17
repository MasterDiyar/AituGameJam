using AITUgameJam.scripts.mapEditor;
using Godot;

namespace AITUgameJam.scripts.actions;

public partial class MossClick : ActionNode
{
    private MossDrawer drawerLayel;
    [Export] Timer timer;
    bool executed = false;
    public override void _Ready()
    {
        drawerLayel = GetTree().GetFirstNodeInGroup("map").GetNode<MossDrawer>("moss");
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
        Item.MainHandAction -= SetMoss;
    }
}