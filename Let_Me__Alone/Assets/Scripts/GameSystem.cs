using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public int currentDay = 1;
    public Player player;
    

    void Start()
    {
        player = FindObjectOfType<Player>();
        Debug.Log($"게임 시작! 첫 번째 날: Day {currentDay}");
    }

    public void NextDay()
    {
        currentDay++;
        Debug.Log($"Day {currentDay} 시작!");

        // 플레이어와 적 상태 업데이트
        player.UpdateState();
    }


    void Update()
    {
        // 키 입력으로 하루가 지나감 ('N'키를 눌러 다음 날로 이동)
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextDay();
        }
    }
}
