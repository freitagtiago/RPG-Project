using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicsTrigger : MonoBehaviour
    {
        private bool _alreadyTriggered = false;
       private void OnTriggerEnter(Collider other)
       {
           if (!_alreadyTriggered  && other.tag =="Player")
           {
                GetComponent<PlayableDirector>().Play();
                _alreadyTriggered = true;
           }
       }
    }

}
