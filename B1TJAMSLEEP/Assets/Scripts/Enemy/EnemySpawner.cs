using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyData slimeData;
    [SerializeField] private Transform player;

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
        float offset = 0.1f;
        int side = Random.Range(0, 4);
        Vector3 randomPosition = Vector3.zero;

        switch (side)
        {
            case 0: 
                randomPosition = new Vector3(-offset, Random.value, 0);
                break;
            case 1:
                randomPosition = new Vector3(1 + offset, Random.value, 0);
                break;
            case 2: 
                randomPosition = new Vector3(Random.value, -offset, 0);
                break;
            case 3: 
                randomPosition = new Vector3(Random.value, 1 + offset, 0);
                break;
        }

        randomPosition.z = -Camera.main.transform.position.z;

        return Camera.main.ViewportToWorldPoint(randomPosition);
    }

    private void SpawnSlime()
    {
        Vector3 position = GetRandomPosition();
        Enemy slimeEnemy = Instantiate(slimeData.enemyPrefab, position, Quaternion.identity);
        slimeEnemy.Player = player;
    }
}
