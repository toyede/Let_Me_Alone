using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static void GoToNextScene()
    {
        int sceneCount = 0;
        sceneCount = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(sceneCount + 1);
    }

    public void CurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
