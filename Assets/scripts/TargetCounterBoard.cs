using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCounterBoard : MonoBehaviour
{
    public TMPro.TextMeshProUGUI textField;

    private int normalTargets = 0;
    private int hiddenTargets = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void targetHit(GameObject obj) {
        if (obj.layer == LayerMask.NameToLayer("OnlyVisibleForLens")) {
            hiddenTargets++;
        } else {
            normalTargets++;
        }
        textField.text = "Targets: " + normalTargets.ToString() + "\nHidden: " + hiddenTargets.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
