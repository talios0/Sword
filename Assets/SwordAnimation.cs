
using UnityEngine;

public class SwordAnimation : MonoBehaviour
{
    public Transform hand;
    public Transform hip;
    public Transform standard;

    public void SetParentToHand() {
        transform.parent = hand;
    }

    public void SetParentToDefault() {
        transform.parent = standard;
    }

    public void SetParentToHip() {
        transform.parent = hip;
    }

}
