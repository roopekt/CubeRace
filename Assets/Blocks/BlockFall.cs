using UnityEngine;

public class BlockFall : MonoBehaviour
{
    [Tooltip("When the object touches BlockDropper, it will wait 0 to maxDelay, before it starts falling.")]
    [SerializeField] private float maxDelay = 0.5f;
    [Tooltip("When the object is falling and Y gets less than this, the object is detroyed.")]
    [SerializeField] private float destroyHeight = -5f;

    private bool falling = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "BlockDropper" && !falling)
        {
            falling = true;
            gameObject.isStatic = false;
            float delay = Random.value * maxDelay;
            Invoke("StartFalling", delay);
        }
    }

    private void StartFalling()
    {
        gameObject.AddComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (falling && transform.position.y < destroyHeight)
            Destroy(gameObject);
    }
}