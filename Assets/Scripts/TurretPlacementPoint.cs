using UnityEngine;
using UnityEngine.UI;

public class TurretPlacementPoint : MonoBehaviour
{
    [Header("Canvas & UI Setup")]
    [SerializeField] private Canvas _worldCanvas;
    [SerializeField] private Button _buttonPrefab;

    [Header("Costs & Data")]
    [SerializeField] private int _buildCost = 100;
    [SerializeField] private int _upgradeCost = 150;

    private BaseTurret _placedTurret;
    private bool _isUpgraded = false;

    private Button _spawnedButton;
    private RectTransform _buttonRect;

    public bool IsOccupied => _placedTurret != null;
    public bool IsUpgraded => _isUpgraded;
    public BaseTurret PlacedTurret => _placedTurret;
    public int BuildCost => _buildCost;
    public int UpgradeCost => _upgradeCost;

    private void Start()
    {
        if (_buttonPrefab != null && _worldCanvas != null)
        {
            _spawnedButton = Instantiate(_buttonPrefab, _worldCanvas.transform);
            _buttonRect = _spawnedButton.GetComponent<RectTransform>();

            _spawnedButton.onClick.AddListener(OnNodeButtonClicked);
        }
    }

    private void Update()
    {
        if (_buttonRect != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

            if (screenPos.z > 0)
            {
                _buttonRect.position = screenPos;
            }
        }
    }

    private void OnDestroy()
    {
        if (_spawnedButton != null)
        {
            Destroy(_spawnedButton.gameObject);
        }
    }

    private void OnNodeButtonClicked()
    {
        if (TurretNodeUIManager.I != null)
        {
            TurretNodeUIManager.I.InspectNode(this);
        }
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position;
    }

    public bool BuildTurret(BaseTurret turretPrefab)
    {
        if (IsOccupied || turretPrefab == null) return false;

        if (CoinManager.I != null && !CoinManager.I.TrySpendCoins(_buildCost))
        {
            return false;
        }

        Vector3 spawnPos = GetBuildPosition();
        _placedTurret = Instantiate(turretPrefab, spawnPos, Quaternion.identity, transform);
        _isUpgraded = false;
        return true;
    }

    public bool UpgradeTurret()
    {
        if (!IsOccupied || _isUpgraded) return false;

        if (CoinManager.I != null && !CoinManager.I.TrySpendCoins(_upgradeCost))
        {
            return false;
        }

        _isUpgraded = true;
        _placedTurret.UpgradeToLevelMax();
        _placedTurret.transform.localScale *= 1.3f;
        return true;
    }

    public int GetRefundAmount()
    {
        int totalSpent = _buildCost + (_isUpgraded ? _upgradeCost : 0);
        return Mathf.FloorToInt(totalSpent * 0.5f);
    }

    public void SellTurret()
    {
        if (!IsOccupied) return;

        int refund = GetRefundAmount();
        if (CoinManager.I != null)
        {
            CoinManager.I.AddCoin(refund);
        }

        Destroy(_placedTurret.gameObject);
        _placedTurret = null;
        _isUpgraded = false;
    }
}