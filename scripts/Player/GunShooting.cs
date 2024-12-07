using UnityEngine;
using TMPro;

public class GunShooting : MonoBehaviour
{
    [Header("Gun Stats")]
    public int damage;
    public float fireRate, baseSpread, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;


    [Header("Audio")]
    public AudioSource source;
    public AudioClip clip;

    bool isShooting, isReadyToShoot, isReloading;

    public Rigidbody rigidBody;
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask EnemyLayer;
    public GameObject muzzleFlash, bulletHoleGraphic;
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        spread = baseSpread;
        bulletsLeft = magazineSize;
        isReadyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        text.SetText(bulletsLeft + " / " + magazineSize);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft != magazineSize)
        {
            Reload();
        }

        if (bulletsLeft == 0)
        {
            Reload();
        }
    }

    private void MyInput()
    {
        if (allowButtonHold) isShooting = Input.GetKey(KeyCode.Mouse0);
        else isShooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (isReadyToShoot && isShooting && !isReloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    private void Shoot()
    {
        source.PlayOneShot(clip);
        isReadyToShoot = false;

        if (rigidBody.velocity.magnitude > 0)
        {
            spread = spread * 1.5f;
        }
        else
        {
            spread = baseSpread;
        }

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, EnemyLayer))
        {
            Debug.Log(rayHit.collider.tag);
            Debug.Log(rayHit.collider.name);

            checkTag();
        }

        if (rayHit.collider)
        {
            if (!(rayHit.collider.CompareTag("EnemyFace") || rayHit.collider.CompareTag("EnemyLimbs") || rayHit.collider.CompareTag("EnemyBody") || rayHit.collider.CompareTag("Enemy")))
            {
                Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
            }
        }

        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShoot", fireRate);

        if (bulletsLeft > 0 && bulletsShot > 0)
        {
            Invoke("Shoot", fireRate);
        }
    }

    private void ResetShoot()
    {
        isReadyToShoot = true;
    }

    private void Reload()
    {
        isReloading = true;
        Invoke("FinishedReload", reloadTime);
    }

    private void FinishedReload()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
    }

    private void checkTag()
    {
        if (rayHit.collider.CompareTag("EnemyBody") || rayHit.collider.CompareTag("EnemyFace") || rayHit.collider.CompareTag("EnemyLimbs") || rayHit.collider.CompareTag("Enemy"))
        {
            rayHit.collider.GetComponent<EnemyTakeDamage>().TakeDamage(damage);
        }
    }
}
