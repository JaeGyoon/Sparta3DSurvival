using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimeCycle : MonoBehaviour
{

    [SerializeField] private float oneDayPercentage;

    [SerializeField] private float oneDayTime = 24 * 60 * 60;
    //[SerializeField] private float oneDayTime = 20;

    [SerializeField] private float startTime;
    [SerializeField] private float timeRate;

    // 기준이 되는 정오 시간( 90도 일때 태양이 중천에 떠있음)
    private Vector3 noon= new Vector3 (90f, 0f, 0f);


    [Header("Sun")]
    [SerializeField] private Light sun;
    [SerializeField] private Gradient sunColor;
    [SerializeField] private AnimationCurve sunIntensity;

    [Header("Moon")]
    [SerializeField] private Light moon;
    [SerializeField] private Gradient moonColor;
    [SerializeField] private AnimationCurve moonIntensity;

    [Header("Other Setting")]
    public AnimationCurve lightIntensity;
    public AnimationCurve reflectionIntensity;
    private void Start()
    {
        startTime = Random.Range(0f, 1f);

        // oneDayTime 이 1초 ( deltaTime ) 지나갈때 마다 증가하는 % 비율
        timeRate = 1f / oneDayTime;

        oneDayPercentage = startTime;
    }

    private void Update()
    {
        // 델타타임 1초가 지날때마다 oneDayPercentage의 %가 timeRate 만큼 증가하고 100% ( 1.0f ) 를 채우면 다시 0%로 돌아가 해당 로직을 반복
        oneDayPercentage = (oneDayPercentage + (timeRate * Time.deltaTime)) % 1f;

        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightIntensity.Evaluate(oneDayPercentage);
        RenderSettings.reflectionIntensity = reflectionIntensity.Evaluate(oneDayPercentage);

        //Debug.Log($"ambientIntensity: {lightIntensity.Evaluate(oneDayPercentage)}");
        //Debug.Log($"reflectionIntensity: {reflectionIntensity.Evaluate(oneDayPercentage)}");
    }

    void UpdateLighting(Light light, Gradient gradient, AnimationCurve animationCurve)
    {
        // animationCurve 에서 oneDayPercentage 만큼의 시간에 설정된 값을 intensity에 넣어준다.
        float intensity = animationCurve.Evaluate(oneDayPercentage);

        if (0 < intensity && intensity < 0.001f)
        {
            intensity = 0f;
        }

        /*Debug.Log($"oneDayPercentage: {oneDayPercentage}");
        Debug.Log($"intensity: {intensity}");*/
        // noon은 기준이 되는 태양의 각도
        // 정오시간을 기준으로 noon 은 90 0 0 의 각도를 가져야 하고 oneDayPercentage 은 0.5이다.
        // 
        light.transform.eulerAngles = noon * (oneDayPercentage - (light == sun ? 0.25f : 0.75f) ) * 4;

        light.color = gradient.Evaluate(oneDayPercentage);
        light.intensity = intensity;

        GameObject lightObject = light.gameObject;

        // activeInHierarchy 와 activeSelf 의 차이점
        // activeSelf는 자신은 활성화 되었지만 부모가 비활성화 되서 안보이면 true를 반환하고,
        // activeInHierarchy는 하이어라키엔 비활성화로 표시되어 있어 false 반환
        if (light.intensity == 0 && lightObject.activeInHierarchy == true)
        {
            lightObject.SetActive(false);
        }
        else if ( light.intensity > 0 && lightObject.activeInHierarchy == false)
        {
            lightObject.SetActive(true);
        }
    }

}
