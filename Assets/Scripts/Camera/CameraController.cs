using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public static class CameraController 
{
 
   public static void CameraChange(GameObject camera1,GameObject camera2)
   {
    camera1.SetActive(false);
    camera2.SetActive(true);
   }

}
