using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnpossessedPlayer : Possessable
{
    public static UnpossessedPlayer Instance;
    new private void Awake()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else
            Instance = this;

        base.Awake();
    }

    new void Possess()
    {
        base.Possess();
    }
}
