using System.Collections;
using TMPro;
using UnityEngine;

public class PanelTypewriterAndFader : MonoBehaviour
{
    [Header("Text Layout")]
    [SerializeField] private TextMeshProUGUI _txtTitle;
    [SerializeField] private float _typeSpeed = 0.05f;

    [Header("Button Animation")]
    [SerializeField] private CanvasGroup _buttonCanvasGroup;
    [SerializeField] private float _fadeDuration = 0.5f;

    private Coroutine _sequenceCoroutine;
    private string _savedText = "";

    private void Awake()
    {
        if (_txtTitle != null)
        {
            _savedText = _txtTitle.text;
        }

        ResetButtonTarget();
    }

    private void OnEnable()
    {
        if (_sequenceCoroutine != null)
        {
            StopCoroutine(_sequenceCoroutine);
        }

        _sequenceCoroutine = StartCoroutine(PlayDisplaySequence());
    }

    private void OnDisable()
    {
        if (_sequenceCoroutine != null)
        {
            StopCoroutine(_sequenceCoroutine);
        }
        ResetButtonTarget();
    }

    private void ResetButtonTarget()
    {
        if (_buttonCanvasGroup != null)
        {
            _buttonCanvasGroup.alpha = 0f;
            _buttonCanvasGroup.interactable = false;
            _buttonCanvasGroup.blocksRaycasts = false;
        }
    }

    private IEnumerator PlayDisplaySequence()
    {
        if (_txtTitle == null) yield break;

        _txtTitle.text = "";

        foreach (char letter in _savedText.ToCharArray())
        {
            _txtTitle.text += letter;
            yield return new WaitForSecondsRealtime(_typeSpeed);
        }

        if (_buttonCanvasGroup != null)
        {
            float elapsed = 0f;

            while (elapsed < _fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                _buttonCanvasGroup.alpha = Mathf.Clamp01(elapsed / _fadeDuration);
                yield return null;
            }

            _buttonCanvasGroup.alpha = 1f;
            _buttonCanvasGroup.interactable = true;
            _buttonCanvasGroup.blocksRaycasts = true;
        }
    }
}
