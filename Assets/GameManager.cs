using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool matchOver;
    public Vector3 highCamPos;

    public GameObject cam;
    public GameObject whiteRing;
    public GameObject endMenu;

    public Image point1, point2, point3;
    static int roundNumber, greenPoints, redPoints;

    static Color pointColor1, pointColor2, pointColor3;
    public TextMeshProUGUI buttonText, roundEndText;

    static float oponentIntelect, oponentDexterity, oponentStrength;
    public float inte, dext, stre;

    bool gameOver;

    public int chalengeRating;

    private void Awake()
    {
        if (oponentIntelect == 0 && oponentDexterity == 0 && oponentStrength == 0)
        {
            chalengeRating = Persistant.GetInstance().challengeRating;

            if (chalengeRating > 15) chalengeRating = 15;

            for (int i = 0; i < chalengeRating; i++)
            {
                switch (Random.Range(0, 3))
                {
                    case 0:
                        if (oponentIntelect < 5) oponentIntelect++;
                        else i--;
                        break;
                    case 1:
                        if (oponentDexterity < 5) oponentDexterity++;
                        else i--;
                        break;
                    case 2:
                        if (oponentStrength < 5) oponentStrength++;
                        else i--;
                        break;
                }
            }
        }

        inte = oponentIntelect;
        dext = oponentDexterity;
        stre = oponentStrength;

        if (pointColor1.a > 0) point1.color = pointColor1;
        if (pointColor2.a > 0) point2.color = pointColor2;
        if (pointColor3.a > 0) point3.color = pointColor3;
    }

    public void MatchOver(PlayerController looserPos)
    {
        endMenu.SetActive(true);
        if (looserPos.isOponent) roundEndText.text = "ROUND WIN";
        else roundEndText.text = "ROUND LOST";

        Quaternion q = new Quaternion(0, 0, 0, 0);
        q.eulerAngles = new Vector3(90, 0, 0);
        Instantiate(whiteRing, looserPos.transform.position, q);

        if (roundNumber == 0)
        {
            if (!looserPos.isOponent)
            {
                point1.color = Color.red;
                pointColor1 = Color.red;
                redPoints++;
            }
            else
            {
                point1.color = Color.green;
                pointColor1 = Color.green;
                greenPoints++;
            }
        }
        else if (roundNumber == 1)
        {
            if (!looserPos.isOponent)
            {
                point2.color = Color.red;
                pointColor2 = Color.red;
                redPoints++;
            }
            else
            {
                point2.color = Color.green;
                pointColor2 = Color.green;
                greenPoints++;
            }
        }
        else if (roundNumber == 2)
        {
            if (!looserPos.isOponent)
            {
                point3.color = Color.red;
                pointColor3 = Color.red;
                redPoints++;
            }
            else
            {
                point3.color = Color.green;
                pointColor3 = Color.green;
                greenPoints++;
            }
        }

        if (greenPoints >= 2)
        {
            roundEndText.text = "MATCH WIN";
            buttonText.text = "Back To Menu";
            gameOver = true;
        }
        else if (redPoints >= 2)
        {
            roundEndText.text = "MATCH LOST";
            buttonText.text = "Back To Menu";
            gameOver = true;
        }

        cam.transform.parent = null;
        cam.transform.position = highCamPos;
        q.eulerAngles = new Vector3(22, 180, 0);
        cam.transform.rotation = q;
        cam.transform.parent = transform;
        matchOver = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Quit();

        if (matchOver)
        {
            transform.Rotate(0, .5f, 0 * Time.unscaledDeltaTime);
        }
    }

    public void NextRoundButton()
    {
        Time.timeScale = 1;
        if (gameOver)
        {
            Quit();
        }
        else
        {
            roundNumber++;
            SceneManager.LoadScene(1);
        }
    }

    void Quit()
    {
        oponentIntelect = 0;
        oponentDexterity = 0;
        oponentStrength = 0;
        roundNumber = 0;
        greenPoints = 0;
        redPoints = 0;
        SceneManager.LoadScene(0);
    }
}