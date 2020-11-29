using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //private Rigidbody2D myRigidbody;
    public string enemyName;
    public float moveSpeed;
    public PlayerMovement player;
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;

    public Dialogue dialogue;
    public bool isDead;
    public BoolValue storedDead;
    public Sprite enemyPortrait;
    public SpriteValue storedEnemyPortrait;
    public FloatValue storedEnemyHealth;
    public bool dialogueStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        //myRigidbody = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
        Debug.Log(target);

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
            player.isCaught = true;
        }

        if (Vector3.Distance(target.position, transform.position) < attackRadius) {
            if (dialogueStarted) {
                return;
            }
            else {
                isDead = true;
                storedDead.RuntimeValue = isDead;
                storedEnemyPortrait.RuntimeValue = enemyPortrait;
                storedEnemyHealth.RuntimeValue = 30;
                FindObjectOfType<DialogueManager>().StartDialogue(dialogue, true);
                dialogueStarted = true;
            }
        }
    }
}
