using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TreeObject
{
    [Range(0, 1)] public float RandomRate;
    [Range(0, 5000)] public int startScore;
    public GameObject Prefab;
    public bool RandomRotate;
    public bool RandomScale;
}

[System.Serializable]
public class MyTree
{
    public GameObject myObject;
    public int id;
}

public class WayManager : MonoBehaviour
{
    #region Singleton
    public static WayManager instance = null;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    #endregion


    [Header("- WAYS -")]
    [SerializeField] private Way[] Ways;
    [HideInInspector] public int wayId;

    [Header("- TREES -")]
    [SerializeField] public TreeObject[] TreePrefabs;
    [Range(3, 30)] public int MaxTreeCount;
    [Range(1, 10)] public int ColunmCount;

    [Header("-VARIABLES-")]
    [HideInInspector] public int Proximity;

    private void Start()
    {
        ResetWays();
        SpawnTrees();
    }

    public void ResetWays(bool resetProximity = true)
    {
        if (resetProximity)
            Proximity = 20;

        wayId = 0;
        for (int i = 0; i < Ways.Length; i++)
        {
            Ways[i].CancelCoroutine();
            Ways[i].transform.position = new Vector3(0, -8.6824f * i, 49.24f * i);
            Ways[i].RandomizeTrees();
        }
    }

    public void NextWay()
    {
        Ways[wayId].Trigger();

        wayId++;
        if (wayId > Ways.Length - 1) wayId = 0;
    }
    private void SpawnTrees()
    {
        List<MyTree> trees = new List<MyTree>();
        for (int i = 0; i < Ways.Length; i++)
        {
            trees.Clear();

            for (int j = 0; j < TreePrefabs.Length; j++)
            {
                int count = (int)(TreePrefabs[j].RandomRate * MaxTreeCount);
                if (count == 0) { count = Random.value > .5 ? 1 : 0; }

                for (int k = 0; k < count; k++)
                {
                    GameObject mTree = GameObject.Instantiate(TreePrefabs[j].Prefab, Ways[i].transform);

                    if (TreePrefabs[j].RandomRotate)
                        mTree.transform.localRotation = Quaternion.Euler(mTree.transform.eulerAngles.x, Random.Range(0, 180), mTree.transform.eulerAngles.z);
                    if (TreePrefabs[j].RandomScale)
                        mTree.transform.localScale *= Random.Range(.8f, 1.25f);

                    MyTree mT = new MyTree();
                    mT.myObject = mTree;
                    mT.id = j;
                    trees.Add(mT);
                }
                print(TreePrefabs[j].Prefab.name + " den " + count + " tane");
            }

            Ways[i].SpawnTrees(trees.ToArray());
        }
    }

    public void UpdateLevelByScore()
    {
        if (Proximity > 8) Proximity--;
    }

}
