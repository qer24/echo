using System;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
///     Add this component to your Cinemachine Virtual Camera to have it shake when calling its ShakeCamera methods.
/// </summary>
[RequireComponent(typeof(CinemachineCameraOffset))]
public class CinemachineShake : MonoBehaviour
{
    private static CinemachineShake _instance;

    /// The default amplitude that will be applied to your shakes if you don't specify one
    public float DefaultShakeAmplitude = 1f;
    /// The default frequency that will be applied to your shakes if you don't specify one
    public float DefaultShakeFrequency = 10f;
    private float _elapsedTime;
    private CinemachineCameraOffset _offset;

    private float _seed;
    private float _startDutch;

    private Vector2 _startPos;

    private float _trauma;


    private CinemachineVirtualCamera _vcam;

    public static CinemachineShake Instance
    {
        get
        {
            // if (_instance == null)
            // {
            //     var instances = FindObjectsOfType<CinemachineShake>();
            //     if (instances.Length > 1)
            //         Debug.LogError("Multiple Instances of CameraShake in scene");
            //     else if (instances.Length == 0)
            //         Debug.LogError("No instances of CameraShake in scene");
            //     else
            //         _instance = instances[0];
            // }

            return _instance;
        }
    }

    private void OnEnable()
    {
        _instance = this;
    }

    // Use this for initialization
    private void Start()
    {
        _seed = Random.Range(0, 999999);
        _vcam = GetComponent<CinemachineVirtualCamera>();
        _offset = _vcam.GetComponent<CinemachineCameraOffset>();
        _startPos = new Vector2(_offset.m_Offset.x, _offset.m_Offset.y);
        _startDutch = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        _trauma -= Time.deltaTime;
        _trauma = Mathf.Clamp(_trauma, 0, 1);

        float xPos = GetPerlin(_seed, DefaultShakeFrequency, DefaultShakeAmplitude);
        float yPos = GetPerlin(_seed + 1, DefaultShakeFrequency, DefaultShakeAmplitude);
        float rot = GetPerlin(_seed + 2, DefaultShakeFrequency, DefaultShakeAmplitude * 10f);
        // transform.position = new Vector3(startPos.x + xPos, startPos.y + yPos, startPos.z);
        // transform.eulerAngles = new Vector3(0, 0, rot);
        _offset.m_Offset.x = _startPos.x + xPos;
        _offset.m_Offset.y = _startPos.y + yPos;
        _vcam.m_Lens.Dutch = _startDutch + rot;

        //_elapsedTime += GameManager.GamePaused ? Time.deltaTime : Time.unscaledDeltaTime;
        _elapsedTime += Time.unscaledDeltaTime;
    }

    private float GetPerlin(float newSeed, float frequency, float strength)
    {
        float noise = Mathf.PerlinNoise(newSeed + _elapsedTime * frequency, newSeed + _elapsedTime * frequency);
        noise = noise * 2 - 1;

        return noise * _trauma * _trauma * strength;
    }

    public void AddTrauma(float trauma)
    {
        _trauma += trauma;
    }

    public void SetTrauma(float trauma)
    {
        _trauma = trauma;
    }
}
