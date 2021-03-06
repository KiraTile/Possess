﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    public static PlayerFollower Instance;
    public Collider Target;
    public Transform PlayerRotationFollower;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        PlayerController.OnPossession.AddListener(SwitchTarget);
    }

    // Update is called once per frame
    void Update()
    {

        Quaternion LookAtRotationOnly_Y = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

        PlayerRotationFollower.rotation = LookAtRotationOnly_Y;

        transform.position = Target.bounds.center;
      
    }
    
    void SwitchTarget()
    {
        Target = PlayerController.CurrentPossessed.GetComponent<Collider>();
    }
    private void OnDestroy()
    {
        PlayerController.OnPossession.RemoveListener(SwitchTarget);
    }
}
