using System.Collections;
using UnityEngine;

public class Way : MonoBehaviour
{
    private MyTree[] trees;
    private Coroutine coroutine=null;
    public void SpawnTrees(MyTree[] _trees)
    {
        trees = _trees;
        RandomizeTrees();


    }

    public void Trigger() => coroutine= StartCoroutine(MoveWay_());
    public void CancelCoroutine() { if (coroutine != null) StopCoroutine(coroutine); }
    private IEnumerator MoveWay_()
    {
        yield return new WaitForSeconds(3f);        
            MoveWay();       
    }

    private void MoveWay()
    {
        if (GameManager.instance.gameStatus == GameStatus.GameOver) return;
        transform.position += new Vector3(0, -86.821f, 492.4f);
        RandomizeTrees();
    }

    public void RandomizeTrees()
    {
        if (trees == null) return;
        for (int a = 0; a < trees.Length; a++)
        {
        //    if (!trees[a].myObject.activeSelf) trees[a].myObject.SetActive(true);
            int r = Random.Range(0, trees.Length);
            MyTree t = trees[r];
            trees[r] = trees[a];
            trees[a] = t;
        }

        int ColunmCount = WayManager.instance.ColunmCount;
        int x = 0;
        int proximity = WayManager.instance.Proximity;
        for (int i = 0; i < trees.Length; i++)
        {

            Vector3 pos =
                new Vector3(Random.Range((x - 1) * proximity, x * proximity) - 15, -1.55f * ((int)(i / ColunmCount) + 1) + .5f, -9 + (9 * (int)(i / ColunmCount)));


            if (WayManager.instance.TreePrefabs[trees[i].id].startScore > GameManager.instance.GetScore())
            {
                trees[i].myObject.SetActive(false);
            }else trees[i].myObject.SetActive(true);


            x++;
            if (x > ColunmCount - 1) x = 0;

            trees[i].myObject.transform.localPosition = pos;
        }
    }

}
