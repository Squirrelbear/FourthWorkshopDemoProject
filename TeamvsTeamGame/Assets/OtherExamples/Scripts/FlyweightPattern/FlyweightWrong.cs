using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyweightWrong : MonoBehaviour
{
    private Renderer _renderer;
    void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private Color randomColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    void Update()
    {
        // This is bad because it creates a copy of the material causing a lot of memory use if you do it frequently.
        _renderer.material.color = randomColor();
    }
}
