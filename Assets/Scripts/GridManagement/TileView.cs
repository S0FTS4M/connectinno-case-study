// GridManager.cs
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TileView : MonoBehaviour
{
    [SerializeField] private Image tileImage;

    [SerializeField] private Image itemImage;

    private Tile.Settings _tileSettings;

    private Tile _tile;

    [Inject]
    private void Construct(Tile.Settings tileSettings)
    {
        _tileSettings = tileSettings;
    }
    
    public void SetTile(Tile tile)
    {
        _tile = tile;
     
        itemImage.sprite = tile.ItemData.itemSprite;
    }

    public void OnTilePressed()
    {
        tileImage.sprite = _tileSettings.pressedSprite;
    }
    
    public void OnTileReleased()
    {
        tileImage.sprite = _tileSettings.defaultSprite;
    }
    
}

