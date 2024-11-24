using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public int currentDay = 1;
    public Player player;
    public List<Enemy> enemies = new List<Enemy>();
    //public  Enemy enemy;
    private bool isstateUpdateEnd = true;
    

    void Start()
    {
        player = FindObjectOfType<Player>();
        //enemy = FindObjectOfType<Enemy>();
        Debug.Log($"게임 시작! 첫 번째 날: Day {currentDay}");
    }

    public void AddEnemy(Enemy newEnemy)
    {
        enemies.Add(newEnemy);
    }

    public void NextDay()
    {
        currentDay++;
        Debug.Log($"Day {currentDay} 시작!");

        // 플레이어와 적 상태 업데이트
    }

    private IEnumerator UpdateGameStateRoutine()
    {
        // 상태 업데이트 (플레이어, 적 등)
        // 특정 컴퓨터에서 이게 씹히는 현상 발생
        player.UpdateState();
        foreach(var enemy in enemies)
        {
            enemy.UpdateState();
            enemy.CompleteMove();
        }

        yield return new WaitForSeconds(0.2f);
        isstateUpdateEnd = true;
    }

    public void EndGame()
    {
        Debug.Log("게임 종료! 적이 플레이어를 잡았습니다.");
        if(player != null)
        {
            Destroy(player.gameObject);
        }
        Time.timeScale = 0; // 게임 멈춤
    }

    void Update()
    {
        // 키 입력으로 하루가 지나감 ('N'키를 눌러 다음 날로 이동)
        if (Input.GetKeyDown(KeyCode.N) && isstateUpdateEnd)
        {
            isstateUpdateEnd = false;
            NextDay();
            StartCoroutine(UpdateGameStateRoutine()); // 상태를 업데이트
        }
    }
}
