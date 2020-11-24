using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : Interactable {

    public float speed = 0.01f;
    public bool isMoving;
    public Vector2 target;

    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");
        
        //If object is still moving, make the object continue moving...
        if (isMoving) {
            transform.position = Vector2.MoveTowards(transform.position, target, speed);
        }
        else if (Input.GetKeyDown(KeyCode.E) && playerInRange) {
            isMoving = true;
            bool isTop;
            bool isBelow;

            if (player.transform.position.y > transform.position.y + 1.65f) {
                isTop = true;
            }
            else {
                isTop = false;
            }
            if (player.transform.position.y < transform.position.y - 0.2f) {
                isBelow = true;
            }
            else {
                isBelow = false;
            }

            Debug.Log("isTop: " + isTop);
            Debug.Log("isBelow: " + isBelow);

            if (isTop) {
                target.y = target.y - 1.5f;
            }
            else if (isBelow) {
                target.y = target.y + 1.5f;
            }
            else if (player.transform.position.x > transform.position.x) {
                target.x = target.x - 1.5f;
            }
            else if (player.transform.position.x < transform.position.x) {
                target.x = target.x + 1.5f;
            }

            //After 1.2s, change isMoving back to false...
            StartCoroutine(moveObject(1.2f));
        }

    }

    IEnumerator moveObject(float seconds) {
        yield return new WaitForSeconds(seconds);
        isMoving = false;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Enemy") {
            Destroy(gameObject);
        }
    }
}
