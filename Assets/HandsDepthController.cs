using Oculus.Haptics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandsDepthController : MonoBehaviour
{
    public HapticSource hapticSource;
    public AudioClip greatClip;

    public enum MovementDirection
    {
        None,
        Rising,
        Falling
    }

    Vector3 startPos;
    private float previousRawDepth;
    private MovementDirection currentDirection = MovementDirection.None;
    private bool isFirstUpdate = true;

    // Minimum movement delta to register a direction change, avoids noise-triggered false positives
    private const float TRIGGER_THRESHOLD = 0.002f;

    private List<float> compressionTimes = new List<float>();
    private const int MAX_RECORDS = 7;

    private List<float> recordedFrequencies = new List<float>();
    private List<float> recordedDepths = new List<float>();

    void Start()
    {
        startPos = transform.localPosition;
    }

    public void ResetAll()
    {
        recordedFrequencies.Clear();
        recordedDepths.Clear();
        compressionTimes.Clear();
        SystemManager.instance.CPRPanel.transform.GetChild(0).GetChild(3).GetComponent<Text>().text = "Depth: 0 cm";
        SystemManager.instance.CPRPanel.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "Frequency: 0 PM";
        SystemManager.instance.CPRPanel.transform.GetChild(0).GetChild(4).GetComponent<Text>().text = " ";
    }

    void Update()
    {
        Vector3 startCenterPos = HandsPositionTrigger.instance.StartCenterPos;
        Vector3 centerPos = HandsPositionTrigger.instance.CenterPos;

        float rawDepth = (centerPos - startCenterPos).y;

        if (isFirstUpdate)
        {
            previousRawDepth = rawDepth;
            isFirstUpdate = false;
            return;
        }

        MovementDirection newDirection = GetMovementDirection(rawDepth);

        if (newDirection != currentDirection)
        {
            HandleDirectionChange(currentDirection, newDirection, rawDepth);
            currentDirection = newDirection;
        }

        previousRawDepth = rawDepth;

        float clampedDepth = Mathf.Clamp(rawDepth, -0.1f, 0.1f);
        transform.localPosition = startPos - Vector3.right * clampedDepth;
    }

    private MovementDirection GetMovementDirection(float currentDepth)
    {
        if (currentDepth - previousRawDepth > TRIGGER_THRESHOLD)
            return MovementDirection.Rising;
        if (previousRawDepth - currentDepth > TRIGGER_THRESHOLD)
            return MovementDirection.Falling;
        return currentDirection;
    }

    private void HandleDirectionChange(MovementDirection oldDirection, MovementDirection newDirection, float depth)
    {
        float frequency = CalculateFrequency();

        if (newDirection == MovementDirection.Falling)
            RecordCompressionTime();

        if (oldDirection == MovementDirection.Rising && newDirection == MovementDirection.Falling)
            OnStartFalling(depth, frequency);
        else if (oldDirection == MovementDirection.Falling && newDirection == MovementDirection.Rising)
            OnStartRising(depth, frequency);
    }

    private void RecordCompressionTime()
    {
        compressionTimes.Add(Time.time);
        while (compressionTimes.Count > MAX_RECORDS)
            compressionTimes.RemoveAt(0);
    }

    private float CalculateFrequency()
    {
        if (compressionTimes.Count < 2)
            return 0f;

        float totalDuration = compressionTimes[compressionTimes.Count - 1] - compressionTimes[0];
        float averageInterval = totalDuration / (compressionTimes.Count - 1);
        return 60f / averageInterval;
    }

    private void OnStartRising(float depth, float frequency)
    {
        if (!SystemManager.instance.IsStartTest)
            return;

        float depthCm = depth * -100f;
        float displayDepth = Mathf.Clamp(depthCm, 0f, 100f);

        SystemManager.instance.CPRPanel.transform.GetChild(0).GetChild(3).GetComponent<Text>().text =
            "Depth: " + displayDepth.ToString("F1") + " cm";
        SystemManager.instance.CPRPanel.transform.GetChild(0).GetChild(2).GetComponent<Text>().text =
            "Frequency: " + frequency.ToString("F0") + " PM";

        bool isGoodDepth = depthCm >= 4f && depthCm <= 6f;
        if (isGoodDepth)
        {
            SystemManager.instance.CPRPanel.transform.GetChild(0).GetChild(4).GetComponent<Text>().text = "Great!";
            hapticSource.Play();
            AudioManager.instance.PlayOneShot(greatClip, 1f);
        }
        else
        {
            SystemManager.instance.CPRPanel.transform.GetChild(0).GetChild(4).GetComponent<Text>().text = "Bad!";
        }

        recordedFrequencies.Add(frequency);
        recordedDepths.Add(depthCm);
    }

    private void OnStartFalling(float depth, float frequency)
    {
        // Reserved for future: play haptic/audio feedback at the start of each downward stroke
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FirstTrigger"))
        {
            other.gameObject.SetActive(false);
            SystemManager.instance.ShowInfoSimulationPanel(
                "Nice job, then repeat the same process as before, trying to keep the frequency and depth of compressions the same as in the tutorial",
                delegate { StartCoroutine(SystemManager.instance.StartTest()); });
        }
    }

    public float CalculateAverageFrequency()
    {
        if (recordedFrequencies.Count == 0)
            return 0f;

        float sum = 0f;
        foreach (float f in recordedFrequencies)
            sum += f;

        return sum / recordedFrequencies.Count;
    }

    public float CalculateDepthAccuracy()
    {
        if (recordedDepths.Count == 0)
            return 0f;

        int correctCount = 0;
        foreach (float depth in recordedDepths)
        {
            if (depth >= 4f && depth <= 6f)
                correctCount++;
        }

        return (float)correctCount / recordedDepths.Count;
    }

    public float CalculateScore()
    {
        float averageFrequency = CalculateAverageFrequency();
        float depthAccuracy = CalculateDepthAccuracy();

        float frequencyScore;
        if (averageFrequency >= 100f && averageFrequency <= 120f)
            frequencyScore = 50f;
        else if (averageFrequency > 120f)
            frequencyScore = 50f * (120f / averageFrequency);
        else
            frequencyScore = 50f * (averageFrequency / 100f);

        float depthScore = depthAccuracy * 50f;

        return frequencyScore + depthScore;
    }
}
