using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 5f; // 적의 이동 속도
    private MapCreator mapCreator; // 맵 데이터 관리 스크립트 참조
    private Player player; // 플레이어 객체 참조
    private LineRenderer lineRenderer; // 적 이동 경로를 시각적으로 표시하는 라인 렌더러

    private int currentX, currentY; // 현재 적의 위치 (그리드 좌표)
    private int targetX, targetY; // 적의 목표 위치 (그리드 좌표, 기본적으로 플레이어 위치)
    private int daysToWait = 0; // 이동하기 전 대기 일수
    private Vector3 targetPosition; // 적이 이동할 목표 위치
    private bool isMoving = false; // 적이 현재 이동 중인지 여부

    void Start()
    {
        mapCreator = FindObjectOfType<MapCreator>(); // MapCreator 객체를 찾음
        player = FindObjectOfType<Player>(); // Player 객체를 찾음

        // 적의 초기 위치를 맵의 오른쪽 아래로 설정
        currentX = mapCreator.width - 1;
        currentY = mapCreator.height - 1;
        transform.position = new Vector3(currentX, -currentY, 0);

        targetX = player.GetCurrentX(); // 플레이어의 현재 X 좌표를 목표로 설정
        targetY = player.GetCurrentY(); // 플레이어의 현재 Y 좌표를 목표로 설정
        targetPosition = transform.position; // 초기 목표 위치는 현재 위치

        InitLineRenderer(); // 경로 표시를 위한 라인 렌더러 초기화
        UpdatePathLine(); // 이동 경로를 시각적으로 업데이트
    }

    private void InitLineRenderer()
    {
        // 라인 렌더러를 초기화하여 적 이동 경로를 표시
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f; // 라인의 시작 너비
        lineRenderer.endWidth = 0.1f; // 라인의 끝 너비
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = new Color(1, 0, 0, 0.5f); // 라인의 시작 색상 (반투명 빨간색)
        lineRenderer.endColor = new Color(1, 0, 0, 0.5f); // 라인의 끝 색상
        lineRenderer.sortingOrder = 5; // 렌더링 순서 설정
    }

    public void UpdateState()
    {
        // 적이 대기 상태일 경우 대기 일수를 감소
        if (daysToWait > 1)
        {
            daysToWait--;
            Debug.Log($"적 대기: 남은 대기 일수 {daysToWait}");
        }
        else if (!isMoving) // 대기가 끝나면 이동 시작
        {
            MoveToNextNode();
        }
    }

    private void MoveToNextNode()
    {
        // 현재 적의 시작 노드 계산
        int startNode = currentY * mapCreator.width + currentX;

        // 플레이어의 노드(목표 노드) 계산
        int targetNode = targetY * mapCreator.width + targetX;

        // 경로가 없을 경우 처리
        if (mapCreator.shortestPaths[startNode, targetNode] == MapCreator.INF)
        {
            Debug.Log("플레이어에게 갈 수 없습니다.");
            return;
        }

        // 플로이드-워셜 알고리즘을 이용하여 다음 이동할 노드 결정
        (int nextX, int nextY) = mapCreator.nextNode[startNode, targetNode];
        if (nextX != -1 && nextY != -1) // 유효한 다음 노드가 있는 경우
        {
            daysToWait = mapCreator.shortestPaths[startNode, nextY * mapCreator.width + nextX]; // 대기 일수 설정
            targetPosition = new Vector3(nextX, -nextY, 0); // 다음 이동 위치 설정
            isMoving = true; // 이동 상태로 전환

            // 적의 현재 위치를 업데이트
            currentX = nextX;
            currentY = nextY;
            Debug.Log($"적 이동 시작: {daysToWait}일 후 이동 예정");
        }
    }

    public void CompleteMove()
    {
        // 이동 완료 처리
        if (isMoving && daysToWait <= 1)
        {
            transform.position = targetPosition; // 적의 위치를 목표 위치로 업데이트
            isMoving = false; // 이동 종료

            UpdatePathLine(); // 이동 경로를 시각적으로 업데이트

            Debug.Log($"적 이동 완료: 현재 위치 ({currentX}, {currentY})");

            // 목표 위치에 도달한 경우 새로운 목표 설정
            if (currentX == targetX && currentY == targetY)
            {
                targetX = player.GetCurrentX(); // 플레이어의 현재 X 좌표를 목표로 설정
                targetY = player.GetCurrentY(); // 플레이어의 현재 Y 좌표를 목표로 설정
                Debug.Log($"새 목표 설정: 플레이어 위치 ({targetX}, {targetY})");

                UpdatePathLine(); // 경로 시각적으로 갱신
            }
        }
    }

    private void UpdatePathLine()
    {
        // 라인 렌더러를 사용해 적 이동 경로를 업데이트
        int startNode = currentY * mapCreator.width + currentX;
        int targetNode = targetY * mapCreator.width + targetX;

        if (mapCreator.shortestPaths[startNode, targetNode] == MapCreator.INF)
        {
            lineRenderer.positionCount = 0; // 경로가 없을 경우 라인 초기화
            return;
        }

        List<Vector3> pathPoints = new List<Vector3>();
        int currentNode = startNode;

        // 현재 노드에서 목표 노드까지의 경로 계산
        while (currentNode != targetNode)
        {
            int x = currentNode % mapCreator.width;
            int y = currentNode / mapCreator.width;

            pathPoints.Add(new Vector3(x, -y, 0)); // 경로 포인트 추가

            (int nextX, int nextY) = mapCreator.nextNode[currentNode, targetNode];

            if (nextX == -1 && nextY == -1) // 유효하지 않은 경로 처리
            {
                lineRenderer.positionCount = 0;
                return;
            }

            currentNode = nextY * mapCreator.width + nextX;
        }

        pathPoints.Add(new Vector3(targetX, -targetY, 0)); // 목표 위치 추가

        lineRenderer.positionCount = pathPoints.Count;
        lineRenderer.SetPositions(pathPoints.ToArray()); // 라인 렌더러에 경로 설정
    }
}
