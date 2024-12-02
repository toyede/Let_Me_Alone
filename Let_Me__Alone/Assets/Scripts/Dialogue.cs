using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public float delay = 0.1f;
    private string fullText = "아이돌 생활을 하고 있던 에밀리아. 에밀리아는 어느 순간 도박에 빠져\n400만원의 빚이 생기고 말았다. 도박에 빠져 일도 불성실하게 하게 되어\n주변 평판도 나빠지고 있었다.\n참다 못한 소속사는 에밀리아를 방출할 계획을 짜게 된다.\n뒤늦게 에밀리아는 소속사에 가서 잘못을 빌었고 \n소속사는 어떻게든 빚을 갚고 평판 회복에 사용될 많은 보석을 요구했다.\n파파라치에게 발각되면 논란을 막기 위해 다시 복귀해야 한다.\n과연 빚을 갚고 무사히 아이돌 생활을 이어갈 수 있을 것인가?\n\n빨간 보석 - 10만원\n노란 보석 - 80만원\n파란 보석 - 100만원 \n \n조작키 - WASD\n다음날 - N\n줌아웃 - Y\n";
    private string currentText = "";
    public Button gamebutton;
    private bool isFinished = false;

    private void Start()
    {
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        for(int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            GetComponent<TMP_Text>().text = currentText;
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(1f);
        isFinished = true;

        if(isFinished)
        {
            gamebutton.gameObject.SetActive(true);
        }
    }

    public void startbutton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


}
