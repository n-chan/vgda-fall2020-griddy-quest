using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public SpriteValue storedPlayerPortrait;
    public SpriteValue storedPlayerSprite;
    public BoolValue unlockedPhea;
    public IntValue storedCharacterNum;

    void Start() {
        GameObject.Find("PheaButton").GetComponent<Button>().interactable = unlockedPhea.RuntimeValue;
    }

    /// <summary>
    /// Stores the sprite of the character that is picked. Stores the portrait of the character that is picked.
    /// Then, move to next scene.
    /// </summary>
    /// <param name="btn">The character button that is clicked.</param>
    public void TransferToMap(Button btn) {
        storedPlayerPortrait.RuntimeValue = btn.transform.Find("CharacterPortraitSaver").GetComponent<SpriteRenderer>().sprite;
        storedPlayerSprite.RuntimeValue = btn.transform.Find("CharacterSpriteSaver").GetComponent<SpriteRenderer>().sprite;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Stores a number that represents the character that was picked.
    /// </summary>
    /// <param name="i">The number that represents the picked character</param>
    public void SetCharacterNum(int i) {
        storedCharacterNum.RuntimeValue = i;
    }
}
