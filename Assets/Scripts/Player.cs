using UnityEngine;

public class Player :MonoBehaviour
{

    private Rigidbody2D rb;
    public float junpForce = 6f;
    public bool isGround;
    private Animator anim;

    //audio

    private AudioSource audioSource;
    public AudioClip jumpClip;
    public AudioClip dieClip;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isGround)
        {
            PlayerJump();
            audioSource.PlayOneShot(jumpClip);
        }
        anim.SetBool("isJumping", isGround);
    }
    void PlayerJump()
    {
        isGround = false;
        rb.linearVelocity = Vector2.up * junpForce;
        GameManager.instance.IncreaseScore();
    }

    private void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag == "Ground")
        {
            isGround = true;
        }
        if (target.gameObject.tag == "Obstacle")
        {
            audioSource.PlayOneShot(dieClip);

            gameObject.SetActive(false);
            GameManager.instance.GameOver();
        }

    }

}
