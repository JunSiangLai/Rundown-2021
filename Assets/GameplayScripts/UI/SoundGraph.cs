using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles SoundGraph
/// <para>Uses line renderer component</para>
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class SoundGraph : MonoBehaviour
{
    public int numPoints = 10;
    public float frequency = 1;
    public float scale = 10;
    public float percentageSample = 20;
    private float averageSampleSize;
    public AudioListener audioListener;

    private LineRenderer line;
    private Vector3[] linePos;
    private ProcessSound soundProcessor;


    private void Awake()
    {
        // Init
        line = GetComponent<LineRenderer>();
        soundProcessor = new ProcessSound();
    }

    void Start()
    {
        line.positionCount = numPoints;
        linePos = new Vector3[numPoints];
        for (int i = 0; i < linePos.Length; i++)
        {
            linePos[i] = new Vector3(i * (50 / numPoints), 0, 0);
        }
        line.SetPositions(linePos);
        averageSampleSize = ProcessSound.sampleSize * (percentageSample / 100) / (numPoints - 2);

        InvokeRepeating("UpdateSoundGraph", 1f, frequency);
    }

    void UpdateSoundGraph()
    {
        soundProcessor.AnalyseSound();

        line.GetPositions(linePos);

        int sampleIndex = -1;
        for(int i = 1; i < linePos.Length - 1; i++)
        {
            float sum = 0;
            for (int j = 0; j < averageSampleSize ; j++)
            {
                sampleIndex++;
                sum += soundProcessor.spectrum[sampleIndex];
            }

            linePos[i] = new Vector3(linePos[i].x, (sum / averageSampleSize) * scale, 0);
        }

        line.SetPositions(linePos);
    }

    void AnaylyseAudio()
    {
        soundProcessor.AnalyseSound();
    }
}
