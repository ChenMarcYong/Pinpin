using System.Collections;
using UnityEngine;
using Cinemachine;
using System.Diagnostics;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private CinemachineVirtualCamera[] _allVirtualCameras;

    [Header("Lerping during jump/fall")]
    [SerializeField] private float _fallScreenY = 0.25f; // Valeur cible de Screen Y pendant la chute
    [SerializeField] private float _normalScreenY = 0.5f; // Valeur normale de Screen Y
    [SerializeField] private float _fallYPanTime = 0.35f; // Durée de l'interpolation
    public float _fallSpeedYDampingChangeThreshold = -15f; // Seuil de vitesse pour détecter une chute

    public bool IsLerpingScreenY { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }

    private Coroutine _lerpScreenYCoroutine;

    private CinemachineVirtualCamera _currentCamera;
    private CinemachineFramingTransposer _framingTransposer;

    private void Awake()
    {
        if (instance == null) instance = this;

        for (int i = 0; i < _allVirtualCameras.Length; i++)
        {
            if (_allVirtualCameras[i].enabled)
            {
                _currentCamera = _allVirtualCameras[i];
                _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
                break;
            }
        }
    }

    #region Lerp the Screen Y

    public void LerpScreenY(bool isPlayerFalling)
    {
        if (_lerpScreenYCoroutine != null)
        {
            StopCoroutine(_lerpScreenYCoroutine);
        }
        _lerpScreenYCoroutine = StartCoroutine(LerpScreenYAction(isPlayerFalling));
    }

    private IEnumerator LerpScreenYAction(bool isPlayerFalling)
    {
        IsLerpingScreenY = true;

        float startScreenY = _framingTransposer.m_ScreenY;
        float endScreenY = isPlayerFalling ? _fallScreenY : _normalScreenY;

        float elapsedTime = 0f;
        while (elapsedTime < _fallYPanTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpedScreenY = Mathf.Lerp(startScreenY, endScreenY, (elapsedTime / _fallYPanTime));
            _framingTransposer.m_ScreenY = lerpedScreenY;

            yield return null;
        }

        _framingTransposer.m_ScreenY = endScreenY;
        IsLerpingScreenY = false;
    }

    #endregion
}
