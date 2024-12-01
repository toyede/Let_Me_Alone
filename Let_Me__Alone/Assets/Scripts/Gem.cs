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
    [SerializeField] private int Gem_Value = 0;

    private void Awake() 
    {
        gemManager = FindObjectOfType<GemManager>();
        gameSystem = FindObjectOfType<GameSystem>();
        GiveValue();
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

    private void GiveValue()
    {
        switch(pickUpType)
        {
            case PickUpType.Gem1:
                // 젬 1은 단위무게당 가치 50
                Gem_Value = 10;
                break; 

            case PickUpType.Gem2:
                // 젬 2은 단위무게당 가치 5
                Gem_Value = 80;
                break; 

            case PickUpType.Gem3:
                // 젬 3은 단위무게당 가치 10
                Gem_Value = 100;
                break; 

            default:
                break;
        }
    }

    public int GetGemValue()
    {
        return Gem_Value;
    }
}
