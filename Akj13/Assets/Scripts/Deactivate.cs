using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivate : MonoBehaviour
{
    public GameObject playButton;
    

    public void Disable()
    {
        playButton.SetActive(false);
    }



}
