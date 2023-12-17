using System;
using UnityEngine;

public class DayNightController : MonoBehaviour
{
    [SerializeField] private float timeSpeed;
    [SerializeField] private float startHour;

    [SerializeField] private Light sun;
    [SerializeField] private GameObject stars;
    [SerializeField] private float sunRise;
    [SerializeField] private float sunSet;
    [SerializeField] private float sunIntensityMax;

    private DateTime currentTime;
    private TimeSpan sunRiseHour;
    private TimeSpan sunSetHour;

    [SerializeField] private AnimationCurve smoothLightChange;
    [SerializeField] private Color daytimeAmbient;
    [SerializeField] private Color nightTimeAmbient;

    // Initialization
    void Start()
    {
        InitializeTimeAndSunParameters();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeOfDay();
        
        RotateSun();
        UpdateLighting();
    }

    private void InitializeTimeAndSunParameters()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);
        sunRiseHour = TimeSpan.FromHours(sunRise);
        sunSetHour = TimeSpan.FromHours(sunSet);
    }

    private TimeSpan TimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        if (difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }
        return difference;
    }

    private void UpdateTimeOfDay()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeSpeed);
    }

    private void RotateSun()
    {
        float sunRotation;

        if (IsDaytime())
        {
            sunRotation = CalculateSunRotation(0, 180, sunRiseHour, sunSetHour);
            stars.SetActive(false);
        }
        else
        {
            sunRotation = CalculateSunRotation(180, 360, sunSetHour, sunRiseHour);
            stars.SetActive(true);
        }

        sun.transform.rotation = Quaternion.AngleAxis(sunRotation, Vector3.right);
    }

    private bool IsDaytime()
    {
        return currentTime.TimeOfDay > sunRiseHour && currentTime.TimeOfDay < sunSetHour;
    }

    private float CalculateSunRotation(float startRotation, float endRotation, TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan timeRange = TimeDifference(fromTime, toTime);
        TimeSpan timeSinceEvent = TimeDifference(fromTime, currentTime.TimeOfDay);

        float percentage = (float)(timeSinceEvent.TotalMinutes / timeRange.TotalMinutes);

        return Mathf.Lerp(startRotation, endRotation, percentage);
    }

    private void UpdateLighting()
    {
        float dotProduct = Vector3.Dot(sun.transform.forward, Vector3.down);
        sun.intensity = Mathf.Lerp(0.01f, sunIntensityMax, smoothLightChange.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(nightTimeAmbient, daytimeAmbient, smoothLightChange.Evaluate(dotProduct));
    }
}
