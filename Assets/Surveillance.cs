using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surveillance : MonoBehaviour
{
    GameObject light;
    Transform target;

    void Start()
    {
        light = transform.GetChild(0).gameObject;
        light.SetActive(false);
    }


    void Update()
    {

    }
    void LookAtTarget()
    {
        //this.transform.LookAt(playerTransform.position);
        Vector3 lookVector = target.position - transform.position;
        lookVector.y = transform.position.y;
        Quaternion rotation = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.01f);
    }

    private void OnTriggerEnter(Collider collider)
    {
        target = collider.transform;
    }

    private void OnTriggerStay(Collider collider)
    {
        Debug.Log("CCTV RANGE: " + collider.name);
        if (collider.tag == "Player")
        {
            Vector3 directionVector = (collider.transform.position - transform.position);
            float targetAngle = Vector3.Angle(transform.forward, directionVector);
            float dotProduct = Vector3.Dot(transform.forward.normalized, directionVector.normalized);
            //60° = 0.5 on Dot Product
            Debug.Log("CCTV to Player. Angle: " + targetAngle + " | Dot Product: " + dotProduct);

            if (targetAngle <= 60)
            {
                Debug.Log("CCTV FOV: ");
                RaycastHit hitObject;
                Physics.Raycast(transform.position, directionVector, out hitObject, 12);
                if (hitObject.collider != null && hitObject.collider.name == collider.name)
                {
                    Debug.Log("Hit: " + hitObject.collider.name);
                    light.SetActive(true);
                }
                else
                {
                    light.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        light.SetActive(false);
        target = null;
    }
}
