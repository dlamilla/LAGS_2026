using UnityEngine;

public class AnimationEventBridge : MonoBehaviour
{
    public Spear spear;

    public void ActivateHitBox() => spear.ActivateHitBox();

    public void DeactivateHitBox() => spear.DeactivateHitBox();
}
