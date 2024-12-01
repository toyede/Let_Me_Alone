using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GemManager : MonoBehaviour
{
    private TMP_Text Gem1Text;
    private TMP_Text Gem2Text;
    private TMP_Text Gem3Text;

    public int currentGem1 = 0;
    public int currentGem2 = 0;
    public int currentGem3 = 0;

    // const: #define이랑 같은 의미
    const string Gem1_AMOUNT_TEXT = "Gem1 Text";
    const string Gem2_AMOUNT_TEXT = "Gem2 Text ";
    const string Gem3_AMOUNT_TEXT = "Gem3 Text";

    private static GemManager GMinstance;

    void Awake()
    {
        if (GMinstance == null)
        {
            GMinstance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 삭제되지 않음
        }
        else
        {
            Destroy(gameObject); // 중복 방지
        }
    }

    public void UpdateCurrentGem(int type)
    {
        switch(type)
        {
            case 1:
                // 1골드를 추가하고 UI를 업데이트하는 메서드
                currentGem1++;
                break; 

            case 2:
                currentGem2++;
                break; 

            case 3:
                currentGem3++;
                break; 

            default:
                break;
        }

        if(Gem1Text == null)
        {
            Gem1Text = GameObject.Find(Gem1_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }

        if(Gem2Text == null)
        {
            Gem2Text = GameObject.Find(Gem2_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }

        if(Gem3Text == null)
        {
            Gem3Text = GameObject.Find(Gem3_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }

        // D3를 인자값으로 주면 무조건 세 글자 이상이 포함되도록 함.
        // ex. 1 -> 001
        Gem1Text.text = ": " + currentGem1.ToString("D2");
        Gem2Text.text = ": " + currentGem2.ToString("D2");
        Gem3Text.text = ": " + currentGem3.ToString("D2");
    }
}
