using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Responsible for controlling the active state of the gun and firing bullet logic
/// </summary>
public class GunController : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePoint;
    public float force = 5f;
    private XRGrabInteractable interactableBase;
    private void Awake()
    {
        interactableBase = GetComponent<XRGrabInteractable>();
        //Registers event function that will call back when an interactable object is captured and the Trigger key is pressed
        interactableBase.activated.AddListener(GrabGunHandler);
    }

    /// <summary>
    /// The conditions for the active callback function are as follows
    /// 1. To be grab by a hand controller
    /// 2. Press the target hand controller trigger button
    /// </summary>
    private void GrabGunHandler(ActivateEventArgs arg0)
    {
        Fire();
    }
    /// <summary>
    /// Firing bullet method, instantiate bullet and add impact force
    /// </summary>
    private void Fire()
    {
        GameObject bulletObj = Instantiate(bullet, firePoint.position,firePoint.rotation);
        bulletObj.GetComponent<BulletController>().AddForce(firePoint.forward * force);
    }

}
