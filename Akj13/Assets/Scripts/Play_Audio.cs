using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play_Audio : MonoBehaviour
{

    public AudioSource buttonBeep;

    private void Start()
    {
        buttonBeep = GetComponent<AudioSource>();
    }


    public void playBeep()
    {
        buttonBeep.Play();
    }

}
