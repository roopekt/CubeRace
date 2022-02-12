using UnityEngine;

public class Explosion : MonoBehaviour
{
    public void SetVelocity(Vector3 velocity)
    {
        foreach (Rigidbody rigidbody in GetComponentsInChildren<Rigidbody>())
        {
            rigidbody.velocity = velocity;
        }
    }
}