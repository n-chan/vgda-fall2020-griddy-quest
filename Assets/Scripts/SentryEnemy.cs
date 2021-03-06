﻿using UnityEngine;

public class SentryEnemy : Enemy
{
    private PlayerMovement player;
    private Transform target;
    public IntValue characterNum;

    public float chaseRadius;
    private Vector3 originalPosition;
    private Animator animator;

    void Start() {
        if (characterNum.RuntimeValue == 0) {
            target = GameObject.FindWithTag("Kwesi").transform;
            player = target.GetComponent<PlayerMovement>();
        }
        else if (characterNum.RuntimeValue == 1) {
            target = GameObject.FindWithTag("Phea").transform;
            player = target.GetComponent<PlayerMovement>();
        }

        animator = GetComponent<Animator>();
        originalPosition = transform.position;

        if (storedDead.RuntimeValue) {
            gameObject.SetActive(false);
        }
    }

    void Update() {
        CheckDistance();
        if (transform.position == originalPosition) {
            animator.SetBool("walking", false);
        }
        else {
            animator.SetBool("walking", true);
        }
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
