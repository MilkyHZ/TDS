using UnityEngine;
using UnityEngine.EventSystems;

public class EnlargeOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Scale Configurations")]
    [SerializeField] private float _hoverScaleMultiplier = 1.1f;
    [SerializeField] private float _scaleSpeed = 15f;

    private Vector3 _initialScale;
    private Vector3 _targetScale;
    private Vector3 _currentDestinationScale;

    private void Awake()
    {
        _initialScale = transform.localScale;
        _targetScale = _initialScale * _hoverScaleMultiplier;
        _currentDestinationScale = _initialScale;
    }

    private void OnDisable()
    {
        transform.localScale = _initialScale;
        _currentDestinationScale = _initialScale;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, _currentDestinationScale, _scaleSpeed * Time.unscaledDeltaTime);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _currentDestinationScale = _targetScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _currentDestinationScale = _initialScale;
    }

    public void OnPointerClick(PointerEventData eventData) 
    {
        if (AudioManager.I != null) AudioManager.I.PlayButtonClick();
    }
}
