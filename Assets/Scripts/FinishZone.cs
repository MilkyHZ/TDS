using UnityEngine;

public class FinishZone : MonoBehaviour
{
    [SerializeField] 
    private Transform _player;
    [SerializeField]
    private float _finishDistance = 2f;
    [SerializeField]
    private GameObject _winPanel;

    public static bool IsGameWon { get; private set; }

    private void Start() => IsGameWon = false;

    private void Update()
    {
        if (IsGameWon || _player == null) return;

        float dist = Vector3.Distance(transform.position, _player.position);

        if (dist <= _finishDistance)
        {
            IsGameWon = true;
            _winPanel.SetActive(true);
        }
    }
}
