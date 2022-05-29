using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junk : MonoBehaviour
{
    [SerializeField] private int health;
    public int damage;
    [SerializeField] private int disintegrationScore;
    private int escapeScore;

    private float disintegrationDelayTime;
    private float escapeDelayTime;

    private GameManager gameManager;

    private AudioSource destroyAudioSource;
    [SerializeField] private AudioClip escapeAudioClip;

    private bool isSetToDestroy = false;
    private float reduceScaleInterval = 5.0f;
    public Material initialDisintegrationMaterial;
    public Material disintegrationMaterial;

    // Start is called before the first frame update
    private void Start()
    {
        escapeScore = -disintegrationScore / 2;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        destroyAudioSource = GetComponent<AudioSource>();
        disintegrationDelayTime = destroyAudioSource.clip.length;
        escapeDelayTime = escapeAudioClip.length;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isSetToDestroy)
        {
            float currentReduceScale = reduceScaleInterval * Time.deltaTime;
            transform.localScale = transform.localScale - new Vector3(currentReduceScale, currentReduceScale, currentReduceScale);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            if (!isSetToDestroy) CollisionWithBullet();
        }
    }

    private void CollisionWithBullet()
    {
        health--;
        if (health == 0) Disintegration();
        else SetMaterial(initialDisintegrationMaterial); // Change material when first hit, this assumes max health is 2
    }

    private void Disintegration()
    {
        // Here add total desintegration effect and play score sound
        isSetToDestroy = true;
        gameManager.UpdateScore(disintegrationScore);
        gameObject.GetComponent<SphereCollider>().enabled = false;
        SetMaterial(disintegrationMaterial);
        destroyAudioSource.Play();
        Destroy(gameObject, disintegrationDelayTime);
    }

    public void Escape()
    {
        if (!isSetToDestroy)
        {
            isSetToDestroy = true;
            gameManager.UpdateScore(escapeScore);
            destroyAudioSource.clip = escapeAudioClip;
            destroyAudioSource.Play();
            Destroy(gameObject, escapeDelayTime);
        }
    }

    private void SetMaterial(Material mat)
    {
        Transform model = gameObject.transform.GetChild(0);
        // Special case for one of the Junk models with 2 objects 
        if (model.name == "Glowing Rock Blue 5")
        {
            model.GetChild(0).GetComponent<MeshRenderer>().material = mat;
            model.GetChild(1).GetComponent<MeshRenderer>().material = mat;
        }
        else model.GetComponent<MeshRenderer>().material = mat;
    }
}

