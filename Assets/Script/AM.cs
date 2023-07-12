using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System;
public class AM : MonoBehaviour
{

    public string ID;
    public string theTime;
    public string theDate;
    //string PositionFile = "";
    string TimerCollisionFile = "";
    string DeltasFile = "";
    string AppendablesFile = "";
    //stopwatch
    public Stopwatch CollisionTimer = new Stopwatch();
    //position and collisions
    public int count = 0;
    public long elapsed_time;
    public long TotalTime;
    public long RemainingTime;
    public long UprightTime;
    //controller deltas
    public float InputHorizontal;
    public float InputNHV;
    public float InputVertical;




    //for rotation by quaternion
    Quaternion rotation;

    int k = 0;

    public float speedX;
    public float speedY;
    public float speedZ;

    //initial velocity
    public float speedPX0;
    //secondary speed
    public float speedPX1;
    //total velocity
    public float speedPX;

    //initial velocity
    public float speedPY0;
    //secondary speed
    public float speedPY1;
    //total velocity
    public float speedPY;

    //initial velocity
    public float speedPZ0;
    //secondary speed
    public float speedPZ1;
    //total velocity
    public float speedPZ;

    public float speedJX;
    float PrevJX;
    float DeltaJX;

    public float speedJY;
    float PrevJY;
    float DeltaJY;

    public float speedJZ;
    float PrevJZ;
    float DeltaJZ;

    //Initial acceleration
    public float accelerationPX0;
    //secondary acceleration
    public float accelerationPX1;
    //total acceleration
    public float accelerationPX;

    //Initial acceleration
    public float accelerationPY0;
    //secondary acceleration
    public float accelerationPY1;
    //total acceleration
    public float accelerationPY;

    //Initial acceleration
    public float accelerationPZ0;
    //secondary acceleration
    public float accelerationPZ1;
    //total acceleration
    public float accelerationPZ;

    public float jerkPX;
    public float jerkPY;
    public float jerkPZ;

    public float Timer;

    //to rotate the chair around pivot
    public GameObject pivot;

    Vector3 targetX;
    Vector3 targetY;
    Vector3 targetZ;

    Quaternion targetXQ;
    Quaternion targetYQ;
    Quaternion targetZQ;

    public AudioSource hit;
    public AudioSource game;
    private GameManagerScript GMS;
    public ShowScore showScore;

    int record = 0;
    Text ScoreText;

    bool isFrozen = false;

    public TextParser textParser;







