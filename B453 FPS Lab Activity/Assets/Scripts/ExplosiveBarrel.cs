using UnityEngine;

public class ExplosiveBarrel : Triggers
{
    public float radius = 5f; // The radius from the center of the explosion that objects are hit by the blast.
    public float power = 500; // The strength of the initial detonation.

    [SerializeField] ParticleSystem explosion; // Reference to the explosion particle system.

    public void Awake()
    {
        MyEvents.someEvent.AddListener(React);
    }

    private void React(int number)
    {
        Debug.Log(number);
    }

    protected override void OneShotTrigger()
    {
        if (!canTrigger) return;
        base.OneShotTrigger();

        Debug.Log("Explosive Barrel Triggered");
        Explode();
    }

    protected override void CooldownTrigger()
    {
        if (!canTrigger) return;
        base.CooldownTrigger();

        Explode();
    }

    public void Explode()
    {
        // First store the position of the barrel.
        Vector3 position = transform.position;

        // Generate a sphere colider at the position of the barrel with a radius equal to the blast radius
        // Store all objects hit by this collider in an array.
        LayerMask mask = LayerMask.GetMask("Knockback");
        Collider[] hitColliders = Physics.OverlapSphere(position, radius, mask);

        // Cycle through each object in the array of hit things.
        foreach (Collider thing in hitColliders)
        {
            // First check to make sure the object hit has a RigidBody component.
            if (thing.GetComponent<Rigidbody>())
            {
                // Grab a reference to the object's RigidBody component.
                Rigidbody rb = thing.GetComponent<Rigidbody>();
                // Apply an explosive force to the object using parameters from the C4 (position, radius, and power).
                // Make sure the ForceMode is set to Impulse so that the entire force is applied in one frame.
                rb.AddExplosionForce(power, position, radius, 2.0f, ForceMode.Impulse);

            }
        }
        // Play the explosion particle effect.
        explosion.Play();
    }
}
