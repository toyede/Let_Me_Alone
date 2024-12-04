using System.Collections.Generic;

public static class QuickSortUtility
{
    public static void Sort(List<Gem> gems, int low, int high)
    {
        if (low < high)
        {
            int pi = Partition(gems, low, high);

            Sort(gems, low, pi - 1);  // 왼쪽 부분 정렬
            Sort(gems, pi + 1, high); // 오른쪽 부분 정렬
        }
    }

    private static int Partition(List<Gem> gems, int low, int high)
    {
        int pivot = gems[high].GetGemValue();
        int i = (low - 1);

        for (int j = low; j < high; j++)
        {
            if (gems[j].GetGemValue() >= pivot) // 높은 가중치 우선
            {
                i++;
                Swap(gems, i, j);
            }
        }
        Swap(gems, i + 1, high);
        return (i + 1);
    }

    private static void Swap(List<Gem> gems, int i, int j)
    {
        Gem temp = gems[i];
        gems[i] = gems[j];
        gems[j] = temp;
    }
}
