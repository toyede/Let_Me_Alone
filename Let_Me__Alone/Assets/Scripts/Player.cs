using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f; // 플레이어의 이동 속도
    private int daysToWait = 0;  // 이동하기 전 대기해야 할 일수
    private Vector3 targetPosition; // 플레이어가 이동할 목표 위치
    private bool isMoving = false; // 현재 플레이어가 이동 중인지 여부

    public int gridWidth = 11; // 맵의 가로 크기
    public int gridHeight = 7; // 맵의 세로 크기

    private MapCreator mapCreator; // 맵 데이터를 관리하는 MapCreator 스크립트 참조
    private int currentX, currentY; // 플레이어의 현재 위치 (그리드 좌표)

    public GameObject arrowPrefab; // 이동 방향을 시각적으로 표시할 화살표 프리팹
    private GameObject currentArrow; // 현재 표시 중인 화살표 인스턴스

    private void Start()
    {
        mapCreator = FindObjectOfType<MapCreator>(); // 씬에서 MapCreator 객체를 찾음

        // 플레이어의 초기 위치를 맵 중앙으로 설정
        currentX = gridWidth / 2;
        currentY = gridHeight / 2;
        transform.position = new Vector3(currentX, -currentY, 0);

        targetPosition = transform.position; // 초기 목표 위치를 현재 위치로 설정
    }

    void Update()
    {
        if (!isMoving) // 플레이어가 이동 중이 아닐 때만 입력을 처리
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        // 방향 키 입력에 따라 이동 시작
        if (Input.GetKeyDown(KeyCode.W)) Move(Vector3.up, 3);    // 위로 이동
        if (Input.GetKeyDown(KeyCode.S)) Move(Vector3.down, 1);  // 아래로 이동
        if (Input.GetKeyDown(KeyCode.A)) Move(Vector3.left, 2);  // 왼쪽으로 이동
        if (Input.GetKeyDown(KeyCode.D)) Move(Vector3.right, 0); // 오른쪽으로 이동
    }

    private void Move(Vector3 direction, int weightIndex)
    {
        // 해당 방향의 가중치 값을 가져옴
        int weight = mapCreator.weights[currentY, currentX, weightIndex];

        // 가중치가 INF(이동 불가능)일 경우 이동 중단
        if (weight == MapCreator.INF)
        {
            Debug.Log("이동 불가: 연결이 없습니다.");
            return;
        }

        // 이동 방향에 화살표 표시
        currentArrow = Instantiate(arrowPrefab, transform.position + direction * 0.3f, Quaternion.identity);

        // 방향에 따라 화살표의 회전 설정
        if (direction == Vector3.up) currentArrow.transform.rotation = Quaternion.Euler(0, 0, -90);
        else if (direction == Vector3.down) currentArrow.transform.rotation = Quaternion.Euler(0, 0, 90);
        else if (direction == Vector3.left) currentArrow.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (direction == Vector3.right) currentArrow.transform.rotation = Quaternion.Euler(0, 0, 180);

        if (weight > 0)
        {
            daysToWait = weight; // 가중치(대기 일수) 설정
            targetPosition = transform.position + direction; // 이동 목표 위치 설정
            isMoving = true; // 이동 상태로 전환
            Debug.Log($"이동 시작: {daysToWait}일 후 이동");

            // 이동 후 현재 위치 업데이트
            currentX += (int)direction.x;
            currentY -= (int)direction.y;
        }
    }

    private void DestroyArrow()
    {
        // 기존 화살표를 삭제
        if (currentArrow != null)
        {
            Destroy(currentArrow);
            currentArrow = null;
        }
    }

    public void UpdateState()
    {
        // 대기 일수를 줄이거나 이동 완료 처리
        if (daysToWait > 1)
        {
            daysToWait--;
            Debug.Log($"남은 대기 일수 {daysToWait}");
        }
        else
        {
            isMoving = false; // 이동 종료
            transform.position = targetPosition; // 플레이어 위치를 목표 위치로 갱신
            DestroyArrow(); // 화살표 제거
            Debug.Log($"이동 완료. 현재 위치: {transform.position}");
        }
    }

    public int GetEffectiveX()
    {
        // 이동 중일 경우 현재 위치를 반환
        if (isMoving)
            return (int)(transform.position.x); // 실제 위치 기준
        return currentX; // 이동 중이 아닐 경우 기본 좌표 반환
    }

    public int GetEffectiveY()
    {
        if (isMoving)
            return (int)(-transform.position.y);
        return currentY;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) // 충돌한 객체가 적인지 확인
        {
            FindObjectOfType<GameSystem>().EndGame(); // 적과 충돌 시 게임 종료
        }
    }
}
