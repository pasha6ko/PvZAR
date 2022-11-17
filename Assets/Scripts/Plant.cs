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
    public bool GetDamage(int rate)
    {
        hp -= rate;
        if (hp <= 0)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}
