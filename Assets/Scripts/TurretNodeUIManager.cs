using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretNodeUIManager : MonoBehaviour
{
    public static TurretNodeUIManager I { get; private set; }

    [Header("Action Panel Root")]
    [SerializeField] private GameObject _actionPanel;

    [Header("Build Options Container (Empty Node)")]
    [SerializeField] private GameObject _buildOptionsGroup;
    [SerializeField] private Button _btnBuildBallista;
    [SerializeField] private Button _btnBuildCannon;
    [SerializeField] private Button _btnBuildSlow;

    [Header("Tower Selected Container (Occupied Node)")]
    [SerializeField] private GameObject _towerSelectedGroup;
    [SerializeField] private TextMeshProUGUI _txtTowerNameAndLevel;
    [SerializeField] private Button _btnUpgrade;
    [SerializeField] private TextMeshProUGUI _txtUpgradeCost;
    [SerializeField] private Button _btnSell;
    [SerializeField] private TextMeshProUGUI _txtSellRefund;

    [Header("Close Button")]
    [SerializeField] private Button _btnClose;

    [Header("Turret Prefabs")]
    [SerializeField] private BaseTurret _ballistaPrefab;
    [SerializeField] private BaseTurret _cannonPrefab;
    [SerializeField] private BaseTurret _slowTurretPrefab;

    private TurretPlacementPoint _selectedNode;

    private void Awake()
    {
        I = this;

        if (_btnBuildBallista != null) _btnBuildBallista.onClick.AddListener(() => BuildTower(_ballistaPrefab));
        if (_btnBuildCannon != null) _btnBuildCannon.onClick.AddListener(() => BuildTower(_cannonPrefab));
        if (_btnBuildSlow != null) _btnBuildSlow.onClick.AddListener(() => BuildTower(_slowTurretPrefab));
        if (_btnUpgrade != null) _btnUpgrade.onClick.AddListener(UpgradeTower);
        if (_btnSell != null) _btnSell.onClick.AddListener(SellTower);
        if (_btnClose != null) _btnClose.onClick.AddListener(CloseMenu);

        CloseMenu();
    }

    public void InspectNode(TurretPlacementPoint node)
    {
        if (node == null) return;

        if (_selectedNode == node && _actionPanel != null && _actionPanel.activeSelf)
        {
            CloseMenu();
            return;
        }

        _selectedNode = node;

        if (_actionPanel != null) _actionPanel.SetActive(true);

        int currentGold = CoinManager.I != null ? CoinManager.I.CoinBank : 0;

        if (!_selectedNode.IsOccupied)
        {
            if (_buildOptionsGroup != null) _buildOptionsGroup.SetActive(true);
            if (_towerSelectedGroup != null) _towerSelectedGroup.SetActive(false);

            if (_txtTowerNameAndLevel != null) _txtTowerNameAndLevel.text = "Unoccupied";

            bool canAffordBuild = currentGold >= _selectedNode.BuildCost;
            if (_btnBuildBallista != null) _btnBuildBallista.interactable = canAffordBuild;
            if (_btnBuildCannon != null) _btnBuildCannon.interactable = canAffordBuild;
            if (_btnBuildSlow != null) _btnBuildSlow.interactable = canAffordBuild;
        }
        else
        {
            if (_buildOptionsGroup != null) _buildOptionsGroup.SetActive(false);
            if (_towerSelectedGroup != null) _towerSelectedGroup.SetActive(true);

            BaseTurret turret = _selectedNode.PlacedTurret;

            if (_txtTowerNameAndLevel != null)
            {
                if (turret != null)
                {
                    string levelText = _selectedNode.IsUpgraded ? "Lv.2 (MAX)" : "Lv.1";
                    _txtTowerNameAndLevel.text = $"{turret.GetType().Name} ({levelText})";
                }
                else
                {
                    _txtTowerNameAndLevel.text = "Unoccupied";
                }
            }

            bool canAffordUpgrade = currentGold >= _selectedNode.UpgradeCost;
            if (_btnUpgrade != null)
            {
                _btnUpgrade.interactable = !_selectedNode.IsUpgraded && canAffordUpgrade;
            }

            if (_txtUpgradeCost != null)
            {
                _txtUpgradeCost.text = _selectedNode.IsUpgraded ? "MAX" : $"Upgrade: {_selectedNode.UpgradeCost}G";
            }

            if (_txtSellRefund != null)
            {
                _txtSellRefund.text = $"Sell: +{_selectedNode.GetRefundAmount()}G";
            }
        }
    }

    public void CloseMenu()
    {
        if (_actionPanel != null) _actionPanel.SetActive(false);
        _selectedNode = null;
    }

    private void BuildTower(BaseTurret prefab)
    {
        if (_selectedNode != null && !_selectedNode.IsOccupied)
        {
            if (_selectedNode.BuildTurret(prefab))
            {
                CloseMenu();
            }
        }
    }

    private void UpgradeTower()
    {
        if (_selectedNode != null && _selectedNode.IsOccupied)
        {
            if (_selectedNode.UpgradeTurret())
            {
                InspectNode(_selectedNode);
            }
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