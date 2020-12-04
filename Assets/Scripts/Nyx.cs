using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nyx : Interactable
{
    public Dialogue dialogue;
    public float enemyHealth;
    public BoolValue storedDead;
    public SpriteValue storedEnemyPortrait;
    public FloatValue storedEnemyHealth;

    public Sprite enemyPortrait;
    // Start is called before the first frame update
    void Start()
    {
        if (storedDead.RuntimeValue) {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange) {
            storedDead.RuntimeValue = true;
            storedEnemyPortrait.RuntimeValue = enemyPortrait;
            storedEnemyHealth.RuntimeValue = enemyHealth;
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, true);
        }
    }
}
