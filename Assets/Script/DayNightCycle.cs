using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time;
    public float fullDayLenght;     //전체 하루 길이
    public float startTime = 0.4f;
    private float timeRate;
    public Vector3 noon; //Vector 90 0 0;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;


    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;      //조명강도배율
    public AnimationCurve reflectionIntensityMultiplier;    //반사강도배율

    private void Start()
    {
        timeRate = 1.0f / fullDayLenght;
        time = startTime;
    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f; //  %를 하는 이유 : 1.0보다 작으면 그대로 유지하려고

        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);
    }

    void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f;
        lightSource.color = gradient.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
        {
            go.SetActive(false);
        }
        else if(lightSource.intensity > 0 && !go.activeInHierarchy)
        {
            go.SetActive(true);
        }
    }
}
