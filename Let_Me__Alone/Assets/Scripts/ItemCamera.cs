using UnityEngine;
using Cinemachine;

public class ItemCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    private Player player;
    private GameSystem gameSystem;

    void Start()
    {
        player = FindObjectOfType<Player>();
        gameSystem = FindObjectOfType<GameSystem>();
    }

    public void SetFollowTarget(Transform target)
    {
        virtualCamera.Follow = target;
        virtualCamera.LookAt = target;
        Debug.Log($"ItemCamera: 카메라가 새로운 대상 {target.name}을 추적합니다.");
    }

    public void SetFollowTargetToPlayer()
    {
        virtualCamera.Follow = player.transform;
        virtualCamera.LookAt = player.transform;
        Debug.Log("ItemCamera: 카메라가 플레이어를 추적합니다.");
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