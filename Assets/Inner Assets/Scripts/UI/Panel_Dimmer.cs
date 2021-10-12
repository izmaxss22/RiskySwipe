using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Dimmer : MonoBehaviour
{
    public event Action Event_OnAnimShowEnd;

    public void On_AnimShowEnd()
    {
        Event_OnAnimShowEnd?.Invoke();
    }

    public void On_AnimHideEnd()
    {
        Destroy(gameObject);
    }
}
