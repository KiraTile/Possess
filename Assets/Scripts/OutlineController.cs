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

        Material[] matArray1 = PreviousOutlinedRenderer.materials;
        if (!matArray1.Where(obj=> obj.shader == OutlineMaterial.shader).FirstOrDefault())
        {
            List<Material> materials = matArray1.ToList();
            materials.Add(OutlineMaterial);
            PreviousOutlinedRenderer.materials = materials.ToArray();
        }
    }

    public void DisableOutlines()
    {


        if (PreviousOutlinedRenderer)
        {
            Material[] matArray1 = PreviousOutlinedRenderer.materials;
            
               Material temp = matArray1.Where<Material>(obj => obj == OutlineMaterial).FirstOrDefault();
               List<Material> materials =  matArray1.ToList();
                materials.Remove(materials.Where(obj => obj.shader == OutlineMaterial.shader).FirstOrDefault());
                PreviousOutlinedRenderer.materials = materials.ToArray();
            
         
            

        }

        PreviousOutlinedRenderer = null;

    }
}
