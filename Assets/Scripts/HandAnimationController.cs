using UnityEngine;

public class HandAnimationController : MonoBehaviour
{
    private Animator animator;

    public OVRInput.RawButton handTypeGrab;
    public OVRInput.RawButton handTypePoint;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (OVRInput.GetDown(handTypeGrab))
            animator.SetBool("Grab", true);
        if (OVRInput.GetUp(handTypeGrab))
            animator.SetBool("Grab", false);

        if (OVRInput.GetDown(handTypePoint))
            animator.SetBool("Point", true);
        if (OVRInput.GetUp(handTypePoint))
            animator.SetBool("Point", false);
    }
}
