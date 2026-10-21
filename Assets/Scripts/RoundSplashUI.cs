using System.Collections;
using TMPro;
using UnityEngine;

public class RoundSplashUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _txtRoundStartTitle;
    [SerializeField] private float _typeSpeed = 0.05f;

    private Coroutine _typeCoroutine;
    private string _currentText = "";

    public void ShowBanner(string fullText, bool instantDisplay = false)
    {
        if (_currentText == fullText && gameObject.activeSelf) return;

        _currentText = fullText;
        gameObject.SetActive(true);

        if (_typeCoroutine != null)
        {
            StopCoroutine(_typeCoroutine);
        }

        if (instantDisplay)
        {
            _txtRoundStartTitle.text = fullText;
        }
        else
        {
            _typeCoroutine = StartCoroutine(TypeText(fullText));
        }
    }

    public void HideBanner()
    {
        _currentText = "";
        gameObject.SetActive(false);
    }

    private IEnumerator TypeText(string textToType)
    {
        _txtRoundStartTitle.text = "";

        foreach (char letter in textToType.ToCharArray())
        {
            _txtRoundStartTitle.text += letter;
            yield return new WaitForSecondsRealtime(_typeSpeed);
        }
    }
}
