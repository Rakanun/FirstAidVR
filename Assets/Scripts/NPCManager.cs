using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public System.Collections.Generic.List<Rigidbody> rigidbodies = new System.Collections.Generic.List<Rigidbody>();

    public void Fall()
    {
        GetComponent<Animator>().enabled = false;
        foreach (var rb in rigidbodies)
        {
            rb.drag = 1f;
            rb.velocity = Vector3.zero;
        }
    }
}
