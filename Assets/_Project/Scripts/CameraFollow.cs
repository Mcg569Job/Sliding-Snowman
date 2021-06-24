using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 distance;
    [SerializeField] [Range(1f, 20f)] private float speed = 5;
    private bool shake;

    void FixedUpdate()
    {
        if (GameManager.instance.gameStatus == GameStatus.GameOver)
        {
            transform.Rotate(0, -30 * Time.deltaTime, 0);
            return;
        }

        transform.position = Vector3.Lerp(transform.position, target.position + distance, Time.deltaTime * speed * 2);
    }

    public void ShakeCamera(float duration, float magnitude) => StartCoroutine(Shake(duration,magnitude));

    private IEnumerator Shake(float duration, float magnitude)
    {

        Vector3 orijinalpos = transform.localPosition;

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            shake = true;
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(orijinalpos.x + x, orijinalpos.y + y, orijinalpos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = orijinalpos;
        shake = false;
    }

    public void ResetCamera()
    {
        transform.localRotation = Quaternion.Euler(25,0,0);
    }
}
