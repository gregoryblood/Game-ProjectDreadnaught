using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterMissile : MonoBehaviour
{
    [SerializeField] float looseness = 5f;
    float timeSinceAwake;

    // Start is called before the first frame update
    void OnEnable()
    {
        timeSinceAwake = Random.Range(-1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceAwake += Time.deltaTime/2f;
        transform.localPosition = new Vector3(Mathf.Sin(timeSinceAwake * looseness) * 2f, 0, 0);
    }
}
