using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    int startingWave = 0;
    [SerializeField] bool looping = false;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllEnemyWavies());
        } while (looping);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator SpawnAllEnemyWavies()
    {
        for(int index = 0; index < waveConfigs.Count; ++index)
        {
            yield return StartCoroutine(SpawnAllEnemiesInWave(waveConfigs[index]));
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        for(int i = 0; i < waveConfig.GetNumberOfEnemies(); ++i)
        {
            GameObject newEnemy = Instantiate(
                waveConfig.GetEnemyPrefab(),
                waveConfig.GetWaypoints()[0].transform.position,
                Quaternion.identity);
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
    }
}
