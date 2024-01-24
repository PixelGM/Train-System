using UnityEngine;

public class TrainController : MonoBehaviour
{
    public float turnSpeed = 50f;

    public float Velocity = 0.0f;      // Current Travelling Velocity
    public float MaxVelocity = 1.0f;   // Maxima Velocity
    public float Acc = 0.0f;           // Current Acceleration
    public float AccAdd = 0.1f;        // Amount to increase Acceleration with.
    public float MaxAcc = 1.0f;        // Max Acceleration
    public float MinAcc = -1.0f;       // Min Acceleration

    void Update()
    {
        // Detect single key press for acceleration
        if (Input.GetKeyDown(KeyCode.UpArrow))
            Acc += AccAdd;

        // Detect single key press for deceleration
        if (Input.GetKeyDown(KeyCode.DownArrow))
            Acc -= AccAdd;

        // Continuous rotation while key is held down
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.RightArrow))
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);

        // Clamp acceleration within limits
        Acc = Mathf.Clamp(Acc, MinAcc, MaxAcc);

        // Update velocity
        Velocity += Acc;
        Velocity = Mathf.Clamp(Velocity, -MaxVelocity, MaxVelocity);

        // Move the train
        transform.Translate(Vector3.right * Velocity * Time.deltaTime);
    }
}