using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class Enemy
{
    public Object enemy;
    public int amount;
}

[System.Serializable]
public class EnemyWave : List<Enemy>
{
    public Enemy[] wave;
}

public class SpawnController : MonoBehaviour {

    public static SpawnController instance;
    public float spawnTime;
    public GameObject[] spawnPoints;
    [ShowInInspector]
    public List<EnemyWave> enemyWaves;

    List<Object> wave; //The enemy wave we are about to spawn.
    int currentStars = -1;

    private void Awake()
    {
        instance = this;
    }

    public void StartSpawn () {
        StartCoroutine(SpawnEnemy(spawnTime));        
    }

    public void EndSpawn() {
        StopAllCoroutines();
        currentStars = 0;
        var cats = FindObjectsOfType<CatProp>();
        for(int i = 0; i< cats.Length; i++ ) {
            cats[i].Hide();
        }
    }

    IEnumerator SpawnEnemy(float spawnTime)
    {
        while (true)
        {
            yield return null;
                yield return null;

                currentStars = PlayerManager.instance.AddWave(enemyWaves.Count);

                wave = new List<Object>();

                for (int i = 0; i < enemyWaves[currentStars].wave.Length; i++)
                {
                    for (int j = 0; j < enemyWaves[currentStars].wave[i].amount; j++)
                    {
                        wave.Add(enemyWaves[currentStars].wave[i].enemy);
                    }
                }

                while (wave.Count > 0)
                {
                    GameObject temp;
                    int enemyR = Random.Range(0, wave.Count);
                    int spawnPointR = Random.Range(0, spawnPoints.Length);

                    temp = ObjectPool.instance.TakePoolObject(wave[enemyR], true);
                    wave.RemoveAt(enemyR);
                    temp.transform.position = spawnPoints[spawnPointR].transform.position;

                    yield return new WaitForSeconds(spawnTime);
                }

                while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
                {
                    yield return new WaitForFixedUpdate();
                }
            
        }
    }
}
