using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    public Text healthText;
    private float health;

    public FloatValue storedCharacterHealth;
    public GameObject characterPortraitBorder;
    public GameObject enemyPortraitBorder;
    public SpriteValue storedCharacterPortrait;
    public SpriteValue storedEnemyPortrait;
    public BoolValue storedUnlockedPhea;

    // Start is called before the first frame update
    void Start()
    {
        if (healthText!= null) {
            healthText.text = "Moves Remaining:\n" + storedCharacterHealth.RuntimeValue.ToString();
            health = storedCharacterHealth.RuntimeValue;
        }

        if (characterPortraitBorder != null) {
            characterPortraitBorder.transform.Find("characterPortrait").GetComponent<SpriteRenderer>().sprite = storedCharacterPortrait.RuntimeValue;
        }
        if (enemyPortraitBorder != null) {
            enemyPortraitBorder.transform.Find("enemyPortrait").GetComponent<SpriteRenderer>().sprite = storedEnemyPortrait.RuntimeValue;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (healthText != null && health != storedCharacterHealth.RuntimeValue) {
            healthText.text = "Moves Remaining:\n" + storedCharacterHealth.RuntimeValue.ToString();
        }

        //TODO: Make this better.
        if (GameObject.Find("CharacterScreenSwitchButton") != null) {
            GameObject.Find("CharacterScreenSwitchButton").GetComponent<Button>().interactable = storedUnlockedPhea.RuntimeValue;
        }
        
    }
}
