using System;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour, IDamagable
{
    public string playerName; // Name of the player
    public int health = 100; // Player health
    public GameObject dieEffectPrefab; // Reference to the die effect prefab

    private Material mat;
    private Color originalColor;


    private void OnEnable(){
        // TODO - add an animation event to play the spawn animation tween  
        // Store the original scale so we can return to it later
        Vector3 initialScale = transform.localScale;
        //scale the crate up from 0 to 1 in 1 second using DOTween  
        transform.localScale = Vector3.zero;  
        transform.DOScale(initialScale, 1f).SetEase(Ease.OutBounce);
    }

    private void Awake()
    {
        // Set the player name and initialize player stats
        gameObject.name = playerName;
    }

    private void Start()
    {
        // Initialize material and original color
        mat = GetComponent<Renderer>().material;
        originalColor = mat.color;
        
        //update the player health in the gamemanager
        GameManager.Instance.SetPlayerHealth(health);
    }

    public void TakeDamage(int damage)
    {
        // Reduce health by damage amount
        health -= damage;

        // Trigger the OnObjectDamaged event (optional)
        HealthEventManager.OnObjectDamaged?.Invoke(gameObject.name, health);
        
        //update the player health in the gamemanager
        GameManager.Instance.SetPlayerHealth(health);


        ShowHitEffect();
        
        //TODO - add a camera shake effect when the player is hit  
        FX_EventManager.ShakeCamera(6f,2f,1f );
        //TODO - add a chromatic aberation lerp effect when the player is hit  
        FX_EventManager.ChromaticAberrationLerp(1f, 1.0f);

        // Check if the player has died
        if (health <= 0)
        {
            health = 0;
            Die();

            // Trigger the OnObjectDestroyed event (optional)
            HealthEventManager.OnObjectDestroyed?.Invoke(gameObject.name, health);
        }
    }

    private void Die()
    {
        // Instantiate the die effect at the player's position
        if (dieEffectPrefab != null)
        {
            Instantiate(dieEffectPrefab, transform.position, Quaternion.identity);
        }

        // Optional: Add any additional death logic (e.g., respawn, game over)
       
        //Destroy(gameObject);
        
        //disable the movement and shooting scripts and render the player invisible
        GetComponent<FixedCameraMovementController>().enabled = false;
        GetComponent<Shoot>().enabled = false;
        GetComponent<Renderer>().enabled = false;

        Debug.Log($"Player {playerName} has died.");
    }

    public void ShowHitEffect()
    {
        // Flash the player material red on hit
        mat.color = Color.red;
        Invoke("ResetMaterial", 0.1f);
    }

    private void ResetMaterial()
    {
        // Reset the player material to the original color
        mat.color = originalColor;
    }
}