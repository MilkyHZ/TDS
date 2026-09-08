using UnityEngine;

public class FinishZone : MonoBehaviour
{
    [SerializeField]
    private Transform _player;

    [SerializeField]
    private float _finishDistance = 2f;

    [SerializeField]
    private GameObject _winPanel;

    private void Update()
    {
        float dist = Vector3.Distance(transform.position, _player.position);

        if (dist <= _finishDistance)
        {
            _winPanel.SetActive(true);
        }
    }
}
