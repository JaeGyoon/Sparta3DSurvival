using System;
using UnityEngine;

public class PlayerCondition : MonoBehaviour, IDamageable
{
    public UICondition conditions;

    ConditionBar healthBar { get { return conditions.conditionBars[(int)Condition.Health]; } }
    ConditionBar staminaBar { get { return conditions.conditionBars[(int)Condition.Stamina]; } }
    ConditionBar hungerBar { get { return conditions.conditionBars[(int)Condition.Hunger]; } }


    public event Action onTakeDamage;
    void Update()
    {
        hungerBar.DownValue(hungerBar.valuePerSecond * Time.deltaTime);
        if ( hungerBar.curValue == 0f)
        {
            healthBar.DownValue(hungerBar.valuePerSecond * Time.deltaTime);
        }

        staminaBar.UpValue(staminaBar.valuePerSecond * Time.deltaTime);

        if (healthBar.curValue == 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("¡Í±›!");
    }

    private void Heal(float amount)
    {
        healthBar.UpValue(amount);
    }

    private void Eat(float amount)
    {
        hungerBar.UpValue(amount);
    }

    public void TakePhysicalDamage(int damage)
    {
        healthBar.DownValue(damage);

        onTakeDamage?.Invoke();
    }
}
