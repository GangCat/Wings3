using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleManager : MonoBehaviour
{
    public void Init()
    {
        curHealMarble = Instantiate(healMarblePrefab, transform).GetComponent<HealMarble>();
        curHealMarble.SetActive(false);
        StartCoroutine(RandomSpawnHealCoroutine());
    }

    private IEnumerator RandomSpawnHealCoroutine()
    {
        yield return new WaitForSeconds(healSpawnDelay);

        while (true)
        {
            curHealMarble.transform.position = marbleSpawnTrs[Random.Range(0, marbleSpawnTrs.Length)].position;
            curHealMarble.SetActive(true);

            while (curHealMarble.IsActive())
                yield return new WaitForSeconds(0.5f);

            yield return new WaitForSeconds(healSpawnDelay);
        }

    }

    [SerializeField]
    private GameObject healMarblePrefab = null;
    [SerializeField]
    private Transform[] marbleSpawnTrs = null;
    [SerializeField]
    private float healSpawnDelay = 10f;

    private HealMarble curHealMarble = null;
}
