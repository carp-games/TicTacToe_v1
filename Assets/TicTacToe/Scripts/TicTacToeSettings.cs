using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TicTacToeSettings : MonoBehaviour {

    [SerializeField]
    private TicTacToeController ticTacToeController;
    [SerializeField]
    private RectTransform canvasRect;
    [SerializeField]
    private Text p1WinsText;
    [SerializeField]
    private Toggle p1AiToggle;
    [SerializeField]
    private Text p2WinsText;
    [SerializeField]
    private Toggle p2AiToggle;
    [SerializeField]
    private Transform settingsPanel;
    [SerializeField]
    private GameObject settingsAiPanel;
    [SerializeField]
    private float animationSpeed = 1;
    [SerializeField]
    private Text speedSliderText;
    [SerializeField]
    private Slider speedSlider;
    [SerializeField]
    private Text gameOverText;
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Button hideButton;
    [SerializeField]
    private Button showButton;
    [SerializeField]
    private float buttonBlinkSpeed = 4;
    [SerializeField]
    private float buttonBlinkDuration = 2.3f;

    private int p1Score = 0;
    private int p2Score = 0;

    private Coroutine hideSettingsCoroutine = null;
    private Coroutine showSettingsCoroutine = null;

    private void Start() {
        ticTacToeController.onGameOverDelegate = OnGameOver;
        StartCoroutine(StartButtonBlinkCoroutine());
    }

    public void CheckIfAnyAiIsActive() {
        settingsAiPanel.SetActive(p1AiToggle.isOn || p2AiToggle.isOn);
    }

    public void OnP1AiToggled(bool active) {
        // Debug.Log("OnP1AiToggled " + active);
        ticTacToeController.p1Ai = active;
        CheckIfAnyAiIsActive();
    }
    public void OnP2AiToggled(bool active) {
        // Debug.Log("OnP2AiToggled " + active);
        ticTacToeController.p2Ai = active;
        CheckIfAnyAiIsActive();
    }
    public void OnShortcutsToggled(bool active) {
        // Debug.Log("OnShortcutsToggled " + active);
        ticTacToeController.useShortcuts = active;
    }
    public void OnVisualizeToggled(bool active) {
        // Debug.Log("OnVisualizeToggled " + active);
        ticTacToeController.visualizeAI = active;
        speedSliderText.gameObject.SetActive(active);
        speedSlider.gameObject.SetActive(active);
    }
    public void OnSpeedChanged(float value) {
        // Debug.Log("OnSpeedChanged " + value);
        ticTacToeController.algorithmStepDuration = value;
        speedSliderText.text = "Step Duration: " + System.Math.Round(value, 2) + "s";
    }
    public void OnStartClicked() {
        // Debug.Log("OnStartClicked");
        startButton.interactable = false;
        OnHideClicked();
        ticTacToeController.StartGame();
    }
    public void OnHideClicked() {
        StopAnimationCoroutines();
        hideSettingsCoroutine = StartCoroutine(HideSettingsCoroutine());
    }
    public void OnShowClicked() {
        StopAnimationCoroutines();
        showSettingsCoroutine = StartCoroutine(ShowSettingsCoroutine());
    }

    private void StopAnimationCoroutines() {
        if (hideSettingsCoroutine != null) {
            StopCoroutine(hideSettingsCoroutine);
            hideSettingsCoroutine = null;
        }
        if (showSettingsCoroutine != null) {
            StopCoroutine(showSettingsCoroutine);
            showSettingsCoroutine = null;
        }
    }

    private IEnumerator HideSettingsCoroutine() {
        hideButton.interactable = false;
        while (true) {
            settingsPanel.localScale = new Vector3(settingsPanel.localScale.x - animationSpeed * Time.deltaTime,
                                                   settingsPanel.localScale.y, settingsPanel.localScale.z);
            if (settingsPanel.localScale.x <= 0.1f) {
                settingsPanel.localScale = new Vector3(0.1f, settingsPanel.localScale.y, settingsPanel.localScale.z);
                showButton.gameObject.SetActive(true);
                LayoutRebuilder.ForceRebuildLayoutImmediate(canvasRect);
                yield break;
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(canvasRect);
            hideSettingsCoroutine = null;
            yield return null;
        }
    }
    private IEnumerator ShowSettingsCoroutine() {
        showButton.gameObject.SetActive(false);
        while (true) {
            settingsPanel.localScale = new Vector3(settingsPanel.localScale.x + animationSpeed * Time.deltaTime,
                                                   settingsPanel.localScale.y, settingsPanel.localScale.z);
            if (settingsPanel.localScale.x >= 1) {
                settingsPanel.localScale = new Vector3(1, settingsPanel.localScale.y, settingsPanel.localScale.z);
                hideButton.interactable = true;
                LayoutRebuilder.ForceRebuildLayoutImmediate(canvasRect);
                showSettingsCoroutine = null;
                yield break;
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(canvasRect);
            yield return null;
        }
    }

    private IEnumerator StartButtonBlinkCoroutine() {
        float animationCounter = 0;

        float hue = 0.35f;
        float saturation = 1;
        float value;

        while (true) {
            ColorBlock colors = startButton.colors;
            if (animationCounter >= buttonBlinkDuration) {
                colors.normalColor = Color.HSVToRGB(hue, saturation, 0);
                startButton.colors = colors;
                gameOverText.text = "";
                yield break;
            }
            animationCounter += Time.deltaTime;
            value = Mathf.Abs(Mathf.Sin(animationCounter * buttonBlinkSpeed) * 0.5f);
            colors.normalColor = Color.HSVToRGB(hue, saturation, value);
            startButton.colors = colors;
            yield return null;
        }
    }

    public void OnGameOver(int win) {
        // Debug.Log("OnGameOver: " + win);
        if (win == -1) {
            gameOverText.text = "DRAW";
        } else if (win == 0) {
            gameOverText.text = "PLAYER 1 WINS";
            p1Score++;
            p1WinsText.text = "wins " + p1Score;
        } else {
            gameOverText.text = "PLAYER 2 WINS";
            p2Score++;
            p2WinsText.text = "wins " + p2Score;
        }

        OnShowClicked();
        startButton.interactable = true;
        StartCoroutine(StartButtonBlinkCoroutine());
    }
}
