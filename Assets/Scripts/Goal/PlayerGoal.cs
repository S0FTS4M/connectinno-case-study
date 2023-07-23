using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerGoal : MonoBehaviour
{
    [SerializeField] private Image goalIcon;
    [SerializeField] private TextMeshProUGUI goalText;

    private Tile.Settings _tileSettings;

    [Inject]
    private void Construct(Tile.Settings tileSettings)
    {
        _tileSettings = tileSettings;
    }

    public void SetGoal(PlayerGoalData playerGoalData)
    {
        var itemData = _tileSettings.GetItemData(playerGoalData.itemName);
        goalIcon.sprite = itemData.itemSprite;
        goalText.text = playerGoalData.targetCount.ToString();
    }

    public class PlayerGoalPool : MonoMemoryPool<PlayerGoal> { }
    
    [System.Serializable]
    public class Settings
    {
        public GameObject goalPrefab;
    }
}
