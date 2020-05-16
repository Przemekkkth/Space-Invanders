﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] float damage = 100f;
    public float GetDamage()
    {
        return damage;
    }
    void Hit()
    {
        Destroy(gameObject);
    }
}
