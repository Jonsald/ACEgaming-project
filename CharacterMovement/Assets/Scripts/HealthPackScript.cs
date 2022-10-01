using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackScript : MonoBehaviour
{
    public void DestroyPack() 
    {
        Destroy(gameObject);
    }
}
