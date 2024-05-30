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

    // ������ �Ǵ� ���� �ð�( 90�� �϶� �¾��� ��õ�� ������)
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

        // oneDayTime �� 1�� ( deltaTime ) �������� ���� �����ϴ� % ����
        timeRate = 1f / oneDayTime;

        oneDayPercentage = startTime;
    }

    private void Update()
    {
        // ��ŸŸ�� 1�ʰ� ���������� oneDayPercentage�� %�� timeRate ��ŭ �����ϰ� 100% ( 1.0f ) �� ä��� �ٽ� 0%�� ���ư� �ش� ������ �ݺ�
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
        // animationCurve ���� oneDayPercentage ��ŭ�� �ð��� ������ ���� intensity�� �־��ش�.
        float intensity = animationCurve.Evaluate(oneDayPercentage);

        if (0 < intensity && intensity < 0.001f)
        {
            intensity = 0f;
        }

        /*Debug.Log($"oneDayPercentage: {oneDayPercentage}");
        Debug.Log($"intensity: {intensity}");*/
        // noon�� ������ �Ǵ� �¾��� ����
        // �����ð��� �������� noon �� 90 0 0 �� ������ ������ �ϰ� oneDayPercentage �� 0.5�̴�.
        // 
        light.transform.eulerAngles = noon * (oneDayPercentage - (light == sun ? 0.25f : 0.75f) ) * 4;

        light.color = gradient.Evaluate(oneDayPercentage);
        light.intensity = intensity;

        GameObject lightObject = light.gameObject;

        // activeInHierarchy �� activeSelf �� ������
        // activeSelf�� �ڽ��� Ȱ��ȭ �Ǿ����� �θ� ��Ȱ��ȭ �Ǽ� �Ⱥ��̸� true�� ��ȯ�ϰ�,
        // activeInHierarchy�� ���̾��Ű�� ��Ȱ��ȭ�� ǥ�õǾ� �־� false ��ȯ
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
