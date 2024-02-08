using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObj/Sound/SoundManager", fileName = "SoundManager")]
    public class SoundData : ScriptableObject
    {
         [Tooltip("모든 오디오 클립은 여기에 넣으면 됨.")]
         [SerializeField] List<AudioClip> _playlist = new List<AudioClip>();

        public List<AudioClip> Playlist => _playlist;
    }
