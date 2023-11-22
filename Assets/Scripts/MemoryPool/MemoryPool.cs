using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPool
{
    public int TotalCnt => ttlCnt;
    public int ActiveCnt => activeCnt;

    /// <summary>
    /// �Է¹��� ������Ʈ�� ������� �޸�Ǯ ����
    /// </summary>
    /// <param name="_poolObject"></param>
    public MemoryPool(GameObject _poolObject, int _increaseCnt = 5, Transform _parentTr = null)
    {
        ttlCnt = 0;
        activeCnt = 0;

        poolObject = _poolObject;

        arrList = new List<GameObject>[100];
        for (int i = 0; i < 100; ++i)
            arrList[i] = new List<GameObject>(30);

        poolListEnable = new List<GameObject>();
        poolQueueDisable = new Queue<GameObject>();

        InstantiateObjects(_increaseCnt, _parentTr);
    }

    /// <summary>
    /// increaseCnt ������ ������Ʈ�� ����
    /// </summary>
    public void InstantiateObjects(int _increaseCnt = 5, Transform _parentTr = null)
    {
        for (int i = 0; i < _increaseCnt; ++i)
        {
            GameObject poolGo = GameObject.Instantiate(poolObject);
            poolGo.SetActive(false);
            if (_parentTr != null)
                poolGo.transform.parent = _parentTr;

            poolQueueDisable.Enqueue(poolGo);
        }

        ttlCnt += _increaseCnt;
    }

    /// <summary>
    /// ���� �������� ��� ������Ʈ�� '����'
    /// ���� �ٲ�ų� ������ ����� �� �� ���� ȣ��
    /// </summary>
    public void DestroyObjects()
    {
        if (poolListEnable == null || poolQueueDisable == null) return;

        int cnt = poolListEnable.Count;
        for (int i = 0; i < cnt; ++i)
            GameObject.Destroy(poolListEnable[i]);

        cnt = poolQueueDisable.Count;
        for (int i = 0; i < cnt; ++i)
            GameObject.Destroy(poolQueueDisable.Dequeue());

        poolListEnable.Clear();
        poolQueueDisable.Clear();
    }

    /// <summary>
    /// �ش� ������Ʈ�� Ȱ��ȭ
    /// </summary>
    /// <returns></returns>
    public GameObject ActivatePoolItem(int _increaseCnt = 5, Transform _parentTr = null)
    {
        if (poolListEnable == null || poolQueueDisable == null) return null;

        if (poolQueueDisable.Count <= 0)
            InstantiateObjects(_increaseCnt, _parentTr);

        GameObject poolGo = poolQueueDisable.Dequeue();
        poolListEnable.Add(poolGo);

        poolGo.SetActive(true);

        ++activeCnt;

        return poolGo;
    }

    public GameObject ActivatePoolItem(Vector3 _spawnPos, int _increaseCnt = 5, Transform _parentTr = null)
    {
        if (poolListEnable == null || poolQueueDisable == null) return null;

        if (poolQueueDisable.Count <= 0)
            InstantiateObjects(_increaseCnt, _parentTr);

        GameObject poolGo = poolQueueDisable.Dequeue();
        poolListEnable.Add(poolGo);
        poolGo.transform.position = _spawnPos;
        poolGo.SetActive(true);

        ++activeCnt;

        return poolGo;
    }


    public GameObject ActivatePoolItemWithIdx(int _idx, int _increaseCnt = 5, Transform _parentTr = null)
    {
        if (arrList == null || poolQueueDisable == null) return null;

        if (poolQueueDisable.Count <= 0)
            InstantiateObjects(_increaseCnt, _parentTr);

        GameObject poolGo = poolQueueDisable.Dequeue();
        arrList[_idx / 30].Add(poolGo);

        poolGo.SetActive(true);

        ++activeCnt;

        return poolGo;
    }

    /// <summary>
    /// �ش� ������Ʈ�� ��Ȱ��ȭ
    /// </summary>
    /// <param name="_removeObject"></param>
    public void DeactivatePoolItem(GameObject _removeObject)
    {
        if (poolListEnable == null || poolQueueDisable == null || _removeObject == null) return;

        int cnt = poolListEnable.Count;
        for (int i = 0; i < cnt; ++i)
        {
            GameObject poolGo = poolListEnable[i];
            if (poolGo == _removeObject)
            {
                poolGo.SetActive(false);
                poolListEnable.Remove(poolGo);
                poolQueueDisable.Enqueue(poolGo);
                //poolGo.transform.position = tempStorePos;

                --activeCnt;

                return;
            }
        }
    }

    public GameObject DeactivatePoolItemWithIdx(GameObject _removeObject, int _idx)
    {
        if (arrList == null || poolQueueDisable == null || _idx < 0) return null;

        int arrIdx = _idx / 30;

        for (int i = 0; i < arrList[arrIdx].Count; ++i)
        {
            GameObject poolGo = arrList[arrIdx][i];
            if (poolGo.Equals(_removeObject))
            {
                poolGo.SetActive(false);
                arrList[arrIdx].RemoveAt(i);
                poolQueueDisable.Enqueue(poolGo);
                //poolGo.transform.position = tempStorePos;

                --activeCnt;

                return poolGo;
            }
        }

        return null;
    }

    /// <summary>
    /// ��� ������Ʈ�� ��Ȱ��ȭ
    /// </summary>
    public void DeactivateAllPoolItems()
    {
        if (poolListEnable == null || poolQueueDisable == null) return;

        int cnt = poolListEnable.Count;
        for (int i = 0; i < cnt; ++i)
        {
            GameObject poolGo = poolListEnable[i];

            poolGo.SetActive(false);
            poolGo.transform.position = tempStorePos;

            poolListEnable.Remove(poolGo);
            poolQueueDisable.Enqueue(poolGo);
        }

        activeCnt = 0;
    }

    public bool IsEnableListEmpty()
    {
        return poolListEnable.Count < 1;
    }

    private int ttlCnt = 0;
    private int activeCnt = 0;

    private Vector3 tempStorePos = new Vector3(-3000f, 0f, 0f);
    private GameObject poolObject = null; // ������Ʈ Ǯ������ �����ϴ� ������Ʈ ������

    private List<GameObject>[] arrList = null;
    private List<GameObject> poolListEnable = null;
    private Queue<GameObject> poolQueueDisable = null;
}