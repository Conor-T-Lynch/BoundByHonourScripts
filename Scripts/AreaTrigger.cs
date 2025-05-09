//@Reference https://www.youtube.com/watch?v=bRcMVkJS3XQ&ab_channel=MoreBBlakeyyy
//@Reference https://docs.unity3d.com/2021.3/Documentation/Manual/collider-interactions-example-scripts.html

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    // The tag used to identify the area zone this trigger belongs to (e.g. BanditHQZone).
    public string areaTag = "BanditHQZone";

    // This method is triggered when another collider enters the trigger collider attached to this object.
    private void OnTriggerEnter(Collider other)
    {
        // Checking to see if the object that entered the trigger has the tag Player.
        if (other.CompareTag("Player"))
        {
            // Calls the RegisterAreaEntry method from the QuestManager and passes the area tag.
            QuestManager.Instance.RegisterAreaEntry(areaTag);
        }
    }
}
