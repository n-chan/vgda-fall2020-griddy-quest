using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 change;
    public float speed;
    private Rigidbody2D myRigidbody;

    //Jump variables
    //private float moveZ = 0;
    private bool Jumped = false;
    private bool walkingHorizontallyBeforeJump = false;
    private float changeX;
    private float leftOrRight;//if left neg if right pos
    private float timeInJump = 0f;
    private float maxJumpTime = 1.5f;//to chang how high or long person jumps chang this value and x or y
    private float horizontalJump = 25;
    private float verticalJump = 20;

    public bool isCaught;
    public float moveZ;
    public VectorValue startingPosition;
    public SpriteValue characterSprite;
    public FloatValue characterNum;

    private bool wasMovingVertical;

    bool isMoving = false;

    public AudioSource audioSrc;

    //public FloatValue currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        audioSrc = GetComponent<AudioSource>();

        isCaught = false;
        if (GetComponent<SpriteRenderer>().sprite != characterSprite.RuntimeValue) {
            GetComponent<SpriteRenderer>().sprite = characterSprite.RuntimeValue;
        }
        else {
            GetComponent<SpriteRenderer>().sprite = characterSprite.initialValue;
        }

        if ((Vector2) transform.position != startingPosition.RuntimeValue) {
            transform.position = startingPosition.RuntimeValue;
        }
        else {
            transform.position = startingPosition.initialValue;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //This is for footstep sounds...
        if (change.x != 0 || change.y != 0) {
            isMoving = true;
        }
        else {
            isMoving = false;
        }
        if (isMoving) {
            if (!audioSrc.isPlaying) {
                audioSrc.Play();
            }
        }
        else {
            audioSrc.Stop();
        }

        //change = Vector3.zero;
        if (isCaught) {
            change.x = 0;
            change.y = 0;
        }
        //Jump
        //Add collision detection for jumping onto objects
        else {
            change.x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed + changeX;
            bool isMovingHorizontal = Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f;
            change.y = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed + moveZ;
            bool isMovingVertical = Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.5f;

            if (isMovingVertical && isMovingHorizontal) {
                if (wasMovingVertical) {
                    change.y = 0;
                }
                else {
                    change.x = 0;
                }
            }
            else if (isMovingHorizontal) {
                wasMovingVertical = false;
            }
            else if (isMovingVertical) {
                wasMovingVertical = true;
            }

            change.z = moveZ;
            if (characterNum.RuntimeValue == 1 && Input.GetKeyDown("space")) {
                Jump();
                if ( change.x != 0)//if player was moving before jump
                {
                walkingHorizontallyBeforeJump = true;
                if ( change.x > 0 )//player moving to right before jump
                {
                    changeX = horizontalJump;
                }
                else if ( change.x < 0 )//player moving to left before jump
                {
                    changeX = -horizontalJump;
                }
            }
            else //player not moving before jump
            {
                walkingHorizontallyBeforeJump = false;
            }
            }
            else if (Jumped == true && timeInJump > maxJumpTime) {
                Jumped = false;
                moveZ = -verticalJump;
                change.z = moveZ;
                timeInJump = 0f;
            }
            else if (Jumped == false && timeInJump > maxJumpTime) {
                moveZ = 0;
                changeX = 0;
                change.z = moveZ;
                GetComponent<Collider2D>().enabled = true;
            }

            timeInJump += Time.deltaTime;
        }
    }

    void Jump ()
    {
        timeInJump = 0f;
        Jumped = true;
        moveZ = verticalJump;
        change.z = moveZ;
        GetComponent<Collider2D>().enabled = false;
    }
    

    void FixedUpdate() {
        myRigidbody.MovePosition(transform.position + change.normalized * speed * Time.deltaTime);
        startingPosition.RuntimeValue = transform.position;
    }

    public void ResetPosition() {
        transform.position = startingPosition.initialValue;
    }
}
