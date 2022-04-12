using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] junk;
    public GameObject powerup;
    public GameObject bullet;

    private float xSpawn;
    private const float X_SPAWN_OFFSET = 1.5f;
    private float ySpawnRange;
    private const float Y_SPAWN_OFFSET = -0.5f;

    private float startDelay = 1.0f;
    private float junkSpawnTime = 1.5f;
    private float powerupSpawnTime = 35.0f;

    private float minDespawnTime = 0.0f;
    private float maxDespawnTime = 3.0f;

    private GameManager gameManager;

    // 0: bullet, 1: junk, 2: powerup
    [SerializeField] private GameObject[] containers;

    // Start is called before the first frame update
    private void Start()
    {
        Vector3 screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        xSpawn = screenSize.x + X_SPAWN_OFFSET;
        ySpawnRange = screenSize.y + Y_SPAWN_OFFSET;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        InvokeRepeating("SpawnRandomJunk", startDelay, junkSpawnTime);
        InvokeRepeating("SpawnPowerup", startDelay, powerupSpawnTime);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void SpawnRandomJunk()
    {
        if (gameManager.isGameActive)
        {
            int randomIndex = Random.Range(0, junk.Length);

            float ySpawn = Random.Range(-ySpawnRange, ySpawnRange);
            Vector3 spawnPos = new Vector3(xSpawn, ySpawn, junk[randomIndex].gameObject.transform.position.z);

            Instantiate(junk[randomIndex], spawnPos, junk[randomIndex].gameObject.transform.rotation, containers[1].transform);
            // Change speed values for new instantiate junk here
        }
    }

    private void SpawnPowerup()
    {
        if (gameManager.isGameActive)
        {
            float ySpawn = Random.Range(-ySpawnRange, ySpawnRange);
            Vector3 spawnPos = new Vector3(xSpawn, ySpawn, powerup.gameObject.transform.position.z);

            Instantiate(powerup, spawnPos, powerup.gameObject.transform.rotation, containers[2].transform);
            // Change speed values for new instantiate powerup here
        }
    }

    public void SpawnBullet(Vector3 pos)
    {
        Instantiate(bullet, pos, bullet.transform.rotation, containers[0].transform);
    }

    public void DespawnAll()
    {
        for(int i = 0; i < containers.Length; i++)
        {
            for(int j = 0; j < containers[i].transform.childCount; j++)
            {
                GameObject despawn = containers[i].transform.GetChild(j).gameObject;
                despawn.GetComponent<Rigidbody>().velocity = Vector3.zero;

                float despawnTime = Random.Range(minDespawnTime, maxDespawnTime);
                Destroy(containers[i].transform.GetChild(j).gameObject, despawnTime);
            }
        }
    }
}
