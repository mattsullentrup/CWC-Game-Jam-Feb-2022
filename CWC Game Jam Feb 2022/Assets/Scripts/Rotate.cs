using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private float _speed;
    [SerializeField] private float xRandom;
    [SerializeField] private float yRandom;
    [SerializeField] private float zRandom;

    private void Start()
    {
        xRandom = Random.Range(-0.9f, 0.9f);
        yRandom = Random.Range(-0.9f, 0.9f);
        zRandom = Random.Range(-0.9f, 0.9f);
        _speed = Random.Range(150, 220);
        //_rotation = new Vector3(Random.Range(0.1f, 0.3f), Random.Range(0.4f, 0.6f), Random.Range(0.7f, 0.9f));
        _rotation = new Vector3(xRandom, yRandom, zRandom);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_speed * Time.deltaTime * _rotation);
    }
}
