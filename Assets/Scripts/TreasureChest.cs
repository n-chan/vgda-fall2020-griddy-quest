using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : Interactable
{
    public bool isOpen;
    public BoolValue storedOpen;
    public Sprite openedSprite;
    private Animator anim;
    public AnimationClip sparksAnimation;

    // Start is called before the first frame update
    void Start()
    {
        isOpen = storedOpen.RuntimeValue;
        anim = GameObject.FindWithTag("Player").transform.Find("SparksObject").GetComponent<Animator>();
        if (isOpen) {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange && !isOpen) {
            isOpen = true;
            storedOpen.RuntimeValue = isOpen;
            StartCoroutine(OpenChest());
        }
    }

    private IEnumerator OpenChest() {
        GetComponent<SpriteRenderer>().sprite = openedSprite;
        anim.Play(sparksAnimation.name);
        yield return new WaitForSeconds(sparksAnimation.length);
        anim.SetBool("sparksAnimationCompleted", true);
    }
}
