using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    CinemachineVirtualCamera followCamera;
    [SerializeField] float duration = 0.1f;

    Transform player;

    bool canFinishLevel = true;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

    }
    void Start()
    {
        canFinishLevel = true;
        followCamera = GetComponent<CinemachineVirtualCamera>();
        player = FindObjectOfType<PlayerController>().transform;
    }

    void Update()
    {
        //LevelFinished();
        if(player == null)
        {
            player = FindObjectOfType<PlayerController>().transform;
        }
        if (GameManager.instance.levelFinished)
        {
            followCamera.enabled = false;
            LerpCameraToFinalPosition();
        }
    }

    public void ShakeIt(float amplitude, float frequency)
    {
        CinemachineBasicMultiChannelPerlin noise = followCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = amplitude;
        noise.m_FrequencyGain = frequency;

        StartCoroutine(StopShake(duration));
    }

    private IEnumerator StopShake(float duration)
    {
        yield return new WaitForSeconds(duration);
        CinemachineBasicMultiChannelPerlin noise = followCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 0.0f;
        noise.m_FrequencyGain = 0.0f;
    }

    private void LevelFinished()
    {
        if (GameManager.instance.levelFinished && canFinishLevel)
        {
            canFinishLevel = false;
            StartCoroutine(LerpCamera());
        }
    }

    private IEnumerator LerpCamera()
    {
        float lerpSpeed = 1f;
        Vector3 offset = new Vector3(-2, 1, -1);
        Vector3 playerPosition = player.transform.position + player.forward * 2f;
        Vector3 finalPosition = playerPosition + offset;
        Quaternion finalRotation = Quaternion.Euler(25, 180, 0);

        Vector3 lerpedPos = Vector3.Lerp(transform.position, finalPosition, lerpSpeed * Time.deltaTime);
        Quaternion lerpedRot = Quaternion.Lerp(transform.rotation, finalRotation, lerpSpeed * Time.deltaTime);

        transform.position = lerpedPos;
        //transform.rotation = lerpedRot;
        //transform.SetPositionAndRotation(lerpedPos, lerpedRot);
        yield return new WaitForSeconds(5f);

    }

    private void LerpCameraToFinalPosition()
    {
        float lerpSpeed = 2f;
        Vector3 offset = new Vector3(0, 2, -1);
        Vector3 playerPosition = player.transform.position + player.forward * 4f;
        Vector3 finalPosition = playerPosition + offset;
        Quaternion finalRotation = Quaternion.Euler(25, 180, 0);

        Vector3 lerpedPos = Vector3.Lerp(Camera.main.transform.position, finalPosition, lerpSpeed * Time.deltaTime);
        Quaternion lerpedRot = Quaternion.Lerp(Camera.main.transform.rotation, finalRotation, lerpSpeed * Time.deltaTime);

        Camera.main.transform.position = lerpedPos;
        Camera.main.transform.rotation = lerpedRot;
    }
}
