// GridManager.cs
using System.Collections.Generic;
using UnityEngine;
// Tile.cs
public class Tile : MonoBehaviour
{
    [HideInInspector]
    public int row;
    [HideInInspector]
    public int col;
    private ItemData _itemData; // The data representing the item on the tile

    public ItemData ItemData => _itemData;

    private TileView _tileView;

    public void SetItemData(ItemData itemData)
    {
        _tileView = GetComponent<TileView>();
        this._itemData = itemData;
        _tileView.SetTile(this);
    }

    public void OnTilePressed()
    {
        _tileView.OnTilePressed();
    }

    public void OnTileReleased()
    {
        _tileView.OnTileReleased();
    }
    
    public void BreakTile()
    {
    }

    public void HideTile()
    {
        _tileView.HideTile();
    }

    public bool IsValidNeighbor(Tile tile)
    {
        // Check if the given row and column indices are adjacent to the current tile
        bool isAdjacent = Mathf.Abs(tile.row - row) <= 1 && Mathf.Abs(tile.col - col) <= 1;
        if (!isAdjacent)
        {
            // The neighbor is not adjacent, return false
            return false;
        }
        
        // The neighbor is both adjacent and within the grid boundaries, return true
        return true;
    }
    // Code to handle tile interactions and visuals

    [System.Serializable]
    public class Settings
    {
        public GameObject tilePrefab;

        public ItemData[] itemDatas;

        public Sprite defaultSprite;

        public Sprite pressedSprite;

        public Sprite[] brokenTileSprites;

        private Dictionary<string, ItemData> _itemDatas;

        public ItemData GetItemData(string itemName)
        {
            if (_itemDatas == null)
            {
                _itemDatas = new Dictionary<string, ItemData>();
                foreach (var itemData in itemDatas)
                {
                    _itemDatas.Add(itemData.name, itemData);
                }
            }

            return _itemDatas[itemName];
        }
    }
}

