using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSystem : MonoBehaviour
{
    [SerializeField] int GemSpawnDate = 20; // 잼 스폰 주기
    [SerializeField] GameObject Gem1, Gem2, Gem3; // 잼 종류
    public int currentDay = 1; // 현재 게임 날짜
    public Player player; // 플레이어 객체 참조
    public List<Enemy> enemies = new List<Enemy>(); // 게임 내 적 리스트
    public bool isStateUpdateEnd = true; // 상태 업데이트 완료 여부
    private TextMeshProUGUI dayText; // 현재 날짜를 표시할 UI 텍스트
    private GemManager gemManager;
    private MapCreator mapCreator;
    public List<Gem> activeGems = new List<Gem>(); // 현재 활성화된 GEM 목록

    void Start()
    {
        player = FindObjectOfType<Player>(); // Player 객체 참조
        gemManager = FindObjectOfType<GemManager>();
        mapCreator = FindObjectOfType<MapCreator>();

        GameObject dayTextObject = GameObject.Find("DayText"); // DayText UI 오브젝트 찾기
        if (dayTextObject != null)
        {
            dayText = dayTextObject.GetComponent<TextMeshProUGUI>(); // 텍스트 참조 가져오기
            UpdateDayText(); // 초기 날짜 표시
        }

        Debug.Log($"게임 시작! 첫 번째 날: Day {currentDay}");
    }

    public void AddEnemy(Enemy newEnemy)
    {
        enemies.Add(newEnemy); // 생성된 적을 리스트에 추가
    }

    public void NextDay()
    {
        currentDay++; // 날짜 증가
        Debug.Log($"Day {currentDay} 시작!");

        UpdateDayText(); // 날짜 UI 갱신

        if(currentDay % GemSpawnDate == 0)
        {
            SpawnGem();
        }
    }

    private void SpawnGem()
    {
        // 맵 크기 가져오기
        int randomX = Random.Range(0, mapCreator.width);
        int randomY = Random.Range(0, mapCreator.height);
        int randomItemType =  Random.Range(1, 4);

        // GEM 프리팹을 맵 좌표에 생성
        Vector3 spawnPosition = new Vector3(randomX, -randomY, 0); // Y축 음수 처리
        GameObject gemPrefab = GetRandomGemPrefab(); // Gem1, Gem2, Gem3 중 하나 반환
        GameObject gemObject = Instantiate(gemPrefab, spawnPosition, Quaternion.identity);

        Gem gem = gemObject.GetComponent<Gem>();
        if (gem != null)
        {
            activeGems.Add(gem); // 활성화된 GEM 목록에 추가
        }

        Debug.Log($"GEM 생성: 위치 ({randomX}, {randomY})");
    }

    private GameObject GetRandomGemPrefab()
    {
        int random = Random.Range(1, 4);
        if (random == 1) return Gem1;
        if (random == 2) return Gem2;
        return Gem3;
    }

    private void UpdateDayText()
    {
        if (dayText != null)
        {
            dayText.text = $"Day: {currentDay}"; // UI 텍스트에 날짜 표시
        }
    }

    private IEnumerator UpdateGameStateRoutine()
    {
        // 플레이어와 적의 상태 업데이트
        player.UpdateState(); // 플레이어 상태 업데이트
        foreach (var enemy in enemies)
        {
            enemy.UpdateState(); // 적 상태 업데이트
            enemy.CompleteMove(); // 적 이동 완료
        }

        yield return new WaitForSeconds(0.2f); // 0.2초 대기
        isStateUpdateEnd = true; // 상태 업데이트 완료
    }

    public void EndGame()
    {
        Debug.Log("게임 종료! 적이 플레이어를 잡았습니다.");
        if (player != null)
        {
            Destroy(player.gameObject); // 플레이어 객체 삭제
        }
        Time.timeScale = 0; // 게임 정지
    }

    void Update()
    {
        // 'N' 키 입력으로 다음 날로 진행
        if (Input.GetKeyDown(KeyCode.N) && isStateUpdateEnd)
        {
            isStateUpdateEnd = false; // 상태 업데이트 시작
            NextDay(); // 날짜 증가
            StartCoroutine(UpdateGameStateRoutine()); // 상태 업데이트 코루틴 실행
        }
    }
}
