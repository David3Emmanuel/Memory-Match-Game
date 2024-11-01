using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Levels levels;

    public void OnPlay()
    {
        SceneManager.LoadScene("Game");
    }
}
