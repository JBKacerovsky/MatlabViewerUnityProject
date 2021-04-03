using System.Collections.Generic;
using UnityEngine;

public class DictTest : MonoBehaviour
{
    private Dictionary<int, System.Action<int>> _test;

    void Start()
    {
        _test = new Dictionary<int, System.Action<int>>
        {
            {1, Function1},
            {2, Function2}
        };
    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetKeyDown(KeyCode.V))
            {
            Debug.Log("Key press V");
            _test[1](3);
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
            Debug.Log("Key press B");
            _test[2](4);
            }
    }

    private void Function1(int x)
    {
        Debug.Log("this is void a1");
        Debug.Log("x = " + x);
    }

    private void Function2(int x)
    {
        Debug.Log("this is void a2");
        Debug.Log("x^2 = " + x*x);
    }
}
