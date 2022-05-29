using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 7.0f;
    [SerializeField] private float maxSpeed = 4.0f;

    [SerializeField] private GameObject outOfBoundsText;
    private float outOfBoundsTimer = 0.0f;
    private const float OUTOFBOUNDS_TIME = 2.0f;

    private Rigidbody playerRb;

    private float bulletTimer = 0.0f;
    private const float BULLET_TIME = 0.45f;
    private Vector3 bulletOffset = new Vector3(0.5f, 0, 0.4f);

    private Vector3 iniPos = new Vector3(-4, 0, -1);

    private bool isInvincible = false;
    private const float SHIELD_TIME_MAIN = 2.0f;
    private const float SHIELD_TIME_LAST = 0.08f;

    [SerializeField] private GameObject spaceship;
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject explosion;

    private AudioSource shieldAudioSource;
    private AudioSource explosionAudioSource;

    private ParticleSystem explosionParticleSystem;
    private const float EXPLOSION_TIME = 2.0f;

    private GameManager gameManager;
    private SpawnManager spawnManager;

    // Start is called before the first frame update
    private void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        playerRb = GetComponent<Rigidbody>();
        shieldAudioSource = shield.GetComponent<AudioSource>();
        explosionAudioSource = explosion.GetComponent<AudioSource>();
        explosionParticleSystem = explosion.GetComponent<ParticleSystem>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (gameManager.isGameActive) HandleShooting();

        if (outOfBoundsTimer > 0)
        {
            outOfBoundsTimer -= Time.deltaTime;
            if (outOfBoundsTimer <= 0)
            {
                outOfBoundsText.gameObject.SetActive(false);
            }
        }
        if (bulletTimer > 0)
        {
            bulletTimer -= Time.deltaTime;
        }
    }

    // FixedUpdate is used when applying physics-related functions
    private void FixedUpdate()
    {
        if (gameManager.isGameActive)
        {
            MovePlayer();
            ConstrainPlayerPosition();
        }
    }

    // Move the player with forces based on WASD or arrows keys input
    void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        playerRb.AddForce(Vector3.up * speed * verticalInput);
        playerRb.AddForce(Vector3.right * speed * horizontalInput);

        // Limit our velocity to maxSpeed
        if (playerRb.velocity.magnitude > maxSpeed)
        {
            playerRb.velocity = playerRb.velocity.normalized * maxSpeed;
        }
    }

    // Prevent the player from leaving screen space, velocity component affected is reset to zero
    // so the player can return to the screen quickly. Also show UI text for some time.
    void ConstrainPlayerPosition()
    {
        bool isOutOfBound = false;
        Vector3 newPos = transform.position;
        Vector3 newVel = playerRb.velocity;

        Vector3 screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        float xBound = screenSize.x;
        float yBound = screenSize.y;

        if (transform.position.y < -yBound)
        {
            newPos.y = -yBound;
            newVel.y = 0;
            isOutOfBound = true;
        }
        else if (transform.position.y > yBound)
        {
            newPos.y = yBound;
            newVel.y = 0;
            isOutOfBound = true;
        }

        if (transform.position.x < -xBound)
        {
            newPos.x = -xBound;
            newVel.x = 0;
            isOutOfBound = true;
        }
        else if (transform.position.x > xBound)
        {
            newPos.x = xBound;
            newVel.x = 0;
            isOutOfBound = true;
        }

        if (isOutOfBound)
        {
            transform.position = newPos;
            playerRb.velocity = newVel;
            outOfBoundsText.gameObject.SetActive(true);
            outOfBoundsTimer = OUTOFBOUNDS_TIME;
        }
    }

    // Shoots a bullet based on space input and a cooldown
    void HandleShooting()
    {
        if (Input.GetKey(KeyCode.Space) && bulletTimer <= 0)
        {
            spawnManager.SpawnBullet(transform.position + bulletOffset);
            bulletTimer = BULLET_TIME;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Junk"))
        {
            // Player can choose to sacrifice shields to destroy junk, ¿score is also increased this way?
            if (!isInvincible)
            {
                gameManager.UpdateShield(collision.gameObject.GetComponent<Junk>().damage);
                if (gameManager.isGameActive) StartCoroutine(ShowShield());
            }
            Destroy(collision.gameObject);
        }
    }

    IEnumerator ShowShield()
    {
        isInvincible = true;
        shield.SetActive(true);
        shieldAudioSource.Play();
        yield return new WaitForSeconds(SHIELD_TIME_MAIN);
        shield.SetActive(false);
        for(int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(SHIELD_TIME_LAST);
            shield.SetActive(true);
            yield return new WaitForSeconds(SHIELD_TIME_LAST);
            shield.SetActive(false);
        }
        isInvincible = false;
    }

    public void Explosion()
    {
        StartCoroutine(ShowExplosion());
    }

    IEnumerator ShowExplosion()
    {
        playerRb.velocity = Vector3.zero;
        spaceship.SetActive(false);
        explosion.SetActive(true);
        explosionAudioSource.Play();
        explosionParticleSystem.Play();
        yield return new WaitForSeconds(EXPLOSION_TIME);
        gameObject.SetActive(false);
    }

    public void ResetPlayer()
    {
        transform.position = iniPos;
        playerRb.velocity = Vector3.zero;
        spaceship.SetActive(true);
        isInvincible = false;
        shield.SetActive(false);
        explosion.SetActive(false);
        bulletTimer = 0;
        outOfBoundsTimer = 0;
        outOfBoundsText.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
