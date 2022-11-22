using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Zombie")
        {
            Destroy(collision.gameObject);
            GameManager.singleton.ZombieIn++;
        }
    }
}
