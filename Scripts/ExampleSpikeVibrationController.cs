using RDG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleSpikeVibrationController : MonoBehaviour
{
    private static readonly long[] spikeVibPat = { 1, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100 };
    private static readonly int[] spikeVibAmp = { 64, 0, 64, 0, 64, 0, 64, 0, 64, 0, 0 };

    void Start()
    {
        Vibration.Vibrate(spikeVibPat, spikeVibAmp, 0);
        Debug.Log("spike example , " + spikeVibAmp[0] + "DangerVib");
    }
    private void OnDisable()
    {
        Vibration.Cancel();
    }
}