using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public float delay = 0.1f;
    private string fullText = "아이돌로 활동하던 에밀리아\n악덕 소속사로 인해 빚만 늘어나는 것을 깨닫고\n여행을 다니며 돈을 모아 빚을 갚고 새롭게 시작하기로 결심한다.\n파파라치에게 잡히면 다시 사장님에게 가게 된다.\n과연 빚을 갚고 새 출발을 할 수 있을까?\n\n조작키 - wasd\n다음날 - n\n";
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
