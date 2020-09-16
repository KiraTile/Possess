using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class OutlineController : MonoBehaviour
{
    public static OutlineController Instance;
    [SerializeField]
    Outline previousOutline;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OutlineSingleRenderer(Interactable InteractableObject)
    {
        DisableOutlines();
        InteractableObject.outline.enabled = true;
        previousOutline = InteractableObject.outline;
    }

    public void DisableOutlines()
    {
        if(previousOutline)
        previousOutline.enabled = false;

    }
}
