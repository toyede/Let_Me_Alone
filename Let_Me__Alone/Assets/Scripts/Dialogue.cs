using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public float delay = 0.1f;
    private string fullText = "���̵��� Ȱ���ϴ� ���и���\n�Ǵ� �Ҽӻ�� ���� ���� �þ�� ���� ���ݰ�\n������ �ٴϸ� ���� ��� ���� ���� ���Ӱ� �����ϱ�� ����Ѵ�.\n���Ķ�ġ���� ������ �ٽ� ����Կ��� ���� �ȴ�.\n���� ���� ���� �� ����� �� �� ������?\n\n����Ű - wasd\n������ - n\n";
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
