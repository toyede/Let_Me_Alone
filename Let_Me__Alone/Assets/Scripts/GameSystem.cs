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
        Debug.Log($"���� ����! ù ��° ��: Day {currentDay}");
    }

    public void NextDay()
    {
        currentDay++;
        Debug.Log($"Day {currentDay} ����!");

        // �÷��̾�� �� ���� ������Ʈ
        player.UpdateState();
    }


    void Update()
    {
        // Ű �Է����� �Ϸ簡 ������ ('N'Ű�� ���� ���� ���� �̵�)
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextDay();
        }
    }
}
