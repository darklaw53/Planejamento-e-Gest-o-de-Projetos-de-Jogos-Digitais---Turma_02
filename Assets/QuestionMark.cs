using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionMark : MonoBehaviour
{
    bool toggle;
    public Image image;
    public GameObject mainMenu, instructionsMenu;

    public void Toggle()
    {
        toggle = !toggle;

        if (toggle)
        {
            image.color = Color.gray;
            mainMenu.SetActive(false);
            instructionsMenu.SetActive(true);
        }
        else
        {
            image.color = Color.white;
            instructionsMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
}
