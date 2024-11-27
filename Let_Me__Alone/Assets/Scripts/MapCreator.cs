using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MapCreator : MonoBehaviour
{
    public int width = 11;
    public int height = 7;
    public int[,,] weights;
    public const int INF = 99999;

    public GameObject cellPrefab; //셀을 나타내는 프리팹
    public Sprite[] weightSprites; // 크기가 9인 배열로 가중치에 따라 스프라이트 지정
    public Sprite emptySprite; // 연결이 없는 경우에 표시할 스프라이트


    public int[,] shortestPaths;
    public (int, int)[,] nextNode;


    void Start()
    {
        weights = new int[height, width, 4];
        InitializeWeights();
        CreateGrid();

        //플로이드워샬
        CalculateFloydWarshall();
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

        //weight가 INF가 아닌 경우
        if (weight != INF)
        {
            GameObject textObject = new GameObject("WeightText");

            //오른쪽 연결일 경우
            if (position == new Vector3(0.5f, 0, 0))
            {
                textObject.transform.parent = spriteObject.transform;
                textObject.transform.localPosition = new Vector3(0, 0.2f, 0); //텍스트를 위쪽에 출력
            }

            // 아래쪽 연결일 경우
            if (position == new Vector3(0, -0.5f, 0)) // 아래쪽 연결의 위치 & INF가 아님
            {
                spriteObject.transform.localRotation = Quaternion.Euler(0, 0, 90);  //회전 추가 (Z축 기준 90도 회전)
                textObject.transform.parent = spriteObject.transform;
                textObject.transform.localPosition = new Vector3(0, -0.2f ,0);       //텍스트를 오른쪽에 출력
            }
        
            TextMeshPro text = textObject.AddComponent<TextMeshPro>();
            text.text = weight.ToString();

            text.fontSize = 2;
            text.color = Color.black;
            text.alignment = TextAlignmentOptions.Center;
            text.sortingOrder = 2;
        }
    }

    public void CalculateFloydWarshall()
    {
        int n = width * height;
        shortestPaths = new int[n,n];
        nextNode = new (int, int)[n,n];

        //초기화
        for(int i = 0; i < n; i++)
        {
            for(int j = 0; j < n; j++)
            {
                if (i == j)
                    shortestPaths[i, j] = 0;
                else
                    shortestPaths[i, j] = INF;
                nextNode[i, j] = (-1, -1);
            }
        }

        //가중치 입력
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int currentNode = y * width + x;

                for (int dir = 0; dir < 4; dir++)
                {
                    int nx = x + (dir == 0 ? 1 : dir == 2 ? -1 : 0);
                    int ny = y + (dir == 1 ? 1 : dir == 3 ? -1 : 0);

                    if (nx >= 0 && ny >= 0 && nx < width && ny < height)
                    {
                        int neighborNode = ny * width + nx;
                        int weight = weights[y, x, dir];

                        if (weight != INF)
                        {
                            shortestPaths[currentNode, neighborNode] = weight;
                            nextNode[currentNode, neighborNode] = (nx, ny);
                        }
                    }
                }
            }
        }

        //플로이드워샬
        for (int k = 0; k < n; k++)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (shortestPaths[i, k] != INF && shortestPaths[k, j] != INF)
                    {
                        int newDist = shortestPaths[i, k] + shortestPaths[k, j];
                        if (newDist < shortestPaths[i, j])
                        {
                            shortestPaths[i, j] = newDist;
                            nextNode[i, j] = nextNode[i, k];
                        }
                    }
                }
            }
        }

        /* 플로이드워샬 결과 출력
        for (int i = 0; i < shortestPaths.GetLength(0); i++)
        {
            for (int j = 0; j < shortestPaths.GetLength(1); j++)
            {
                Debug.Log($"shortestPaths[{i}, {j}] = {shortestPaths[i, j]}");
            }
        }
        */
    }
}
