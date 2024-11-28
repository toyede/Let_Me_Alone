using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 5f;
    private MapCreator mapCreator;
    private Player player;
    private LineRenderer lineRenderer;

    private int currentX, currentY;
    private int targetX, targetY;
    private int daysToWait = 0;
    private Vector3 targetPosition;
    private bool isMoving = false;


    void Start()
    {
        mapCreator = FindObjectOfType<MapCreator>();
        player = FindObjectOfType<Player>();

        currentX = mapCreator.width - 1;
        currentY = mapCreator.height - 1;
        transform.position = new Vector3(currentX, -currentY, 0);

        targetX = player.GetCurrentX();
        targetY = player.GetCurrentY();
        targetPosition = transform.position;

        InitLineRenderer();
    }

    private void InitLineRenderer()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = new Color(1, 0, 0, 0.5f);
        lineRenderer.endColor = new Color(1, 0, 0, 0.5f);
        lineRenderer.sortingOrder = 5;
    }

    public void UpdateState()
    {
        if(daysToWait > 1)
        {
            daysToWait--;
            Debug.Log($"적 대기: 남은 대기 일수 {daysToWait}");
        }
        else if(!isMoving)
        {
            MoveToNextNode();
        }
    }

    private void MoveToNextNode()
    {
        int startNode = currentY * mapCreator.width + currentX;

        int targetNode = targetY * mapCreator.width + targetX;

        //안뜨길 간절히 기도하셈
        if (mapCreator.shortestPaths[startNode, targetNode] == MapCreator.INF)
        {
            Debug.Log("플레이어에게 갈 수 없습니다.");
            return;
        }

        (int nextX, int nextY) = mapCreator.nextNode[startNode, targetNode];
        if(nextX != -1 && nextY != -1)
        {
            daysToWait = mapCreator.shortestPaths[startNode, nextY * mapCreator.width + nextX];
            targetPosition = new Vector3(nextX, -nextY, 0);
            isMoving = true;

            // 위치 업데이트
            currentX = nextX;
            currentY = nextY;
            Debug.Log($"적 이동 시작: {daysToWait}일 후 이동 예정");

            UpdatePathLine();
        }
    }
    public void CompleteMove()
    {
        if (isMoving && daysToWait <= 1)
        {
            transform.position = targetPosition;
            isMoving = false;

            Debug.Log($"적 이동 완료: 현재 위치 ({currentX}, {currentY})");

            // 목표 위치에 도달하면 새 목표 설정
            if (currentX == targetX && currentY == targetY)
            {
                targetX = player.GetCurrentX();
                targetY = player.GetCurrentY();
                Debug.Log($"새 목표 설정: 플레이어 위치 ({targetX}, {targetY})");
                
                UpdatePathLine();
            }
        }
    }

    private void UpdatePathLine()
    {
        int startNode = currentY * mapCreator.width + currentX;
        int targetNode = targetY * mapCreator.width + targetX;

        if (mapCreator.shortestPaths[startNode, targetNode] == MapCreator.INF)
        {
            lineRenderer.positionCount = 0; // LineRenderer 초기화
            return;
        }

        List<Vector3> pathPoints = new List<Vector3>();
        int currentNode = startNode;

        while (currentNode != targetNode)
        {
            int x = currentNode % mapCreator.width;
            int y = currentNode / mapCreator.width;

            pathPoints.Add(new Vector3(x, -y, 0));

            (int nextX, int nextY) = mapCreator.nextNode[currentNode, targetNode];

            if (nextX == -1 && nextY == -1)
            {
                lineRenderer.positionCount = 0; // LineRenderer 초기화
                return;
            }

            currentNode = nextY * mapCreator.width + nextX;
        }

        pathPoints.Add(new Vector3(targetX, -targetY, 0));

        lineRenderer.positionCount = pathPoints.Count;
        lineRenderer.SetPositions(pathPoints.ToArray());
    }

}
