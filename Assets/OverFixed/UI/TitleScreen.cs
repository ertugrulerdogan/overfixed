using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public GameObject creditsOverlay;

    public void StartGame()
    {
        Application.LoadLevel(1);
    }

    public void Credits()
    {
        creditsOverlay.SetActive(true);
    }

    public void CreditsClose() {
        creditsOverlay.SetActive(false);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
