using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Cinemachine_Switcher : MonoBehaviour
{


    [SerializeField]
    private CinemachineVirtualCamera vcam1; //Title Cam

    [SerializeField]
    private CinemachineVirtualCamera vcam2; // Game Cam

    private bool title_cam = true;


    public void SwitchPriority()
    {
        if (title_cam)
        {
            vcam1.Priority = 0;
            vcam2.Priority = 1;
        }else
        {
            vcam1.Priority = 1;
            vcam2.Priority = 0;
        }
        title_cam = !title_cam;
    }



}
