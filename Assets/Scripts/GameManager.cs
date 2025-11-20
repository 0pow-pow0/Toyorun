using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EntitiesCostants;

public class GameManager : MonoBehaviour
{
    public static bool isGamePaused = false;
    public static string animState;
    public static float animationTimer;
    public static float toleranceTimer;
    public static float delayTimer;
    [SerializeField] public TextMeshProUGUI _debugAnimationState;
    [SerializeField] public TextMeshProUGUI _debugAnimationTimer;
    [SerializeField] public TextMeshProUGUI _debugToleranceTimer;
    [SerializeField] public TextMeshProUGUI _debugDelayTimer;
    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _debugAnimationState.text = animState;
    }
}
