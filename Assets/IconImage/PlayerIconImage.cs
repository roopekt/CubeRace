using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
public class PlayerIconImage : MonoBehaviour
{
    [SerializeField] private float initialVelocity = 5f;
    [Tooltip("Moving over one one speed up tile will give this much more velocity.")]
    [SerializeField] private float oneTileSpeedUp = 5f;
    [Tooltip("minimum acceleration while on speed up tile")]
    [SerializeField] private float minSpeedUpAcceleration = 5f;
    [SerializeField] private float avrgVelX_DeltaTime = 1f / 10f;
    [Tooltip("Instantiated, when player explodes.")]
    [SerializeField] private GameObject explosion;
    [SerializeField] private Menu menu;
    [SerializeField] private float trackLenght = 20f;
    public float speed = 5f;
    public float accel = 5f;
    public float time = 3f;

    public bool Alive { get => alive; }
    public bool Finished { get => finished; }
    public Vector3 Velocity { get => rigidbody.velocity; }

    private new Rigidbody rigidbody;
    private Vector3 initPos;
    private float sensitivity;//mouse sensitivity

    private bool grounded = true;
    private bool alive = true;
    private bool exploded = false;
    private bool finished = false;
    private bool speedUp = false;
    private bool slowDown = false;
    private float avrgVelX = 0f;
    private float avrgVelX_DeltaDist = 0f;
    private float avrgVelX_Timer = 0f;
    
    private float progress { get => (transform.position.z - initPos.z) / trackLenght; }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        initPos = transform.position;
        sensitivity = Settings.mouseSensitivity;

        //lock and hide mouse
        Cursor.lockState = CursorLockMode.Locked;

        rigidbody.velocity = Vector3.forward * initialVelocity;

        rigidbody.velocity += Vector3.right * speed;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        //controlled movement
        if (alive && !finished)
        {
            if (Time.time > time)
            {
                ScreenCapture.CaptureScreenshot("CubeRace_Icon" + Random.Range(0, 10000).ToString() + ".png");
                Debug.Break();
            }

            float delta = 0f;// Input.GetAxis("Mouse X") * sensitivity;
            transform.Translate(Vector3.right * delta);

            avrgVelX_DeltaDist += delta;
            avrgVelX_Timer += Time.deltaTime;
            if (avrgVelX_Timer > avrgVelX_DeltaTime)
            {
                avrgVelX = avrgVelX_DeltaDist / avrgVelX_Timer;
                avrgVelX_DeltaDist = 0f;
                avrgVelX_Timer = 0f;
            }
        }
    }

    private void FixedUpdate()
    {
        if (alive)
            rigidbody.AddForce(Vector3.left * accel);

        if (!grounded)
            Die();

        if (speedUp || slowDown)
        {
            float acceleration = Mathf.Max(minSpeedUpAcceleration, oneTileSpeedUp * rigidbody.velocity.z);
            if (speedUp)
                rigidbody.AddForce(Vector3.forward * acceleration, ForceMode.Acceleration);
            if (slowDown)
                rigidbody.AddForce(Vector3.back * acceleration, ForceMode.Acceleration);
        }

        if (progress > 1f)
            Finish();

        grounded = false;
        speedUp = false;
        slowDown = false;
    }

    private void OnTriggerStay(Collider other)
    {
        string tag = other.transform.tag;

        if (new [] { "Floor", "SpeedUp", "SlowDown"}.Contains(tag))
        {
            if (alive)
                grounded = true;
            else
                Explode();
        }
        else if (tag == "Wall")
        {
            Explode();
        }

        if (alive)
        {
            if (tag == "SpeedUp")
                speedUp = true;
            else if (tag == "SlowDown")
                slowDown = true;
        }
    }

    private void Finish()
    {
        if (alive && !finished)
        {
            finished = true;
            Cursor.lockState = CursorLockMode.None;//unlock and unhide mouse
            menu.FinishScreen();
        }
    }

    private void Die()
    {
        if (alive && !finished)
        {    
            rigidbody.useGravity = true;
            rigidbody.velocity += Vector3.right * avrgVelX;
            alive = false;

            Cursor.lockState = CursorLockMode.None;//unlock and unhide mouse
            menu.DeathScreen(progress);
        }
    }

    private void Explode()
    {
        Die();

        if (!exploded && !finished)
        {
            GetComponent<MeshRenderer>().enabled = false;
            Transform particleSystem = transform.GetChild(0);
            particleSystem.GetComponent<ParticleSystem>().Stop();
            particleSystem.parent = null;

            GameObject _explosion = Instantiate(explosion, transform.position, transform.rotation);
            _explosion.GetComponent<Explosion>().SetVelocity(rigidbody.velocity);
            exploded = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(initPos + Vector3.forward * trackLenght, .5f);
    }
}