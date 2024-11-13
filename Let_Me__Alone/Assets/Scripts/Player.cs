using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    //public int currentDay = 1; GameSystem에서 관리
    private int daysToWait = 0;
    private Vector3 targetPosition;
    private bool isMoving = false;

    public int gridWidth = 11;
    public int gridHeight = 7;

    private MapCreator mapCreator;
    private int currentX, currentY;

    private void Start()
    {
        mapCreator = FindObjectOfType<MapCreator>();

        // 중앙 위치로 초기 설정
        currentX = gridWidth / 2;
        currentY = gridHeight / 2;
        transform.position = new Vector3(currentX, -currentY, 0);

        targetPosition = transform.position;
    }

    void Update()
    {
        if (!isMoving)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) Move(Vector3.up, 3);    // 위쪽
        if (Input.GetKeyDown(KeyCode.S)) Move(Vector3.down, 1);  // 아래쪽
        if (Input.GetKeyDown(KeyCode.A)) Move(Vector3.left, 2);  // 왼쪽
        if (Input.GetKeyDown(KeyCode.D)) Move(Vector3.right, 0); // 오른쪽
    }

    private void Move(Vector3 direction, int weightIndex)
    {
        // 선택한 방향의 가중치 확인
        int weight = mapCreator.weights[currentY, currentX, weightIndex];

        // 가중치가 INF인 경우 이동하지 않음
        if (weight == MapCreator.INF)
        {
            Debug.Log("이동 불가: 연결이 없습니다.");
            return;
        }

        if (weight > 0)
        {
            daysToWait = weight;
            targetPosition = transform.position + direction;
            isMoving = true;
            Debug.Log($"이동 시작: {daysToWait}일 후 이동");

            // 이동 완료 후 현재 위치 업데이트
            currentX += (int)direction.x;
            currentY -= (int)direction.y;
        }
    }

    public void UpdateState()
    {
        if (daysToWait > 0)
        {
            daysToWait--;
            Debug.Log($"남은 대기 일수 {daysToWait}");
        }
        else
        {
            isMoving = false;
            transform.position = targetPosition;
            Debug.Log($"이동 완료. 현재 위치: {transform.position}");
        }
    }
}
