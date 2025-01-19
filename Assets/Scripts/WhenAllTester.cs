using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class WhenAllTester : TestBase
    {
        private readonly TextureDownloader _textureDownloader = new();
        private readonly CancellationTokenSource _cts = new();
        
        [SerializeField] private TextMeshProUGUI _timeElapsed;
        
        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
        
        private async UniTask DownloadTexturesUsingCycleAsync(List<string> uriList, IProgress<float> progress,
            CancellationToken cancellationToken)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            for (var i = 0; i < uriList.Count; i++)
            {
                await _textureDownloader.DownloadTextureAsyncViaUniTask(uriList[i], cancellationToken);
                var currentItemNumber = i + 1;
                progress.Report((float)currentItemNumber / uriList.Count);
            }

            stopwatch.Stop();
            SetUiActive(true);
            _timeElapsed.text = $"Milliseconds elapsed: {stopwatch.ElapsedMilliseconds.ToString()}";
        }

        private async UniTask DownloadTexturesUsingWhenAllAsync(List<string> uriList,
            IProgress<float> progress, CancellationToken cancellationToken)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var tasks = new List<UniTask<Texture2D>>();

            for (var i = 0; i < uriList.Count; i++)
            {
                var newTask = _textureDownloader.DownloadTextureAsyncViaUniTask(uriList[i], cancellationToken);
                tasks.Add(newTask);
                var currentItemNumber = i + 1;
                progress.Report((float)currentItemNumber / uriList.Count);
            }

            await UniTask.WhenAll(tasks);
            stopwatch.Stop();
            SetUiActive(true);
            _timeElapsed.text = $"Milliseconds elapsed: {stopwatch.ElapsedMilliseconds.ToString()}";
        }

        protected override void OnFirstButtonClicked()
        {
            AwaitInCycle();
        }

        protected override void OnSecondButtonClicked()
        {
            AwaitInWhenAll();
        }

        private void AwaitInWhenAll()
        {
            var progress = InitializeProgressBar();
            SetUiActive(false);
            DownloadTexturesUsingWhenAllAsync(UriHolder.UrisList, progress, _cts.Token).Forget();
        }

        private void AwaitInCycle()
        {
            var progress = InitializeProgressBar();
            SetUiActive(false);
            DownloadTexturesUsingCycleAsync(UriHolder.UrisList, progress, _cts.Token).Forget();
        }
    }
}