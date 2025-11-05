using UnityEngine;
using DG.Tweening;

public class ExplodingCrate : MonoBehaviour, IDamagable
{
    public int health = 10;
    public GameObject explosionEffectPrefab;
    private Material mat;
    private Color originalColor;

    private void OnEnable(){
        // TODO - add a tween animation to play the spawn animation tween  
        // Store the original scale so we can return to it later
        Vector3 initialScale = transform.localScale;
        //scale the crate up from 0 to 1 in 1 second using DOTween  
        transform.localScale = Vector3.zero;  
        transform.DOScale(initialScale, 1f).SetEase(Ease.OutBounce);
    }
    
    private void Start()
    {
        mat = GetComponent<Renderer>().material;
        originalColor = mat.color;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        // Trigger the OnObjectDamaged event
        HealthEventManager.OnObjectDamaged?.Invoke(gameObject.name, health);

        ShowHitEffect();

        if (health <= 0)
        {
            Explode();

            // Trigger the OnObjectDestroyed event
            HealthEventManager.OnObjectDestroyed?.Invoke(gameObject.name, health);
            Destroy(gameObject);
        }
    }

    public void ShowHitEffect()
    {
        mat.color = Color.red; // Flash red to show a hit effect
        Invoke("ResetMaterial", 0.1f);
    }

    private void ResetMaterial()
    {
        mat.color = originalColor;
    }

    private void Explode()
    {
        // Instantiate explosion effect
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }
        
        //TODO - add and audio feedback when the crate explodes 
    }
}