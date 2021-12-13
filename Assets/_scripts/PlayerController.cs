using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
public class PlayerController : NetworkBehaviour
{
    public Animator anim;

    //private static PlayerController playerController;


    //Rigidbody2D body;

    float clampx = 9.5f;
    float clampy = 4.5f;

    float horizontal;
    float vertical;

    public int score;
    public Canvas scoreBoard;

    public float runSpeed = 10.0f;
    private bool turnRight;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        // body = GetComponent<Rigidbody2D>();
        if (isLocalPlayer)
        {
            scoreBoard.gameObject.SetActive(true);
            scoreBoard = GameObject.FindGameObjectWithTag("ScoreBoard").GetComponent<Canvas>();
        }
        
        score = 0;
        float minX = -clampx, maxX = clampx ;
        float minY = -clampy, maxY = clampy;
        transform.position = new Vector2(Random.RandomRange(minX, maxX), Random.RandomRange(minY, maxY));
        Transform start = transform;
        //Transform start = new Transform(new Vector2(Random.RandomRange(minX, maxX), Random.RandomRange(minY, maxY)),Quaternion.identity,transform.localScale);
        NetworkManager.RegisterStartPosition(transform);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Collectibles"))
        {
            Destroy(other.gameObject);
            Debug.Log("gameobj destroyed");
            score += 1;
            scoreBoard.GetComponentInChildren<TextMeshProUGUI>().text = score.ToString();
            //scoreBoard.GetComponent<TextMeshProUGUI>().text = score.ToString();
        }
    }
    void HandleMovement()
    {
        if (isLocalPlayer)
        {
            Debug.Log("localPlayer");
            horizontal = Input.GetAxisRaw("Horizontal") * runSpeed;
            vertical = Input.GetAxisRaw("Vertical") * runSpeed;
            //body.velocity = new Vector2(horizontal, vertical);
            anim.SetFloat("Speed", Mathf.Max(Mathf.Abs(horizontal),Mathf.Abs(vertical)));
            

            float x = transform.position.x + horizontal * Time.deltaTime;
            float y = transform.position.y + vertical * Time.deltaTime;
            if (x > clampx)
            {
                x = clampx;
            }
            else if (x < -1 * clampx)
                x = -1 * clampx;
            if (y > clampy)
            {
                y = clampy;
            }
            else if (y < -1 * clampy)
                y = -1 * clampy;

            transform.position = new Vector2(x, y);
            if (turnRight && horizontal > 0)
            {
                Flip();
            }
            else if (!turnRight && horizontal < 0)
            {
                Flip();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        HandleMovement();  
       
    }

    private void FixedUpdate()
    {
        
    }

    void Flip()
    {
        turnRight = !turnRight;
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }
}
