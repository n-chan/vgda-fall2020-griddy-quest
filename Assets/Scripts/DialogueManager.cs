using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public LevelLoader loader;
    private bool isSceneChanger;

    public Animator animator;

    private Queue<string> sentences;

    // Start is called before the first frame update
    void Start() {
        sentences = new Queue<string>();
        //StartDialogue(dialogue);
    }

    public void StartDialogue(Dialogue dialogue, bool sc) {
        isSceneChanger = sc;

        if (nameText != null) {
            nameText.text = dialogue.name;
        }
        
        animator.SetBool("IsOpen", true);

        sentences.Clear();
        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }


        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence) {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue() {
        animator.SetBool("IsOpen", false);

        if (isSceneChanger) {
            loader.LoadNextLevel();
        }
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
