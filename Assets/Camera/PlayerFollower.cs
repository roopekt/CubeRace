using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [SerializeField] private Player player;
    [Tooltip("Deacceleration applied when player dies.")]
    [SerializeField] private float stopAcceleration = 2f;

    private Transform playerTransform;
    private Vector3 ofset;

    private bool follow = true;
    private new Rigidbody rigidbody;

    private void Start()
    {
        playerTransform = player.transform;
        ofset = transform.position - playerTransform.position + Vector3.up * playerTransform.position.y;
    }

    private void Update()
    {
        if (follow)
            if (player.Alive && !player.Finished)
            {
                transform.position = ofset + playerTransform.position - Vector3.up * playerTransform.position.y;
            }
            else
            {
                follow = false;
                rigidbody = gameObject.AddComponent<Rigidbody>();
                rigidbody.useGravity = false;
                rigidbody.velocity = player.Velocity;
            }
        else//slow down
        {
            if (rigidbody.velocity.magnitude > 0)
                rigidbody.AddForce(-rigidbody.velocity.normalized * stopAcceleration, ForceMode.Acceleration);
            else
                rigidbody.velocity = Vector3.zero;

        }
    }
}