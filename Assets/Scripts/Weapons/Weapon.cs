using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField]
    protected float _recoilStrenght;
    [SerializeField]
    protected int maxAmmo;

    int _currentAmmo;
    public int currentAmmo {
        get { return _currentAmmo; }
    }

    public GameObject projectilePrefab;
    public float projectileSpeed;

    public Transform firePoint;

    public int shotPerSecond = 3;
    int _framesTillNexShot = 0;
    int _currentFramesTillNexShot = 0;
    public bool IsReadyToShoot {
        get { return _currentFramesTillNexShot <= 0 && currentAmmo > 0; }
    }

    public float recoilStrenght {
        get { return _recoilStrenght; }
    }

    public virtual void FireWeapon(Vector2 direction) {
        if (!IsReadyToShoot) return;

        _currentFramesTillNexShot = _framesTillNexShot;

        _currentAmmo--;

        if (projectilePrefab == null) return;

        Instantiate(projectilePrefab, firePoint.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = direction.normalized * projectileSpeed;
    }

    public virtual void Reload(int ammoAmount) {
        _currentAmmo += ammoAmount;
    }

    public virtual void FullReload() {
        _currentAmmo = maxAmmo;
    }

    public virtual void AimWeapon(Vector2 targetPosition) {
        //NÃO ENCOSTEM. EU TAMBÉM NÃO SEI COMO FUNCIONA

        // vector from this object towards the target location
        Vector3 vectorToTarget = new Vector3(targetPosition.x, targetPosition.y, 0) - this.transform.position;
        // rotate that vector by 90 degrees around the Z axis
        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * vectorToTarget;

        // get the rotation that points the Z axis forward, and the Y axis 90 degrees away from the target
        // (resulting in the X axis facing the target)
        this.transform.rotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
    }

    private void Awake() {
        _framesTillNexShot = 60 / shotPerSecond;

        FullReload();
    }

    private void FixedUpdate() {
        if (_currentFramesTillNexShot > 0)
            _currentFramesTillNexShot--;
    }
}
