using RDG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamplePatrolVibrationController : MonoBehaviour
{
    private static readonly long[] patEnemyVibPat = { 1, 50, 50, 200, 700 };
    private static readonly int[] patEnemyVibAmp = { 64, 0, 64, 0, 0 };
    void Start()
    {
        Vibration.Vibrate(patEnemyVibPat, patEnemyVibAmp, 0);
        Debug.Log("Patrol example , " + patEnemyVibAmp[0] + "DangerVib");
    }
    private void OnDisable()
    {
        Vibration.Cancel();
    }
}
