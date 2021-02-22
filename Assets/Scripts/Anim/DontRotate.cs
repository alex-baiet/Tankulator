using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontRotate : MonoBehaviour
{
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
