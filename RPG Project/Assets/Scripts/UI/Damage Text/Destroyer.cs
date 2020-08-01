using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] GameObject target = null;

    public void DestroyTarget()
    {
        Destroy(target,2f);
    }
}
