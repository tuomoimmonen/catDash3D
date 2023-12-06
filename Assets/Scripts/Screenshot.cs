using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TakeScreenshot();
    }

    public void TakeScreenshot()
    {
        if(Input.GetKeyDown(KeyCode.Return)) 
        {
            ScreenCapture.CaptureScreenshot("first.png", 2);
        }
    }
}
