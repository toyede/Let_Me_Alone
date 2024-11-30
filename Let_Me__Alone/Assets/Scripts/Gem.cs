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
    [SerializeField] private int Gem_Weights = 0;

    private void Awake() 
    {
        gemManager = FindObjectOfType<GemManager>();
    }

    private void Start() 
    {
        GiveWeights();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.GetComponent<Player>())
        // 만약 닿은게 플레이어라면
        {
            Debug.Log("닿았음");
            DetectPickUpType();
            Destroy(gameObject);
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
}
