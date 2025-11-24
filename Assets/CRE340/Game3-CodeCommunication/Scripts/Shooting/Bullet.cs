using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1; // The amount of damage the bullet deals

    private Rigidbody rb;
    
    private BulletPool bulletPool;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        // Find the BulletPool manager
        bulletPool = FindObjectOfType<BulletPool>();
        
        // Enable the bullet's collider and reset gravity
        GetComponent<Collider>().enabled = true;
        rb.useGravity = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Stop the bullet's movement and enable gravity
        rb.linearVelocity = Vector3.zero;
        rb.useGravity = true;

        //check if the bullet hit something that has the 'IDamagable' interface  
        if (collision.gameObject.GetComponent<IDamagable>() != null){  
		
            //Get the IDamagable Interface from the collider object
            IDamagable damageable = collision.gameObject.GetComponent<IDamagable>();  
		    
            // Call the IDamagable interface to Take damage and show hit effect 
            damageable.TakeDamage(damage);  
            damageable.ShowHitEffect();  
            
            //TODO - add and audio feedback when hitting an object
            AudioEvent.PlaySFX("Slap Heavy", 1.0f, true); // with random pitch
        }
        
        // Return the bullet to the pool or destroy it if pooling is not enabled
        StartCoroutine(WaitAndDestroy(0.5f));
    }

    private IEnumerator WaitAndDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        if (bulletPool != null)
        {
            bulletPool.ReturnBullet(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}