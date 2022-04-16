using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{

    // TODO: here load last config options and update UI if file exists, otherwise create new options file with default config
    private void Start()
    {
        if (!LoadOptions()) CreateOptions();
        ApplyOptions();
    }

    private void CreateOptions()
    {

    }
    
    private bool LoadOptions()
    {
        if (!PlayerPrefs.HasKey("Fullscreen")) return false;

        return true;
    }

    private void ApplyOptions()
    {

    }
}
