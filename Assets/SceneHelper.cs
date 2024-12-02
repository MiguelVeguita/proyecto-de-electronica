using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHelper : MonoBehaviour
{
    public static void LoadScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public static void CloseG1ame()
    {
        Application.Quit();
    }
    public static void menuscene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
