using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruct : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestructionCoro());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DestructionCoro()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
