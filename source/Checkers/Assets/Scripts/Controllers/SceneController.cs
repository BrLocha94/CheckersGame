using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoSingleton<SceneController>
{
    public void LoadOpen()
    {
        SceneManager.LoadScene("Open");
    }

    public void LoadMain()
    {
        SceneManager.LoadScene("Main");
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
