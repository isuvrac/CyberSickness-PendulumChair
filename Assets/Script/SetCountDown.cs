using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCountDown : MonoBehaviour
{
    private GameManagerScript GMS;

    public void SetCountDownNow()
    {
        GMS = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        GMS.counterDownDone = true;
    }
}
