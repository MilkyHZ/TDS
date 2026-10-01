using UnityEngine;

public class CoinPopup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private float _distDetection = 1f;
    
    private RectTransform _rect;
    private RectTransform _targetUI;

    private void Awake() => _rect = GetComponent<RectTransform>();

    private void Start() => _targetUI = CoinManager.I.CoinRect;
    
    private void Update()
    {
        _rect.position = Vector3.Lerp(_rect.position, _targetUI.position, _speed * Time.deltaTime);

        var dist = Vector3.Distance(_rect.position, _targetUI.position);
        if (dist < _distDetection) 
        {
            CoinManager.I.AddCoin();
            Destroy(gameObject);    
        }
    }
    public void SetSpawn(Vector2 pos) => _rect.position = pos;
}
