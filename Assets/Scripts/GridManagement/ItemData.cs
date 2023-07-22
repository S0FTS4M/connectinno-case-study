// GridManager.cs
using UnityEngine;
// ItemData.cs
[CreateAssetMenu(menuName = "MatchTile/ItemData")]
public class ItemData : ScriptableObject
{
    public Sprite itemSprite; // The sprite representing the item
    public string itemName; // The name of the item (e.g., "Apple", "Banana", etc.)
}

