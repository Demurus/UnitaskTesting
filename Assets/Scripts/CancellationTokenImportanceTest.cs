using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class CancellationTokenImportanceTest : TestBase
    {
        private readonly TextureDownloader _textureDownloader = new();
        
        [SerializeField] private Button _cancelOperation;
        [SerializeField] private MeshRenderer _cubeRenderer;
        
        [CanBeNull] private CancellationTokenSource _cts;
        
        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        protected override void Awake()
        {
            base.Awake();
            _cancelOperation.onClick.AddListener(CancelOperation);
        }
        
        private async UniTask DownloadAndApplyTexturesAsync(List<string> uriList, IProgress<float> progress, CancellationToken cancellationToken)
        {
            for (var i = 0; i < uriList.Count; i++)
            {
                try
                {
                    var texture2D = await _textureDownloader.DownloadTextureAsyncViaUniTask(uriList[i], cancellationToken);
                    var currentItemNumber = i + 1;
                    progress.Report((float)currentItemNumber / uriList.Count);
                    _cubeRenderer.material.mainTexture = texture2D;
                }
                catch (OperationCanceledException e)
                {
                    Debug.LogWarning(e);
                    _progressImage.fillAmount = 0;
                    break;
                }
            }
            
            SetUiActive(true);
        }

        protected override void OnFirstButtonClicked()
        {
            StartWithCancellationToken();
        }

        protected override void OnSecondButtonClicked()
        {
            StartWithoutCancellationToken();
        }

        private void StartWithoutCancellationToken()
        {
            SetUiActive(false);
            var progress = InitializeProgressBar();
            DownloadAndApplyTexturesAsync(UriHolder.UrisList, progress, CancellationToken.None).Forget();
        }

        private void StartWithCancellationToken()
        {
            SetUiActive(false);
            var progress = InitializeProgressBar();
            _cts = new CancellationTokenSource();
            DownloadAndApplyTexturesAsync(UriHolder.UrisList, progress, _cts.Token).Forget();
        }

        private void CancelOperation()
        {
            _cts?.Cancel();
        }
    }
}