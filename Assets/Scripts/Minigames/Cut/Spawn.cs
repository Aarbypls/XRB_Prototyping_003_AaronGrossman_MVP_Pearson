using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objects = new List<GameObject>();
    [SerializeField] private float time = 3f;
    [SerializeField] private int x = 1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;

        if (time <= 0.0f)
        {
            timerEnded();
        }

    }

    void timerEnded()
    {
        float y = Random.Range(0,2);
        if(y < 0.5f)
        {
            x = 0;
        } else
        {
            x = 1;
        }
        Instantiate(_objects[x], transform.position, Quaternion.identity);
        time = Random.Range(1f, 5f);
    }
}
