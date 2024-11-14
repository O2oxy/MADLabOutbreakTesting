using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum FireMode { SemiAuto, Burst, Automatic }
    public FireMode fireMode;

    [Header("Gun Settings")]
    public int magazineSize = 30;
    public int burstCount = 3;
    public float fireRate = 10f;
    public float reloadTime = 1.5f;
    public float bulletSpread = 0.05f;
    public float aimSpread = 0.01f;
    public float bulletRange = 50f;
    public float damage = 20f;
    public float minDistanceForTracer = 3f; // Minimum distance to fire a tracer

    [Header("Bullet Tracer Settings")]
    public GameObject bulletTracerPrefab;
    public float tracerSpeed = 100f;

    [Header("Camera Settings")]
    public Camera playerCamera;

    [Header("Audio Settings")]
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip dryFireSound;
    private AudioSource audioSource;

    private int ammoCount;
    private float nextFireTime;
    private int shotsFiredInBurst;
    private bool isReloading;

    void Start()
    {
        ammoCount = magazineSize;

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

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

        // Check if the player is trying to fire without ammo, and reload if necessary
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

        if (Input.GetKeyDown(KeyCode.R) && ammoCount < magazineSize)
        {
            StartCoroutine(Reload());
        }
    }

    private void Shoot()
    {
        if (Time.time < nextFireTime || ammoCount <= 0)
            return;

        PlayShootSound();

        // Raycast from the center of the camera to the reticle
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, bulletRange))
        {
            targetPoint = hit.point;

            // Apply damage only if the target is NOT tagged as "Player"
            if (hit.collider.CompareTag("Player") == false)
            {
                EntityHealth targetHealth = hit.collider.GetComponent<EntityHealth>();
                if (targetHealth != null)
                {
                    targetHealth.TakeDamage(damage);
                }
            }
        }
        else
        {
            targetPoint = ray.GetPoint(bulletRange);
        }

        // Calculate the direction from the gun to the target point
        Vector3 shotDirection = (targetPoint - transform.position).normalized;

        // Apply spread based on whether the player is aiming or not
        float currentSpread = Input.GetButton("Fire2") ? aimSpread : bulletSpread;
        Vector3 spread = new Vector3(Random.Range(-currentSpread, currentSpread), Random.Range(-currentSpread, currentSpread), 0);
        shotDirection += transform.TransformDirection(spread);

        // Check distance to prevent close-range tracers
        if (Vector3.Distance(transform.position, targetPoint) >= minDistanceForTracer)
        {
            CreateBulletTracer(transform.position, transform.position + shotDirection * bulletRange);
        }

        ammoCount--;
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

    private void CreateBulletTracer(Vector3 startPosition, Vector3 endPosition)
    {
        GameObject tracer = Instantiate(bulletTracerPrefab, startPosition, Quaternion.identity);
        StartCoroutine(MoveTracer(tracer, endPosition));
    }

    private System.Collections.IEnumerator MoveTracer(GameObject tracer, Vector3 targetPosition)
    {
        Vector3 startPosition = tracer.transform.position;
        float distance = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;

        while (Vector3.Distance(tracer.transform.position, targetPosition) > 0.1f)
        {
            float traveled = (Time.time - startTime) * tracerSpeed;
            tracer.transform.position = Vector3.Lerp(startPosition, targetPosition, traveled / distance);
            yield return null;
        }

        Destroy(tracer);
    }

    // Audio functions
    private void PlayShootSound()
    {
        if (shootSound != null)
        {
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 direction = transform.forward * bulletRange;
        Gizmos.DrawLine(transform.position, transform.position + direction);
    }
}
