using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseMenu : MonoBehaviour
{
    public bool menuOpen = false;

    public void Start()
    {
        transform.localScale = Vector3.zero;
    }

    public void Update()
    {
        if (menuOpen)
            transform.localScale = Vector3.one;
        else
            transform.localScale = Vector3.zero;
    }

    public void OpenCloseButton()
    {
        if(menuOpen)
            menuOpen = false;
        else
            menuOpen = true;
    }
}
