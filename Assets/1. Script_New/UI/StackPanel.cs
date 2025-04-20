using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackPanel : MonoBehaviour
{
    Stack<GameObject> stackPanels = new();

    public void OnPopAndPush(GameObject go)
    {
        if (stackPanels.Count != 0)
        {
            stackPanels.Pop().SetActive(false);
        }
        stackPanels.Push(go);
        go.SetActive(true);
    }

    public void OnPeekAndPush(GameObject go)
    {
        if(stackPanels.Count != 0)
        {
            stackPanels.Peek().SetActive(false);
        }
        stackPanels.Push(go);
        go.SetActive(true);
    }

    public void OnClosePanel()
    {
        if(stackPanels.Count != 0)
        {
            stackPanels.Pop().SetActive(false);
            stackPanels.Peek().SetActive(true);
        }
    }

    public void OnPopAll()
    {
        for (int i = 0; i < stackPanels.Count; i++)
        {
            stackPanels.Pop().SetActive(false);
        }
    }
}
