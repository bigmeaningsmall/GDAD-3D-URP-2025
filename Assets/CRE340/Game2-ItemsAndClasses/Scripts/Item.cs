using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class Item with virtual DisplayInfo method
public class Item : MonoBehaviour
{
    protected string itemName;      // Name of the item (accessible to derived classes)
    protected string description;   // Description of the item (accessible to derived classes)
    
    [SerializeField] protected Color itemColor = Color.white; // Color of the item, set in Inspector on the interited classes

    // Default constructor for Item, called when an Item object is created
    public Item()
    {
        itemName = "Generic Item";
        description = "A generic item.";
        Debug.Log("1st Item Constructor Called");
    }

    // Constructor with parameters, allows setting name and description during instantiation
    public Item(string newItemName, string newDescription)
    {
        itemName = newItemName;
        description = newDescription;
        Debug.Log("2nd Item Constructor Called");
    }

    // Virtual method to be overridden in derived classes
    public virtual void DisplayInfo()
    {
        Debug.Log($"{itemName}: {description}");
    }

    // A simple method to greet
    public void SayHello()
    {
        Debug.Log("Hello, I am an item.");
    }
    
    // Rotation speed for the item
    protected float rotationSpeed = 100f;

    private void Update()
    {
        // Rotate every item slowly around the Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
    
    // To prevent multiple clicks during the pulse effect
    private bool canClick = true;
    
    // Simple visual feedback for all items
    protected virtual void OnMouseDown()
    {
        // Pulse scale effect when clicked  
        if (canClick){
            canClick = false;
            StartCoroutine(PulseEffect());  
        }
    }
    // Coroutine to handle the pulse effect
    private IEnumerator PulseEffect()
    {
        Vector3 originalScale = transform.localScale;
        transform.localScale = originalScale * 1.2f;
        yield return new WaitForSeconds(0.2f);
        transform.localScale = originalScale;
        
        canClick = true; // Re-enable clicking after the effect
    }
}
