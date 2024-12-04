using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrintScore : MonoBehaviour
{
    private TMP_Text GameResultText;
    private TMP_Text ScoreText;
    private TMP_Text ResultGemText;
    private TMP_Text SurviveDayText;

    private int resultGemNum = 0;
    private int Score = 0;
    private int surviveDay = 0;

    const string GAME_RESULT_TEXT = "GameResult Text";
    const string RESULT_GEM_TEXT = "Result Gem Text";
    const string SURVIVE_DAY_TEXT = "Survival Day Text";
    const string SCORE_TEXT = "Score Text";

    private GameSystem gameSystem;
    private CalculateScore calculateScore;

    void Awake()
    {
        // 클래스 레벨 변수 초기화
        calculateScore = FindObjectOfType<CalculateScore>();
        gameSystem = FindObjectOfType<GameSystem>();
    }

    private void Start()
    {
        PrintText(); // 텍스트 출력
    }

    public void PrintText()
    {
        if (GameResultText == null)
        {
            GameResultText = GameObject.Find(GAME_RESULT_TEXT)?.GetComponent<TMP_Text>();
            if (GameResultText == null)
                Debug.LogError($"'{GAME_RESULT_TEXT}' 텍스트를 찾을 수 없습니다.");
        }

        if (ScoreText == null)
        {
            ScoreText = GameObject.Find(SCORE_TEXT)?.GetComponent<TMP_Text>();
            if (ScoreText == null)
                Debug.LogError($"'{SCORE_TEXT}' 텍스트를 찾을 수 없습니다.");
        }

        if (ResultGemText == null)
        {
            ResultGemText = GameObject.Find(RESULT_GEM_TEXT)?.GetComponent<TMP_Text>();
            if (ResultGemText == null)
                Debug.LogError($"'{RESULT_GEM_TEXT}' 텍스트를 찾을 수 없습니다.");
        }

        if (SurviveDayText == null)
        {
            SurviveDayText = GameObject.Find(SURVIVE_DAY_TEXT)?.GetComponent<TMP_Text>();
            if (SurviveDayText == null)
                Debug.LogError($"'{SURVIVE_DAY_TEXT}' 텍스트를 찾을 수 없습니다.");
        }

        // 텍스트 업데이트
        if (calculateScore != null)// && gameSystem != null)
        {
            Debug.Log(calculateScore.isClear);
            GameResultText.text = calculateScore.isClear ? "Game Clear" : "Game Over";

            resultGemNum = calculateScore.resultGemNum;
            ResultGemText.text = $"Result Gem: {resultGemNum:D2}";

            // surviveDay = gameSystem.currentDay;
            // SurviveDayText.text = $"Survive Day: {surviveDay:D2}";

            Score = (surviveDay * 10) + (resultGemNum * 100);
            ScoreText.text = $"Score: {Score:D2}";
        }
        else
        {
            Debug.LogError("CalculateScore 또는 GameSystem이 초기화되지 않았습니다.");
        }
    }
}