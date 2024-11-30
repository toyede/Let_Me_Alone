using UnityEngine;
using Cinemachine;

public class ItemCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    private GameSystem gameSystem;
    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
        gameSystem = FindObjectOfType<GameSystem>();
    }

    public void UpdateTargetGem()
    {
        if (gameSystem.activeGems.Count == 0) return;

        // GEM 리스트 Quick Sort
        QuickSortUtility.Sort(gameSystem.activeGems, 0, gameSystem.activeGems.Count - 1);

        // 가장 높은 가중치의 GEM 선택
        Gem highestWeightGem = gameSystem.activeGems[0];
        virtualCamera.Follow = highestWeightGem.transform; // 카메라가 해당 GEM을 추적
        virtualCamera.LookAt = highestWeightGem.transform;

        Debug.Log($"ItemCamera: 최고 가중치 GEM 추적 시작 - {highestWeightGem.GetWeight()}");
    }
    
    private void Update() 
    {
        if (gameSystem.currentDay < gameSystem.GemSpawnDate)
        {
            virtualCamera.Follow = player.transform; // 카메라가 플레이어를 추적
            virtualCamera.LookAt = player.transform;
        }

        this.transform.position = virtualCamera.transform.position;
    }
}