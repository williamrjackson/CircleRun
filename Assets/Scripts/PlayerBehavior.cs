using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {
    public float jumpHeight = 1f;
    public float jumpLength = .5f;

    public AutoRotate rotatingParent;
    public UnityEngine.UI.Image completionIndicatorRing;

    private UnityEngine.UI.Image m_IndicatorRingClone;
    private TouchAxisCtrl m_Touch;
    private bool m_bGrounded = true;
    private bool m_bMovingDown = false;
    private float m_InitialLandRotation;
    private float m_CompletionPercent = 0f;
    private Wrj.Utils.MapToCurve.MappedCurvePlayer m_CurvePlayer;

	// Use this for initialization
	void Start () {
        m_Touch = FindObjectOfType<TouchAxisCtrl>();
        m_Touch.OnTouch += OnTouch;
        m_Touch.OnUntouch += OnUntouch;
        Land();
	}

    private void OnTouch()
    {
        if (m_bGrounded)
        {
            m_CurvePlayer = Wrj.Utils.MapToCurve.EaseIn.Move(transform, Vector3.up * jumpHeight, jumpLength, mirrorCurve: true, onDone: OnUntouch);
            m_bGrounded = false;
        }
    }

    private void OnUntouch()
    {
        if (!m_bGrounded && !m_bMovingDown)
        {
            if (m_CurvePlayer != null)
                m_CurvePlayer.Stop();

            m_bMovingDown = true;
            m_CurvePlayer = Wrj.Utils.MapToCurve.EaseIn.Move(transform, Vector3.zero, jumpLength * Mathf.InverseLerp(0, jumpHeight, transform.localPosition.y), onDone: Land);
        }
    }

    private void Land()
    {
        m_bGrounded = true;
        m_bMovingDown = false;
        m_CurvePlayer = null;
        NewRingClone();
    }

    private void NewRingClone()
    {
        m_InitialLandRotation = rotatingParent.accruingAngle;
        m_IndicatorRingClone = Instantiate(completionIndicatorRing);
        m_IndicatorRingClone.rectTransform.parent = completionIndicatorRing.rectTransform.parent;
        m_IndicatorRingClone.rectTransform.localScale = completionIndicatorRing.rectTransform.localScale;
        m_IndicatorRingClone.rectTransform.localPosition = completionIndicatorRing.rectTransform.localPosition;
        m_IndicatorRingClone.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update () {
		if (m_bGrounded)
        {
            Vector3 targetRotation = m_IndicatorRingClone.rectTransform.localEulerAngles;
            targetRotation.z = rotatingParent.accruingAngle;
            m_IndicatorRingClone.rectTransform.localEulerAngles = targetRotation;
            m_IndicatorRingClone.fillAmount = Mathf.InverseLerp(m_InitialLandRotation, m_InitialLandRotation + 360, rotatingParent.accruingAngle);
        }
	}
}
