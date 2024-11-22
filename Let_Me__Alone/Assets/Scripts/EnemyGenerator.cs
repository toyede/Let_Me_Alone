using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int spawnInterval = 20;  //적 생성 간격
    private GameSystem gameSystem;  
    private int nextSpawnDay = 20;  //다음 적 생성일

    void Start()
    {
        gameSystem = FindObjectOfType<GameSystem>();

        SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        CheckAndSpawnEnemy();
    }

    private void CheckAndSpawnEnemy()
    {
        if(gameSystem.currentDay >= nextSpawnDay)
        {
            SpawnEnemy();
            nextSpawnDay += spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        GameObject newEnemyObject = Instantiate(enemyPrefab);
        Enemy newEnemy = newEnemyObject.GetComponent<Enemy>();

        FindObjectOfType<GameSystem>().AddEnemy(newEnemy);
        

        Debug.Log($"Day {gameSystem.currentDay}: 새로운 적 생성!");
    }
}
