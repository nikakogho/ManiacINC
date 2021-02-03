using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveLauncher : MonoBehaviour {
    public Transform spawnParent;
    Transform[] spawnPoints;
    public EnemyBlueprint[] enemies;
    public static int enemyCount = 0;
    static bool spawning = false;
    int wave = 0;
    public static bool IsWaveInProcess { get { return spawning || enemyCount > 0; } }

    public static WaveLauncher instance;
    public Text waveText;
    public GameObject shopUI;

    public AudioSource enemyAudio;

    void Awake()
    {
        instance = this;

        spawnPoints = new Transform[spawnParent.childCount];
        for(int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i] = spawnParent.GetChild(i);
        }

        NextWave();
    }

    public void NextWave()
    {
        wave++;
        waveText.text = "Wave " + wave;
        
        StartCoroutine(SpawnWave());
    }

    public void EndWave()
    {
        StartCoroutine(EndTheWave());
    }

    IEnumerator EndTheWave()
    {
        yield return new WaitForSeconds(2);
        shopUI.SetActive(true);
        WeaponManager.instance.EndWave();
    }

    IEnumerator SpawnWave()
    {
        spawning = true;
        int amount = GetTroopAmount();
        float waitTime = GetWaitTime();

        Transform waveObject = new GameObject("Wave " + wave).transform;

        for(int i = 0; i < amount; i++)
        {
            List<EnemyBlueprint> possibles = new List<EnemyBlueprint>();
            float total = 0;

            foreach(EnemyBlueprint e in enemies)
            {
                if(e.unlockWave <= wave)
                {
                    total += e.chance;
                    possibles.Add(e);
                }
            }

            float rand = Random.Range(0, total);
            float sum = 0;
            EnemyBlueprint chosen = null;

            foreach(EnemyBlueprint e in possibles)
            {
                sum += e.chance;

                if(sum >= rand)
                {
                    chosen = e;
                    break;
                }
            }

            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject troop = Instantiate(chosen.prefab, spawnPoint.position, Quaternion.identity, waveObject);
            enemyCount++;

            Enemy enemy = troop.GetComponent<Enemy>();
            float quality = Random.Range(0f, 1f);

            enemy.value = chosen.minValue + (int)(quality * chosen.maxValue);
            enemy.moveSpeed = chosen.minSpeed + quality * chosen.maxSpeed;
            enemy.health += (1 - quality * 2) * chosen.healthDelta;

            if(i < amount - 1) yield return new WaitForSeconds(waitTime);
        }

        spawning = false;
    }

    float GetWaitTime()
    {
        float x = wave - 1;
        x /= 0.8f;

        return 1f / (Mathf.Pow(x, 1.2f) + 2f + Mathf.Sin(x)) * 8f;
    }

    int GetTroopAmount()
    {
        float x = wave - 1;
        x /= 0.6f;

        return (int)(Mathf.Pow(x, 1.4f) + 5f + Mathf.Sin(x));
    }
}
