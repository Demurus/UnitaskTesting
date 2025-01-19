using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class TestBase : MonoBehaviour
    {
        [SerializeField] protected Image _progressImage;
        [SerializeField] private Button _firstButton;
        [SerializeField] private Button _secondButton;
        [SerializeField] private TextMeshProUGUI _progressText;
        
        protected virtual void Awake()
        {
            _firstButton.onClick.AddListener(OnFirstButtonClicked);
            _secondButton.onClick.AddListener(OnSecondButtonClicked);
        }

        protected virtual void OnSecondButtonClicked()
        {
            
        }

        protected virtual void OnFirstButtonClicked()
        {
            
        }
        
        protected Progress<float> InitializeProgressBar()
        {
            _progressImage.fillAmount = 0;
            var progress = new Progress<float>(UpdateProgress);
            return progress;
        }
        
        protected void SetUiActive(bool setActive)
        {
            _firstButton.interactable = setActive;
            _secondButton.interactable = setActive;
            _progressText.text = !setActive ? "Processing..." : string.Empty;
        }
        
        private void UpdateProgress(float progress)
        {
            _progressImage.fillAmount = progress;
        }
    }
}