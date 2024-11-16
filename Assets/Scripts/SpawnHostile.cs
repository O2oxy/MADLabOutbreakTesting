using UnityEngine;

public class SpawnHostile : MonoBehaviour
{
    public GameObject enemyPrefab;       // Enemy prefab to spawn
    public int enemyCount = 5;           // Number of enemies to spawn at the start
    public Vector2 spawnArea = new Vector2(20f, 20f); // Width and height of spawn area

    private void Start()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            // Generate a random position within the spawn area
            Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnArea.x / 2, spawnArea.x / 2),
                0f,  // Set to 0 or adjust for your scene's Y position
                Random.Range(-spawnArea.y / 2, spawnArea.y / 2)
            );

            // Instantiate the enemy at the calculated position
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the spawn area in the editor for visualization
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(spawnArea.x, 0.1f, spawnArea.y));
    }
}
