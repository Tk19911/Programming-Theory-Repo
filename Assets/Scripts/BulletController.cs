using UnityEngine;

public class BulletController : MonoBehaviour
{
    private PlayerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
            Debug.Log(enemyController);
            Debug.Log(playerController);

            enemyController.SetHp(-playerController.GetPlayerDmg());
            Destroy(gameObject);
        }

    }
}
