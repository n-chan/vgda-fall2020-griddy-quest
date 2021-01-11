using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCharacter : MonoBehaviour
{
    public GameObject kwesi, phea;
    public IntValue characterNum;

    // Start is called before the first frame update
    void Start()
    {
        switch(characterNum.RuntimeValue) {
            case 0:
                kwesi.gameObject.SetActive(true);
                phea.gameObject.SetActive(false);
                break;
            case 1:
                phea.gameObject.SetActive(true);
                kwesi.gameObject.SetActive(false);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
