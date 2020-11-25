using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStranger : Log {
    public Dialogue dialogue;
    public bool isDead;
    public BoolValue storedDead;
    public Sprite enemyPortrait;
    public SpriteValue storedEnemyPortrait;
    private bool caughtPlayer = false;
    private bool dialogueStarted = false;
    // Start is called before the first frame update
    void Start() {
        isDead = storedDead.RuntimeValue;
        Debug.Log("isDead? " + isDead);
        if (isDead) {
            gameObject.SetActive(false);
        }
    }

    public override void CheckDistance() {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius) {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            caughtPlayer = true;
        }

        if (Vector3.Distance(target.position, transform.position) < attackRadius) {
            if (dialogueStarted) {
                return;
            }
            else {
                dialogueStarted = true;
                isDead = true;
                storedDead.RuntimeValue = isDead;
                storedEnemyPortrait.RuntimeValue = enemyPortrait;
                FindObjectOfType<DialogueManager>().StartDialogue(dialogue, true);
            }

        }
    }
}
