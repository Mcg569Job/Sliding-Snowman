using System.Collections;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
   
    void Start()
    {
        StartCoroutine(DestroyMe());
    }


    IEnumerator DestroyMe()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
