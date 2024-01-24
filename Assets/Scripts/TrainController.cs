using UnityEngine;

public class TrainController : MonoBehaviour
{
    public GameObject targetObject;
    public float maxSpeed = 10f;
    public float acceleration = 5f;
    public float deceleration = 5f;
    public float friction = 1f;
    public float minimumSafeDistance = 5f; // Minimum safe distance

    private float currentSpeed = 0f;
    private bool isMovingRight = true;
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
        float dynamicSafeDistance = CalculateDynamicSafeDistance();
        float distanceToTarget = Vector3.Distance(transform.position, targetObject.transform.position);

        if (distanceToTarget <= dynamicSafeDistance && !isStopping)
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

    float CalculateDynamicSafeDistance()
    {
        // Assuming safe distance is proportional to the square of the speed
        return minimumSafeDistance + currentSpeed * currentSpeed / (2 * deceleration);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, CalculateDynamicSafeDistance());
    }
}
