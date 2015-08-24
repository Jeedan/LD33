using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public void OnPlayClick()
    {
        Application.LoadLevel("battleScene");
    }

    public void OnBackToMenuClick()
    {
        Application.LoadLevel("mainMenu");
    }

    public void OnCreditsClick()
    {
        Application.LoadLevel("credits");
    }

    public void OnExitClick()
    {
        Application.Quit();
    }
}
