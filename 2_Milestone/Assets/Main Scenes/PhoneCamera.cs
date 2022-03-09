using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture BackCam;
    private Texture defaultbg;

    public RawImage background;
    public AspectRatioFitter filter;
    
    // Start is called before the first frame update
    void Start()
    {

        defaultbg = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if(devices.Length == 0)
        {
            Debug.Log("No Camera Detected");
            camAvailable = false;
            return;
        }

        for (int i = 0; i<devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                BackCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if (BackCam == null)
        {
            Debug.Log("Unable to find the back cam");
            return;
        }

        BackCam.Play();
        background.texture = BackCam;
    }

    // Update is called once per frame
    void Update()
    {
        if (!camAvailable)
            return;

        float ratio = (float)BackCam.width / (float)BackCam.height;
        filter.aspectRatio = ratio;

        float scaleY = BackCam.videoVerticallyMirrored ? 1f : -1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = BackCam.videoRotationAngle;
        background.rectTransform.localScale = new Vector3(0, 0, orient);
    }
}
