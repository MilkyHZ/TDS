using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurretPlacementPoint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Canvas & UI Setup")]
    [SerializeField]
    private Canvas _worldCanvas;

    [SerializeField]
    private Button _buttonPrefab;

    private BaseTurret _placedTurret;
    private bool _isUpgraded = false;
    private Button _spawnedButton;
    private RectTransform _buttonRect;
    public bool IsOccupied => _placedTurret != null;
    public bool IsUpgraded => _isUpgraded;
    public BaseTurret PlacedTurret => _placedTurret;
    private int _amountSpentOnThisNode = 0;

    private void Start()
    {
        if (_buttonPrefab != null && _worldCanvas != null)
        {
            _spawnedButton = Instantiate(_buttonPrefab, _worldCanvas.transform);
            _buttonRect = _spawnedButton.GetComponent<RectTransform>();
            _spawnedButton.onClick.AddListener(OnNodeButtonClicked);

            EventTrigger trigger = _spawnedButton.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry entryEnter = new() { eventID = EventTriggerType.PointerEnter };
            entryEnter.callback.AddListener((data) => OnPointerEnter((PointerEventData)data));
            trigger.triggers.Add(entryEnter);

            EventTrigger.Entry entryExit = new() { eventID = EventTriggerType.PointerExit };
            entryExit.callback.AddListener((data) => OnPointerExit((PointerEventData)data));
            trigger.triggers.Add(entryExit);
        }
    }

    private void Update()
    {
        if (_buttonRect != null && Camera.main != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            if (screenPos.z > 0) _buttonRect.position = screenPos;
        }
    }

    private void OnDestroy()
    {
        if (_spawnedButton != null) Destroy(_spawnedButton.gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetRangeVisible(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (TurretNodeUIManager.I != null && TurretNodeUIManager.I.SelectedNode == this) return;

        SetRangeVisible(false);
    }

    private void OnNodeButtonClicked()
    {
        if (TurretNodeUIManager.I != null) TurretNodeUIManager.I.InspectNode(this);
    }

    public void SetRangeVisible(bool show)
    {
        if (_placedTurret != null) _placedTurret.ToggleRangeVisual(show);
    }

    public Vector3 GetBuildPosition() => transform.position;

    public int GetTurretBuildCost(BaseTurret prefab)
    {
        if (prefab == null) return 0;

        int inflation = TurretNodeUIManager.I != null ? TurretNodeUIManager.I.GetGlobalPriceIncrease() : 0;
        return prefab.BaseBuildCost + inflation;
    }

    public int GetTurretUpgradeCost()
    {
        if (_placedTurret == null) return 0;

        int inflation = TurretNodeUIManager.I != null ? TurretNodeUIManager.I.GetGlobalPriceIncrease() : 0;
        return _placedTurret.BaseUpgradeCost + inflation;
    }

    public bool BuildTurret(BaseTurret turretPrefab)
    {
        if (IsOccupied || turretPrefab == null) return false;

        int costToBuild = GetTurretBuildCost(turretPrefab);

        if (CoinManager.I != null && !CoinManager.I.TrySpendCoins(costToBuild)) return false;

        Vector3 spawnPos = GetBuildPosition();
        _placedTurret = Instantiate(turretPrefab, spawnPos, Quaternion.identity, transform);
        _isUpgraded = false;
        _amountSpentOnThisNode = costToBuild;

        SetRangeVisible(false);

        if (AudioManager.I != null) AudioManager.I.PlayBuildTurret();

        return true;
    }

    public bool UpgradeTurret()
    {
        if (!IsOccupied || _isUpgraded) return false;

        int costToUpgrade = GetTurretUpgradeCost();

        if (CoinManager.I != null && !CoinManager.I.TrySpendCoins(costToUpgrade)) return false;

        _isUpgraded = true;
        _amountSpentOnThisNode += costToUpgrade;
        _placedTurret.UpgradeToLevelMax();
        _placedTurret.transform.localScale *= 1.3f;

        if (AudioManager.I != null) AudioManager.I.PlayUpgradeTurret();

        return true;
    }

    public int GetRefundAmount() => Mathf.FloorToInt(_amountSpentOnThisNode * 0.5f);

    public void SellTurret()
    {
        if (!IsOccupied) return;

        int refund = GetRefundAmount();
        if (CoinManager.I != null) CoinManager.I.AddCoin(refund);

        if (AudioManager.I != null) AudioManager.I.PlaySellTurret();

        Destroy(_placedTurret.gameObject);
        _placedTurret = null;
        _isUpgraded = false;
        _amountSpentOnThisNode = 0;
    }
}