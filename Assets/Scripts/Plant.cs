using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
    using System.Net;
#endif

public class Plant : MonoBehaviour
{
    public bool GenerateSun;
    public int money;
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform ShootPoint;
    public float CoolDown;
    public int Damage,hp, price;
    float Clock;
    List<Zombie> TargetZombies = new List<Zombie>(); // Зомби котороые поедают обект 
    GameManager gameManager;

    void Start()
    {
        Clock = 0;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        
    }
    public void ActivateAbility()
    {
        if (!GenerateSun)
        {
            GameObject clone = Instantiate(Bullet, ShootPoint.position, Quaternion.Euler(0, 0, 0));
            clone.GetComponent<Bullet>().Damage = Damage;
            Destroy(clone, 10);
        }
        else
        {
            GameManager.singleton.Money += money;
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
            GameManager.singleton.RemovePlantEvent();
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

