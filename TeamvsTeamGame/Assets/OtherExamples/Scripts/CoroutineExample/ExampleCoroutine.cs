using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleCoroutine : MonoBehaviour
{
    private Vector3 origin;
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(movePlaces());
        }
    }

    private IEnumerator movePlaces()
    {
        for(int i = 0; i < 10; i++)
        {
            Vector3 modifiedPosition = new Vector3(Random.Range(origin.x - 4, origin.x + 4),
                                                Random.Range(origin.y - 4, origin.y + 4),
                                                Random.Range(origin.z - 4, origin.z + 4));
            transform.position = modifiedPosition;
            yield return new WaitForSeconds(1f);
        }
        transform.position = origin;
    }
}
