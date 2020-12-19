using UnityEngine;

public class SentryEnemy : Enemy
{
    public PlayerMovement player;
    private Transform target;
    public float chaseRadius;
    private Vector3 originalPosition;

    void Start() {
        target = GameObject.FindWithTag("Player").transform;
        originalPosition = transform.position;

        if (storedDead.RuntimeValue) {
            gameObject.SetActive(false);
        }
    }

    void Update() {
        CheckDistance();
    }

    public override void CheckDistance() {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius) {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
        else if (Vector3.Distance(target.position, transform.position) > chaseRadius) {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(target.position, transform.position) < attackRadius) {
            if (dialogueStarted) {
                return;
            }
            else {
                if (Vector3.Distance(target.position, transform.position) <= attackRadius) {
                    player.isCaught = true;
                }
                storedDead.RuntimeValue = true; 
                storedEnemyPortrait.RuntimeValue = enemyPortrait;
                storedEnemyHealth.RuntimeValue = enemyHealth;
                FindObjectOfType<DialogueManager>().StartDialogue(dialogue, true);
                dialogueStarted = true;
            }
        }
    }
}
