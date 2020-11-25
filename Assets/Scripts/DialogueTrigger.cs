using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    void Start() {
        //This is done so that the two Start() function does not conflict each other in Prologue Scene. (DialogueManager and DialogueTrigger)
        //Have it wait for 0.5 seconds, before triggering prologue text.
        StartCoroutine(LateStart(0.5f));
    }

    IEnumerator LateStart(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, true);
    }
}
