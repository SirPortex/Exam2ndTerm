using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : MonoBehaviour
{
    public float force, minTimeToExplode, maxTimeToExplode;
    public int minFireworks, maxFireworks;
    public GameObject fireworkPrefab;
    public int maxExplosions = 3;

    private Rigidbody2D _rb;
    private SpriteRenderer _rend;
    private int _count = 0;
    private Vector2 _dir = Vector2.up;
    private float currentTime, timeToExplode;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;

        _rb = GetComponent<Rigidbody2D>();
        _rend = GetComponent<SpriteRenderer>();

        GameManager.instance.SetFireWorks(GameManager.instance.GetFireWorks()+1);

        _rb.velocity = _dir * force; //fuerza inicial, primer fuego

        _rend.color = Random.ColorHSV(); // color aleatorio

        timeToExplode = Random.Range(minTimeToExplode, maxTimeToExplode); // destruccion en tiempo aleatorio dentro del rango establecido ss

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;

        if (currentTime > timeToExplode)
        {
            print("FUEGOSS!!!");
            Instantiate(fireworkPrefab);
            Destroy(gameObject, timeToExplode);
        }

        //_rb.velocity = _dir * force;

        Destroy(gameObject, Random.Range(minTimeToExplode, maxTimeToExplode)); // destruccion en tiempo aleatorio dentro del rango establecido ss


    }
}
