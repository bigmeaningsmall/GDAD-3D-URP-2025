using UnityEngine;
using DG.Tweening;

public class Enemy : EnemyBase
{
    public EnemyData enemyData; // Reference to the EnemyData ScriptableObject
    public GameObject dieEffectPrefab; // Reference to the die effect prefab
    public int damage = 10; // Damage dealt by the enemy

    private int health = 10;
    

    private void OnEnable(){
        // Store the original scale so we can return to it later
        Vector3 initialScale = transform.localScale;
        //scale the crate up from 0 to 1 in 1 second using DOTween  
        transform.localScale = Vector3.zero;  
        transform.DOScale(initialScale, 1f).SetEase(Ease.OutBounce);
    }
    private void Awake()
    {
        // Apply the data from the ScriptableObject to the enemy
        gameObject.name = enemyData.enemyName;
        health = enemyData.health;
        damage = enemyData.damage;
        
        GetComponent<Renderer>().material.color = enemyData.enemyColor;

        Debug.Log($"Enemy {enemyData.enemyName} spawned with {enemyData.health} health and {enemyData.speed} speed.");
    }
    

    // Method to handle taking damage (from player or other sources)
    public override void TakeDamage(int damage)
    {
        health -= damage;

        // Trigger the OnObjectDamaged event
        HealthEventManager.OnObjectDamaged?.Invoke(gameObject.name, health);

        ShowHitEffect();
        
        //TODO - add and audio feedback when the enemy is hit  
        AudioEvent.PlaySFX("Flesh Hit", 1.0f, true); // with random pitch

        if (health <= 0)
        {
            Die();

            // Trigger the OnObjectDestroyed event
            HealthEventManager.OnObjectDestroyed?.Invoke(gameObject.name, health);
        }
    }

    protected override void Die()
    {
        // Instantiate die effect and apply area damage
        if (dieEffectPrefab != null)
        {
            Instantiate(dieEffectPrefab, transform.position, Quaternion.identity);
        }

        //increase the players score in the game manager
        GameManager.Instance.AddScore(10);
        
        //TODO - add and audio feedback when the enemy dies  
        AudioEvent.PlaySFX("Explosion Flesh", 1.0f, true); // with random pitch
        
        // Debug log to show that the enemy has died
        Debug.Log("Enemy has died");
        
        // Optional: add death logic, like spawning loot or playing an animation
        Destroy(gameObject);


        

        

    }

    public override void Move()
    {
        // Define movement specific to this enemy, if needed
    }

    // Method for the enemy to deal damage to another IDamagable object
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the IDamagable interface
        IDamagable damagableObject = collision.gameObject.GetComponent<IDamagable>();
        // Prevent enemy from damaging other enemies (check the tag or another distinguishing property)
        if (damagableObject != null && collision.gameObject.tag != "Enemy")
        {
            // Call TakeDamage on the object, dealing the enemy's damage amount
            damagableObject.TakeDamage(damage);
            Debug.Log($"{gameObject.name} dealt {damage} damage to {collision.gameObject.name}.");
        }
    }
}
