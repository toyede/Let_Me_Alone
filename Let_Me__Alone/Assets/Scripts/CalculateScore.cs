using System.Collections.Generic;
using UnityEngine;

public class CalculateScore : MonoBehaviour
{
    public int targetAmount = 400; // 계산할 목표 금액 (거스름돈)
    private int[] gemValues = { 10, 80, 100 }; // Gem의 가치
    private string[] gemNames = { "Gem1", "Gem2", "Gem3" }; // Gem 이름
    private int currentFullGemsNum = 0;
    private int optimizedGemsNum = 0;
    private int resultGemNum = 0;
    [SerializeField] public bool isClear = true;
    GemManager gemManager;
    
    void Awake()
    {
        // GemManager를 통해 현재 사용 가능한 Gem 개수 가져오기
        gemManager = FindObjectOfType<GemManager>();
        currentFullGemsNum = gemManager.currentGem1 + gemManager.currentGem2 + gemManager.currentGem3;

        if (gemManager != null)
        {
            Debug.Log($"현재 Gem 개수: Gem1={gemManager.currentGem1}, Gem2={gemManager.currentGem2}, Gem3={gemManager.currentGem3}");

            // DP 동전 알고리즘 (거스름돈 targetAmount, gem의 액면 gemvalues개, 사용가능한 각 동전 개수)
            var result = CalculateMinimumGems(targetAmount, gemValues, new int[] 
            {
                gemManager.currentGem1, // 사용 가능한 Gem1 개수
                gemManager.currentGem2, // 사용 가능한 Gem2 개수
                gemManager.currentGem3  // 사용 가능한 Gem3 개수
            });

            // 결과 출력
            if (result != null)
            {
                Debug.Log($"거스름돈 {targetAmount}원을 구성하는 최소 Gem 조합:");
                for (int i = 0; i < result.Length; i++)
                {
                    Debug.Log($"{gemNames[i]}: {result[i]}개");
                    optimizedGemsNum += result[i];
                }
                Debug.Log($"총 {optimizedGemsNum}개");
            }
            else
            {
                isClear = false;
                optimizedGemsNum = 0;
                Debug.Log("거스름돈을 구성할 수 없습니다.");
            }
        }
    }

    // DP 알고리즘: 최소 Gem 조합 계산
    private int[] CalculateMinimumGems(int amount, int[] values, int[] available) // 사용가능한 Gem 개수
    {
        int n = values.Length; // Gem 종류 개수
        int[] dp = new int[amount + 1]; // dp[i]: i원을 구성하는 최소 Gem 개수
        int[][] gemUsed = new int[amount + 1][]; // gemUsed[i]: i원을 구성하기 위해 사용된 Gem 조합

        // DP 테이블 초기화
        for (int i = 0; i <= amount; i++)
        {
            dp[i] = int.MaxValue; // 초기값 무한대
            gemUsed[i] = new int[n]; // Gem 사용 배열 초기화
        }
        dp[0] = 0; // 0원을 구성하는 데 필요한 Gem은 0개

        // DP 알고리즘 수행
        for (int i = 0; i <= amount; i++) // 도달 불가능한 금액은 스킵해도 되도록 만드는 반복문
        {
            if (dp[i] == int.MaxValue) continue; // 맨처음 0은 여기 안걸림

            for (int j = 0; j < n; j++) // 모든 Gem 타입에 대해 반복
            {
                if (i + values[j] <= amount && gemUsed[i][j] < available[j]) 
                // Gem을 추가했을때 금액 초과? 0(다른 잼 계산 아직 안됨) + 10 <= 40 && i원을 내야 할 때 쓴 10원 개수 < 사용가능 10원 개수
                {
                    // 기존 dp[i + values[j]]보다 적은 Gem 개수로 구성 가능하다면 업데이트
                    if (dp[i] + 1 < dp[i + values[j]])
                    {
                        dp[i + values[j]] = dp[i] + 1;

                        // 기존 Gem 사용 배열 복사
                        for (int k = 0; k < n; k++)
                        {
                            gemUsed[i + values[j]][k] = gemUsed[i][k];
                        }
                        // 현재 Gem 타입 사용 개수 증가
                        gemUsed[i + values[j]][j]++;
                    }
                }
            }
        }

        // 목표 금액을 구성할 수 없는 경우 null 반환
        if (dp[amount] == int.MaxValue)
        {
            Debug.Log("DP 결과: 목표 금액 구성 불가능");
            return null;
        }

        // 목표 금액을 구성하는 Gem 조합 반환
        return gemUsed[amount];
    }

    public int GetResultGemNum()
    {
        if(optimizedGemsNum != 0)
        {
            Debug.Log($"가지고 온 보석의 수는 {currentFullGemsNum}개");
            Debug.Log($"최적의 보석 수는 {optimizedGemsNum}개");

            resultGemNum = currentFullGemsNum - optimizedGemsNum;

            Debug.Log($"남은 보석의 수는 {resultGemNum}개");

            return resultGemNum;
        }

        return optimizedGemsNum;
    }
}
