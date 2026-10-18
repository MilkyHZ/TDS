using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPreviewCard : MonoBehaviour
{
    [SerializeField] private Image _imgEnemyIcon;
    [SerializeField] private TextMeshProUGUI _txtEnemyCount;

    public void Setup(Sprite icon, int count)
    {
        if (_imgEnemyIcon != null)
        {
            _imgEnemyIcon.sprite = icon;
            _imgEnemyIcon.gameObject.SetActive(icon != null);
        }

        if (_txtEnemyCount != null)
        {
            _txtEnemyCount.text = $"x{count}";
        }
    }
}