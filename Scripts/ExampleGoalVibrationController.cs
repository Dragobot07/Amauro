using RDG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleGoalVibrationController : MonoBehaviour //Todo Extend ExampleVib
{
    private static readonly long[] goalVibPat = { 1, 100, 100, 50, 50, 250, 450 };
    private static readonly int[] goalVibAmp = { 64, 0, 64, 0, 64, 0, 0 };
    void Start()
    {
        Vibration.Vibrate(goalVibPat, goalVibAmp, 0);
        Debug.Log("goal example , " + goalVibAmp[0] + "DangerVib");
    }
    private void OnDisable()
    {
        Vibration.Cancel();
    }
}