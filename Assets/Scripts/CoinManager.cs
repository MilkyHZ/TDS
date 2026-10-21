using System;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager I;

    [SerializeField] 
    private TextMeshProUGUI _coinText;
    [SerializeField] 
    private RectTransform _coinRect;
    public RectTransform CoinRect => _coinRect;

    [SerializeField] 
    private float _jumpHeight = 20f;
    [SerializeField] 
    private float _lerpSpeed = 3f;

    [SerializeField] private int _coinBank = 300;
    public int CoinBank
    {
        get { return _coinBank; }
        set
        {
            if (_coinBank != value)
            {
                _displayedVal = _coinBank;
                _coinBank = value;
                ChangeValue();
                OnCoinChanged?.Invoke(_coinBank);
            }
        }
    }

    public event Action<int> OnCoinChanged;

    private int _displayedVal;
    private Vector3 _oldPos;
    private float _timer = 1f;

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        if (_coinRect != null)
        {
            _oldPos = _coinRect.position;
        }

        if (_coinText != null)
        {
            _coinText.text = _coinBank.ToString();
        }

        _displayedVal = _coinBank;
    }

    private void Update()
    {
        if (_timer < 1f)
        {
            _timer += Time.deltaTime * _lerpSpeed;
            float t = Mathf.Clamp01(_timer);

            var controlPos = _oldPos + (Vector3.up * _jumpHeight);

            if (_coinRect != null)
            {
                _coinRect.position = Interpolation.QuadraticFast(_oldPos, controlPos, _oldPos, t);
            }

            if (_coinText != null)
            {
                _coinText.text = Mathf.Round(Mathf.Lerp(_displayedVal, _coinBank, t)).ToString();
            }
        }
    }

    private void ChangeValue() => _timer = 0f;

    public void AddCoin(int amount = 10) => CoinBank += amount;

    public bool HasEnoughCoins(int amount) => CoinBank >= amount;

    public void ResetCoins(int initialCoins = 300)
    {
        _coinBank = initialCoins;
        _displayedVal = initialCoins;
        _timer = 1f;

        if (_coinText != null)
        {
            _coinText.text = _coinBank.ToString();
        }

        if (_coinRect != null)
        {
            _coinRect.position = _oldPos;
        }

        CoinPopup[] activePopups = FindObjectsByType<CoinPopup>(FindObjectsSortMode.None);
        foreach (var popup in activePopups)
        {
            popup.Release(popup);
        }

        OnCoinChanged?.Invoke(_coinBank);
    }

    public bool TrySpendCoins(int amount)
    {
        if (HasEnoughCoins(amount))
        {
            CoinBank -= amount;
            return true;
        }
        return false;
    }
}