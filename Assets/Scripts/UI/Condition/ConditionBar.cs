using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionBar : MonoBehaviour
{
    public float curValue;
    [SerializeField] private float initValue;
    [SerializeField] private float maxValue;
    public float valuePerSecond;
    [SerializeField] private Image UIFill;

    // Start is called before the first frame update
    void Start()
    {
        curValue = initValue;
    }

    // Update is called once per frame
    void Update()
    {
        UIFill.fillAmount = GetPerCentage();
    }

    private float GetPerCentage()
    {
        //Debug.Log(curValue / maxValue);
        return curValue / maxValue;
    }

    public void UpValue(float value)
    {
        curValue = Mathf.Min(curValue+value, maxValue);
    }

    public void DownValue(float value)
    {
        curValue = Mathf.Max(curValue - value, 0);
    }
}
