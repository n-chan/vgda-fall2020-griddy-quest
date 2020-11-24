using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Signal signal;
    public bool playerInRange;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && other.isTrigger) {
            
            playerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player") && other.isTrigger) {
            playerInRange = false;
        }
    }
}
