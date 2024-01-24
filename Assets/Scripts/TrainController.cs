using UnityEngine;

public class TrainController : MonoBehaviour
{
    public GameObject targetObject;
    public float maxSpeed = 10f;
    public float acceleration = 5f;
    public float deceleration = 5f;
    public float friction = 1f;
    public float minimumSafeDistance = 5f;

    private float currentSpeed = 0f;
    private bool isStopping = false;
    public bool autoAccelerate = true;

    void Update()
    {
        if (autoAccelerate)
        {
            AutomaticAcceleration();
        }
        ApplyFriction();
        MoveTrain();
        AutomaticStopCheck();
    }

    void AutomaticAcceleration()
    {
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
    }

    void ApplyFriction()
    {
        if (currentSpeed > 0 && !isStopping)
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
        float dynamicSafeDistance = CalculateDynamicSafeDistance();
        float distanceToTarget = Vector3.Distance(transform.position, targetObject.transform.position);

        if (distanceToTarget <= dynamicSafeDistance && !isStopping)
        {
            isStopping = true;
            autoAccelerate = false; // Disable automatic acceleration
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
        //autoAccelerate = true; // Re-enable automatic acceleration if needed
    }

    float CalculateDynamicSafeDistance()
    {
        return minimumSafeDistance + currentSpeed * currentSpeed / (2 * deceleration);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, CalculateDynamicSafeDistance());
    }
}
