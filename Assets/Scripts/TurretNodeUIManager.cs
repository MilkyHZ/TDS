using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretNodeUIManager : MonoBehaviour
{
    public static TurretNodeUIManager I { get; private set; }

    [Header("Action Panel Root")]
    [SerializeField]
    private GameObject _actionPanel;

    [Header("Build Cards Container")]
    [SerializeField]
    private GameObject _buildOptionsGroup;
    [SerializeField]
    private Transform _turretCardsContainer;
    [SerializeField]
    private TurretPreviewCard _turretCardPrefab;

    [Header("Hover Info Display")]
    [SerializeField]
    private TextMeshProUGUI _txtHoverName;
    [SerializeField]
    private TextMeshProUGUI _txtHoverCost;

    [Header("Tower Selected Container")]
    [SerializeField]
    private GameObject _towerSelectedGroup;
    [SerializeField]
    private TextMeshProUGUI _txtTowerNameAndLevel;
    [SerializeField]
    private Button _btnUpgrade;
    [SerializeField]
    private TextMeshProUGUI _txtUpgradeCost;
    [SerializeField]
    private Button _btnSell;
    [SerializeField]
    private TextMeshProUGUI _txtSellRefund;

    [Header("Close Button")]
    [SerializeField]
    private Button _btnClose;

    [Header("Turret Prefabs")]
    [SerializeField]
    private BaseTurret _ballistaPrefab;
    [SerializeField]
    private BaseTurret _cannonPrefab;
    [SerializeField]
    private BaseTurret _slowTurretPrefab;

    [Header("Node Range Preview LineRenderer")]
    [SerializeField]
    private LineRenderer _nodeRangePreview;

    [Header("Economy Settings")]
    [SerializeField]
    private int _priceIncreasePerLevel = 50;

    private TurretPlacementPoint _selectedNode;
    public TurretPlacementPoint SelectedNode => _selectedNode;

    private bool _lastBallistaAffordState = false;
    private bool _lastCannonAffordState = false;
    private bool _lastSlowAffordState = false;
    private bool _lastCanAffordUpgradeState = false;

    private void Awake()
    {
        I = this;

        if (_btnUpgrade != null) _btnUpgrade.onClick.AddListener(UpgradeTower);
        if (_btnSell != null) _btnSell.onClick.AddListener(SellTower);
        if (_btnClose != null) _btnClose.onClick.AddListener(CloseMenu);

        HidePreviewRange();
        CloseMenu();
    }

    private void Update()
    {
        if (_selectedNode == null || _actionPanel == null || !_actionPanel.activeSelf) return;

        int currentGold = CoinManager.I != null ? CoinManager.I.CoinBank : 0;

        if (!_selectedNode.IsOccupied)
        {
            bool canAffordBallista = _ballistaPrefab != null && currentGold >= _selectedNode.GetTurretBuildCost(_ballistaPrefab);
            bool canAffordCannon = _cannonPrefab != null && currentGold >= _selectedNode.GetTurretBuildCost(_cannonPrefab);
            bool canAffordSlow = _slowTurretPrefab != null && currentGold >= _selectedNode.GetTurretBuildCost(_slowTurretPrefab);

            if (canAffordBallista != _lastBallistaAffordState ||
                canAffordCannon != _lastCannonAffordState ||
                canAffordSlow != _lastSlowAffordState)
            {
                _lastBallistaAffordState = canAffordBallista;
                _lastCannonAffordState = canAffordCannon;
                _lastSlowAffordState = canAffordSlow;

                RefreshCurrentNode();
            }
        }
        else
        {
            int upgradeCost = _selectedNode.GetTurretUpgradeCost();
            bool canAfford = currentGold >= upgradeCost;

            if (canAfford != _lastCanAffordUpgradeState)
            {
                _lastCanAffordUpgradeState = canAfford;
                RefreshCurrentNode();
            }
        }
    }

    public int GetGlobalPriceIncrease()
    {
        int totalTowerLevelsOnMap = 0;
        TurretPlacementPoint[] nodes = FindObjectsByType<TurretPlacementPoint>(FindObjectsSortMode.None);

        foreach (var node in nodes)
        {
            if (node.IsOccupied)
            {
                totalTowerLevelsOnMap += 1;
                if (node.IsUpgraded) totalTowerLevelsOnMap += 1;
            }
        }

        return totalTowerLevelsOnMap * _priceIncreasePerLevel;
    }

    public void InspectNode(TurretPlacementPoint node)
    {
        if (node == null) return;

        if (_selectedNode == node && _actionPanel != null && _actionPanel.activeSelf)
        {
            CloseMenu();
            return;
        }

        if (_selectedNode != null) _selectedNode.SetRangeVisible(false);

        _selectedNode = node;
        _selectedNode.SetRangeVisible(true);

        _lastBallistaAffordState = false;
        _lastCannonAffordState = false;
        _lastSlowAffordState = false;
        _lastCanAffordUpgradeState = false;

        RefreshCurrentNode();
    }

    private void RefreshCurrentNode()
    {
        if (_selectedNode == null) return;

        if (_actionPanel != null) _actionPanel.SetActive(true);

        int currentGold = CoinManager.I != null ? CoinManager.I.CoinBank : 0;

        if (!_selectedNode.IsOccupied)
        {
            if (_buildOptionsGroup != null) _buildOptionsGroup.SetActive(true);
            if (_towerSelectedGroup != null) _towerSelectedGroup.SetActive(false);

            ClearTurretHoverInfo();
            PopulateTurretCards(currentGold);
        }
        else
        {
            if (_buildOptionsGroup != null) _buildOptionsGroup.SetActive(false);
            if (_towerSelectedGroup != null) _towerSelectedGroup.SetActive(true);

            BaseTurret turret = _selectedNode.PlacedTurret;

            if (_txtTowerNameAndLevel != null)
            {
                string levelText = _selectedNode.IsUpgraded ? "Lv.2 (MAX)" : "Lv.1";
                _txtTowerNameAndLevel.text = turret != null ? $"{turret.GetType().Name} ({levelText})" : "Unoccupied";
            }

            int upgradeCost = _selectedNode.GetTurretUpgradeCost();
            bool canAffordUpgrade = currentGold >= upgradeCost;

            if (_btnUpgrade != null) _btnUpgrade.interactable = !_selectedNode.IsUpgraded && canAffordUpgrade;
            if (_txtUpgradeCost != null) _txtUpgradeCost.text = _selectedNode.IsUpgraded ? "MAX" : $"Upgrade: {upgradeCost}G";
            if (_txtSellRefund != null) _txtSellRefund.text = $"Sell: +{_selectedNode.GetRefundAmount()}G";
        }
    }

    private void PopulateTurretCards(int currentGold)
    {
        if (_turretCardsContainer == null || _turretCardPrefab == null) return;

        foreach (Transform child in _turretCardsContainer) Destroy(child.gameObject);

        _lastBallistaAffordState = _ballistaPrefab != null && currentGold >= _selectedNode.GetTurretBuildCost(_ballistaPrefab);
        _lastCannonAffordState = _cannonPrefab != null && currentGold >= _selectedNode.GetTurretBuildCost(_cannonPrefab);
        _lastSlowAffordState = _slowTurretPrefab != null && currentGold >= _selectedNode.GetTurretBuildCost(_slowTurretPrefab);

        CreateTurretCard(_ballistaPrefab, "Ballista", currentGold);
        CreateTurretCard(_cannonPrefab, "Cannon", currentGold);
        CreateTurretCard(_slowTurretPrefab, "Slow", currentGold);
    }

    private void CreateTurretCard(BaseTurret prefab, string name, int currentGold)
    {
        if (prefab == null) return;

        int cost = _selectedNode.GetTurretBuildCost(prefab);
        bool canAfford = currentGold >= cost;

        TurretPreviewCard card = Instantiate(_turretCardPrefab, _turretCardsContainer);
        card.Setup(prefab, prefab.PreviewIcon, name, cost, canAfford, BuildTower);
    }

    public void ShowTurretHoverInfo(string name, int cost, float range)
    {
        if (_txtHoverName != null) _txtHoverName.text = name;
        if (_txtHoverCost != null) _txtHoverCost.text = $"{cost}G";

        ShowPreviewRange(range);
    }

    public void ClearTurretHoverInfo()
    {
        if (_txtHoverName != null) _txtHoverName.text = "Select Turret";
        if (_txtHoverCost != null) _txtHoverCost.text = "";

        HidePreviewRange();
    }

    public void ShowPreviewRange(float radius)
    {
        if (_selectedNode == null || _nodeRangePreview == null) return;

        _nodeRangePreview.enabled = true;
        int points = 40;
        _nodeRangePreview.positionCount = points + 1;
        _nodeRangePreview.useWorldSpace = true;

        Vector3 center = _selectedNode.GetBuildPosition();
        float angle = 0f;

        for (int i = 0; i <= points; i++)
        {
            float x = center.x + Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            float z = center.z + Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            _nodeRangePreview.SetPosition(i, new Vector3(x, center.y + 0.1f, z));
            angle += 360f / points;
        }
    }

    public void HidePreviewRange()
    {
        if (_nodeRangePreview != null) _nodeRangePreview.enabled = false;
    }

    public void CloseMenu()
    {
        if (_selectedNode != null) _selectedNode.SetRangeVisible(false);

        ClearTurretHoverInfo();
        HidePreviewRange();

        if (_actionPanel != null) _actionPanel.SetActive(false);

        _selectedNode = null;
    }

    private void BuildTower(BaseTurret prefab)
    {
        if (_selectedNode != null && !_selectedNode.IsOccupied)
        {
            if (_selectedNode.BuildTurret(prefab)) CloseMenu();
        }
    }

    private void UpgradeTower()
    {
        if (_selectedNode != null && _selectedNode.IsOccupied)
        {
            if (_selectedNode.UpgradeTurret()) RefreshCurrentNode();
        }
    }

    private void SellTower()
    {
        if (_selectedNode != null && _selectedNode.IsOccupied)
        {
            _selectedNode.SellTurret();
            CloseMenu();
        }
    }
}