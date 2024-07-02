using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed = 10f;
    public float turnSpeed = 5f;
    public Transform destination1;
    public Transform destination2;
    public Transform destination3;
    public Transform destination4;
    public Transform myDestination;
    public Transform intersection;

    public float distanceToIntersection;
    public float turningDistance = 5;

    private bool isWaiting = false;
    public int j = 0;
    public float initDotForward;
    public bool hasAlreadyWaited;
    private void Start()
    {
        ChooseDestination();
    }

    public void ChooseDestination()
    {
        int destinationNo = Random.Range(1, 5); // Changed to 5 to include destination 4

        if (destinationNo == 4)
        {
            myDestination = destination4;
        }
        else if (destinationNo == 3)
        {
            myDestination = destination3;
        }
        else if (destinationNo == 2)
        {
            myDestination = destination2;
        }
        else
        {
            myDestination = destination1;
        }
        if (Vector3.Distance(transform.position, myDestination.position) < 5f)
        {
            ChooseDestination();
        }


    }

    void Update()
    {
        if (isWaiting)
        {
            return;
        }

        distanceToIntersection = Vector3.Distance(transform.position, intersection.position);

        if (distanceToIntersection < 15)
        {
            Vector3 directionToDestination = (myDestination.position - transform.position).normalized;

            float dotForward = Vector3.Dot(transform.forward, directionToDestination);
            if (j == 0)
            {
                initDotForward = Vector3.Dot(transform.forward, directionToDestination);
                j = 1;
            }
            if (initDotForward >= 0.97f && !hasAlreadyWaited) // Check if the destination is straight ahead and very close to the intersection
            {
                StartCoroutine(WaitAtIntersection(2));
            }
            else if (initDotForward <= .2f && initDotForward >= .14f && !hasAlreadyWaited)
            {
                StartCoroutine(WaitAtIntersection(4));

            }
            else if (dotForward < 0.97f && distanceToIntersection < turningDistance) // Adjust the threshold as needed
            {
                // Rotate towards the direction of the destination
                Vector3 newDir = Vector3.RotateTowards(transform.forward, directionToDestination, turnSpeed * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);
            }
            else
            {
                // Move forward towards the destination
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }
        else
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, myDestination.position) < 8)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator WaitAtIntersection(float waitTime)
    {
        hasAlreadyWaited = true;
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
    }
}
