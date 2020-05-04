using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class AsteroidEnemy : Enemy
{
    public bool CanRotate = true;
    public bool RandomColor = true;
    public float RotationFactor = 50;

    protected override void Start()
    {
        base.Start();

        if (CanRotate)
        {
            var directionFactor = Random.Range(0, 2) > 0 ? 1 : -1;
            GetComponent<Rigidbody2D>().angularVelocity = Random.Range(RotationFactor, RotationFactor * 2) * directionFactor;
        }

        if (RandomColor)
        {
            var color = Random.ColorHSV(0, 1, 0.2f, 0.9f, 0.25f, 0.9f);
            GetComponentInChildren<Light2D>().color = color;
        }
    }
}