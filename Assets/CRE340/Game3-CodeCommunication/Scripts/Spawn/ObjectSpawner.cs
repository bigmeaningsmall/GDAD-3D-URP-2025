using UnityEngine;

public class ObjectSpawner : MonoBehaviour  
{  
    public GameObject objectPrefab;  // A single prefab to spawn  
    public Vector3 spawnArea;        // x, y, z (width, height, depth) of the spawn area  
    public float spawnHeight = 0.5f; // Height at which to spawn objects
    public float minSpawnInterval = 1f; // Minimum spawn interval (1 second)  
    public float maxSpawnInterval = 3f; // Maximum spawn interval (3 seconds)  
      
    void Start()  
    {  
        // Start invoking the SpawnObject method at a random interval  
        InvokeRepeating("SpawnRandomObject", Random.Range(minSpawnInterval, maxSpawnInterval), Random.Range(minSpawnInterval, maxSpawnInterval));  
    }  
  
    void SpawnRandomObject()  
    {  
        if (objectPrefab == null) return;  // Ensure there is something to spawn  
  
        // Generate a random position within the spawn area  
        Vector3 randomPosition = new Vector3(  
            Random.Range(-spawnArea.x / 2, spawnArea.x / 2),  
            Random.Range(spawnHeight, spawnHeight + spawnArea.y),  
            Random.Range(-spawnArea.z / 2, spawnArea.z / 2)  
        );  
  
        // Instantiate the prefab at the random position  
        Instantiate(objectPrefab, randomPosition, Quaternion.identity);  
          
        // Reschedule the next spawn with a new random interval  
        CancelInvoke("SpawnRandomObject");  
        Invoke("SpawnRandomObject", Random.Range(minSpawnInterval, maxSpawnInterval));  
    }  
  
    // Method to visualize the spawn area in the Scene view  
    void OnDrawGizmosSelected()  
    {  
        Gizmos.color = Color.green;  
        Gizmos.DrawWireCube(transform.position, spawnArea);  
    }  
}
