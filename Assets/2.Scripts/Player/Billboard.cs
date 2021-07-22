using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
   void LateUpdade()
   {
      transform.LookAt(Camera.main.transform.position);
   }
}
