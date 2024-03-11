using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Story", menuName = "Story")]
public class Story : ScriptableObject
{
    public string title;
    public List<StoryLine> stories;

    [System.Serializable]
    public class StoryLine
    {
        public string subtitle;
        public AudioClip audioClip;
        public Sprite characterSprite;
    } 
}
