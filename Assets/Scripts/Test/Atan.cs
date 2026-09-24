//using UnityEngine;

//public class Atan : MonoBehaviour
//{
//    [SerializeField]
//    private Transform _enemy;
//    [SerializeField]
//    private float _rotSpeed = 5;
//    void Update()
//    {
//        if (_enemy == null) 
//            return;    

//        var dir = _enemy.position - transform.position;

//        var angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
//        //transform.rotation = Quaternion.Slerp(
//        //    transform.rotation,
//        //    Quaternion.Euler(0, angle, 0),
//        //    _rotSpeed * Time.deltaTime);

//        var dot = Vector3.Dot(transform.forward, dir.normalized);
//        Debug.Log(dot);
//    }
//}
