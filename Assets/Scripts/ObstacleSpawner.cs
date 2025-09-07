using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject [] obstacles;
    public bool isGameOver;
    public static ObstacleSpawner instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        StartCoroutine(SpawnObstacle());

    }
    private void Update()
    {
        
    }

    void createObtacle()
    {
        int random = Random.Range(0, obstacles.Length);
        Instantiate(obstacles[random], transform.position, Quaternion.identity);
    }
    IEnumerator SpawnObstacle()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(2f);
            createObtacle();
        }
    }
}
