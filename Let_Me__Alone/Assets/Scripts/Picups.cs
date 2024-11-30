using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picups : MonoBehaviour
{
    // 픽업 아이템의 종류를 나타내는 열거형
    private enum PickUpType
    {
        HighPassTicket,    // 통행 티켓: 모든 셀의 연결 가중치를 1로 변경 (1일 동안)
        Shackles,          // 쇠고랑: 가장 먼저 생성된 적 제거
        Dice,              // 주사위: 모든 셀의 연결 가중치를 초기화
        ElectricShockGun,  // 전기 충격기: 적과 접촉 시 적 제거
        MakeupKit          // 변장 도구: 모든 생성된 적 제거
    }

    [SerializeField] private PickUpType pickUpType; // 아이템 유형
    private MapCreator mapCreator; // 맵 데이터를 관리하는 MapCreator 스크립트 참조
    private GameSystem gameSystem; // 게임 상태를 관리하는 GameSystem 스크립트 참조
    private ItemManager itemManager;

    private void Start()
    {
        // 씬에서 MapCreator와 GameSystem 참조 가져오기
        mapCreator = FindObjectOfType<MapCreator>();
        gameSystem = FindObjectOfType<GameSystem>();
        itemManager = FindObjectOfType<ItemManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 충돌했을 때만 아이템 효과 실행
        if (other.gameObject.GetComponent<Player>())
        {
            Debug.Log("아이템과 접촉하였습니다.");
            DetectPickUpType(); // 현재 아이템 유형에 맞는 효과 실행
            Destroy(gameObject); // 아이템을 제거
        }
    }

    private void DetectPickUpType()
    {
        // 현재 픽업 아이템의 유형에 따라 다르게 동작
        switch (pickUpType)
        {
            case PickUpType.HighPassTicket:
                itemManager.haveHighPassTicket = true;
                break;

            case PickUpType.Shackles:
                UseShackles(); // 쇠고랑 사용
                break;

            case PickUpType.Dice:
                UseDice(); // 주사위 사용
                break;

            case PickUpType.ElectricShockGun:
                UseElectricShockGun(); // 전기 충격기 사용
                break;

            case PickUpType.MakeupKit:
                UseMakeupKit(); // 변장 도구 사용
                break;

            default:
                break;
        }
    }

    public void UseHighPassTicket() // 하이패스 티켓
    {
        // 모든 셀의 연결 가중치를 1로 변경
        for (int y = 0; y < mapCreator.height; y++)
        {
            for (int x = 0; x < mapCreator.width; x++)
            {
                for (int dir = 0; dir < 4; dir++)
                {
                    if (mapCreator.weights[y, x, dir] != MapCreator.INF) // 연결 가능한 경우만 변경
                    {
                        mapCreator.weights[y, x, dir] = 1; // 가중치를 1로 설정
                    }
                }
            }
        }

        mapCreator.RefreshGrid();
        Debug.Log("High Pass Ticket 사용: 모든 가중치가 1로 설정되었습니다.");
    }

    public void ResetWeightsAfterOneDay()
    {
        mapCreator.RestoreOriginalWeights();
        mapCreator.RefreshGrid();
        mapCreator.CalculateFloydWarshall(); // 원래 가중치를 기반으로 경로 재계산
        Debug.Log("High Pass Ticket 효과 종료: 가중치가 원래대로 복구되었습니다.");
    }

    private void UseShackles() // 
    {
        // 첫 번째 생성된 적 제거
        if (gameSystem.enemies.Count > 0) // 적이 있는 경우에만 실행
        {
            Enemy firstEnemy = gameSystem.enemies[0]; // 첫 번째 적 가져오기
            Destroy(firstEnemy.gameObject); // 적 객체 제거
            gameSystem.enemies.RemoveAt(0); // 적 리스트에서 제거
            Debug.Log("Shackles 사용: 첫 번째 생성된 적이 제거되었습니다.");
        }
        else
        {
            Debug.Log("Shackles 사용 실패: 제거할 적이 없습니다.");
        }
    }

    private void UseDice()
    {
        // 모든 셀의 연결 가중치를 초기화
        mapCreator.InitializeWeights(); // 초기 가중치로 재설정
        mapCreator.CalculateFloydWarshall(); // 플로이드-워셜 알고리즘 다시 실행
        Debug.Log("Dice 사용: 모든 가중치가 초기화되었습니다.");
    }

    private void UseElectricShockGun()
    {
        // 전기 충격기는 적과 접촉 시 적을 제거하는 지속 효과
        // 이 부분은 적 스크립트에서 접촉 로직을 추가해야 완전 구현 가능
        itemManager.haveElectricShockGun = true;
        Debug.Log("Electric Shock Gun 활성화: 적 제거 효과 준비 완료.");
    }

    private void UseMakeupKit()
    {
        // 현재 생성된 모든 적 제거
        foreach (var enemy in new List<Enemy>(gameSystem.enemies)) // 적 리스트 복사로 안전하게 반복
        {
            Destroy(enemy.gameObject); // 적 객체 제거
        }
        gameSystem.enemies.Clear(); // 적 리스트 초기화
        Debug.Log("Makeup Kit 사용: 모든 적이 제거되었습니다.");
    }
}
