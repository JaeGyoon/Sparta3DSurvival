using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    [SerializeField] private float tickRate = 0.5f;

    List<IDamageable> damageableList = new List<IDamageable>();

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("AoEDamage", 0, tickRate);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( other.TryGetComponent(out IDamageable damageable) )
        {
            damageableList.Add(damageable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageableList.Remove(damageable);
        }
    }

    void AoEDamage()
    {
        for ( int i = 0; i < damageableList.Count; i++ )
        {
            damageableList[i].TakePhysicalDamage(damage);
        }
    }
}
