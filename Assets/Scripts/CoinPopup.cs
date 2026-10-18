using UnityEngine;

public class CoinPopup : HasPool<CoinPopup>
{
    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private float _distDetection = 10f;

    [SerializeField]
    private int _amount;
    public int Amount 
    {
        get => _amount;
        set => _amount = value;
    }

    private RectTransform _rect;
    private RectTransform _targetUI;
    private float _sqrDistDetection;

    private void Awake() 
    {
        _rect = GetComponent<RectTransform>();
        _sqrDistDetection = _distDetection * _distDetection;
    }
    private void Start() => _targetUI = CoinManager.I.CoinRect;
    private void Update() => MoveAndCheckDistance();

    private void MoveAndCheckDistance()
    {
        _rect.position = Vector3.Lerp(_rect.position, _targetUI.position, _speed * Time.deltaTime);
        if ((_rect.position - _targetUI.position).sqrMagnitude < _sqrDistDetection)
        {
            ExecuteCollection();
        }
    }

    private void ExecuteCollection()
    {
        CoinManager.I.AddCoin(_amount);
        _pool.Release(this);
    }

    public CoinPopup SetSpawn(Vector3 pos)
    {
        _rect.position = pos;
        return this;
    }

    public CoinPopup SetValue(int value)
    {
        _amount = value;
        return this;
    }
}