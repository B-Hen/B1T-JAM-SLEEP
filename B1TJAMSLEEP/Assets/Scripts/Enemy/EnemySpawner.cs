using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyData slimeData;

    private float slimeTimer;

    private void Awake()
    {
        slimeTimer = slimeData.spawnTimer;
    }

    private void Update()
    {
        slimeTimer -= Time.deltaTime;

        if(slimeTimer < 0f)
        {
            slimeTimer = slimeData.spawnTimer;
            SpawnSlime();
        }
    }

    private Vector3 GetRandomPosition()
    {
        return Vector3.one;
    }

    private void SpawnSlime()
    {
        Vector3 position = GetRandomPosition();

        Enemy slimeEnemy = Instantiate(slimeData.enemyPrefab, position, Quaternion.identity);
    }
}
