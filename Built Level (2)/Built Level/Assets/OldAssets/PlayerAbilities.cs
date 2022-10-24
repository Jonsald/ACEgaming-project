using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public float health = 100f;
    public float pushPower = 2f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "MoveableBox")
        {
            Rigidbody box = hit.collider.GetComponent<Rigidbody>();

            if (box != null)
            {
                Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, 0);
                box.velocity = pushDirection * pushPower;
            }
        }

        if (hit.transform.tag == "HealthPack")
        {
            HealthPackScript hps = hit.collider.GetComponent<HealthPackScript>();

            if (hps != null) 
            {
                hps.DestroyPack();
                health = 100f;
            }
        }

        if (hit.transform.tag == "Enemy")
        {
            EnemyScript es = hit.collider.GetComponent<EnemyScript>();
            
            if (es != null) 
            {
                health -= es.atkDmg;
                if (health <= 0) 
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    void Update() 
    {
        if (health <= 0) 
        {
            Destroy(gameObject);
        }
    }
}
