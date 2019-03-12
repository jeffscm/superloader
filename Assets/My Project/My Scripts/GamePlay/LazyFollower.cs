// =====================================
// Author: Jefferson Scomacao (2019)
//
// Progressive Async Scene Loading
// using reactive code pattern
//
// Class LazyFollower
// Follow a Rotation with Lazy Following
// Delay to start follow
// Accelerate towards targer
// =====================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazyFollower : MonoBehaviour
{
    public static bool Follow = false;
    public Transform targetTransform;
    public Transform cameraTransform;
    float _accel = 0f;
    float _rotSpeed = 0.75f;
    float _currDot = 0f;
    bool _canRotate = false;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(CheckDot), 0.25f, 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Follow) return;
        if (targetTransform == null) return;

        if (_canRotate || _accel > 0f)
        {
            var q = Quaternion.Lerp(targetTransform.rotation, cameraTransform.rotation, _accel * Time.deltaTime * _rotSpeed);
            targetTransform.rotation = q;

            if (_accel < 1f)
            {
                _accel += Time.deltaTime/2f;
            }

        }
        else
        {
            if (_accel > 0)
            {
                _accel -= Time.deltaTime * 2f;
            }
            _canRotate = false;
        }

    }

    void CheckDot()
    {
        _currDot = Vector3.Dot(targetTransform.position, cameraTransform.position);
        _canRotate = _currDot < 0.75f;
    }
}
