using System.Collections;
using UnityEngine;

public class Phea : Interactable
{
    public Dialogue dialogue;
    public BoolValue storedUnlockedPhea;
    // Start is called before the first frame update
    void Start()
    {
        if (storedUnlockedPhea.RuntimeValue) {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange) {
            StartCoroutine(TalkToPhea());
        }
    }

    private IEnumerator TalkToPhea() {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, false);
        yield return new WaitForSeconds(2.5f);
        storedUnlockedPhea.RuntimeValue = true;
        gameObject.SetActive(false);
    }

}
