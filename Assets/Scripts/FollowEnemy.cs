using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : Enemy
{
    public PlayerMovement player;
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        /*
        if (storedDead.RuntimeValue) {
            gameObject.SetActive(false);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
    }

    public override void CheckDistance() {
        if (Vector3.Distance(target.position, transform.position) > attackRadius) {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }
}
