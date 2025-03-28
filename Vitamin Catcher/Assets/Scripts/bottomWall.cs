using UnityEngine;

public class bottomWall : MonoBehaviour
{
    public Manager manager;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
        //manager.StartSpawn();
    }
}
