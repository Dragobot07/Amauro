using System;
using RDG;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : Entity
{
    //TODO refactor all of player into chunks/ move functions to seperate controller classes
    /*
    public float moveSpeed = 5f; //speed of moment to target
    public Transform movePoint; //targets movements\
    
    // needed to make move pretty */

    private Vector3 facingDirection = new Vector3(1, 0);
    private float stepSize = .4f;
    public LayerMask wall;
    public LayerMask spike;
    public LayerMask enemy;
    public LayerMask goal;
    public LayerMask directing;
    public Transform internalCompass;
    public Transform entityParent;
    public OverlayController overlayController;
    public int maxVib = 64;

    private string tileInFront;
    private int currentDistanceFromEnemy = -1;
    private bool isOnEnemyPath;

    private Quaternion internalCompassStartRotation;

    #region Vibration Patterns               
    //TODO Clean up and get patterns/amps from a single source for this and example vibs



    // format pattern   {spacer,on,off,"off",off,on,off,"off",off} spacer and off=1
    //format amplitude  {on,off,"off",off,on,off,"off",off,spacer} spacer and off=1

    //need spacer values at beginning of pattern and end of amp to line up correctly
    private static readonly int spacer = 1;


    private static readonly long[] wallVibAmpPat = { spacer, 100, 1, 900, 1 };
    private static readonly int[] wallVibAmp = { 64, 0, 1, 0, spacer };
    private static readonly long[] spikeVibAmpPat = { spacer, 100, 1, 100, 1, 100, 1, 100, 1, 100, 1, 100, 1, 100, 1, 100, 1, 100, 1, 100, 1 };
    private static readonly int[] spikeVibAmp = { 64, 0, 0, 0, 64, 0, 0, 0, 64, 0, 0, 0, 64, 0, 0, 0, 64, 0, 0, 0, spacer };
    private static readonly long[] patEnemyVibAmpPat = { spacer, 50, 1, 50, 1, 200, 1, 700, 0 };
    private static readonly int[] patEnemyVibAmp = { 64, 0, 0, 0, 64, 0, 0, 0, spacer };
    private static readonly long[] goalVibAmpPat = { spacer, 100, 1, 100, 1, 50, 1, 50, 1, 250, 1, 450, 0 };
    private static readonly int[] goalVibAmp = { 64, 0, 0, 0, 64, 0, 0, 0, 64, 0, 0, 0, spacer };
    private static readonly long[] emptyVibAmpPat = { spacer, 1, 1, 1000, 1 };
    private static readonly int[] emptyVibAmp = { 1, 0, 1, 0, spacer };
    

    private static readonly long[] wallVibPat = { spacer, 100, 900};
    private static readonly long[] spikeVibPat = { spacer, 100, 100,100,100,100,100,100,100,100,100 };
    private static readonly long[] patEnemyVibPat = { spacer, 50, 50, 200, 700 };
    private static readonly long[] goalVibPat = { spacer, 100, 100, 50, 50, 250, 450 };
    private static readonly long[] emptyVibPat= { spacer};

    bool amplitudeCompatible=false;



    #endregion

    public Text txt;

    override public void Start()
    {
        base.Start();
        internalCompassStartRotation = internalCompass.rotation;
        if (RDG.Vibration.HasAmplitudeControl())
        {
            amplitudeCompatible = true;
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        Vibrate(focus);            
    }       //stops and resumes the vibrations when app unfocused

    private void Vibrate(bool vibrate)          //stops and resumes the vibrations given a bool
    {
        
        if (!vibrate)
        {
            Vibration.Cancel();

        }
        if (vibrate)
        {
            UpdatePlayerInterface();
        }
    }

    private void Update()
    {
        //Tests for death conditions and handles death
        if (Physics2D.OverlapCircle(entityImage.position, .1f, spike) || Physics2D.OverlapCircle(entityImage.position, .1f, enemy))
        {
            Vibrate(false);
            overlayController.ShowDeathPanel(true);
        }

        //Tests for win conditions and handles win
        else if (Physics2D.OverlapCircle(entityImage.position , .1f, goal))
        {
            Vibrate(false);
            overlayController.ShowVictoryPanel(true);
        }

        //else updates isOnEnemyPath and tileInFront(if nessesary)
        else
        {
            SetIsOnEnemyPath();
            
            //Checks if the mask in front is the same as the mask of tileInFront                 (a mask that would be -1 i.e. doenst exist becomes 0 and is interpreted by IsMaskInFrontOf as there being no mask in front of it)
            if (!IsMaskInFrontOf((int)Math.Pow(2, LayerMask.NameToLayer(tileInFront))))
            {
                //DebugLog("Tile: "+tileInFront+" layer: "+ LayerMask.NameToLayer(tileInFront)+"|");
                    DetectTileInFront();
            }

        }

       

    }

    private void UpdatePlayerInterface()
    {
        
        int[] finalAmp;
        string finalAmpText="";
        if (amplitudeCompatible)
        {
            switch (tileInFront)
            {
                case "Wall":
                    finalAmp = ModAmpForPath(CopyArray(wallVibAmp));
                    for (int i = 0; i < finalAmp.Length; i++)
                    {
                        finalAmpText += (finalAmp[i] + " ");
                    }
                    Vibration.Vibrate(wallVibAmpPat, finalAmp, 0, true, this);
                    Debug.Log("Wall in front, " + finalAmpText + "DangerVib");
                    break;
                case "Spike":
                    finalAmp = ModAmpForPath(CopyArray(spikeVibAmp));
                    Vibration.Vibrate(spikeVibAmpPat, finalAmp, 0);
                    Debug.Log("Spike in front, " + ModAmpForPath(finalAmp)[0] + "DangerVib");
                    break;
                case "Patrolling Enemy":
                    finalAmp = ModAmpForPath(CopyArray(patEnemyVibAmp));
                    Vibration.Vibrate(patEnemyVibAmpPat, finalAmp, 0);
                    Debug.Log("Patrol in front, " + ModAmpForPath(finalAmp)[0] + "DangerVib");
                    break;
                case "Goal":
                    finalAmp = ModAmpForPath(CopyArray(goalVibAmp));
                    Vibration.Vibrate(goalVibAmpPat, finalAmp, 0);
                    Debug.Log("Goal in front, " + ModAmpForPath(finalAmp)[0] + "DangerVib");
                    break;
                case "Empty":

                    finalAmp = ModAmpForPath(CopyArray(emptyVibAmp));
                    for (int i = 0; i < finalAmp.Length; i++)
                    {
                        finalAmpText += (finalAmp[i] + " ");
                    }
                    Vibration.Vibrate(emptyVibAmpPat, finalAmp, 0, true, this);
                    Debug.Log("Empty in front, " + finalAmpText + "DangerVib");

                    break;
                default:
                    Debug.LogError("tileInFront switch hit default");
                    break;
            }

        }
        else
        {
            switch (tileInFront)
            {
                case "Wall":
                    Vibration.Vibrate(wallVibPat, null, 0, true, this);
                    Debug.Log("Wall in front");
                    break;
                case "Spike":
                    Vibration.Vibrate(spikeVibPat, null, 0, true, this);
                    Debug.Log("Spike in front");
                    break;
                case "Patrolling Enemy":
                    Vibration.Vibrate(patEnemyVibPat, null, 0, true, this);
                    Debug.Log("Patrol in front");
                    break;
                case "Goal":
                    Vibration.Vibrate(goalVibPat, null, 0, true, this);
                    Debug.Log("Goal in front" );
                    break;
                case "Empty":
                    Vibration.Vibrate(emptyVibPat, null, 0, true, this);
                    Debug.Log("Empty in front, " + finalAmpText + "DangerVib");

                    break;
                default:
                    Debug.LogError("tileInFront noamp switch hit default");
                    break;
            }

        }
    }

    private int[] CopyArray(int[] array)
    {
        int[] copy = new int[array.Length];
        for (int i=0; i<array.Length;i++ )
        {
            copy[i] = array[i];
        }
        return copy;
    }

    public int GetDistanceFromEnemy()
    {
        return currentDistanceFromEnemy;
    }

    internal void SetDistanceFromEnemy(int distance) // sets current distance equal to the least of all enemies distances unless their distance is -1 
    {
        if ((currentDistanceFromEnemy > distance || currentDistanceFromEnemy == -1 )&& distance!=-1)  //when currentDistanceFromEnemy == -1, it will be set to any other value, otherwise only set when it is greater than the tested distance
        {
            currentDistanceFromEnemy = distance;
            UpdatePlayerInterface();//to update the DangerVibration //PROBLEM can display msg multiple times due to it being outputed each time a smaller distance is detected
        }
        else if (!isOnEnemyPath)
        {
            currentDistanceFromEnemy = -1;
        }
    }

    internal void DebugLog(string v)
    {
        txt.text += v;
        Debug.Log(v);
    }

    private int[] ModAmpForPath(int[] initVibAmp)
    {
        int[] finVibAmp = initVibAmp;
        if (isOnEnemyPath)
        {        // V first "off" amp index |||     V  distance between "off"s
            for (int i = 2; i < finVibAmp.Length; i+=4)
            {
                if  (finVibAmp[i] == 0)
                {
                    //Debug.Log(finVibAmp[i]);

                    finVibAmp[i] = maxVib/ currentDistanceFromEnemy;
                }
            }
        }
        return finVibAmp;
    }

    private void DetectTileInFront()
    {
        if (IsMaskInFrontOf(wall))
        {
            tileInFront = "Wall";
        }
        else if (IsMaskInFrontOf(spike))
        {
            tileInFront = "Spike";
        }
        else if (IsMaskInFrontOf(enemy))
        {
            tileInFront = "Patrolling Enemy";
        }
        else if (IsMaskInFrontOf(goal))
        {
            tileInFront = "Goal";
        }
        else
        {
            tileInFront = "Empty";
        }
        UpdatePlayerInterface();
    }

    private void SetIsOnEnemyPath()
    {
        
        if (Physics2D.OverlapCircle(entityImage.position, .1f, directing))
        {
            isOnEnemyPath = true;
        }
        else
            isOnEnemyPath = false;
    }

    public void PlayerMove()
    {
        if (!IsMaskInFrontOf(wall))
        {
            entityImage.position += facingDirection * stepSize;
        }

    }

    public void RotateLeft()
    {
        entityImage.Rotate(new Vector3(0, 0, 1), -90);
        facingDirection = new Vector3(-facingDirection.y, facingDirection.x);
        internalCompass.Rotate(Vector3.forward, 90, Space.Self);
    }

    public void RotateRight()
    {
        entityImage.Rotate(new Vector3(0, 0, 1), -90);
        facingDirection = new Vector3(facingDirection.y, -facingDirection.x);
        internalCompass.Rotate(Vector3.forward, -90,Space.Self);
    }

    public void Kill()
    {
        
        for (int n = 0; n < entityParent.childCount; n++)
        {
            Transform possibleEntity = entityParent.GetChild(n);
            possibleEntity.GetComponent<Entity>().Reset();
        }
        facingDirection =new Vector3(1,0);
        internalCompass.SetPositionAndRotation(internalCompass.position, internalCompassStartRotation);
        overlayController.ShowDeathPanel(false);
    }

    public Boolean IsMaskInFrontOf(LayerMask mask)
    {
        if (mask != 0)
        {
            //DebugLog("Is " + LayerMask.LayerToName((int)Math.Log(mask, 2)) + " in front? ");
        }
        else
        {
            //DebugLog("Is Empty in front? ");
        }
        if (mask == 0)
        {
            if (tileInFront == "Empty")
            {
                //DebugLog("Test each possible mask: ");
                for (int i = 11; i < 31; i++) // NOT FREAKING 32!!!! ITS 31 !!!!! THANK THE LORD I FOUND THE BUG!!!!!
                {
                    
                    if (IsMaskInFrontOf((int)Math.Pow(2, i))) // if it is true another mask is in front
                    {
                        //DebugLog((int)Math.Pow(2, i) + " is in front so not empty, return false. ");
                        return false;       // then it's not empty
                    }
                       
                }
                //DebugLog("Return True ");
                return true;
            }
            else
            {

                Debug.LogError("Unexpected mask-in-front-of value: tileInFront:"+tileInFront+ " "+ LayerMask.NameToLayer(tileInFront));
                //DebugLog("Return False on error ");
                return false;
            }
        }
       
        else
            return Physics2D.OverlapCircle(entityImage.position + facingDirection * stepSize, .1f, mask);
    }

    public string GetTileInFront() 
    {
        return tileInFront;
    }

    public Boolean GetIsOnEnemyPath()
    {
        return isOnEnemyPath;
    }

}
