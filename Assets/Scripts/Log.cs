using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : Enemy
{
    //private Rigidbody2D myRigidbody;
    public PlayerMovement player;
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;

    // Start is called before the first frame update
    void Start()
    {
        //myRigidbody = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
        Debug.Log(target);
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
    }

    public virtual void CheckDistance() {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius) {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            //myRigidbody.MovePosition(temp);
        }
    }
}
