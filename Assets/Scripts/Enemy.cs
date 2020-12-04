using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public float moveSpeed;
    public PlayerMovement player;
    public Transform target;
    public float chaseRadius;
    public float attackRadius;

    public Dialogue dialogue;
    public bool isDead;
    public BoolValue storedDead;
    public Sprite enemyPortrait;
    public SpriteValue storedEnemyPortrait;
    public FloatValue storedEnemyHealth;
    public bool dialogueStarted = false;

    public float enemyHealth;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;

        isDead = storedDead.RuntimeValue;
        if (isDead) {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
    }

    public virtual void CheckDistance() {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius) {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(target.position, transform.position) < attackRadius) {
            if (dialogueStarted) {
                return;
            }
            else {
                if (Vector3.Distance(target.position, transform.position) <= attackRadius) {
                    player.isCaught = true;
                }
                isDead = true;
                storedDead.RuntimeValue = isDead;
                storedEnemyPortrait.RuntimeValue = enemyPortrait;
                storedEnemyHealth.RuntimeValue = enemyHealth;
                FindObjectOfType<DialogueManager>().StartDialogue(dialogue, true);
                dialogueStarted = true;
            }
        }
    }
}
