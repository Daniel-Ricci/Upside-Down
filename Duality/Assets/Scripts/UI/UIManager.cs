using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image _crystalImage;
    [SerializeField] private TextMeshProUGUI _crystalCountText;
    [SerializeField] private TextMeshProUGUI _endLevelCrystalCountText;
    [SerializeField] private Image _endLevelPanel;
    [SerializeField] private Slider _fuelSlider;
    [SerializeField] private Image _fuel;

    private int _numCrystals = 0;

    private void Awake()
    {
        _fuel.fillAmount = 1.0f;
        _endLevelCrystalCountText.text = string.Format("{0} / {1}", _numCrystals, LevelManager.Instance.numCrystals);
    }

    public void AddCrystal()
    {
        _numCrystals++;
        _crystalCountText.text = string.Format("x {0}", _numCrystals);
        _endLevelCrystalCountText.text = string.Format("{0} / {1}", _numCrystals, LevelManager.Instance.numCrystals);
    }

    public void UpdateFuel(float value)
    {
        _fuel.fillAmount = value;
    }

    public void OnEndLevel()
    {
        _crystalImage.gameObject.SetActive(false);
        _crystalCountText.gameObject.SetActive(false);
        _fuelSlider.gameObject.SetActive(false);
        _endLevelPanel.gameObject.SetActive(true);
    }
}
