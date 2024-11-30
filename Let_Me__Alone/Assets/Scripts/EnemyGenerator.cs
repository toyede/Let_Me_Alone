using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject enemyPrefab; // 생성할 적 프리팹
    public int spawnInterval = 20; // 적 생성 간격 (일 단위)
    private GameSystem gameSystem; // GameSystem 스크립트 참조
    private int nextSpawnDay = 20; // 다음 적 생성일

    private TextMeshProUGUI enemySpawnText; // 적 생성 알림 텍스트

    void Start()
    {
        gameSystem = FindObjectOfType<GameSystem>(); // GameSystem 객체를 찾음
        GameObject enemySpawnTextObject = GameObject.Find("EnemySpawnText"); // 적 생성 알림 텍스트 오브젝트 찾기

        if (enemySpawnTextObject != null)
        {
            enemySpawnText = enemySpawnTextObject.GetComponent<TextMeshProUGUI>(); // 텍스트 참조 가져오기
            enemySpawnText.gameObject.SetActive(false); // 초기 상태에서 비활성화
        }

        SpawnEnemy(); // 게임 시작 시 첫 번째 적 생성
    }

    void Update()
    {
        CheckAndSpawnEnemy(); // 매 프레임마다 적 생성 조건 확인
    }

    private void CheckAndSpawnEnemy()
    {
        // 현재 날짜가 다음 생성일 이상일 경우 새로운 적 생성
        if (gameSystem.currentDay >= nextSpawnDay)
        {
            SpawnEnemy();
            nextSpawnDay += spawnInterval; // 다음 생성일 갱신
        }
    }

    private void SpawnEnemy()
    {
        // 새로운 적 생성
        GameObject newEnemyObject = Instantiate(enemyPrefab);
        Enemy newEnemy = newEnemyObject.GetComponent<Enemy>();

        // 생성된 적을 GameSystem에 추가
        FindObjectOfType<GameSystem>().AddEnemy(newEnemy);

        StartCoroutine(ShowEnemySpawnMessage()); // 적 생성 알림 표시
    }

    private IEnumerator ShowEnemySpawnMessage()
    {
        // 적 생성 메시지를 2초간 표시
        if (enemySpawnText != null)
        {
            enemySpawnText.gameObject.SetActive(true); // 메시지 활성화
            yield return new WaitForSeconds(2f);       // 2초 대기
            enemySpawnText.gameObject.SetActive(false); // 메시지 비활성화
        }
    }
}
