using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class IntValue : ScriptableObject, ISerializationCallbackReceiver {
    public int initialValue;

    [HideInInspector]
    public int RuntimeValue;

    public void OnAfterDeserialize() {
        RuntimeValue = initialValue;
    }
    public void OnBeforeSerialize() { }
}
