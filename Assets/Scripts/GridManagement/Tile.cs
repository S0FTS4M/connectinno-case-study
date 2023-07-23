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
            if(_itemDatas == null)
            {
                _itemDatas = new Dictionary<string, ItemData>();
                foreach(var itemData in itemDatas)
                {
                    _itemDatas.Add(itemData.name, itemData);
                }
            }
            
            return _itemDatas[itemName];
        }
    }
}

