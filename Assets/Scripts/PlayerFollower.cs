using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    public Collider Target;
    // Start is called before the first frame update
    private void Awake()
    {
        PlayerController.Possession.AddListener(SwitchTarget);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Target.bounds.center;
    }
    
    void SwitchTarget()
    {
        Target = PlayerController.CurrentPossessed.GetComponent<Collider>();
    }
}
