using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float enemyHealth;
    public float moveSpeed;

    public Dialogue dialogue;
    public BoolValue storedDead;
    public Sprite enemyPortrait;

    public SpriteValue storedEnemyPortrait;
    public FloatValue storedEnemyHealth;

    public bool dialogueStarted;
    public float attackRadius;

    // Start is called before the first frame update
    void Start()
    {
        dialogueStarted = false;
        if (storedDead.RuntimeValue) {
            gameObject.SetActive(false);
        }
        
    }

    public abstract void CheckDistance();

    // Update is called once per frame

}
