using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float health = 100f;
    public float atkDmg = 2f;
    // public float xRange = 5f;
    // private bool isPatrol = true;

    public void takeDmg(float dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
