using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuStart : MonoBehaviour
{
    //GameObject gameSession;

    public void StartFirstLEvel()
    {
        ResetGameSession();
        SceneManager.LoadScene(1);
    }
    public void BackToMenu()
    {
        ResetGameSession();
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void ResetGameSession()
    {
        if (FindObjectOfType<GameSession>() !=null)
        {
            var gameSession = FindObjectOfType<GameSession>().gameObject;
            Destroy(gameSession);
        }

        else
        {
            return;
        }
        
    }
}
