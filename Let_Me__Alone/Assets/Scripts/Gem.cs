using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    // 픽업 아이템의 종류를 나타내는 열거형
    private enum PickUpType
    {
        Gem1,
        Gem2,
        Gem3,
    }

    [SerializeField] private PickUpType pickUpType; // 아이템 유형
    private GemManager gemManager;
    private GameSystem gameSystem;
    public bool istouched = false;
    [SerializeField] private int Gem_Weights = 0;

    private void Awake() 
    {
        gemManager = FindObjectOfType<GemManager>();
        gameSystem = FindObjectOfType<GameSystem>();
        GiveWeights();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.GetComponent<Player>())
        // 만약 닿은게 플레이어라면
        {
            Debug.Log("닿았음");
            DetectPickUpType();

            // GameSystem에서 activeGems 리스트에서 제거
            if (gameSystem != null)
            {
                gameSystem.activeGems.Remove(this);
            }

            Destroy(gameObject); // GEM 파괴

            // QuickSort 재적용 및 카메라 업데이트
            if (gameSystem != null)
            {
                // 정렬 후 카메라 업데이트
                gameSystem.UpdateItemCameraTarget();
            }
        }
    }

    private void DetectPickUpType()
    {
        switch(pickUpType)
        {
            case PickUpType.Gem1:
                // 1골드를 추가하고 UI를 업데이트하는 메서드
                gemManager.UpdateCurrentGem(1);
                break; 

            case PickUpType.Gem2:
                gemManager.UpdateCurrentGem(2);
                break; 

            case PickUpType.Gem3:
                gemManager.UpdateCurrentGem(3);
                break; 

            default:
                break;
        }
    }

    private void GiveWeights()
    {
        switch(pickUpType)
        {
            case PickUpType.Gem1:
                // 1골드를 추가하고 UI를 업데이트하는 메서드
                Gem_Weights = 1;
                break; 

            case PickUpType.Gem2:
                Gem_Weights = 2;
                break; 

            case PickUpType.Gem3:
                Gem_Weights = 3;
                break; 

            default:
                break;
        }
    }

    public int GetWeight()
    {
        return Gem_Weights;
    }
}
