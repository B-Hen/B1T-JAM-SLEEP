using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog 
{
    public string name;

    [TextArea(5, 10)]
    public List<string> sentences;
}
