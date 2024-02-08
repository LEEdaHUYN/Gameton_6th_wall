using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObj/Sound/SoundManager", fileName = "SoundManager")]
    public class SoundData : ScriptableObject
    {
         [Tooltip("��� ����� Ŭ���� ���⿡ ������ ��.")]
         [SerializeField] List<AudioClip> _playlist = new List<AudioClip>();

        public List<AudioClip> Playlist => _playlist;
    }
