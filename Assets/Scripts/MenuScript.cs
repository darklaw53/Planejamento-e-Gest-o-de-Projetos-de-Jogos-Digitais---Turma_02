using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI CRvalue;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("BGM").GetComponent<PersistentBGM>().StopMusic();
        CRvalue.text = "" + Mathf.RoundToInt(slider.value);
        Persistant.GetInstance().challengeRating = Mathf.RoundToInt(slider.value);
    }

    public void StartMatch()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OnValueChange()
    {
        CRvalue.text = "" + Mathf.RoundToInt(slider.value);
        Persistant.GetInstance().challengeRating = Mathf.RoundToInt(slider.value);
    }
}
