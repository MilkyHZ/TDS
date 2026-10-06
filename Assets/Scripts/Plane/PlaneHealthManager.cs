using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaneHealthManager : MonoBehaviour
{
    public static PlaneHealthManager I;

    [SerializeField]
    private int _maxHp = 5;

    private void Start() => I = this;

    private void Update()
    {
        if(_maxHp <= 0) 
            RestartScene();
    }

    public void TakeDamage(int dmg = 1) => _maxHp -= dmg;

    private void RestartScene()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
