using UnityEngine;

public class PatrolEnemy : Enemy
{
    private PlayerMovement player;
    private Transform target;

    public Transform[] path;
    public int currentPoint;
    public Transform currentGoal;
    public float roundingDistance;

    private bool caughtPlayer;
    private Vector3 prevPos;

    private Animator animator;
    public IntValue characterNum;

    public void Start() {
        animator = GetComponent<Animator>();
        if (characterNum.RuntimeValue == 0) {
            target = GameObject.FindWithTag("Kwesi").transform;
            player = target.GetComponent<PlayerMovement>();
        }
        else if (characterNum.RuntimeValue == 1) {
            target = GameObject.FindWithTag("Phea").transform;
            player = target.GetComponent<PlayerMovement>();
        }


        prevPos = transform.position;
        caughtPlayer = false;
        if (storedDead.RuntimeValue) {
            gameObject.SetActive(false);
        }
    }

    public void Update() {
        CheckDistance();
        Vector3 move = transform.position - prevPos;
        animator.SetFloat("moveX", move.x);
        animator.SetFloat("moveY", move.y);
        animator.SetBool("moving", true);
        prevPos = transform.position;
    }

    public override void CheckDistance() {
        Vector3 start = transform.position;
        Vector3 direction = path[currentPoint].position - transform.position;
        direction.Normalize();

        float distance = 6.5f;  // Length of raycast line

        RaycastHit2D[] hits = Physics2D.RaycastAll(start, direction, distance);

        //Checks for ray collision
        for (int it = 0; it < hits.Length; it++) {
            if (hits[it].collider != null && hits[it].collider.tag == "Object") {
                break;
            }
            else if (hits[it].collider != null && (hits[it].collider.tag == "Kwesi" || hits[it].collider.tag == "Phea") && player.moveZ == 0) {
                player.isCaught = true;
                caughtPlayer = true;
            }
        }

        //Draw ray in editor to see in scene editor
        if (caughtPlayer) {
            Debug.DrawLine(start, start + (direction * distance), Color.green, 2.5f, false);
            
        }
        else {
            Debug.DrawLine(start, start + (direction * distance), Color.red, 2.5f, false);
        }
        
        //If raycasts line catches the player
        if (caughtPlayer) {
            // Continue moving towards the Player when the Player is caught.
            if (Vector3.Distance(target.position, transform.position) > attackRadius) {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            }
            else {
                //Once player has stopped, go to the puzzle game.
                if (dialogueStarted) {
                    return;
                }
                else {
                    storedDead.RuntimeValue = true;
                    storedEnemyPortrait.RuntimeValue = enemyPortrait;
                    storedEnemyHealth.RuntimeValue = enemyHealth;
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogue, true);
                    dialogueStarted = true;
                }
                
            }
        }
        //Else, continue its patrol
        else {
            if (Vector3.Distance(transform.position, path[currentPoint].position) > roundingDistance) {
                transform.position = Vector3.MoveTowards(transform.position, path[currentPoint].position, moveSpeed * Time.deltaTime);
            }
            else {
                ChangeGoal();
            }
        }
    }

    /// <summary>
    /// Make enemy switch patrol target location
    /// </summary>
    private void ChangeGoal() {
        if (currentPoint == path.Length - 1) {
            currentPoint = 0;
            currentGoal = path[0];
        }
        else {
            currentPoint++;
            currentGoal = path[currentPoint];
        }
    }
}

