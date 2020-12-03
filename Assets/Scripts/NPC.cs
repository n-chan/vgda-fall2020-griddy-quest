using UnityEngine;

public class NPC : Interactable {
    public Dialogue dialogue;
    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange) {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, false);
        }
    }
}
