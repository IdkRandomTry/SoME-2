using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomGraph : MonoBehaviour
{
    public GameObject inputField;

    void Start()
    {
        var x = inputField.GetComponent<InputField>();
        x.onEndEdit.AddListener(Enter);
    }

    private void Enter(string ping)
    {
        Debug.Log(ping);        
    }
}
