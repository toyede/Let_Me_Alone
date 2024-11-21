using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 5f;
    private MapCreator mapCreator;
    private Player player;

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
    }


    public void UpdateState()
    {
        if(daysToWait > 0)
        {
            daysToWait--;
            Debug.Log($"�� ���: ���� ��� �ϼ� {daysToWait}");
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

        //�ȶ߱� ������ �⵵�ϼ�
        if (mapCreator.shortestPaths[startNode, targetNode] == MapCreator.INF)
        {
            Debug.Log("�÷��̾�� �� �� �����ϴ�.");
            return;
        }

        (int nextX, int nextY) = mapCreator.nextNode[startNode, targetNode];
        if(nextX != -1 && nextY != -1)
        {
            daysToWait = mapCreator.shortestPaths[startNode, nextY * mapCreator.width + nextX];
            targetPosition = new Vector3(nextX, -nextY, 0);
            isMoving = true;

            // ��ġ ������Ʈ
            currentX = nextX;
            currentY = nextY;
            Debug.Log($"�� �̵� ����: {daysToWait}�� �� �̵� ����");
        }
    }
    public void CompleteMove()
    {
        if (isMoving && daysToWait <= 0)
        {
            transform.position = targetPosition;
            isMoving = false;

            Debug.Log($"�� �̵� �Ϸ�: ���� ��ġ ({currentX}, {currentY})");

            // ��ǥ ��ġ�� �����ϸ� �� ��ǥ ����
            if (currentX == targetX && currentY == targetY)
            {
                targetX = player.GetCurrentX();
                targetY = player.GetCurrentY();
                Debug.Log($"�� ��ǥ ����: �÷��̾� ��ġ ({targetX}, {targetY})");
            }
        }
    }
}
