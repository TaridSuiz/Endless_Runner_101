using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        
    }
    private void Update()
    {
        if (transform.position.x < -12f)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (!ObstacleSpawner.instance.isGameOver)
        {
            rb.linearVelocity = Vector2.left * speed;
        }
       
    }
}
