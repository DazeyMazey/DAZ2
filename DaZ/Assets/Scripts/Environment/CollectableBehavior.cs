using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBehavior : MonoBehaviour
{
    public void destroy()
    {
        Destroy(this.gameObject);
    }
}
