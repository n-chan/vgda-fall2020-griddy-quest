using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpriteValue : ScriptableObject, ISerializationCallbackReceiver {
    public Sprite initialValue;

    [HideInInspector]
    public Sprite RuntimeValue;

    public void OnAfterDeserialize() {
        RuntimeValue = initialValue;
    }
    public void OnBeforeSerialize() { }
}
