using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A display for when someone changes dimensions
/// </summary>
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(CanvasRenderer))]
public class DimensionLeapDisplay : MonoBehaviour
{
    [Tooltip("Pauses starting and ending a flash (a started or ended flash will continue starting or ending)")]
    public bool pauseFlash = false;

    public static DimensionLeapDisplay _instance;

    private Image image;
    private bool flashing = false;
    private float flashProgress = 0f;

    /// <summary>
    /// Instantiate all necessary thingfs
    void Start()
    {
        _instance = this;
        image = GetComponent<Image>();

        SetAlpha(0);        
    }

    /// <summary>
    /// Do all flash logic
    /// </summary>
    void Update()
    {
        if (flashing && flashProgress < 0.5f)
        {
            //add some flash
            flashProgress += Time.deltaTime;

            SetAlpha(Mathf.Sin(flashProgress * Mathf.PI));
        }

        if (!flashing && flashProgress < 1f && flashProgress != 0)
        {
            //remove some flash
            flashProgress += Time.deltaTime;

            SetAlpha(Mathf.Sin(flashProgress*Mathf.PI));
        }

        if (flashProgress >= 1)
        {
            flashProgress = 0;
        }
    }

    /// <summary>
    /// start a flash if flashing isn't paused
    /// </summary>
    public void Flash()
    {
        if (!pauseFlash)
        {
            flashing = true;
        }        
    }

    /// <summary>
    /// end the flash if flashing isn't paused
    /// </summary>
    public void StopFlash()
    {
        if (!pauseFlash)
        {
            flashing = false;
        }
    }

    /// <summary>
    /// set the alpha value of the flash (make whiter or less white)
    /// </summary>
    /// <param name="alpha">How white should life be?</param>
    private void SetAlpha(float alpha)
    {
        Color clr = image.color;
        clr.a = alpha;
        image.color = clr;
    }
}
