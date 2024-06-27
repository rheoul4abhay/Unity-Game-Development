using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //To switch between scenes or different levels of the game 
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{

    public float speedX = 5f; //Making public so the speed will be visible directly on unity so we can change it anytime
    public float speedY = 8f; //Making the speed  along Y axis as public
    private float inputHorizontal = 0f; //By default player will be at the  ground
    private Rigidbody2D player; //To access the rigidBody2D properties of the player 


    //For ground check to make player be able to jump only if it is grounded and not in mid air 
    public Transform groundCheck; //This will mark the bottom of the player or its feet to keep a track if its in ground
    public float groundCheckRadius; //Radius of the circle to be drawn near the groundChecks position
    public LayerMask groundLayer;  //To actually define what is ground for a player,to make it work,we give layer tags to platforms
    private bool isGrounded;

    //For animator
    private Animator playerAnimation;

    //For respawning player if it falls down the tile or platform
    private Vector3 respawnPoint;
    public GameObject FallDetector;

    //To display text & healthBar in the screen
    public Text scoreText;

    //To update the health if the player takes damage
    public HealthBar healthBar; //HealthBar Class ka object hai ye 'healthBar';

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();

        //As respawn point is always the one from where the player starts from,hence it must be called just for once
        respawnPoint = transform.position;
        scoreText.text = "Score : " + ScoreHandler.totalScore; //String Concatenation
    }

    void Update()
    {
        inputHorizontal = Input.GetAxis("Horizontal"); //To get the information about the keys pressed under 'Horizontal' settings
        player.velocity = new Vector2(inputHorizontal * speedX, player.velocity.y);
        
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        //groundCheck.position refers to the position of groundCheck in each frame as it will follow the feet of the player

        if(inputHorizontal > 0)  //When user presses right or 'd';
        {
            player.velocity = new Vector2(inputHorizontal * speedX, player.velocity.y); //Moving Right
            player.transform.localScale = new Vector2(0.22218f, 0.22218f);//To make player face right

        }
        else if(inputHorizontal < 0){
            player.velocity = new Vector2(inputHorizontal * speedX, player.velocity.y); //Moving Left
            player.transform.localScale = new Vector2(-0.22218f, 0.22218f);//To make player face left
        }
        else  //Means player is not moving as no key is pressed,so its position along x axis is 0;
        {
            player.velocity = new Vector2(0, player.velocity.y);
        }

        if (Input.GetButtonDown("Jump")&& isGrounded)
        {
            player.velocity = new Vector2(player.velocity.x, speedX);
        }

        playerAnimation.SetFloat("Speed", Mathf.Abs(player.velocity.x));
        playerAnimation.SetBool("OnGround", isGrounded);

        //To make the fall detector move along with the player 
        FallDetector.transform.position = new Vector2(transform.position.x, FallDetector.transform.position.y);
        //FallDetector's Y position is same as its own and not of player's because we do not want it to move along Y with
        //player.

        //To make player Climb Ladder
    }

    private void OnTriggerEnter2D(Collider2D collision) //To make something happen if any collision is triggered
    {
        if (collision.tag == "FallDetector")
        {
            transform.position = respawnPoint;
            Health.totalHealth = 1f;
            healthBar.barImage.color = Color.green;
            healthBar.setScale(Health.totalHealth);
        }

        else if(collision.tag == "CheckPoint")
        {
            respawnPoint = transform.position; //To save the players current position as respawn point as soon as it 
            //collides into checkPoint collider
        }
        else if(collision.tag == "NextLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if(collision.tag == "PreviousLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else if(collision.tag == "Crystal Small")
        {
            ScoreHandler.totalScore += 1;
            scoreText.text = "Score : " + ScoreHandler.totalScore;
            collision.gameObject.SetActive(false);//Make crystal dissapear from scene after player have collected crystal
        }
        else if(collision.tag == "Crystal Medium")
        {
            ScoreHandler.totalScore += 2;
            scoreText.text = "Score : " + ScoreHandler.totalScore;
            collision.gameObject.SetActive(false);
        }
        else if(collision.tag == "Crystal Large")
        {
            ScoreHandler.totalScore += 5;
            scoreText.text = "Score : " + ScoreHandler.totalScore;
            collision.gameObject.SetActive(false);
        }
    }

    //We cannot update the health of the player in above method i.e inside OnTriggerEnter2D as it will only decrease 
    //the health of the player just for one time as soon as the player collides with the spike and it will not keep
    //reducing the health of the player if it stands on it,so we need another method for healthBar;

    private void OnTriggerStay2D(Collider2D collision) //i.e this method will work as long as player remains in colliding 
     //state with the spike.
    {
        if(collision.tag == "Spike")
        {
            healthBar.Damage(0.002f);
        }
    }
}
