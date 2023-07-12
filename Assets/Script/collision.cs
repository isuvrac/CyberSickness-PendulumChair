using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using UnityEngine.UI;
using System.Diagnostics;


public class collision : MonoBehaviour
{
    string PositionFile = "";
    public string theTime;
    public string theDate;
    public string ID;
    public Stopwatch CollisionTimer = new Stopwatch();
    [SerializeField]
    private AM bn;

    // Start is called before the first frame update
    void Start()
    {
     Invoke("StartCollisionTimer", 3);

    theTime = System.DateTime.Now.ToString("HH.mm");
    theDate = System.DateTime.Now.ToString("MM-dd");
    PositionFile = Application.dataPath + ("/" + "P" + bn.ID +
    "_" + "Positions" + "_" + theDate + "-22" + "_" + theTime +
    ".csv");
    //CollisionTimer.Start();
    //Headers and repeated call for Position Mapping
    InvokeRepeating("WritePosition", 3, 0.5f);
    TextWriter tw = new StreamWriter(PositionFile, false);
    tw.WriteLine("PositionX,PositionY,PositionZ,Pitch,Yaw,Roll,Time");
    tw.Close();
    }
    private void StartCollisionTimer()
    {
        CollisionTimer.Start();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "HITTING")
        {
            bn.FreezeChair();
            Invoke("ResetChairHelper", 1f);
            bn.count = bn.count + 1;
            bn.Appendables();
            bn.CollisionStopwatchEndAndRecord();
        }
        
    }

    private void ResetChairHelper()
    {
        bn.ResetChair();
    }

    public void WritePosition()
    {
        TextWriter tw = new StreamWriter(PositionFile, true);
        tw.WriteLine(transform.position.x + "," + transform.position.y + "," +
        transform.position.z + "," + transform.rotation.x + ","
        + transform.rotation.y + "," + transform.rotation.z + "," +
       CollisionTimer.ElapsedMilliseconds);
        tw.Close();
    }
}
