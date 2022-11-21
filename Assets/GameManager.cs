using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager singleton { get; set; }


    public int Money;
    [SerializeField]List<float> TimeInTimeLine = new List<float>();
    [SerializeField] List<int> LevelInTimeLine = new List<int>();
    public float GameTime = 0;
    [SerializeField] Transform SpawnPointsParent;
    [SerializeField] GameObject[] Zombies;
    [SerializeField] Vector3 OriginDifference;
    GameObject CurrentPlant;

    List<EventByTime> PlantEventList = new List<EventByTime>();

    [SerializeField] float SpawnCooldown;
    public float[] Waves;
    List<Transform> SpawnPoints = new List<Transform>();

    private float CoolDownSaveTime = 0;

    [SerializeField] TMPro.TextMeshProUGUI MoneyLable;

    int Level;

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
        if (Input.GetMouseButtonDown(0))
        {
            Touch(Input.mousePosition);
        }
    }
    private void FixedUpdate()
    {

        GameTime += Time.deltaTime;
        try
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
        catch { }
        MoneyLable.text = Money.ToString();
        //Спавн Противников
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
        Ray ray = Camera.main.ScreenPointToRay(vec);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "Ground" && hit.collider.gameObject.transform.childCount == 0)
            {
                print("Create");
                GameObject clone = Instantiate(CurrentPlant, hit.transform.position, Quaternion.Euler(0, 0, 0));
                clone.transform.parent = hit.transform;
                print(clone.GetComponent<Plant>());
                Plant ClonePlant = clone.GetComponent<Plant>();
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