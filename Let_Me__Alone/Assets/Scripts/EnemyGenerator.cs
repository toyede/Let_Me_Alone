using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int spawnInterval = 20;  //적 생성 간격
    private GameSystem gameSystem;  
    private int nextSpawnDay = 20;  //다음 적 생성일
    
    private TextMeshProUGUI enemySpawnText;

    void Start()
    {
        gameSystem = FindObjectOfType<GameSystem>();
        GameObject enemySpawnTextObject = GameObject.Find("EnemySpawnText");

        if (enemySpawnTextObject != null)
        {
            enemySpawnText = enemySpawnTextObject.GetComponent<TextMeshProUGUI>();
            enemySpawnText.gameObject.SetActive(false);
        }

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

        StartCoroutine(ShowEnemySpawnMessage());
    }

    private IEnumerator ShowEnemySpawnMessage()
    {
        if (enemySpawnText != null)
        {
            enemySpawnText.gameObject.SetActive(true);  // 메시지 활성화
            yield return new WaitForSeconds(2f);        // 2초 후
            enemySpawnText.gameObject.SetActive(false); // 메시지 비활성화
        }
    }
}
