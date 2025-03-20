using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;

    private Rigidbody rb;
    private Vector3 moveInput;
    private Vector3 dashDirection;
    private AudioSource playerAudioSource;

    public GameObject bulletPrefab;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI gameOverText;
    public Image hpBar;
    public Image dashBar;
    public Button restartButton;
    public AudioClip shootAudio;

    private float bulletSpeed = 10.0f;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float zBound = 24;
    private float xBound = 24;


    private int hp;
    private int maxHp;
    private int dashPoints;
    private int maxDashPoints;
    private int dmg;
    private bool isDashbarRecharhing = false;
    private int money = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //bulletPrefab = GameObject.FindGameObjectWithTag("Bullet");
        hp = 10;
        maxHp = hp;
        dashPoints = 10;
        maxDashPoints = dashPoints;
        dmg = 2;
        money = 0;
        moneyText.text = "Money: " + money;

        playerAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Get movement input
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveInput = new Vector3(h, 0, v).normalized;

        dashBar.fillAmount = Mathf.Clamp01((float)dashPoints / maxDashPoints);

        // Handle dash input
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && dashPoints >= 5)
        {
            // ABSTRACTION
            StartDash();
        }

        // ABSTRACTION
        CheckBoundries();

        // recharge Dashboard
        if (dashPoints < 10 && !isDashbarRecharhing)
        {
            StartCoroutine(RechargeDashbar());
        }

        if (Input.GetMouseButtonDown(0))
        {
            // ABSTRACTION
            ShootBullet();
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.linearVelocity = dashDirection * dashSpeed;

            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0)
            {
                EndDash();
            }
        }
        else
        {
            rb.linearVelocity = moveInput * moveSpeed;
        }
    }

    // ENCAPSULATION hp is a private variable and it can be adjusted by calling public method not dirretcly
    public void SetHp(int hpAdjustion)
    {
        hp += hpAdjustion;
        hpBar.fillAmount = Mathf.Clamp01((float)hp / maxHp);
        if (hp <= 0)
        {
            gameOverText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            Destroy(gameObject);
        }    
    }

    public void SetMoney(int moneyAdjustable)
    {
        money += moneyAdjustable;
        moneyText.text = "Money: " + money;
    }

    public int GetPlayerDmg()
    {
        return dmg;
    }

    // ABSTRACTION
    void StartDash()
    {
        isDashing = true;
        isDashbarRecharhing = false;
        dashTimer = dashDuration;
        dashPoints -= 5;
        dashDirection = moveInput == Vector3.zero ? transform.forward : moveInput;
    }

    // ABSTRACTION
    void ShootBullet()
    {
        // Instantiate the bullet at the shooting point's position and rotation
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

        // Get the Rigidbody2D (if you're using 2D) or Rigidbody (for 3D) to apply force
        var rb = bullet.GetComponent<Rigidbody>();

        // If you're using 3D physics, you would use Rigidbody instead
        if (rb != null)
        {
            playerAudioSource.PlayOneShot(shootAudio, 0.6f);
            // Apply force to the bullet to move it forward
            rb.linearVelocity = transform.forward * bulletSpeed;
        }
    }

    void EndDash()
    {
        isDashing = false;
    }

    // ABSTRACTION
    void CheckBoundries()
    {
        if (transform.position.z < -zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zBound);
        }
        else if (transform.position.x < -xBound)
        {
            transform.position = new Vector3(-xBound, transform.position.y, transform.position.z);
        }

        if (transform.position.z > zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBound);
        }
        else if (transform.position.x > xBound)
        {
            transform.position = new Vector3(xBound, transform.position.y, transform.position.z);
        }
    }

    IEnumerator RechargeDashbar()
    {
        isDashbarRecharhing = true;

        while (dashPoints < 10)
        {
            yield return new WaitForSeconds(1f);
            dashPoints++;
        }

        isDashbarRecharhing = false;
    }
}
