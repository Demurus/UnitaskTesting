using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class TextureDownloader
{
    private const int AWAIT_TIME_MILLISEC = 50;

    public async Task<Texture2D> DownloadTextureAsyncViaTask(string uri, CancellationToken cancellationToken)
    {
        using var request = UnityWebRequestTexture.GetTexture(uri);
        cancellationToken.ThrowIfCancellationRequested();
        using var requestResult = await request.SendWebRequest();
        
        if (requestResult.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"unable to load texture: {request.error}");
            return null;
        }
        var downloadedTexture = DownloadHandlerTexture.GetContent(requestResult);
        await Task.Delay(AWAIT_TIME_MILLISEC, cancellationToken);
        return downloadedTexture;
    }
    
    public async UniTask<Texture2D> DownloadTextureAsyncViaUniTask(string uri, CancellationToken cancellationToken)
    {
        using var request = UnityWebRequestTexture.GetTexture(uri);
        using var requestResult = await request.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);
        
        if (requestResult.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"unable to load texture: {request.error}");
            return null;
        }
        var downloadedTexture = DownloadHandlerTexture.GetContent(requestResult);
        await UniTask.Delay(AWAIT_TIME_MILLISEC, cancellationToken: cancellationToken);
        return downloadedTexture;
    }
}