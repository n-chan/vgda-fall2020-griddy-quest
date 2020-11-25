using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Selection : MonoBehaviour
{
    public SpriteValue storedPlayerPortrait;
    public SpriteValue storedPlayerSprite;
    public BoolValue unlockedPhea;

    void Start() {
        GameObject.Find("PheaButton").GetComponent<Button>().interactable = unlockedPhea.RuntimeValue;
    }

    public void TransferToMap(Button btn) {
        storedPlayerPortrait.RuntimeValue = btn.transform.Find("CharacterPortraitSaver").GetComponent<SpriteRenderer>().sprite;
        storedPlayerSprite.RuntimeValue = btn.transform.Find("CharacterSpriteSaver").GetComponent<SpriteRenderer>().sprite;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
