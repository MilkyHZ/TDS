using UnityEngine;

public class TrigoMover : MonoBehaviour
{
    [SerializeField]
    private float _xLenght = 1;
    [SerializeField]
    private float _yLenght = 1;
    
    [SerializeField]
    private float _timeMult = 1;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime * _timeMult;

        transform.position += new Vector3(
            _xLenght * Mathf.Cos(timer * Mathf.Deg2Rad),
            _yLenght * Mathf.Sin(timer * Mathf.Deg2Rad),
            0
            ) * Time.deltaTime;
    }
}
