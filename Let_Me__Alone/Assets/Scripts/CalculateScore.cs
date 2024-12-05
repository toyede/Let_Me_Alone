using System.Collections.Generic;
using UnityEngine;

public class CalculateScore : MonoBehaviour
{
    public int targetAmount = 400; // 목표 금액
    private int[] gemValues = { 100, 80, 10 }; // Gem의 가치 (높은 순)
    private string[] gemNames = { "Gem3", "Gem2", "Gem1" }; // Gem 이름

    private int currentFullGemsNum = 0; // 플레이어가 게임에서 먹은 Gem 개수
    private int optimizedGemsNum = 0; // 목표 금액을 달성하기 위해 사용된 Gem의 총 개수
    public int resultGemNum = 0; // 남은 Gem 개수 (currentFullGemsNum - optimizedGemsNum)한 값

    [SerializeField] public bool isClear = true;
    GemManager gemManager;

    void Awake()
    {
        gemManager = FindObjectOfType<GemManager>();
        currentFullGemsNum = gemManager.currentGem3 + gemManager.currentGem2 + gemManager.currentGem1; // Gem 총 개수
        if (gemManager != null)
        {
            Debug.Log($"현재 Gem 개수: Gem3={gemManager.currentGem3}, Gem2={gemManager.currentGem2}, Gem1={gemManager.currentGem1}");

            // 동전 탐욕 알고리즘: 목표 금액, 잼 가치, 사용가능 Gem 개수를 인수로 넘겨줌
            var result = CoinGreedy(targetAmount, gemValues, new int[]
            {
                gemManager.currentGem3, // 사용 가능한 Gem1 개수
                gemManager.currentGem2, // 사용 가능한 Gem2 개수
                gemManager.currentGem1  // 사용 가능한 Gem3 개수
            });

            if (result != null)
            {
                // 결과 출력
                Debug.Log($"거스름돈 {targetAmount}원을 구성하는 최소 Gem 조합:");

                for (int i = 0; i < result.Length; i++)
                {
                    Debug.Log($"{gemNames[i]}: {result[i]}개"); // 사용된 각 Gem 타입 개수 출력
                    optimizedGemsNum += result[i]; // 총 사용된 Gem 개수 계산
                }
                
                Debug.Log($"총 {optimizedGemsNum}개");
                resultGemNum = currentFullGemsNum - optimizedGemsNum;
            }

            else
            {
                // 목표 금액 달성 실패 시 처리
                isClear = false;
                optimizedGemsNum = 0;
            }
        }
    }

    private int[] CoinGreedy(int amount, int[] values, int[] available)
    {
        int n = values.Length; // Gem 종류 개수 (3)
        int[] gemUsed = new int[n]; // 각 Gem 별 얼마나 썼는지 넣어놓을 변수
        int remainingAmount = amount; // 달성해야 할 남은 금액

        // 가치가 높은 것부터 선택
        for (int i = 0; i < n; i++)
        {
            // 해당 동전을 추가하면 남은 금액에서 초과되지는 않는지, 사용 가능 개수가 0 이상인지 검사
            while (remainingAmount >= values[i] && available[i] > 0) // i번째 동전
            {
                if(i == 2 && values[i] * available[i] > remainingAmount) // ex) 100*2, 80*3, 10*2일떄 조건문 없으면 100원2개, 80원2개, 10원 2개 쓰고 80원 써버림
                {
                    gemUsed[i]++;              // 현재 Gem 타입 사용 개수 증가
                    remainingAmount -= values[i]; // 남은 금액에서 Gem 가치 차감
                    available[i]--;            // 사용 가능한 Gem 개수 감소
                }
            }
        }

        // 갚아야 할 돈이 있는데 남는 보석도 있으면 써야함 
        if(remainingAmount > 0)
        {
            for (int i = 0; i < n; i++)
            {
                while (remainingAmount > 0 && available[i] > 0)
                {
                    gemUsed[i]++;              // 현재 Gem 타입 사용 개수 증가
                    remainingAmount -= values[i]; // 남은 금액에서 Gem 가치 차감
                    available[i]--;            // 사용 가능한 Gem 개수 감소
                    Debug.Log($"{remainingAmount} 남은 돈");
                }
            }
        }

        // 남는 돈도 없고 남은 금액이 0이 아니라면 목표 금액 달성이 불가능
        if (remainingAmount > 0)
        {
            return null; // 실패를 나타내기 위해 null 반환
        }

        return gemUsed; // 사용된 Gem 개수 배열 반환
    }
}
