using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurretPreviewCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image _imgIcon;

    [SerializeField]
    private Button _btnBuild;

    private BaseTurret _turretPrefab;

    private string _turretName;

    private int _buildCost;

    public void Setup(BaseTurret prefab, Sprite icon, string name, int cost, bool canAfford, System.Action<BaseTurret> onSelect)
    {
        _turretPrefab = prefab;
        _turretName = name;
        _buildCost = cost;

        if (_imgIcon != null)
        {
            _imgIcon.sprite = icon;
            _imgIcon.gameObject.SetActive(icon != null);
        }

        if (_btnBuild != null)
        {
            _btnBuild.interactable = canAfford;
            _btnBuild.onClick.RemoveAllListeners();
            _btnBuild.onClick.AddListener(() => onSelect?.Invoke(_turretPrefab));
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_turretPrefab != null && TurretNodeUIManager.I != null)
        {
            TurretNodeUIManager.I.ShowTurretHoverInfo(_turretName, _buildCost, _turretPrefab.AttackRange);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (TurretNodeUIManager.I != null)
        {
            TurretNodeUIManager.I.ClearTurretHoverInfo();
        }
    }
}