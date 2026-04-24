using UnityEngine;

public class HandsPositionTrigger : MonoBehaviour
{
    public static HandsPositionTrigger instance;

    public GameObject LeftHand;
    public GameObject RightHand;
    public GameObject LeftController;
    public GameObject RightController;

    private bool isReady;
    private bool leftHold;
    private bool rightHold;
    private Vector3 centerPos;
    private Vector3 startCenterPos;

    public bool IsReady => isReady;
    public Vector3 CenterPos => centerPos;
    public Vector3 StartCenterPos => startCenterPos;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        isReady = LeftHand.activeSelf && RightHand.activeSelf;

        if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
        {
            rightHold = true;
            InitStartCenterPos();
        }
        if (OVRInput.GetDown(OVRInput.RawButton.LHandTrigger))
        {
            leftHold = true;
            InitStartCenterPos();
        }

        if (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger))
            rightHold = false;
        if (OVRInput.GetUp(OVRInput.RawButton.LHandTrigger))
            leftHold = false;

        if (leftHold && rightHold && isReady)
        {
            centerPos = (RightController.transform.parent.parent.position +
                         LeftController.transform.parent.parent.position) / 2f;
        }
        else
        {
            startCenterPos = Vector3.zero;
            centerPos = Vector3.zero;
        }
    }

    private void InitStartCenterPos()
    {
        if (isReady && leftHold && rightHold)
            startCenterPos = (RightController.transform.parent.parent.position +
                              LeftController.transform.parent.parent.position) / 2f;
        else
            startCenterPos = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LeftHand"))
        {
            LeftHand.SetActive(true);
            LeftController.SetActive(false);
            leftHold = false;
        }
        if (other.CompareTag("RightHand"))
        {
            RightHand.SetActive(true);
            RightController.SetActive(false);
            rightHold = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LeftHand"))
        {
            LeftHand.SetActive(false);
            LeftController.SetActive(true);
        }
        if (other.CompareTag("RightHand"))
        {
            RightHand.SetActive(false);
            RightController.SetActive(true);
        }
    }
}
