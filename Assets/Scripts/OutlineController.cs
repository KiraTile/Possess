using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class OutlineController : MonoBehaviour
{
    public Material OutlineMaterial;
    public static OutlineController Instance;
    public Renderer PreviousOutlinedRenderer;
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

    public void OutlineSingleRenderer(Renderer renderer)
    {
        if (PreviousOutlinedRenderer == renderer)
            return;


        if (PreviousOutlinedRenderer)
            DisableOutlines();

        PreviousOutlinedRenderer = renderer;
      
        

        Material[] matArray = renderer.materials;
        matArray[1] = OutlineMaterial;
        renderer.materials = matArray;
    }

    public void DisableOutlines()
    {


        if (PreviousOutlinedRenderer)
        {
            Material[] matArray1 = PreviousOutlinedRenderer.materials;
            matArray1[1] = null;
            PreviousOutlinedRenderer.materials = matArray1;
            PreviousOutlinedRenderer = null;
        }
               
         
    }
}
