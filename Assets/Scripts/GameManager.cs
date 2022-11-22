using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager singleton { get; set; }

    bool PCMode;
    [Range(0, 10f)] public float SpawnTime;
    public int Money;
    [SerializeField]List<float> TimeInTimeLine = new List<float>();
    [SerializeField] List<int> LevelInTimeLine = new List<int>();
    public float GameTime = 0;
    [SerializeField] Transform SpawnPointsParent;
    [SerializeField] GameObject[] Zombies;
    [SerializeField] Vector3 OriginDifference;
    GameObject CurrentPlant;

    List<EventByTime> PlantEventList = new List<EventByTime>();

    public List<float> Waves = new List<float>();
    List<Transform> SpawnPoints = new List<Transform>();

    private float CoolDownSaveTime = 0;

    [SerializeField] TMPro.TextMeshProUGUI MoneyLable;
    [SerializeField] float TimeForWaveSpawn;
    [SerializeField] GameObject EndGameUI;
    int Level;
    float LastSpawnTime=0;
    float LastSpawnTimeWave=0;

    bool GameIsRunning = true;
    bool SpawningWave;
    int UnspawnedEnemis;
    [SerializeField] GameObject WaveMessseg, win, lose;
    public int ZombieIn;    

    private void Awake()
    {
        singleton = this;
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
        if (CurrentPlant != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ScreenTouch(Input.mousePosition);
            }
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                print("Touch");
                ScreenTouch(Input.GetTouch(0).position);
            }
        }
        
    }
    private void FixedUpdate()
    {

        if (ZombieIn > 5)
        {
            EndGameUI.SetActive(true);
            lose.SetActive(true);
        }
        if (GameIsRunning)
        {
            GameTime += Time.deltaTime;
            if (PlantEventList.Count > 0)
            {

                foreach (EventByTime e in PlantEventList)
                {
                    if (e.GlobalTimeEvent <= GameTime && e.CoolDown != 0f)
                    {
                        print(e.GlobalTimeEvent);
                        e.obj.ActivateAbility();
                        e.GlobalTimeEvent += e.CoolDown;
                    }
                }
            }
            MoneyLable.text = Money.ToString();
            if (TimeInTimeLine.Count > 1)
            {
                if (TimeInTimeLine[1] <= GameTime)
                {
                    Level = LevelInTimeLine[1];
                    print("level change");
                    TimeInTimeLine.RemoveAt(0);
                    LevelInTimeLine.RemoveAt(0);

                }
            }
            else
            {
                EndGameUI.SetActive(true);
                win.SetActive(true);
                GameIsRunning = false;
            }
                
            if (GameTime >= LastSpawnTime + SpawnTime / Level && Level != 0)
            {
                LastSpawnTime = GameTime;
                SpawnEnemie();
            }
            if (!SpawningWave)
            {
                if (Waves.Count > 0)
                {
                    if (GameTime >= Waves[0])
                    {
                        StartCoroutine(ShowWaveMessege());
                        SpawningWave = true;
                        UnspawnedEnemis = Level * 2;
                        Waves.RemoveAt(0);
                        LastSpawnTimeWave = GameTime;
                    }
                }
            }      
            else
            {
                if (GameTime >= LastSpawnTimeWave + TimeForWaveSpawn / Level * 2)
                {
                    SpawnEnemie();
                    print("SpawnedByWave");
                    UnspawnedEnemis--;
                    LastSpawnTimeWave = GameTime;
                    if (UnspawnedEnemis <= 0)
                    {
                        SpawningWave = false;
                    }

                }
                
            }
        }
       
    }
    IEnumerator ShowWaveMessege()
    {
        WaveMessseg.SetActive(true);
        yield return new WaitForSeconds(3);
        WaveMessseg.SetActive(false);
    }
    private GameObject GetRandomZombie()
    {
        int rnd = Random.Range(0, Level + 1);
        switch(rnd)//Ќадо бы изменить на изменение в инспекторе 
        {
            case int n when (n <=4):
                return Zombies[0];

            case int n when (n>4 && n<=6):
                return Zombies[1];

            case int n when (n > 6 && n <= 8):
                return Zombies[2];
        }
        return null;
    }

    void SpawnEnemie()
    {
        int SpawnPointIndex = Random.Range(0, SpawnPoints.Count);

        Instantiate(GetRandomZombie(), SpawnPoints[SpawnPointIndex].position + OriginDifference, Quaternion.Euler(0, 0, 0));
    }
    void ScreenTouch(Vector3 vec)
    {
        Ray ray = Camera.main.ScreenPointToRay(vec);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "Ground" && hit.collider.gameObject.transform.childCount == 0 && Money>=CurrentPlant.GetComponent<Plant>().price)
            {
                GameObject clone = Instantiate(CurrentPlant, hit.transform.position, Quaternion.Euler(0, 0, 0));
                Plant ClonePlant = clone.GetComponent<Plant>();
                Money -= ClonePlant.price;
                clone.transform.parent = hit.transform;
                print(clone.GetComponent<Plant>());
                PlantEventList.Add(new EventByTime(ClonePlant, ClonePlant.CoolDown, ClonePlant.CoolDown + GameTime));
            }
        }
    }

    public void RemovePlantEvent()
    {
        for (int i = 0; i < PlantEventList.Count; i++)
        {
            if (PlantEventList[i].obj == null)
            {
                print("del");
                PlantEventList.RemoveAt(i);
                i--;

            }
        }
    }

    public void SetCurrentPlant(GameObject p)
    {
        CurrentPlant = p;
    }


    public void Exit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync(0);
    }
}

public class EventByTime
{
    public Plant obj;
    public float CoolDown;
    public float GlobalTimeEvent;
    public EventByTime(Plant obj, float CoolDown,float GlobalTimeEvent)
    {
        this.obj = obj;
        this.CoolDown = CoolDown;
        this.GlobalTimeEvent = GlobalTimeEvent;
    }

}
/*
public class EventZombiesSpawn
{
    public EventZombiesSpawn(int alarm , int time,int level, GameObject[] Zombies)
    {
        
    }
}*/