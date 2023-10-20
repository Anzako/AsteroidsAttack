using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{
    public int marchValue;

    // Start is called before the first frame update

    private void Awake()
    {
        marchValue = Random.Range(-2, 30);
    }
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public int GetMarchValue()
    {
        return marchValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
