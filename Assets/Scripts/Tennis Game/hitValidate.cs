using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitValidate : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    [SerializeField]
    private AudioClip audioClipValidate;
    [SerializeField]
    private AudioClip audioClipFail;

    private AudioSource audioSource;


    public bool validate = false;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        Invoke("DestroyBall", 2f);
    }

    private void DestroyBall()
    {
        if (!validate)
        {
            audioSource.PlayOneShot(audioClipFail, 0.5f);
            Destroy(this.gameObject, 1f);
        }else{
            Destroy(this.gameObject, 2f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        string colTag = collision.gameObject.tag;

        if (colTag == "Shield")
        {
            meshRenderer.material.color = Color.green;
            audioSource.PlayOneShot(audioClipValidate);
            validate = true;
            LaunchMgr.control.hitCount++;
        }

        CancelInvoke();
        DestroyBall();
    }
}
