using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    public int width = 11;
    public int height = 7;
    public int[,,] weights;
    public const int INF = -1;

    public GameObject cellPrefab; //���� ��Ÿ���� ������
    public Sprite[] weightSprites; // ũ�Ⱑ 9�� �迭�� ����ġ�� ���� ��������Ʈ ����
    public Sprite emptySprite; // ������ ���� ��쿡 ǥ���� ��������Ʈ

    void Start()
    {
        weights = new int[height, width, 4];
        InitializeWeights();
        CreateGrid();
    }

    void InitializeWeights()
    {
        System.Random rand = new System.Random();

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (j < width - 1)
                {
                    int weight = rand.Next(10);
                    weights[i, j, 0] = (weight == 0) ? INF : weight;
                    weights[i, j + 1, 2] = (weight == 0) ? INF : weight;
                }
                else
                {
                    weights[i, j, 0] = INF;
                }

                if (i < height - 1)
                {
                    int weight = rand.Next(10);
                    weights[i, j, 1] = (weight == 0) ? INF : weight;
                    weights[i + 1, j, 3] = (weight == 0) ? INF : weight;
                }
                else
                {
                    weights[i, j, 1] = INF;
                }

                if (j == 0) weights[i, j, 2] = INF;
                if (i == 0) weights[i, j, 3] = INF;
            }
        }
    }

    void CreateGrid()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject cell = Instantiate(cellPrefab, new Vector3(j, -i, 0), Quaternion.identity, transform);

                // ������ ������ ����
                AddConnectionSprite(cell, weights[i, j, 0], new Vector3(0.5f, 0, 0));

                // �Ʒ��� ������ ����
                AddConnectionSprite(cell, weights[i, j, 1], new Vector3(0, -0.5f, 0));
            }
        }
    }

    void AddConnectionSprite(GameObject cell, int weight, Vector3 position)
    {
        GameObject spriteObject = new GameObject("Connection");
        spriteObject.transform.parent = cell.transform;
        spriteObject.transform.localPosition = position;

        SpriteRenderer renderer = spriteObject.AddComponent<SpriteRenderer>();
        // ���� ��������Ʈ�� �� ��������Ʈ���� ���� ǥ���ϱ� ���� sortingOrder�� ���� ������ ����
        renderer.sortingOrder = 1; // ������ ���� ���̾� ����

        if (weight == INF)
        {
            renderer.sprite = emptySprite; // ������ ���� ��� emptySprite ���
        }
        else if (weight > 0 && weight <= 9)
        {
            renderer.sprite = weightSprites[weight - 1]; // ����ġ�� �´� ��������Ʈ ����
        }
    }
}
