using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Surveillance : MonoBehaviour
{
    GameObject lens;
    GameObject light;
    [SerializeField] float rotateSpeed = 0.001f;
    [SerializeField] float turnClamp = 90f;

    Vector3 targetVector;
    Vector3 lookVector;
    Vector3 startingDirectionVector;

    void Start()
    {
        lens = transform.GetChild(0).gameObject;
        light = lens.transform.GetChild(0).gameObject;
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
        //Quaternion rotation = Quaternion.LookRotation(lookVector);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed);

        float lookAngle = Vector3.SignedAngle(startingDirectionVector, targetVector, Vector3.up);

        //lens.transform.RotateAround(Vector3.up, lookAngle);
        //lens.transform = Quaternion.Euler(lens.transform.rotation.x, lookVector.y, lens.transform.rotation.z);


        //float looktAngle = Vector3.Angle(startingDirectionVector, lookVector);

        //Debug.Log("!!! " + looktAngle);

        //if (looktAngle < -turnClamp || looktAngle > turnClamp)
        //{

        //    //transform.rotation = Quaternion.Euler(transform.rotation.x, Mathf.Clamp(transform.rotation.y, -turnClamp, turnClamp), transform.rotation.z);
        //}



        //Debug.Log(transform.rotation.x + ", " + transform.rotation.y + ", " + transform.rotation.z);





        //transform.rotation = Quaternion.Euler(transform.rotation.x, Mathf.Clamp(transform.rotation.y, -turnClamp, turnClamp), transform.rotation.z);

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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TargetLost();
    }
}
