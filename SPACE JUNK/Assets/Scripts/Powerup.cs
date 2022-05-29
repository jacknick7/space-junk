using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private int shield = 40;

    private float usedDelayTime;

    private GameManager gameManager;

    private AudioSource usedAudioSource;

    private bool isSetToDestroy = false;
    private float increaseScaleInterval = 15.0f;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        usedAudioSource = GetComponent<AudioSource>();
        usedDelayTime = usedAudioSource.clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSetToDestroy)
        {
            float currentIncreaseScale = increaseScaleInterval * Time.deltaTime;
            transform.GetChild(1).localScale = transform.GetChild(1).localScale + new Vector3(currentIncreaseScale, currentIncreaseScale, currentIncreaseScale);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isSetToDestroy) CollisionWithPlayer();
        }
    }

    private void CollisionWithPlayer()
    {
        isSetToDestroy = true;
        gameManager.UpdateShield(shield);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        usedAudioSource.Play();
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        Destroy(gameObject, usedDelayTime);
    }
}
