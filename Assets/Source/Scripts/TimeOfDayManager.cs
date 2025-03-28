using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class TimeOfDayManager : MonoBehaviour
{
    [Range(0f, 0.5f)]
    public float TimeOfDay;
    
    [SerializeField]
    private Transform MoonAndSun;

    [SerializeField]
    private Gradient GradientSky;

    [SerializeField]
    private float GameDayMinutes;

    [SerializeField]
    private List<SpriteRenderer> Sky;
    [SerializeField]
    private List<SpriteRenderer> Stars;

    [SerializeField]
    private SpriteRenderer m_Light;

    private int operate = 1;

    private void Update()
    {
        TimeOfDay += (Time.deltaTime / (GameDayMinutes * 60)) * operate;
        if (TimeOfDay >= 0.5f ) operate = -1;
        else if (TimeOfDay <= 0f) operate = 1;

        MoonAndSun.localRotation = Quaternion.Euler(0, 0, -(TimeOfDay * operate) * 360f);
        foreach (SpriteRenderer sky in Sky)
        {
            Color color = GradientSky.Evaluate(TimeOfDay);
            sky.color = new Color(color.r, color.g, color.b);
        }

        foreach (SpriteRenderer star in Stars)
        {
            Color color = GradientSky.Evaluate(TimeOfDay);
            star.color = new Color(255,255,255, color.a);
        }

        m_Light.color = GradientSky.Evaluate(TimeOfDay);

    }
}
