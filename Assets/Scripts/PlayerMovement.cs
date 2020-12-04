using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 change;
    private float speed;
    private Rigidbody2D myRigidbody;
    //Player Animations (Sarah)
    private Animator playerAnimator;

    //Jump variables
    private bool Jumped = false;
    private float changeX;
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

    // Start is called before the first frame update
    void Start()
    {
        //Player Animations (Sarah)
        playerAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        audioSrc = GetComponent<AudioSource>();

        isCaught = false;
        if (GetComponent<SpriteRenderer>().sprite != characterSprite.RuntimeValue) {
            GetComponent<SpriteRenderer>().sprite = characterSprite.RuntimeValue;
            //temp fix to allow for switching char
            playerAnimator.enabled = false;
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
        bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift);
        if (isShiftKeyDown) {
            speed = 14f;
        }
        else {
            speed = 8f;
        }
        //Animations for moving  and idle, check method below
        UpdateAnimationAndMove();

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
                if ( change.x > 0 )//player moving to right before jump
                {
                    changeX = horizontalJump;
                }
                else if ( change.x < 0 )//player moving to left before jump
                {
                    changeX = -horizontalJump;
                }
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
    /**
     * UpdateAnimationAndMove is meant to take care of animations like idle, moving
     */
    void UpdateAnimationAndMove()
    {
        //When moving
        if (change != Vector3.zero)
        {
            //Player Animations (Sarah)
            //MoveCharacter();
            playerAnimator.SetFloat("moveX", change.x);
            playerAnimator.SetFloat("moveY", change.y);
            playerAnimator.SetBool("moving", true);
        }
        else
        {
            //Walking animation stops
            playerAnimator.SetBool("moving", false);
        }
    }

    /// <summary>
    /// Makes it so the character jumps.
    /// </summary>
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

    /// <summary>
    /// Resets the player's position back to default (in town).
    /// </summary>
    public void ResetPosition() {
        transform.position = startingPosition.initialValue;
    }
}
