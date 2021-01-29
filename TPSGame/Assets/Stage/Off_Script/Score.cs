using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    [SerializeField] GameObject R;

    [SerializeField] Text playerscore;
    [SerializeField] Text AIscore;
    [SerializeField] Text playerR;
    [SerializeField] Text AIR;
    [SerializeField] Text WL;
    int Pscore = 0;
    int Ascore = 0;
    void Start()
    {
        
    }

    void Update()
    {
        score();
        if (Pscore >= 7 || Ascore >= 7)
        {
            Rscore();
            if (Pscore > Ascore)
            {
                WL.text = "YOU WIN";
            }
            else
            {
                WL.text = "YOU LOSE";
            }
            R.SetActive(true);
        }
    }

    void Rscore()
    {
        playerR.text = "プレイヤー:" + Pscore.ToString();
        AIR.text = "AI:" + Ascore.ToString();
    }

    void score()
    {
        playerscore.text = Pscore.ToString();
        AIscore.text = Ascore.ToString();
    }

    public void addP(int p)
    {
        Pscore += p;
    }

    public void addA(int a)
    {
        Ascore += a;
    }
}
