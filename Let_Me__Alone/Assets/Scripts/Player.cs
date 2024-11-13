using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    //public int currentDay = 1; GameSystem���� ����
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

        // �߾� ��ġ�� �ʱ� ����
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
        if (Input.GetKeyDown(KeyCode.W)) Move(Vector3.up, 3);    // ����
        if (Input.GetKeyDown(KeyCode.S)) Move(Vector3.down, 1);  // �Ʒ���
        if (Input.GetKeyDown(KeyCode.A)) Move(Vector3.left, 2);  // ����
        if (Input.GetKeyDown(KeyCode.D)) Move(Vector3.right, 0); // ������
    }

    private void Move(Vector3 direction, int weightIndex)
    {
        // ������ ������ ����ġ Ȯ��
        int weight = mapCreator.weights[currentY, currentX, weightIndex];

        // ����ġ�� INF�� ��� �̵����� ����
        if (weight == MapCreator.INF)
        {
            Debug.Log("�̵� �Ұ�: ������ �����ϴ�.");
            return;
        }

        if (weight > 0)
        {
            daysToWait = weight;
            targetPosition = transform.position + direction;
            isMoving = true;
            Debug.Log($"�̵� ����: {daysToWait}�� �� �̵�");

            // �̵� �Ϸ� �� ���� ��ġ ������Ʈ
            currentX += (int)direction.x;
            currentY -= (int)direction.y;
        }
    }

    public void UpdateState()
    {
        if (daysToWait > 0)
        {
            daysToWait--;
            Debug.Log($"���� ��� �ϼ� {daysToWait}");
        }
        else
        {
            isMoving = false;
            transform.position = targetPosition;
            Debug.Log($"�̵� �Ϸ�. ���� ��ġ: {transform.position}");
        }
    }
}
