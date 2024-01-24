using UnityEngine;

public class TrainController : MonoBehaviour
{
    public GameObject targetObject;
    public float maxSpeed = 10f;
    public float acceleration = 5f;
    public float deceleration = 5f;
    public float friction = 1f;
    public float safeDistance = 10f;

    private float currentSpeed = 0f;
    private bool isStopping = false;

    void Update()
    {
        HandleInput();
        ApplyFriction();
        MoveTrain();
        AutomaticStopCheck();
    }

    void HandleInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            currentSpeed -= deceleration * Time.deltaTime;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
    }

    void ApplyFriction()
    {
        if (currentSpeed > 0 && !Input.GetKey(KeyCode.W))
        {
            currentSpeed -= friction * Time.deltaTime;
            currentSpeed = Mathf.Max(currentSpeed, 0);
        }
    }

    void MoveTrain()
    {
        transform.Translate(Vector3.right * currentSpeed * Time.deltaTime);
    }

    void AutomaticStopCheck()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetObject.transform.position);
        if (distanceToTarget <= safeDistance && !isStopping)
        {
            isStopping = true;
            StartCoroutine(StopTrain());
        }
    }

    System.Collections.IEnumerator StopTrain()
    {
        while (currentSpeed > 0)
        {
            currentSpeed -= deceleration * Time.deltaTime;
            yield return null;
        }
        currentSpeed = 0;
        isStopping = false;
    }
}