using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : AttackableObject
{
    public delegate void DestroyBombDelegate(GameObject _bombGo);
    public void Init(float _launchDuration, float _lengthPerSec, float _initWidth, float _initHeight, Color _curColor, int _idx)
    {
        launchDuration = _launchDuration;
        lengthPerSec = _lengthPerSec;
        initWidth = _initWidth;
        initHeight = _initHeight;
        myColor = _curColor;
        hdrColor = _curColor * 30f;
        myIdx = _idx;

        waitFixedTime = new WaitForFixedUpdate();
        GetComponent<MeshRenderer>().material.SetColor("_BaseColor", myColor);
        GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", hdrColor);
        mf = GetComponent<MeshFilter>();
        mc = GetComponent<MeshCollider>();
        Mesh mesh = new Mesh();
        mf.mesh = mesh;
        mc.sharedMesh = mesh;

        StartCoroutine(LaunchLaserCoroutine(Time.time));
    }

    public int GetIdx => myIdx;

    private IEnumerator LaunchLaserCoroutine(float _startTime)
    {
        while(Time.time - _startTime < launchDuration)
        {
            //사운드
            // 연출

            curHeight += lengthPerSec * Time.fixedDeltaTime;
            ChangeForm();

            yield return waitFixedTime;
        }
    }

    private void ChangeForm()
    {
        float halfInitWidth = initWidth * 0.5f;
        float halfInitHeigth = initHeight * 0.5f;

        // 버텍스 버퍼
        Vector3[] verticesArr = new Vector3[]
            {
                new Vector3(-halfInitWidth, halfInitHeigth, 0f),
                new Vector3(halfInitWidth, halfInitHeigth, 0f),
                new Vector3(-halfInitWidth, -halfInitHeigth, 0f),
                new Vector3(halfInitWidth, -halfInitHeigth, 0f),
                new Vector3(-initWidth * (0.5f + curHeight * increaseSizeRatio), initHeight * (0.5f + curHeight * increaseSizeRatio), curHeight),
                new Vector3(initWidth * (0.5f + curHeight * increaseSizeRatio), initHeight * (0.5f + curHeight * increaseSizeRatio), curHeight),
                new Vector3(-initWidth * (0.5f + curHeight * increaseSizeRatio), -initHeight * (0.5f + curHeight * increaseSizeRatio), curHeight),
                new Vector3(initWidth * (0.5f + curHeight * increaseSizeRatio), -initHeight * (0.5f + curHeight * increaseSizeRatio), curHeight)
            };
        Vector3[] vertices = SetVertices(verticesArr);

        // 인덱스 버퍼
        int[] indices = SetIndices();

        // 노멀값
        Vector3[] normals = CalcNormal(vertices, indices);

        mf.mesh.Clear();
        mf.mesh.vertices = vertices;
        mf.mesh.triangles = indices;
        mf.mesh.normals = normals;

        mc.sharedMesh = mf.mesh;
    }

    private Vector3[] SetVertices(Vector3[] _verticesArr)
    {
        Vector3[] vertices = new Vector3[]
            {
            // 전
            _verticesArr[0],
            _verticesArr[1],
            _verticesArr[2],
            _verticesArr[3],
            // 상
            _verticesArr[4],
            _verticesArr[5],
            _verticesArr[0],
            _verticesArr[1],
            // 우
            _verticesArr[1],
            _verticesArr[5],
            _verticesArr[3],
            _verticesArr[7],
            // 좌
            _verticesArr[4],
            _verticesArr[0],
            _verticesArr[6],
            _verticesArr[2],
            // 하
            _verticesArr[2],
            _verticesArr[3],
            _verticesArr[6],
            _verticesArr[7],
            // 후
            _verticesArr[6],
            _verticesArr[7],
            _verticesArr[4],
            _verticesArr[5]
            };

        return vertices;
    }

    private int[] SetIndices()
    {
        int[] indices = new int[]
            {
                // 전
                0, 1, 2,
                1, 3 ,2,
                // 상
                4, 5, 6,
                5, 7, 6,
                // 우
                8, 9, 10,
                9, 11, 10,
                // 좌
                12, 13, 14,
                13, 15, 14,
                // 하
                16, 17, 18,
                17, 19, 18,
                // 후
                20, 21, 22,
                21, 23, 22
            };

        return indices;
    }


    private Vector3[] CalcNormal(Vector3[] _vertices, int[] _indices)
    {
        // 바깥 방향으로 노멀값이 나가야 함.
        Vector3[] normals = new Vector3[_vertices.Length];
        Vector3 normal = Vector3.zero;
        for (int i = 0; i < _indices.Length; i += 3)
        {
            normal = Vector3.Cross(
                _vertices[_indices[i + 2]] - _vertices[_indices[i + 1]],
                _vertices[_indices[i]] - _vertices[_indices[i + 1]]);

            normals[_indices[i]] = normal;
            normals[_indices[i + 1]] = normal;
            normals[_indices[i + 2]] = normal;
        }

        return normals;
    }



    private void OnTriggerEnter(Collider _other)
    {
        AttackDmg(_other);
    }

    [SerializeField]
    private float increaseSizeRatio = 0.005f;

    private float launchDuration = 0f;
    private float lengthPerSec = 0f;
    private WaitForFixedUpdate waitFixedTime = null;

    private MeshFilter mf = null;
    private MeshCollider mc = null;
    private float initWidth = 0f;
    private float initHeight = 0f;
    private float curHeight = 0f;
    private Color myColor = Color.white;
    private Color hdrColor = Color.white;
    private int myIdx = -1;
}
