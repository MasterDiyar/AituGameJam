using Godot;
using AITUgameJam.scripts.items;
using AITUgameJam.scripts.mobs;
using AITUgameJam.scripts.playerThings;

namespace AITUgameJam.scripts.shop
{
    public partial class ShopUIControllerSimple : CanvasLayer
    {
        private Cat _player;
        
        [Export] public NodePath MoneyLabelPath;
        private Label _moneyLabel;

        [Export] public NodePath PopupPanelPath;
        [Export] public NodePath CloseButtonPath;

        [Export] public NodePath SellGridPath;
        [Export] public NodePath BuyGridPath;

        private Control _popup;
        private Button _close;

        private Inventory _inventory;
        private StaticInfo _staticInfo;

        public override void _Ready()
        {
            _player = GetTree().GetFirstNodeInGroup("player") as Cat;
            _popup = GetNode<Control>(PopupPanelPath);
            _moneyLabel = GetNodeOrNull<Label>(MoneyLabelPath);
            _close = GetNode<Button>(CloseButtonPath);
            _close.Pressed += Close;

            // StaticInfo как Autoload
            _staticInfo = GetNodeOrNull<StaticInfo>("/root/StaticInfo");
            if (_staticInfo == null)
                GD.PushError("StaticInfo not found at /root/StaticInfo (Autoload).");
            
            HookGrid(GetNode<GridContainer>(SellGridPath));
            HookGrid(GetNode<GridContainer>(BuyGridPath));

            RefreshMoney();
            Close();
        }
        private void RefreshMoney()
        {
            if (_moneyLabel == null || _staticInfo == null) return;
            _moneyLabel.Text = $"{(int)_staticInfo.Money} coins";
        }

        private void ResolveInventory()
        {
            if (_inventory != null) return;
            _inventory = GetTree().GetFirstNodeInGroup("inventory") as Inventory;
            if (_inventory == null)
                GD.PushError("Inventory not found. Add group 'inventory' to the Inventory node instance used in game.");
        }


        private void HookGrid(GridContainer grid)
        {
            for (int i = 0; i < grid.GetChildCount(); i++)
            {
                SeedButton btn = null;
                var child = grid.GetChild(i);
                if (child is SeedButton s) btn = s;
                else if (child.GetChildCount() > 0 && child.GetChild(1) is SeedButton s2) btn = s2;

                if (btn != null)
                {
                    GD.Print("Hooking button: ", btn.Name);
                    SeedButton currentBtn = btn;
                    currentBtn.Pressed += () =>
                    {
                        GD.Print("press");
                        OnSeedPressed(currentBtn);
                    };
                }
            }
        }

        private void OnSeedPressed(SeedButton btn)
        {
            GD.Print("Pressed: ", btn.Name);
            ResolveInventory();
            if (_inventory == null || _staticInfo == null) return;

            var baseScene = btn.GetBaseScene();
            if (baseScene == null) return;

            if (btn.IsSell) SellAll(baseScene, btn.Price, btn.id);
            else BuyOne(baseScene, btn.Price, btn.id);
        }



        public void Open()
        {
            _player.UiBlocked = true;
            ResolveInventory();
            RefreshMoney();
            Visible = true;
            _popup.Visible = true;

            if (_player != null) _player.UiBlocked = true;
        }

        public void Close()
        {
            _player.UiBlocked = false;
            _popup.Visible = false;
            Visible = false;

            if (_player != null) _player.UiBlocked = false;
        }


        private void SellAll(PackedScene baseScene, int sellPrice, int itemId)
        {
            int idx = FindInventoryIndex(baseScene);
            if (idx < 0) return;

            int count = _inventory.counter[idx];
            if (count <= 0) return;

            _staticInfo.Money += count * sellPrice;

            _inventory.counter[idx] = 0;
            _inventory.RemoveItem(idx);
            RefreshMoney();
        }

        private void BuyOne(PackedScene baseScene, int buyPrice, int itemId)
        {
            if (_staticInfo.Money < buyPrice) return;

            if (!CanAddToInventory(baseScene)) return;

            var temp = baseScene.Instantiate<Item>();
            temp.Count = 1;

            bool added = _inventory.AddItem(temp);
            temp.QueueFree();

            if (!added) return;

            _staticInfo.Money -= buyPrice;
            RefreshMoney();
        }

        private int FindInventoryIndex(PackedScene scene)
        {
            string path = scene.ResourcePath;
            for (int i = 0; i < _inventory.items.Length; i++)
            {
                var s = _inventory.items[i];
                
                GD.Print($"Comparing: {s.ResourcePath} == {path}");
                if (s != null && s.ResourcePath == path)
                    return i;
            }
            return -1;
        }

        private bool CanAddToInventory(PackedScene scene)
        {
            string path = scene.ResourcePath;

            for (int i = 0; i < _inventory.items.Length; i++)
            {
                var s = _inventory.items[i];
                if (s != null && s.ResourcePath == path) return true;
            }
            for (int i = 0; i < _inventory.items.Length; i++)
            {
                if (_inventory.items[i] == null) return true;
            }
            return false;
        }
    }
}
