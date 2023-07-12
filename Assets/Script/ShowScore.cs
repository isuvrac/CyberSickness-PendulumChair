using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowScore : MonoBehaviour
{
    public Text ScoreText;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame

    public void UpdateScore(int score)
    {
        ScoreText.text = ("Best Record: 22 \n \n \n Number of your \n  Collisions: " + score.ToString());
        //ScoreText.text = score.ToString();

    }



}
