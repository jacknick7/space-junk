using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField] private float minSpeed; // = 1.0f;
    [SerializeField] private float maxSpeed; // = 6.0f;

    [SerializeField] private float minHorizontalTorque; // = 1.0f;
    [SerializeField] private float maxHorizontalTorque; // = 6.0f;
    [SerializeField] private float minVerticalTorque; // = 1.0f;
    [SerializeField] private float maxVerticalTorque; // = 6.0f;

    // This should not be hardcoded, ask to GameManager or find how to get bounds from camera
    private float xBound;
    private const float X_BOUND_OFFSET = 2.5f;
    private float yBound;
    private const float Y_BOUND_OFFSET = 2.0f;

    private Rigidbody objectRb;

    // Start is called before the first frame update
    private void Start()
    {
        Vector3 screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        xBound = screenSize.x + X_BOUND_OFFSET;
        yBound = screenSize.y + Y_BOUND_OFFSET;
        objectRb = GetComponent<Rigidbody>();
        DefineSetVelocity();
        DefineSetTorque();
    }

    // Update is called once per frame
    private void Update()
    {
        DestroyOutOfBounds();
    }

    private void DefineSetVelocity()
    {
        Vector3 direction;
        if (gameObject.CompareTag("Junk"))
        {
            Vector3 finalPos = new Vector3(-xBound, Random.Range(-yBound, yBound), transform.position.z);
            direction = (finalPos - transform.position).normalized;
        }
        else if (gameObject.CompareTag("Bullet")) direction = Vector3.right;
        else direction = Vector3.left;  // else means that gameObject has tag Powerup
        float speed = Random.Range(minSpeed, maxSpeed);
        objectRb.velocity = direction * speed;
    }

    private void DefineSetTorque()
    {
        float horizontalTorque = Random.Range(minHorizontalTorque, maxHorizontalTorque);
        objectRb.AddTorque(Vector3.up * horizontalTorque, ForceMode.Impulse);

        float verticalTorque = Random.Range(minVerticalTorque, maxVerticalTorque);
        objectRb.AddTorque(Vector3.right * verticalTorque, ForceMode.Impulse);
    }

    private void DestroyOutOfBounds()
    {
        if (transform.position.x < -xBound || transform.position.x > xBound || transform.position.y < -yBound || transform.position.y > yBound)
        {
            if (gameObject.CompareTag("Junk"))
            {
                // add a bool var to junk so Escape isn't called 100 times before beeing destroyed
                gameObject.GetComponent<Junk>().Escape();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

}
