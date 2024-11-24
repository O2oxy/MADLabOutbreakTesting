using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum FireMode { SemiAuto, Burst, Automatic }
    public FireMode fireMode;

    [Header("Gun Settings")]
    public int magazineSize = 30;
    public int burstCount = 3;
    public float fireRate = 10f;         // Bullets per second
    public float reloadTime = 1.5f;
    public float bulletSpread = 0.05f;  // Spread when hip-firing
    public float aimSpread = 0.01f;     // Spread when aiming
    public float bulletRange = 50f;
    public float damage = 20f;
    public int bulletsPerShot = 1;      // Number of bullets per shot

    [Header("Effects")]
    public GameObject bulletTracerPrefab;
    public float tracerSpeed = 100f;
    public ParticleSystem muzzleFlash;

    [Header("Audio Settings")]
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip dryFireSound;
    private AudioSource audioSource;

    [Header("Camera Settings")]
    public Camera playerCamera;

    [Header("Ammo Tracking")]
    public int ammoCount { get; private set; }
    public int magazineSizePublic { get; private set; }

    private float nextFireTime;
    private int shotsFiredInBurst;
    private bool isReloading;
    private Vector3 originalCameraPosition;

    void Start()
    {
        ammoCount = magazineSize;
        magazineSizePublic = magazineSize;

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        originalCameraPosition = playerCamera.transform.localPosition;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (isReloading)
            return;

        // Auto-reload if ammo is 0 and fire button is pressed
        if (ammoCount <= 0 && (Input.GetButtonDown("Fire1") || Input.GetButton("Fire1")))
        {
            PlayDryFireSound();
            StartCoroutine(Reload());
            return;
        }

        if (fireMode == FireMode.SemiAuto && Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        else if (fireMode == FireMode.Burst && Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(BurstFire());
        }
        else if (fireMode == FireMode.Automatic && Input.GetButton("Fire1"))
        {
            Shoot();
        }

        // Manual reload
        if (Input.GetKeyDown(KeyCode.R) && ammoCount < magazineSize)
        {
            StartCoroutine(Reload());
        }
    }

    private void Shoot()
    {
        if (Time.time < nextFireTime || ammoCount <= 0)
            return;

        // Loop to fire multiple bullets per shot
        for (int i = 0; i < bulletsPerShot; i++)
        {
            if (ammoCount <= 0) break; // Stop if no ammo left

            PlayShootSound();
            PlayMuzzleFlash();

            // Calculate the shooting direction
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            Vector3 targetPoint;

            if (Physics.Raycast(ray, out hit, bulletRange))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(bulletRange);
            }

            // Calculate the direction from the gun to the target point
            Vector3 shotDirection = (targetPoint - transform.position).normalized;

            // Apply bullet spread based on whether the player is aiming
            float currentSpread = Input.GetButton("Fire2") ? aimSpread : bulletSpread;
            Vector3 spread = new Vector3(Random.Range(-currentSpread, currentSpread), Random.Range(-currentSpread, currentSpread), 0);
            shotDirection += transform.TransformDirection(spread);

            // Fire bullet tracer
            CreateBulletTracer(transform.position, transform.position + shotDirection * bulletRange);

            ammoCount--;
        }

        nextFireTime = Time.time + 1f / fireRate;
    }

    private System.Collections.IEnumerator BurstFire()
    {
        shotsFiredInBurst = 0;

        while (shotsFiredInBurst < burstCount && ammoCount > 0)
        {
            Shoot();
            shotsFiredInBurst++;
            yield return new WaitForSeconds(1f / fireRate);
        }
    }

    private System.Collections.IEnumerator Reload()
    {
        isReloading = true;
        PlayReloadSound();
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);
        ammoCount = magazineSize;
        isReloading = false;
        Debug.Log("Reload complete");
    }

    private void CreateBulletTracer(Vector3 startPosition, Vector3 targetPosition)
    {
        GameObject tracer = Instantiate(bulletTracerPrefab, startPosition, Quaternion.identity);
        StartCoroutine(MoveTracer(tracer, targetPosition));
    }

    private System.Collections.IEnumerator MoveTracer(GameObject tracer, Vector3 targetPosition)
    {
        Vector3 startPosition = tracer.transform.position;
        float distance = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;

        while (Vector3.Distance(tracer.transform.position, targetPosition) > 0.1f)
        {
            float traveled = (Time.time - startTime) * tracerSpeed;
            Vector3 nextPosition = Vector3.Lerp(startPosition, targetPosition, traveled / distance);

            // Check for collisions along the tracer's path
            if (Physics.Raycast(tracer.transform.position, nextPosition - tracer.transform.position, out RaycastHit hit, Vector3.Distance(tracer.transform.position, nextPosition)))
            {
                // Apply damage to the hit target
                EntityHealth targetHealth = hit.collider.GetComponent<EntityHealth>();
                if (targetHealth != null)
                {
                    targetHealth.TakeDamage(damage);
                }

                // Move the tracer to the hit point and destroy it
                tracer.transform.position = hit.point;
                break;
            }

            // Move the tracer
            tracer.transform.position = nextPosition;
            yield return null;
        }

        Destroy(tracer);
    }

    private void PlayShootSound()
    {
        if (shootSound != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f); // Slight pitch variation
            audioSource.PlayOneShot(shootSound);
        }
    }

    private void PlayReloadSound()
    {
        if (reloadSound != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }
    }

    private void PlayDryFireSound()
    {
        if (dryFireSound != null)
        {
            audioSource.PlayOneShot(dryFireSound);
        }
    }

    private void PlayMuzzleFlash()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 direction = transform.forward * bulletRange;
        Gizmos.DrawLine(transform.position, transform.position + direction);
    }
}
