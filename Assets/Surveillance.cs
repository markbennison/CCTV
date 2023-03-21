using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surveillance : MonoBehaviour
{
    GameObject light;
    Transform target;
    [SerializeField] float rotateSpeed = 0.01f;

    Vector3 targetVector;
    Vector3 lookVector;
    Vector3 startingDirectionVector;

    void Start()
    {
        light = transform.GetChild(0).gameObject;
        light.SetActive(false);
        startingDirectionVector = transform.forward;
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
        lookVector = startingDirectionVector;
    }


    void LookAtTarget()
    {
        Quaternion rotation = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed);
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
            targetVector = (collider.transform.position - transform.position);
            float targetAngle = Vector3.Angle(transform.forward, targetVector);

            if (targetAngle <= 60)
            {
                RaycastHit hitObject;
                Physics.Raycast(transform.position, targetVector, out hitObject, 12);
                if (hitObject.collider != null && hitObject.collider.name == collider.name)
                {
                    Debug.Log("Hit: " + hitObject.collider.name);
                    TargetAcquired();
                }
                else
                {
                    TargetLost();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TargetLost();
    }
}
