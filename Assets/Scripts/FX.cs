using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX : MonoBehaviour
{
    protected float currentTime;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length + 0.1f)
        {
            //TODO optimize FX destruction? Maybe pool?
            Destroy(gameObject);
        }
    }
}
