using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class TaskVsUniTaskTest : TestBase
    {
        private readonly TextureDownloader _textureDownloader = new();
        
        [SerializeField] private TextMeshProUGUI _memoryAllocatedText;
        
        private async Task DownloadTexturesViaTasksAsync(List<string> uriList, IProgress<float> progress)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
          
            var initialMemory = GC.GetTotalMemory(false);

            for (int i = 0; i < uriList.Count; i++)
            {
                await _textureDownloader.DownloadTextureAsyncViaTask(uriList[i], CancellationToken.None);
                var currentItemNumber = i + 1;
                progress.Report((float)currentItemNumber / uriList.Count);
            }
            
            var finalMemory = GC.GetTotalMemory(false);
            PrintMemoryAllocatedText(initialMemory, finalMemory);
            SetUiActive(true);
        }

        private async UniTask DownloadTexturesViaUniTasksAsync(List<string> uriList, IProgress<float> progress)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            var initialMemory = GC.GetTotalMemory(false);

            for (var i = 0; i < uriList.Count; i++)
            {
                await _textureDownloader.DownloadTextureAsyncViaUniTask(uriList[i], CancellationToken.None);
                var currentItemNumber = i + 1;
                progress.Report((float)currentItemNumber / uriList.Count);
            }

            var finalMemory = GC.GetTotalMemory(false);
            PrintMemoryAllocatedText(initialMemory, finalMemory);
            SetUiActive(true);
        }

        private void PrintMemoryAllocatedText(long initialMemory, long finalMemory)
        {
            var valueInKb = (finalMemory - initialMemory) * 0.001;
            _memoryAllocatedText.text = $"Memory allocated: {valueInKb} KBytes";
        }
        
        protected override void OnFirstButtonClicked()
        {
            SetUiActive(false);
            var progress = InitializeProgressBar();
            DownloadTexturesViaTasksAsync(UriHolder.UrisList, progress);
        }

        protected override void OnSecondButtonClicked()
        {
            SetUiActive(false);
            var progress = InitializeProgressBar();
            DownloadTexturesViaUniTasksAsync(UriHolder.UrisList, progress);
        }
    }
}