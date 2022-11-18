using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform ShootPoint;
    [SerializeField] float CoolDown;
    [SerializeField] int hp;

    float Clock;
    List<Zombie> TargetZombies = new List<Zombie>(); // Зомби котороые поедают обект 


    void Start()
    {
        Clock = 0;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Clock += Time.deltaTime;
        if (Clock >= CoolDown)
        {
            Clock = 0;
            GameObject clone = Instantiate(Bullet, ShootPoint.position,Quaternion.Euler(0,0,0));
            Destroy(clone,10);
        }
    }
    public void GetDamage(int rate)
    {
        hp -= rate;
        if (hp <= 0)
        {
            foreach(Zombie zombie in TargetZombies)
            {
                zombie.ChangeState(false);
            }
            Destroy(gameObject);
            
        }
    }

    public void AddZombieToPlantTarget(Zombie z)
    {
        if (!TargetZombies.Contains(z))
        {
            TargetZombies.Add(z);
        }
    }
}
