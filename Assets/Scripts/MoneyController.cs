using UnityEngine;

public class MoneyController : MonoBehaviour
{
    private AudioSource moneyAudioSource;
    public AudioClip moneyAudio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moneyAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.SetMoney(Random.Range(1, 5));
            moneyAudioSource.PlayOneShot(moneyAudio, 0.4f);
            Destroy(gameObject);
        }
    }
}
