using UnityEngine;
using DG.Tweening;

public class CollectableItem : MonoBehaviour 
{
    public InventoryItem itemData; // Reference to the ScriptableObject

    private InventoryManager inventoryManager; // Reference to the InventoryManager - we will use this to add the item to the inventory
    
    private void OnEnable(){
        // TODO - add a tween animation to play the spawn animation tween  
        // Store the original scale so we can return to it later
        Vector3 initialScale = transform.localScale;
        //scale the crate up from 0 to 1 in 1 second using DOTween  
        transform.localScale = Vector3.zero;  
        transform.DOScale(initialScale, 1f).SetEase(Ease.OutBounce);
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) { // Ensure the player is the one collecting
            GameObject player = other.gameObject;
            if (player.GetComponent<InventoryManager>() != null) {
                inventoryManager = player.GetComponent<InventoryManager>(); // Get the InventoryManager reference
                
                // Check if there's room in the inventory
                if (inventoryManager.CanAddItem()) {
                    Collect(); // Collect the item
                } else {
                    Debug.Log("Cannot collect item, inventory is full");
                }
            }
        }
    }
    
    public void Collect() {
        inventoryManager.AddItem(itemData); // Add the item to the inventory
        Collected();
        
        //TODO - add and audio feedback when collecting an item
        AudioEvent.PlaySFX("Special Powerup", 1.0f, true); // with random pitch
    }

    private void Collected() {
        Destroy(gameObject); // Optionally destroy the item in the scene after collection
    }
}