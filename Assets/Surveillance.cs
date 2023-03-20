using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surveillance : MonoBehaviour
{
    GameObject light;

    void Start()
    {
        light = transform.GetChild(0).gameObject;
        light.SetActive(false);
    }


    void Update()
    {
        
    }

	private void OnTriggerStay(Collider collider)
	{
        Debug.Log("CCTV RANGE: " + collider.name);
        if(collider.tag == "Player")
		{
            Vector3 directionVector = (collider.transform.position - transform.position);
            float targetAngle = Vector3.Angle(transform.forward, directionVector);
            float dotProduct = Vector3.Dot(transform.forward.normalized, directionVector.normalized);
            //60° = 0.5 on Dot Product
            Debug.Log("CCTV to Player. Angle: " + targetAngle + " | Dot Product: " + dotProduct);

            if(targetAngle <= 60)
			{
                Debug.Log("CCTV FOV: ");
                light.SetActive(true);
			}
			else
			{
                light.SetActive(false);
            }
        }
    }

	private void OnTriggerExit(Collider other)
	{
        light.SetActive(false);
    }
}