    public void FreezeChair()
    {
        isFrozen = true;
        //Manually reset position
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(0, Vector3.right), 1f);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(0, Vector3.forward), 1f);

        hit.Play();

    }


    public void ResetChair()
    {
        isFrozen = false;
        //Score();
    }
    private void Start()
    {
        Invoke("StartCollisionTimer", 3);
        game.Play();
        GMS = GameObject.Find("GameManager").GetComponent<GameManagerScript>();




        theTime = System.DateTime.Now.ToString("HH.mm");
        theDate = System.DateTime.Now.ToString("MM-dd");
        AppendablesFile = Application.dataPath + ("/" + "P" + ID + "_" + "Appendables" + "_" + theDate + "-22" + "_" + theTime + ".csv");

        TimerCollisionFile = Application.dataPath + ("/" + "P" + ID + "_" +
       "TrialTimes" + "_" + theDate + "-22" + "_" + theTime + ".csv");
        DeltasFile = Application.dataPath + ("/" + "P" + ID + "_" +
       "ControllerInputs" + "_" + theDate + "-22" + "_" + theTime + ".csv");


        //Headers and repeated call for Input Mapping
        TextWriter wt = new StreamWriter(DeltasFile, false);
        wt.WriteLine("H Change, V Change, NHV Change, Time");
        wt.Close();
        InvokeRepeating("ControllerDeltas", 3, .5f);
        //Headers for appendables file
        TextWriter ww = new StreamWriter(AppendablesFile, false);
        ww.WriteLine("ID, TotalTime, TotalCollisions, AvgTime, StDevTime, MedTime, " +
            "FirstTime, LastTime, First5Avg, First5StDev, First5Med, Last5Avg, Last5StDev, Last5Med");
        ww.Write(ID + "," + "," + "," + "," + "," + "," + "," + "," + "," + "," + "," + "," + "," + ",");
        ww.Close();
    }

    private void StartCollisionTimer()
    {
        CollisionTimer.Start();
    }


    public void CollisionStopwatchEndAndRecord()
    {
        TextWriter tw = new StreamWriter(TimerCollisionFile, true);
        tw.WriteLine("AirTime" + count + "(ms):" + "," +
       UprightTime +
        "," + "TimeStamp(ms)" + "," +
       elapsed_time);
        elapsed_time = CollisionTimer.ElapsedMilliseconds;
        tw.Close();
    }
    public void TotalTimer()
    {
        TotalTime = CollisionTimer.ElapsedMilliseconds;
        RemainingTime = TotalTime - elapsed_time;
        TextWriter tw = new StreamWriter(TimerCollisionFile, true);
        tw.WriteLine("RemainingTime(ms)" + "," + RemainingTime);
        tw.WriteLine("Total Time (ms)" + "," +
       TotalTime);
        tw.WriteLine("Total Collisions" + "," + count);
        tw.Close();
        UnityEngine.Debug.Log("Timer Recorded");
    }


    public void ControllerDeltas()
    {
        TextWriter tw = new StreamWriter(DeltasFile, true);
        tw.WriteLine((Input.GetAxis("Horizontal") - InputHorizontal) + "," +
       (Input.GetAxis("Vertical") - InputVertical) + ","
        + (Input.GetAxis("NHV") - InputNHV) + "," +
       CollisionTimer.ElapsedMilliseconds);
        InputHorizontal = Input.GetAxis("Horizontal");
        InputVertical = Input.GetAxis("Vertical");
        InputNHV = Input.GetAxis("NHV");
        tw.Close();
    }

    public void Appendables()
    {
        UprightTime = CollisionTimer.ElapsedMilliseconds - elapsed_time;
        elapsed_time = CollisionTimer.ElapsedMilliseconds;
        TextWriter ww = new StreamWriter(AppendablesFile, true);
        ww.Write(UprightTime + ",");
        ww.Close();
    }

    public void AppendablesEnd()
    {
        TextWriter ww = new StreamWriter(AppendablesFile, true);
        ww.WriteLine(",");
        ww.WriteLine("," + TotalTime);
        ww.Close();
        UnityEngine.Debug.Log("AppendableFile is Finished");
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
            TotalTimer();
        if (Input.GetKeyDown(KeyCode.Space))
            AppendablesEnd();

       
        if (isFrozen)
        {
            return;
        }



        if (GMS.counterDownDone == true)
        {


            Timer = Timer + 1 * Time.deltaTime;


            if (Input.GetAxis("Horizontal") >= -1 & Input.GetAxis("Horizontal") <= 1)
            {

                DeltaJX = Mathf.Abs(Input.GetAxis("Horizontal")) - Mathf.Abs(PrevJX);


                if (DeltaJX > 0)
                {
                    if (Input.GetAxis("Horizontal") > 0 & Input.GetAxis("Horizontal") <= 1)

                    {
                        speedJX = Input.GetAxis("Horizontal") * speedJX;

                        speedJX--;

                    }
                    if (Input.GetAxis("Horizontal") >= -1 & Input.GetAxis("Horizontal") < 0)

                    {
                        speedJX = -Input.GetAxis("Horizontal") * speedJX;

                        speedJX++;

                    }
                    if (Input.GetAxis("Horizontal") == 0)

                    {
                        if (speedJX > 0)
                        {
                            speedJX--;
                        }
                        if (speedJX < 0)
                        {
                            speedJX++;
                        }
                    }
                }
                else
                {
                    if (Input.GetAxis("Horizontal") > 0 & Input.GetAxis("Horizontal") <= 1)

                    {
                        speedJX--;
                    }

                    if (Input.GetAxis("Horizontal") >= -1 & Input.GetAxis("Horizontal") < 0)

                    {
                        speedJX++;
                    }

                    if (Input.GetAxis("Horizontal") == 0)

                    {
                        if (speedJX > 0)
                        {
                            speedJX--;
                        }
                        if (speedJX < 0)
                        {
                            speedJX++;
                        }
                    }
                }


            }
            PrevJX = Input.GetAxis("Horizontal");


            if (Input.GetAxis("Vertical") >= -1 & Input.GetAxis("Vertical") <= 1)
            {

                DeltaJZ = Mathf.Abs(Input.GetAxis("Vertical")) - Mathf.Abs(PrevJZ);


                if (DeltaJZ > 0)
                {
                    if (Input.GetAxis("Vertical") > 0 & Input.GetAxis("Vertical") <= 1)

                    {
                        speedJZ = Input.GetAxis("Vertical") * speedJZ;

                        speedJZ--;

                    }
                    if (Input.GetAxis("Vertical") >= -1 & Input.GetAxis("Vertical") < 0)

                    {
                        speedJZ = -Input.GetAxis("Vertical") * speedJZ;

                        speedJZ++;

                    }
                    if (Input.GetAxis("Vertical") == 0)

                    {
                        if (speedJZ > 0)
                        {
                            speedJZ--;
                        }
                        if (speedJZ < 0)
                        {
                            speedJZ++;
                        }
                    }
                }
                else
                {
                    if (Input.GetAxis("Vertical") > 0 & Input.GetAxis("Vertical") <= 1)

                    {
                        speedJZ--;
                    }

                    if (Input.GetAxis("Vertical") >= -1 & Input.GetAxis("Vertical") < 0)

                    {
                        speedJZ++;
                    }

                    if (Input.GetAxis("Vertical") == 0)

                    {
                        if (speedJZ > 0)
                        {
                            speedJZ--;
                        }
                        if (speedJZ < 0)
                        {
                            speedJZ++;
                        }
                    }
                }


            }
            PrevJZ = Input.GetAxis("Vertical");

            if (Input.GetAxis("NHV") >= -1 & Input.GetAxis("NHV") <= 1)
            {

                DeltaJY = Mathf.Abs(Input.GetAxis("NHV")) - Mathf.Abs(PrevJY);


                if (DeltaJY > 0)
                {
                    if (Input.GetAxis("NHV") > 0 & Input.GetAxis("NHV") <= 1)

                    {
                        speedJY = Input.GetAxis("NHV") * speedJY;

                        speedJY++;

                    }
                    if (Input.GetAxis("NHV") >= -1 & Input.GetAxis("NHV") < 0)

                    {
                        speedJY = -Input.GetAxis("NHV") * speedJY;

                        speedJY--;

                    }
                    if (Input.GetAxis("NHV") == 0)

                    {
                        if (speedJY > 0)
                        {
                            speedJY--;
                        }
                        if (speedJY < 0)
                        {
                            speedJY++;
                        }
                    }
                }
                else
                {
                    if (Input.GetAxis("NHV") > 0 & Input.GetAxis("NHV") <= 1)

                    {
                        speedJY++;
                    }

                    if (Input.GetAxis("NHV") >= -1 & Input.GetAxis("NHV") < 0)

                    {
                        speedJY--;
                    }

                    if (Input.GetAxis("NHV") == 0)

                    {
                        if (speedJY > 0)
                        {
                            speedJY--;
                        }
                        if (speedJY < 0)
                        {
                            speedJY++;
                        }
                    }
                }


            }
            PrevJY = Input.GetAxis("NHV");

            //to have a jerk


            if (Timer > textParser.Time1[k] & Timer < textParser.Time2[k])
            {

                //increasing the accelerationx 1 each second
                //jerkPX = 0;
                jerkPX = textParser.jerkPX0[k];
                accelerationPX0 = textParser.accelerationPX0[k];
                accelerationPX1 += jerkPX * Time.deltaTime;
                accelerationPX = accelerationPX0 + accelerationPX1;

                speedPX0 = textParser.speedPX0[k];
                speedPX1 += accelerationPX * Time.deltaTime;
                speedPX = speedPX0 + speedPX1;

                speedX = speedPX + speedJX;

                targetX = Vector3.right * speedX; // degree pitch



                //increasing the accelerationy 2 each second
                jerkPY = textParser.jerkPY0[k];

                accelerationPY0 = textParser.accelerationPY0[k];
                accelerationPY1 += jerkPY * Time.deltaTime;
                accelerationPY = accelerationPY0 + accelerationPY1;

                speedPY0 = textParser.speedPY0[k];
                speedPY1 += accelerationPY * Time.deltaTime;
                speedPY = speedPY1 + speedPY0;

                speedY = speedPY + speedJY;

                targetY = Vector3.up * speedY; //degree yaw


                //increasing the accelerationz 0 each second
                jerkPZ = textParser.jerkPZ0[k];

                accelerationPZ0 = textParser.accelerationPZ0[k];
                accelerationPZ1 += jerkPZ * Time.deltaTime;
                accelerationPZ = accelerationPZ0 + accelerationPZ1;

                speedPZ0 = textParser.speedPZ0[k];
                speedPZ1 += accelerationPZ * Time.deltaTime;
                speedPZ = speedPZ0 + speedPZ1;

                speedZ = speedPZ + speedJZ;


                targetZ = Vector3.forward * speedZ; // degree roll



                targetXQ = Quaternion.AngleAxis(speedX * Time.deltaTime, Vector3.right);
                targetYQ = Quaternion.AngleAxis(speedY * Time.deltaTime, Vector3.up);
                targetZQ = Quaternion.AngleAxis(speedZ * Time.deltaTime, Vector3.forward);

                transform.rotation = transform.rotation * targetXQ;
                transform.rotation = transform.rotation * targetYQ;
                transform.rotation = transform.rotation * targetZQ;
            }
            else
            {
                k++;
                accelerationPX1 = 0;
                speedPX1 = 0;
                accelerationPY1 = 0;
                speedPY1 = 0;
                accelerationPZ1 = 0;
                speedPZ1 = 0;
            }

        
        }


    }

}

