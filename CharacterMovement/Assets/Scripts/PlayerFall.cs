using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFall : MonoBehaviour
{
    public BoxCollider box;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("An object entered.");
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("An object is still inside of the trigger");
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("An object left.");
    }
}
