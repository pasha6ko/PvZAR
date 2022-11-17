using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Vector3 Diraction;
    [SerializeField] float BulletSpeed;
    [SerializeField] LayerMask zombieMask;
    [SerializeField] int Damage;
    void Start()
    {
        
    }
    private void Update()
    {
        transform.position += Diraction * BulletSpeed*Time.deltaTime;
    }
    private void FixedUpdate()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Zombie")
        {
            Zombie DamagedZombie = other.gameObject.GetComponent<Zombie>();
            DamagedZombie.GetDamage(Damage);

            Destroy(gameObject);

        }
    }

}
