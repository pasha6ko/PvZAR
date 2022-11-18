using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int Money;
    public float GameTime = 0;
    [SerializeField] Transform SpawnPointsParent;
    [SerializeField] GameObject[] Zombies;
    [SerializeField] Vector3 OriginDifference;
    [SerializeField] GameObject Plants;


    [SerializeField] float SpawnCooldown;



    public float[] Waves;
    List<Transform> SpawnPoints = new List<Transform>();

    private float CoolDownSaveTime = 0;
    private void Awake()
    {

    }

    private void Start()
    {
        for (int i = 0; i < SpawnPointsParent.childCount; i++)
        {
            SpawnPoints.Add(SpawnPointsParent.GetChild(i));
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Touch(Input.mousePosition);
        }
    }
    private void FixedUpdate()
    {

        GameTime += Time.deltaTime;
        if (GameTime - CoolDownSaveTime >= SpawnCooldown)
        {
            CoolDownSaveTime = GameTime;
            SpawnEnemies(1);
        }
    }

    void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int ZombieIndex = Random.Range(0, Zombies.Length);
            int SpawnPointIndex = Random.Range(0, SpawnPoints.Count);

            Instantiate(Zombies[ZombieIndex], SpawnPoints[SpawnPointIndex].position + OriginDifference, Quaternion.Euler(0, 0, 0));

        }
    }
    void Touch(Vector3 vec)
    {
        Debug.Log("Touch");

        Ray ray = Camera.main.ScreenPointToRay(vec);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            print("Raycast");
            if (hit.collider.gameObject.tag == "Ground")
            {
                print("Create");
                Instantiate(Plants, hit.transform.position,Quaternion.Euler(0,0,0));
            }
        }
    }
}