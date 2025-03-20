using UnityEngine;

// INHERITANCE
public class FoxController : EnemyController
{
    // POLYMORPHISM
    public override void Death()
    {
        enemyAudio.PlayOneShot(deathAudio, 1.7f);
        var effect = Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(effect, 3f);
        Destroy(gameObject);
        if (Random.Range(1, 10) >= 5)
        {
            Instantiate(moneyPrefab, transform.position, moneyPrefab.transform.rotation);
        }
    }
}
