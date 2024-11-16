using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("References")]
    [SerializeField] private Transform _playerTransform;

    [Header("Flip Rotation Stats")]
    [SerializeField] private float _flipYRotationTime = 0.5f;


    private Coroutine _turnCoroutine;

    private PlayerController _player;

    private bool _isFacingRight;

    private void Awake()
    {
         _player = _playerTransform.gameObject.GetComponent<PlayerController>();
        _isFacingRight = _player.IsFacingRight;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _playerTransform.position;
    }

    public void CallTurn()
    {
        //_turnCoroutine = StartCoroutine(FlipYLerp());
        LeanTween.rotateY(gameObject, DetermineEndRotation(), _flipYRotationTime).setEaseInOutSine();
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while(elapsedTime < _flipYRotationTime) 
        {
            elapsedTime += Time.deltaTime;
            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / _flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }
    }

    private float DetermineEndRotation()
    {
        _isFacingRight = !_isFacingRight;

        if(_isFacingRight)
        {
            return 180f;
        }

        else
        {
            return 0f;
        }
    }
}
