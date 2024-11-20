using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    public int width = 11;
    public int height = 7;
    public int[,,] weights;
    public const int INF = -1;

    public GameObject cellPrefab; //셀을 나타내는 프리팹
    public Sprite[] weightSprites; // 크기가 9인 배열로 가중치에 따라 스프라이트 지정
    public Sprite emptySprite; // 연결이 없는 경우에 표시할 스프라이트

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

                // 오른쪽 연결을 설정
                AddConnectionSprite(cell, weights[i, j, 0], new Vector3(0.5f, 0, 0));

                // 아래쪽 연결을 설정
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
        // 연결 스프라이트를 셀 스프라이트보다 위에 표시하기 위해 sortingOrder를 높은 값으로 설정
        renderer.sortingOrder = 1; // 셀보다 높은 레이어 설정

        if (weight == INF)
        {
            renderer.sprite = emptySprite; // 연결이 없는 경우 emptySprite 사용
        }
        else if (weight > 0 && weight <= 9)
        {
            renderer.sprite = weightSprites[weight - 1]; // 가중치에 맞는 스프라이트 설정
        }
    }
}
