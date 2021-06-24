using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] [Range(10f, 100f)] private float speed;
    [SerializeField] private LayerMask floorLayer;


    private Rigidbody rb;
    private Coroutine coroutine;
    private CapsuleCollider boxCollider;

    [SerializeField] private GameObject snowman;
    [SerializeField] private Transform[] arms;
    [SerializeField] private GameObject ragdoll;
    [SerializeField] private ParticleSystem snowParticle;
    [SerializeField] private ParticleSystem diamondParticle;

    void Awake()
    {
        boxCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WayTrigger")
        {
            WayManager.instance.NextWay();

        }
        if (other.tag == "Item")
        {

            diamondParticle.Play();
            other.gameObject.SetActive(false);
            GameManager.instance.AddCoin(1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.tag == "Object")
        {
            GameOver();
        }

    }

    void FixedUpdate()
    {
        if (!GameManager.instance.gameStatusIsNormal()) return;
        MovingPlayer();

    }

    private void MovingPlayer()
    {
        if (isFloor())
            if (!snowParticle.gameObject.activeSelf)
            {
                snowParticle.gameObject.SetActive(true);
                AudioManager.instance.PlaySound(AT.Floor);
            }

        rb.velocity = new Vector3(0, isFloor() ? -8.78f : rb.velocity.y, speed * 2);
        GameManager.instance.AddScore(speed * Time.deltaTime);

        if (rb.velocity.y > 5)
        {
            Vector3 vel = rb.velocity;
            vel.y = 0;
            rb.velocity = vel;

            if (coroutine == null)
                coroutine = StartCoroutine(RotatePlayer());
        }
    }

    private bool isFloor()
    {
        Debug.DrawRay(transform.position + new Vector3(0, 0, 1.5f), -transform.up * .3f, Color.red);
        return Physics.Raycast(transform.position + new Vector3(0, 0, 1.5f), -transform.up, .3f, floorLayer);
    }

    private IEnumerator RotatePlayer()
    {

        float t = 0;
        float z = Random.Range(-20, 20);
        AudioManager.instance.PlaySound(AT.Jump);
        snowParticle.gameObject.SetActive(false);
        while (t < 2)
        {
            t += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 180, z), t);

            arms[0].localRotation = Quaternion.Lerp(arms[0].localRotation, Quaternion.Euler(90, 0, 120), t / 2);
            arms[1].localRotation = Quaternion.Lerp(arms[1].localRotation, Quaternion.Euler(0, 120, 0), t / 2);

            yield return null;
        }
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 360, 0), t);

            arms[0].localRotation = Quaternion.Lerp(arms[0].localRotation, Quaternion.Euler(90, 0, 0), t);
            arms[1].localRotation = Quaternion.Lerp(arms[1].localRotation, Quaternion.Euler(0, 0, 0), t);

            yield return null;
        }
        coroutine = null;

    }


    private void GameOver()
    {
        GameManager.instance.GameOver();
        GameObject rd = Instantiate(ragdoll);
        rd.transform.position = transform.position;
        snowParticle.gameObject.SetActive(false);
        snowman.SetActive(false);
        boxCollider.enabled = false;
        rb.isKinematic = true;
    }

    public void ResetPlayer(bool resetPosition = true)
    {

        boxCollider.enabled = true;
        rb.isKinematic = false;
        snowman.SetActive(true);
        if (resetPosition)
        {
            speed = 25;
            transform.position = new Vector3(0, 3.21f, -83);
        }
        else
        {
            transform.position = new Vector3(0, 6f, -20);
            StartCoroutine(Untouchable());
        }
    }

    private IEnumerator Untouchable()
    {
        boxCollider.enabled = false;
        for (int i = 0; i < 10; i++)
        {
            snowman.SetActive(!snowman.activeSelf);
            yield return new WaitForSeconds(.25f);
        }
        boxCollider.enabled = true;
    }

    public void UpdateSpeedByScore()
    {
        if (speed < 28)
            speed += .5f;
    }
}
