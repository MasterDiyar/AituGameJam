using Godot;

namespace AITUgameJam.scripts.shop
{
    public partial class SeedTileButton : Button
    {
        [Signal] public delegate void TilePressedEventHandler(int index, bool isSell);

        [Export] public NodePath NameLabelPath;
        [Export] public NodePath IconPath;
        [Export] public NodePath PriceLabelPath;

        private Label _name;
        private TextureRect _icon;
        private Label _price;

        private int _index;
        private bool _isSell;

        public override void _Ready()
        {
            _name = GetNode<Label>(NameLabelPath);
            _icon = GetNode<TextureRect>(IconPath);
            _price = GetNode<Label>(PriceLabelPath);

            Pressed += () => EmitSignal(SignalName.TilePressed, _index, _isSell);
        }

        public void Setup(int index, string displayName, Texture2D icon, int price, bool isSell, bool enabled)
        {
            _index = index;
            _isSell = isSell;

            _name.Text = displayName;
            _icon.Texture = icon;
            _price.Text = $"{price} coins";

            Disabled = !enabled;
            Visible = true;
        }
    }
}