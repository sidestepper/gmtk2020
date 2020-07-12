﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ClickPlay() {
        SceneManager.LoadScene("GameScene");
    }

    public void ClickQuit() {
        Application.Quit();
    }
}
