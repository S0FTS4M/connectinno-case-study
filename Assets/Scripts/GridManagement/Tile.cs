// GridManager.cs
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Zenject;
// Tile.cs
public class Tile : MonoBehaviour
{
    [HideInInspector]
    public int row;
    [HideInInspector]
    public int col;
    private ItemData _itemData; // The data representing the item on the tile

    public bool IsTileAvailable { get; private set; }

    public ItemData ItemData => _itemData;

    private TileView _tileView;

    private Settings _settings;

    private RectTransform _rectTransform;
    private GameObject broken;
    private Coroutine _coroutine;

    [Inject]
    private void Construct(Settings settings)
    {
        _settings = settings;
        _rectTransform = GetComponent<RectTransform>();
    }

    public void SetItemData(ItemData itemData)
    {
        _tileView = GetComponent<TileView>();
        this._itemData = itemData;
        _tileView.SetTile(this);
        IsTileAvailable = true;

        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
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
        if (gameObject.activeSelf == false)
            return;

        broken = Instantiate(_settings.brokenTilePrefab, transform.position, Quaternion.identity);
        broken.transform.SetParent(transform);
        broken.transform.localPosition = Vector3.zero;
        broken.transform.localScale = Vector3.one;

        foreach (Transform child in broken.transform)
        {
            var rb = child.GetComponent<Rigidbody2D>();
            var rndVec = UnityEngine.Random.insideUnitCircle.normalized;
            rb.AddForce(5 * rndVec, ForceMode2D.Impulse);
        }
        _coroutine = StartCoroutine(DestroyTile(broken));

    }

    private IEnumerator DestroyTile(GameObject broken)
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(broken);
        broken = null;
    }

    public void HideTile()
    {
        _tileView.HideTile();
        IsTileAvailable = false;
    }

    public void ShowTile()
    {
        _tileView.ShowTile();
        IsTileAvailable = true;
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


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.Label(transform.position, row + " " + col);
    }

#endif
    public void Clear()
    {
        _itemData = null;
        OnTileReleased();
        if (broken != null)
        {
            Destroy(broken);
            broken = null;
            StopCoroutine(_coroutine);
        }
    }


    [System.Serializable]
    public class Settings
    {
        public GameObject tilePrefab;

        public ItemData[] itemDatas;

        public Sprite defaultSprite;

        public Sprite pressedSprite;

        public GameObject brokenTilePrefab;

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

