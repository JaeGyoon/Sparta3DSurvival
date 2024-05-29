using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public ConditionBar[] conditionBars;

    private void Awake()
    {
        conditionBars = GetComponentsInChildren<ConditionBar>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CharacterManager.Instance.Player.playerCondition.conditions = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum Condition
{
    Health = 0,
    Stamina = 1,
    Hunger = 2,
}