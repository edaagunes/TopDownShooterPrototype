using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float timeToDestroy;
    [HideInInspector] public WeaponManager weapon;

    private void Start()
    {
        Destroy(this.gameObject,timeToDestroy);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<EnemyAI>())
        {
            EnemyAI enemyHealth = collision.gameObject.GetComponentInParent<EnemyAI>();
            enemyHealth.TakeDamage(weapon.damage);
        }
        Destroy(this.gameObject);
    }
}
