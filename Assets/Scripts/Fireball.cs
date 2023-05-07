using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int id = 0;
    private void OnEnable()
    {
        Invoke("Deactivate", 2f);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
