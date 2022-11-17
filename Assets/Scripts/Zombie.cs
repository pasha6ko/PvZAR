using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] Vector3 Diraction;
    [SerializeField] float MainMovementSpeed,CoolDown;
    [SerializeField] LayerMask PlantMask;
    [SerializeField] int hp, Power;

    bool EatState = false;
    float MoveSpeed;


    Plant TargetPlant;

    float Clock;

    private void Start()
    {
        MoveSpeed = MainMovementSpeed;
    }
    private void Update()
    {
        transform.position += Diraction * MoveSpeed * Time.deltaTime;
    }
    public void GetDamage(int rate)
    {
        hp -= rate;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        print("collison");
        if (other.gameObject.tag== "Plant")
        {
            print("Plant");
            TargetPlant = other.gameObject.GetComponent<Plant>();
            ChangeState(true);
        }
    }
    private void FixedUpdate()
    {
        if (EatState)
        {
            Clock += Time.deltaTime;
            if (Clock >= CoolDown)
            {
                Clock = 0;
                if (TargetPlant.GetDamage(Power))
                {
                    ChangeState(false);
                    TargetPlant = null;
                }
            }
        }
    }
    void ChangeState(bool newState){
        print("StateHasBennChaned");
        EatState = newState;
        if (EatState)
        {
            MoveSpeed = 0;
        }
        else
        {
            MoveSpeed = MainMovementSpeed;
        }
        Clock = 0;
    }
}