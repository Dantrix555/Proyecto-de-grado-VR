using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticule : MonoBehaviour
{
    public Pointer m_pointer;
    public SpriteRenderer m_CircleRenderer;

    public Sprite m_OpenSprite;
    public Sprite m_ClosedSprite;

    private Camera m_Camera = null;

    void Awake()
    {
        m_pointer.OnPointerUpdate += UpdateSprite;
        m_Camera = Camera.main;
    }

    void Update()
    {
        transform.LookAt(m_Camera.gameObject.transform);
    }

    void OnDestroy()
    {
        m_pointer.OnPointerUpdate -= UpdateSprite;
    }

    void UpdateSprite(Vector3 point, GameObject hitObject)
    {
        transform.position = point;
        if (hitObject)
        {
            m_CircleRenderer.sprite = m_ClosedSprite;
        }
        else
        {
            m_CircleRenderer.sprite = m_OpenSprite;
        }
    }
}
