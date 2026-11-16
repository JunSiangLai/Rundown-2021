using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessSound
{
    public float refValue = 0.1f;
    public const int sampleSize = 1024;

    private float dbValue;
    //private float pitchValue;

    private float[] samples;
    public float[] spectrum;
    //private float sampleRate;

    public ProcessSound()
    {
        samples = new float[sampleSize];
        spectrum = new float[sampleSize];
        //sampleRate = AudioSettings.outputSampleRate;
    }

    public void AnalyseSound()
    {
        // Samples
        //AudioListener.GetOutputData(samples, 0);
        //Debug.Log(samples);

        //// RMS Value
        //float sum = 0;
        //for (int i = 0; i < sampleSize; i++)
        //{
        //    sum += samples[i] * samples[i];
        //}
        //float rmsValue = Mathf.Sqrt(sum / sampleSize);

        //// DB Value
        //dbValue = 20 * Mathf.Log10(rmsValue / refValue);

        // Spectrum
        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
    }
}
