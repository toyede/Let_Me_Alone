using System.Collections.Generic;

public static class SortUtility // 퀵소트에서 삽입정렬로 변경 (이미 정렬된 리스트..)
{
    public static void Sort(List<Gem> gems) // 활성화되어있는 잼들을 입력받아 정렬시킴
    {
        int n = gems.Count; // n은 잼의 수

        for (int i = 0; i < n - 1; i++) 
        {
            int j = i + 1; // 맨 첫번째는 정렬되어있는것으로 판정

            while (j > 0 && gems[j - 1].GetGemValue() < gems[j].GetGemValue()) // 키값 하나 뽑아서 인덱스 1번째 까지 계속 앞의 인덱스 원소값과 비교
            {
                Swap(gems, j - 1, j);
                j--;
            }
        }
    }

    private static void Swap(List<Gem> gems, int i, int j)
    {
        // 'C언어식' 교환...
        Gem temp = gems[i];
        gems[i] = gems[j];
        gems[j] = temp;
    }
}
