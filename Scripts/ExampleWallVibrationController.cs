using RDG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleWallVibrationController : MonoBehaviour
{
    private static readonly long[] wallVibPat = { 1, 100, 900 };
    private static readonly int[] wallVibAmp = { 64, 0, 0 };

    void Start()
    {
        Vibration.Vibrate(wallVibPat, wallVibAmp, 0);
        Debug.Log("wall example , " + wallVibAmp[0] + "DangerVib");
    }
    private void OnDisable()
    {
        Vibration.Cancel();
    }
}