using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PatrolLog : Log
{
    public Transform[] path;
    public int currentPoint;
    public Transform currentGoal;
    public float roundingDistance;

    private bool caughtPlayer = false;

    public void Start() {
        isDead = storedDead.RuntimeValue;
        Debug.Log("isDead? " + isDead);
        if (isDead) {
            gameObject.SetActive(false);
        }
    }
    public override void CheckDistance() {
        Vector3 start = transform.position;
        Vector3 direction = path[currentPoint].position - transform.position;
        direction.Normalize();

        float distance = 2.5f;
        

        RaycastHit2D[] hits = Physics2D.RaycastAll(start, direction, distance);

        for (int it = 0; it < hits.Length; it++) {
            if (hits[it].collider != null && hits[it].collider.tag == "Object") {
                break;
            }
            else if (hits[it].collider != null && hits[it].collider.tag == "Player" && player.moveZ == 0) {
                player.isCaught = true;
                caughtPlayer = true;
            }
        }

        //draw ray in editor to see in scene editor
        if (player.isCaught) {
            Debug.DrawLine(start, start + (direction * distance), Color.green, 2.5f, false);
            
        }
        else {
            Debug.DrawLine(start, start + (direction * distance), Color.red, 2.5f, false);
        }
        
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
                    isDead = true;
                    storedDead.RuntimeValue = isDead;
                    storedEnemyPortrait.RuntimeValue = enemyPortrait;
                    storedEnemyHealth.RuntimeValue = 20;
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogue, true);
                    dialogueStarted = true;
                }
                
                Debug.Log("Done");
            }
        }
        else {
            if (Vector3.Distance(transform.position, path[currentPoint].position) > roundingDistance) {
                transform.position = Vector3.MoveTowards(transform.position, path[currentPoint].position, moveSpeed * Time.deltaTime);
            }
            else {
                ChangeGoal();
            }
        }
    }

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


/*
  if (player.isCaught) {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
        if (Vector3.Distance(target.position, transform.position) > attackRadius && 
            currentGoal == path[1] && Mathf.Abs(target.position.y - transform.position.y) <= 1.5 && 
            target.position.x >= transform.position.x 
            && target.position.x <= path[1].position.x) {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                Debug.Log("this happened");
                player.isCaught = true;
        }
        else {
            if (Vector3.Distance(transform.position, path[currentPoint].position) > roundingDistance) {
                transform.position = Vector3.MoveTowards(transform.position, path[currentPoint].position, moveSpeed * Time.deltaTime);
            }
            else {
                ChangeGoal();
            }
        }

    */

