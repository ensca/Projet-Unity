using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class panelController : MonoBehaviour
{

    public Button btn_cam1, btn_cam2, btn_cam3, btn_cam4;
    public Camera cam1, cam2, cam3, cam4;
    public Image zone1, zone2, zone3, zone4, zone5;

    // Use this for initialization
    void Start()
    {
        zone1.gameObject.SetActive(true);
        zone2.gameObject.SetActive(true);
        zone3.gameObject.SetActive(true);
        zone4.gameObject.SetActive(true);
        zone5.gameObject.SetActive(true);
        cam1.gameObject.SetActive(true);
        cam2.gameObject.SetActive(false);
        cam3.gameObject.SetActive(false);
        cam4.gameObject.SetActive(false);
        btn_cam1.onClick.AddListener(() => SwitchCamera1());
        btn_cam2.onClick.AddListener(() => SwitchCamera2());
        btn_cam3.onClick.AddListener(() => SwitchCamera3());
        btn_cam4.onClick.AddListener(() => SwitchCamera4());
    }

    // Update is called once per frame
    void Update()
    {
    }

    void SwitchCamera1()
    {
        cam1.gameObject.SetActive(true);
        //btn_cam1.gameObject.GetComponent<Transform>() = Color.yellow;
        cam2.gameObject.SetActive(false);
        cam3.gameObject.SetActive(false);
        cam4.gameObject.SetActive(false);
    }

    void SwitchCamera2()
    {
        cam1.gameObject.SetActive(false);
        cam2.gameObject.SetActive(true);
        cam3.gameObject.SetActive(false);
        cam4.gameObject.SetActive(false);
    }
    void SwitchCamera3()
    {
        cam1.gameObject.SetActive(false);
        cam2.gameObject.SetActive(false);
        cam3.gameObject.SetActive(true);
        cam4.gameObject.SetActive(false);
    }
    void SwitchCamera4()
    {
        cam1.gameObject.SetActive(false);
        cam2.gameObject.SetActive(false);
        cam3.gameObject.SetActive(false);
        cam4.gameObject.SetActive(true);
    }

    public void HideImage(int image)
    {
        Debug.Log("Hide image : " + image);
        if (image == 1)
            zone1.gameObject.SetActive(false);
        if (image == 2)
            zone2.gameObject.SetActive(false);
        if (image == 3)
            zone3.gameObject.SetActive(false);
        if (image == 4)
            zone4.gameObject.SetActive(false);
        if (image == 5)
            zone5.gameObject.SetActive(false);
    }
}