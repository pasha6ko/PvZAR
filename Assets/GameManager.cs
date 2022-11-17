using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int Money;
    public float GameTime=0;
    

    public float[] Waves;

    private void Awake()
    {
                
    }

    private void Start()
    {
        
    }
    private void FixedUpdate()
    {
        GameTime+=Time.deltaTime;
        
    }

    
}
