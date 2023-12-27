using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class SoundManager : BaseManager
{
    private AudioClip[] soundClips;

    public override void OnInit()
    {
        soundClips = Resources.LoadAll<AudioClip>("Sound");
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="soundType"></param>
    /// <param name="volume"></param>
    public void PlaySound(SoundType soundType, float volume = 1)
    {
        bool isFind = soundClips.ToList().Where(x => x.name == soundType.ToString()).Count() > 0;
        if (isFind)
        {
            GameObject obj = new GameObject();
            AudioSource source = obj.AddComponent<AudioSource>();
            source.clip = soundClips.ToList().Where(x => x.name == soundType.ToString()).First();
            source.volume = volume;
            source.Play();

            RemoveSound(source);
        }
        else
        {
            Debug.LogError("沒有找到音效:" + soundType.ToString());
        }
    }

    /// <summary>
    /// 移除音效
    /// </summary>
    /// <param name="source"></param>
    private async void RemoveSound(AudioSource source)
    {
        await Task.Delay((int)(source.clip.length * 1000));
        GameObject.Destroy(source.gameObject);
    }
}
