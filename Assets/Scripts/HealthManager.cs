using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Text healthText;
    private float health;
    private bool unlockedPhea;

    public FloatValue storedCharacterHealth;
    public GameObject characterPortraitBorder;
    public GameObject enemyPortraitBorder;
    public SpriteValue storedCharacterPortrait;
    public SpriteValue storedEnemyPortrait;
    public BoolValue storedUnlockedPhea;

    // Start is called before the first frame update
    void Start()
    {
        healthText.text = storedCharacterHealth.RuntimeValue.ToString();
        health = storedCharacterHealth.RuntimeValue;

        if (storedUnlockedPhea != null) {
            unlockedPhea = storedUnlockedPhea.RuntimeValue;
            GameObject.Find("CharacterScreenSwitchButton").GetComponent<Button>().interactable = unlockedPhea;
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
        if (health != storedCharacterHealth.RuntimeValue) {
            healthText.text = storedCharacterHealth.RuntimeValue.ToString();
        }
    }
}
