using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Fire Rate")]
    [SerializeField]  float fireRate;
    float fireRateTimer;
    [SerializeField]  bool semiAuto;

    [Header("Bullet Properties")] 
    [SerializeField] GameObject bullet;
    [SerializeField] Transform barrelPos;
    [SerializeField]  float bulletVelocity;
    [SerializeField]  int bulletPerShot;
    
     AimStateManager aim;

     [SerializeField] AudioClip gunShot;

      AudioSource audioSource;
   
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        aim = GetComponentInParent<AimStateManager>();
        fireRateTimer = fireRate;
    }

    
    void Update()
    {
        if(ShouldFire()) Fire();
    }

    bool ShouldFire()
    {
        fireRateTimer += Time.deltaTime;
        if (fireRateTimer < fireRate) return false;
        if (semiAuto && Input.GetKeyDown(KeyCode.Mouse0)) return true;
        if (!semiAuto && Input.GetKey(KeyCode.Mouse0)) return true;
        return false;
    }
    

    void Fire()
    {
        fireRateTimer = 0;
        barrelPos.LookAt(aim.aimPos);
        audioSource.PlayOneShot(gunShot);
        for (int i = 0; i < bulletPerShot; i++)
        {
            GameObject currentBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation);
            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
            rb.AddForce(barrelPos.forward * bulletVelocity,ForceMode.Impulse);
        }
    }
}
