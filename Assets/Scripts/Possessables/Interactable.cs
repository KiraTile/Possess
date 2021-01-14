using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour
{
    [HideInInspector]
    [Tooltip("Ref for outline component of the interactable object")]
    public Outline outline;
    // Start is called before the first frame update
    virtual protected void Awake()
    {
       outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
