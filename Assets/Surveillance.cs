using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Surveillance : MonoBehaviour
{
    GameObject lens;
    GameObject light;
    [SerializeField] float rotateSpeed = 0.005f;
    [SerializeField] float turnClamp = 90f;

    Vector3 targetVector;
    Vector3 lookVector;
    Vector3 startingDirectionVector;

    void Start()
    {
        lens = transform.GetChild(0).gameObject;
        light = lens.transform.GetChild(0).gameObject;

        TargetLost();
    }


    void Update()
    {
        LookAtTarget();
    }

    void TargetAcquired()
    {

        light.SetActive(true);
        lookVector = targetVector;

    }

    void TargetLost()
    {
        light.SetActive(false);
        lookVector = transform.forward;
    }


    void LookAtTarget()
    {
        Quaternion rotation = Quaternion.LookRotation(lookVector);
        lens.transform.rotation = Quaternion.Slerp(lens.transform.rotation, rotation, rotateSpeed);
    }

    private void OnTriggerStay(Collider collider)
    {
        //Debug.Log("CCTV RANGE: " + collider.name);
        if (collider.tag == "Player")
        {
            targetVector = (collider.transform.position - transform.position);
            float targetAngle = Vector3.Angle(transform.forward, targetVector);

            if (targetAngle <= 60)
            {
                RaycastHit hitObject;
                Physics.Raycast(transform.position, targetVector, out hitObject, 12);
                if (hitObject.collider != null && hitObject.collider.name == collider.name)
                {
                    //Debug.Log("Hit: " + hitObject.collider.name);
                    TargetAcquired();
                }
                else
                {
                    TargetLost();
                }
            }
            else
            {
                TargetLost();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TargetLost();
    }
}
