using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public enum Team
{
    none,
    blue,
    yellow
}
public class GameManager : MonoBehaviour
{
    public Volume volume;
    public TMP_Dropdown qualityDropdown;
    public Toggle visualEffectsToggle;

    public GameObject Camera;

    [SerializeField] public Team team;

    public GameObject Robot;

    public List<Vector3> blueSquares;
    public List<Vector3> yellowSquares;
    public float squareRadius;
    public float plantRadius;
    public int points;
    [SerializeField] TextMeshProUGUI bluePointsText;
    [SerializeField] TextMeshProUGUI yellowPointsText;
    [SerializeField] TextMeshProUGUI timerText;
    public float gameTime = 0;

    public bool isTouchControls;
    public GameObject touchControls;
    public Joystick joystick1;
    public Joystick joystick2;


    // Start is called before the first frame update
    void Start()
    {
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        visualEffectsToggle.isOn = true;


        switch (Application.platform)
        {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                Debug.Log("Running on Windows");
                break;
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
                Debug.Log("Running on macOS");
                break;
            case RuntimePlatform.LinuxPlayer:
                Debug.Log("Running on Linux");
                break;
            case RuntimePlatform.Android:
                Debug.Log("Running on Android");
                isTouchControls = Input.touchSupported;
                break;
            case RuntimePlatform.IPhonePlayer:
                Debug.Log("Running on iOS");
                isTouchControls = Input.touchSupported;
                break;
            default:
                Debug.Log("Running on unknown platform");
                isTouchControls = Input.touchSupported;
                break;
        }
        Application.targetFrameRate = (int)Math.Ceiling(Screen.currentResolution.refreshRateRatio.value) + 1;
        print("trying to go to " + Application.targetFrameRate);

        touchControls.SetActive( isTouchControls );
    }

    // Update is called once per frame
    void Update()
    {
        // points = CalculateGamePoints();
        // bluePointsText.text = points.blue.ToString();
        // yellowPointsText.text = points.yellow.ToString();
        // gameTime += Time.deltaTime * Time.timeScale;
        timerText.text = FormatTime(gameTime);
    }
    static string FormatTime(float timeInSeconds)
    {
        // Convert time to TimeSpan
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeInSeconds);

        // Format the time
        string formattedTime = string.Format("{0:0}:{1:00}.{2:0}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds/100);

        return formattedTime;
    }
    public void VisualEffectsChange(bool isOn)
    {
        volume.enabled = isOn;
    }
    public void QualityChange(int choix)
    {
        QualitySettings.SetQualityLevel(choix);
        Debug.Log("Quality level set to: " + QualitySettings.names[choix]);
    }
}
