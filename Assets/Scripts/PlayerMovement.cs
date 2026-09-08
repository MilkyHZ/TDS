using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Move(new(x,0f,z));
    }

    private void Move(Vector3 dir) 
    {
        dir.Normalize();
        transform.position += dir * speed * Time.deltaTime;
    }
}
