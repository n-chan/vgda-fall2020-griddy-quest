using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitPiece : MonoBehaviour
{
    public enum FruitType {
        APPLE,
        BLUEBERRY,
        DIAMOND,
        GRAPE,
        HONEY,
        KIWI,
        ANY,
        COUNT
    };

    [System.Serializable]
    public struct FruitSprite {
        public FruitType fruitType;
        public Sprite sprite;
    };

    private FruitType fruit;
    public FruitSprite[] fruitSprites;
    private SpriteRenderer sprite;
    private Dictionary<FruitType, Sprite> fruitSpriteDict;

    public FruitType GetFruitType() {
        return fruit;
    }

    public void SetFruitType(FruitType fT) {
        fruit = fT;
    }

    void Awake() {
        sprite = transform.Find("piece").GetComponent<SpriteRenderer>();
        fruitSpriteDict = new Dictionary<FruitType, Sprite>();
        for (int i = 0; i < fruitSprites.Length; i++) {
            if (!fruitSpriteDict.ContainsKey(fruitSprites[i].fruitType)) {
                fruitSpriteDict.Add(fruitSprites[i].fruitType, fruitSprites[i].sprite);
            }
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFruit(FruitType newFruit) {
        fruit = newFruit;

        if (fruitSpriteDict.ContainsKey(newFruit)) {
            sprite.sprite = fruitSpriteDict[newFruit];
        }
    }

    public int GetNumFruits() {
        return fruitSprites.Length;
    }
}
