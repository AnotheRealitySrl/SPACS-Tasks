using SPACS.Tasks;

using UnityEngine;

public class SetTaskStepSetterIndex : MonoBehaviour
{
    public int index;

    private void Start()
    {
        var stepSetter = GetComponent<TaskStepSetter>();
        stepSetter.SetStepIndex(index);
        stepSetter.InitDefaultStep();
    }
}
