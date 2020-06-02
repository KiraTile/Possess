using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class OutlineController : MonoBehaviour
{
    public Material OutlineMaterial;
    public static OutlineController Instance;
    public List<Renderer> OutlinedRenderers;
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
        if(!OutlinedRenderers.Contains(renderer))
        OutlinedRenderers.Add(renderer);
        foreach (Renderer outlinedRenderer in OutlinedRenderers)
        {
            Material[] matArray1 = outlinedRenderer.materials;
            matArray1[1] = null ;
            outlinedRenderer.materials = matArray1;
        }
        

        Material[] matArray = renderer.materials;
        matArray[1] = OutlineMaterial;
        renderer.materials = matArray;
    }

    public void DisableOutlines()
    {
       
       
            foreach (Renderer outlinedRenderer in OutlinedRenderers)
            {
                Material[] matArray1 = outlinedRenderer.materials;
                matArray1[1] = null;
                outlinedRenderer.materials = matArray1;
            }
    }
}
