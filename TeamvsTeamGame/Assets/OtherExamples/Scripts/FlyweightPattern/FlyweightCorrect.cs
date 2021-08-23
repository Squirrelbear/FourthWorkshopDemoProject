using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyweightCorrect : MonoBehaviour
{
    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _propBlock = new MaterialPropertyBlock();
    }

    private Color randomColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    void Update()
    {
        // Get current properties
        _renderer.GetPropertyBlock(_propBlock);
        // Change the properties (does not apply to the renderer yet)
        _propBlock.SetColor("_Color", randomColor());
        // Apply changed properties back to renderer
        _renderer.SetPropertyBlock(_propBlock);
    }
}
