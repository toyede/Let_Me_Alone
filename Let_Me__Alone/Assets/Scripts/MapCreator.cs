using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MapCreator : MonoBehaviour
{
    public int width = 11; // 맵의 가로 크기
    public int height = 7; // 맵의 세로 크기
    public int[,,] weights; // 각 셀의 방향별 가중치를 저장하는 3차원 배열
    private int[,,] originalWeights; // 원래 가중치를 저장할 배열
    public const int INF = 99999; // 무한 가중치를 나타내는 상수

    public GameObject cellPrefab; // 셀을 나타내는 프리팹
    public Sprite[] weightSprites; // 가중치에 따른 스프라이트 배열
    public Sprite emptySprite; // 연결이 없는 경우 표시할 스프라이트
    public TextMeshPro text;

    public int[,] shortestPaths; // 플로이드-워셜 알고리즘 결과를 저장하는 최단 거리 배열
    public (int, int)[,] nextNode; // 최단 경로 상의 다음 노드 정보를 저장

    void Start()
    {
        weights = new int[height, width, 4]; // 가중치 배열 초기화
        originalWeights = new int[height, width, 4]; // 원래 가중치 배열 초기화
        InitializeWeights(); // 맵의 가중치를 초기화
        BackupWeights(); // 초기 가중치를 백업
        CreateGrid(); // 맵 셀을 생성

        CalculateFloydWarshall(); // 플로이드-워셜 알고리즘 실행
    }

    public void InitializeWeights()
    {
        System.Random rand = new System.Random();

        // 모든 셀의 연결 가중치를 무작위로 초기화
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (j < width - 1) // 오른쪽 연결 초기화
                {
                    int weight = rand.Next(10);
                    weights[i, j, 0] = (weight == 0) ? INF : weight;
                    weights[i, j + 1, 2] = (weight == 0) ? INF : weight;
                }
                else
                {
                    weights[i, j, 0] = INF;
                }

                if (i < height - 1) // 아래쪽 연결 초기화
                {
                    int weight = rand.Next(10);
                    weights[i, j, 1] = (weight == 0) ? INF : weight;
                    weights[i + 1, j, 3] = (weight == 0) ? INF : weight;
                }
                else
                {
                    weights[i, j, 1] = INF;
                }

                if (j == 0) weights[i, j, 2] = INF; // 왼쪽 연결 초기화
                if (i == 0) weights[i, j, 3] = INF; // 위쪽 연결 초기화
            }
        }
    }

    public void BackupWeights()
    {
        // 현재 가중치를 백업 배열에 저장
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                for (int dir = 0; dir < 4; dir++)
                {
                    originalWeights[i, j, dir] = weights[i, j, dir];
                }
            }
        }
        Debug.Log("가중치가 백업되었습니다.");
    }

    public void RestoreOriginalWeights()
    {
        // 백업된 가중치를 복원
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                for (int dir = 0; dir < 4; dir++)
                {
                    weights[i, j, dir] = originalWeights[i, j, dir];
                }
            }
        }
        RefreshGrid(); // 복원된 가중치로 그리드 갱신
        Debug.Log("원래 가중치가 복원되었습니다.");
    }

    public void CreateGrid()
    {
        // 셀을 생성하고 연결 상태를 시각적으로 표시
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject cell = Instantiate(cellPrefab, new Vector3(j, -i, 0), Quaternion.identity, transform);

                // 오른쪽 연결 시각화
                AddConnectionSprite(cell, weights[i, j, 0], new Vector3(0.5f, 0, 0));

                // 아래쪽 연결 시각화
                AddConnectionSprite(cell, weights[i, j, 1], new Vector3(0, -0.5f, 0));
            }
        }
    }

    void AddConnectionSprite(GameObject cell, int weight, Vector3 position)
    {
        // 연결 정보를 표시할 스프라이트 추가
        GameObject spriteObject = new GameObject("Connection");
        spriteObject.transform.parent = cell.transform;
        spriteObject.transform.localPosition = position;

        SpriteRenderer renderer = spriteObject.AddComponent<SpriteRenderer>();
        renderer.sortingOrder = 1; // 셀보다 위에 표시

        if (weight == INF)
        {
            renderer.sprite = emptySprite; // 연결 없음 표시
        }
        else if (weight > 0 && weight <= 9)
        {
            renderer.sprite = weightSprites[weight - 1]; // 가중치에 따른 스프라이트 설정
        }

        // 연결된 경우 가중치 텍스트 추가
        if (weight != INF)
        {
            GameObject textObject = new GameObject("WeightText");

            if (position == new Vector3(0.5f, 0, 0)) // 오른쪽 연결
            {
                textObject.transform.parent = spriteObject.transform;
                textObject.transform.localPosition = new Vector3(0, 0.2f, 0); // 텍스트 위치 조정
            }

            if (position == new Vector3(0, -0.5f, 0)) // 아래쪽 연결
            {
                spriteObject.transform.localRotation = Quaternion.Euler(0, 0, 90); // 회전 적용
                textObject.transform.parent = spriteObject.transform;
                textObject.transform.localPosition = new Vector3(0, -0.2f, 0);
            }

            TextMeshPro text = textObject.AddComponent<TextMeshPro>();
            text.text = weight.ToString(); // 가중치 값 표시
            text.fontSize = 2;
            text.color = Color.black;
            text.alignment = TextAlignmentOptions.Center;
            text.sortingOrder = 2;
        }
    }

    public void RefreshGrid()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        CreateGrid();
    }

    public void CalculateFloydWarshall()
    {
        int n = width * height; // 노드 수
        shortestPaths = new int[n, n];
        nextNode = new (int, int)[n, n];

        // 플로이드-워셜 알고리즘 초기화
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (i == j)
                    shortestPaths[i, j] = 0; // 자기 자신으로 가는 거리는 0
                else
                    shortestPaths[i, j] = INF; // 초기 거리 값은 INF
                nextNode[i, j] = (-1, -1); // 경로 초기화
            }
        }

        // 가중치 입력
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
                            shortestPaths[currentNode, neighborNode] = weight; // 초기 거리 값 설정
                            nextNode[currentNode, neighborNode] = (nx, ny); // 다음 노드 설정
                        }
                    }
                }
            }
        }

        // 플로이드-워셜 알고리즘 실행
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
                            shortestPaths[i, j] = newDist; // 최단 거리 갱신
                            nextNode[i, j] = nextNode[i, k]; // 경로 갱신
                        }
                    }
                }
            }
        }
    }
}
