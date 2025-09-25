using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    void Start()
    {        
        Invoke(nameof(NextScene), 2);
    }

    void NextScene()
    {
        SceneManager.LoadScene("Game");
    }
    
}
