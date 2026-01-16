using Godot;
using System.Collections.Generic;
namespace AITUgameJam.scripts.mapEditor;

public partial class MossDrawer : TileMapLayer
{
    /* 1  2   4
     * 8  0  16
     * 32 64 128
    */
    
    private List<List<int>> _tileMasks = [
        [208, 248, 104, 64, 80, 72, 88, 26],
        [214, 255, 107, 66, 18, 10, 82, 74],
        [22, 31, 11, 2, 127, 223, 95, 250],
        [16, 24, 8, 0, 251, 254, 123, 94],
        [210, 106, 216, 120, 206, 91, 126],
        [86, 75, 30, 27, 94, 90, 126, 255 ]
    ];
    
    private Dictionary<int, Vector2I> _maskToAtlas = new Dictionary<int, Vector2I>();

    public override void _Ready()
    {
        
        for (int y = 0; y < _tileMasks.Count; y++)
        {
            for (int x = 0; x < _tileMasks[y].Count; x++)
            {
                int mask = _tileMasks[y][x];
                if (!_maskToAtlas.ContainsKey(mask))
                {
                    _maskToAtlas[mask] = new Vector2I(x, y);
                }
            }
        }
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("lm"))
        {
            SetCellAtMouse(GetGlobalMousePosition());
        }
    }

    public void SetCellAtMouse(Vector2 position)
    {
        Vector2I putPoint = LocalToMap(ToLocal(position));

        SetCell(putPoint, 0, new Vector2I(0, 0)); 

        // 3. Update the 3x3 area around the placed tile
        for (int x = putPoint.X - 1; x <= putPoint.X + 1; x++)
        {
            for (int y = putPoint.Y - 1; y <= putPoint.Y + 1; y++)
            {
                UpdateTileMask(new Vector2I(x, y));
            }
        }
    }

    private void UpdateTileMask(Vector2I coords)
    {
        if (GetCellSourceId(coords) == -1) return;

        int mask = 0;

        bool top = HasTile(coords + Vector2I.Up);
        bool bottom = HasTile(coords + Vector2I.Down);
        bool left = HasTile(coords + Vector2I.Left);
        bool right = HasTile(coords + Vector2I.Right);
        
        if (top)    mask += 2;
        if (left)   mask += 8;
        if (right)  mask += 16;
        if (bottom) mask += 64;

        if (top && left && HasTile(coords + new Vector2I(-1, -1)))    mask += 1;
        if (top && right && HasTile(coords + new Vector2I(1, -1)))    mask += 4;
        if (bottom && left && HasTile(coords + new Vector2I(-1, 1)))  mask += 32;
        if (bottom && right && HasTile(coords + new Vector2I(1, 1)))  mask += 128;

        if (_maskToAtlas.TryGetValue(mask, out Vector2I atlasCoords))
            SetCell(coords, 1, atlasCoords);
        else {
            int simpleMask = (top ? 2 : 0) + (bottom ? 64 : 0) + (left ? 8 : 0) + (right ? 16 : 0);
            if (_maskToAtlas.TryGetValue(simpleMask, out Vector2I fallbackCoords))
                SetCell(coords, 1, fallbackCoords);
        }
    }

    private bool HasTile(Vector2I coords) => GetCellSourceId(coords) != -1;
}