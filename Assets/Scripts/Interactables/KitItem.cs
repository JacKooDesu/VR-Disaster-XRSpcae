using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitItem : InteracableObject
{

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Update()
    {
        base.Update();

        transform.Rotate(Vector3.up*20 * Time.deltaTime);
    }

}
