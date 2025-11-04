using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject projectilePrefab1;
    public Transform firePoint;

    Camera cam;

    bool useAltProjectile = false;


    void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            useAltProjectile = !useAltProjectile;
        }
    }

    void Shoot()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint = ray.GetPoint(50f);
        Vector3 direction = (targetPoint - firePoint.position).normalized;

        GameObject selectedPrefab = useAltProjectile ? projectilePrefab1 : projectilePrefab;
        Instantiate(selectedPrefab, firePoint.position, Quaternion.LookRotation(direction));
    }

}
