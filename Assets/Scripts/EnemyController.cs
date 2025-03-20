using JetBrains.Annotations;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody enemyRb;
    private GameObject player;
    public GameObject moneyPrefab;
    protected AudioSource enemyAudio;
    public AudioClip deathAudio;
    public ParticleSystem deathParticle;

    public float speed = 3.0f;
    public int dmg = 2;
    public int hp = 3;
    public float knockbackForce = 15f;
    public float knockbackDuration = 0.3f;
    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
        enemyAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isKnockedBack)
        {
            // Count down the knockback duration
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnockedBack = false;
            }
            return; // Skip movement while knocked back     
        }
        if (player != null)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            enemyRb.linearVelocity = direction * speed;
        }
    }

    public void SetHp(int hpAdjustion)
    {
        hp += hpAdjustion;
        if (hp <= 0)
        {
            Death();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerController = player.GetComponent<PlayerController>();
            playerController.SetHp(-dmg);

            // Knockback logic
            Rigidbody enemyRb = GetComponent<Rigidbody>();
            if (enemyRb != null)
            {
                // Direction: push enemy away from player
                Vector3 knockbackDirection = (transform.position - collision.transform.position).normalized;
                Debug.Log(knockbackDirection);
                // Apply knockback force
                enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
                isKnockedBack = true;
                knockbackTimer = knockbackDuration;
            }
        }   
    }

    public virtual void Death()
    {
        enemyAudio.PlayOneShot(deathAudio, 0.7f);
        var effect = Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(effect, 2f);
        Destroy(gameObject);
        if (Random.Range(1, 10) > 5)
        {
            Instantiate(moneyPrefab, transform.position, moneyPrefab.transform.rotation);
        }
    }
}
