using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class StoryBlock {
    public string id;
    public string text;
    public string weiter;
    public string start;
    public string person;
}

[Serializable]
public class StoryData {
    public List<StoryBlock> story_blocks;
}
