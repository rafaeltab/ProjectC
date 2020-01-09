using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles the back button in the options menu
/// </summary>
public class BackButton : IOptionsButton
{
    public Button pageButton;
    public Canvas parentCanvas;
    public OptionsMenu optionsMenu;
    public Vector2 buttonLocations;

    /// <summary>
    /// Constructor for the back button in options menu
    /// </summary>
    public BackButton(Button pageButton, Canvas parentCanvas, OptionsMenu optionsMenu, Vector2 buttonLocations)
    {
        this.parentCanvas = parentCanvas;
        this.optionsMenu = optionsMenu;
        this.buttonLocations = buttonLocations;
        Rescale(pageButton,0);
    }

    public void Disable()
    {
        //nothing goes here just needs to exist
    }

    public bool IsBackButton()
    {
        return true;
    }

    /// <summary>
    /// Scales the button for different resolutions
    /// </summary>
    public void Rescale(Button pageButton,int ind,float oldWidth = 1920, float oldHeight = 1080)
    {
        if (this.pageButton == null)
        {
            this.pageButton = Object.Instantiate(pageButton, parentCanvas.transform);
        }
        else
        {
            this.pageButton.transform.localScale = pageButton.transform.localScale;
        }

        float widthPerc = oldWidth / Screen.width;
        float heightPerc = oldHeight / Screen.height;

        Vector3 newScale = this.pageButton.transform.localScale;
        newScale.x /= widthPerc;
        newScale.y /= heightPerc;

        this.pageButton.transform.localScale = newScale;

        HandleButton();
    }

    /// <summary>
    /// Closes the options menu
    /// </summary>
    public void Enable()
    {
        OptionsLoader.GetInstance((c)=>{ c.Close(); });
    }

    /// <summary>
    /// Grabs the page button
    /// </summary>
    public Button GetButton()
    {
        return pageButton;
    }

    /// <summary>
    /// Positions the back button in the correct place
    /// </summary>
    public void HandleButton()
    {
        Vector3 pos = new Vector3(Screen.width * (buttonLocations.x), 0, 0);
        pos.y = (((1 - buttonLocations.y)) * Screen.height);
        pageButton.transform.position = pos;

        pageButton.GetComponentInChildren<TextMeshProUGUI>().text = "Back";

        pageButton.onClick.AddListener(HandleClick);
    }

    public void HandleClick()
    {
        Enable();
    }
}


