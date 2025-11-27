using UnityEngine;

public class KartEngine : MonoBehaviour
{

    [SerializeField] float maxTorque = 400f;
    [SerializeField] float idleRpm = 1000f;
    [SerializeField] float maxRpm = 8000f;

    [SerializeField] AnimationCurve torqueCurve;


    public float CurrentTorque { get; private set; }
    public float CurrentRPM { get; private set; }

    public float Simulate(float throttleInput, float forwardSpeed, float gearRatio, float wheelRadius, float deltaTime)
    {
        float throttle = Mathf.Clamp01(throttleInput);

        float wheelOmewga = forwardSpeed / Mathf.Max(wheelRadius, float.MinValue);
        float wheelRpm = wheelOmewga * 60 / (2 * Mathf.PI);
        float kinematicRpm = Mathf.Abs(wheelRpm*gearRatio);

        CurrentRPM = Mathf.Lerp(CurrentRPM, Mathf.Max(kinematicRpm, idleRpm), deltaTime);
        CurrentRPM = Mathf.Clamp(CurrentRPM, idleRpm, maxRpm);

        float maxTorque = torqueCurve.Evaluate(CurrentRPM);
        CurrentTorque = maxTorque * throttle;

        return CurrentTorque;
    }


}
