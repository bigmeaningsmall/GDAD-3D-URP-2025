using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Vector3 spawnPosition;
    [Range(0, 2)] public float spawnDelay = 0.5f;

    void Start(){
        Invoke("SpawnPlayer", spawnDelay);
    }

    private void SpawnPlayer(){
        //call the spawnplayer function in the gamemanager
        GameManager.Instance.SpawnPlayer(spawnPosition);
    }
}
