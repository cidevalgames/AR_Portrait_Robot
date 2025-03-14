using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MyTargetsManager : MonoBehaviour
{
    public static MyTargetsManager Instance;

    public MyImageTarget CurrentActiveTarget 
    { 
        get { return currentActiveTarget; } 
        private set 
        { 
            DisableEveryTarget();

            if (value != currentActiveTarget)
            {
                currentActiveTarget = value;

                if (value != null)
                {
                    CurrentActiveTarget.ShowImage();
                }
            }
            else
            {
                currentActiveTarget = null;
            }
        } 
    }
    public List<MyImageTarget> Targets { get { return targets; } private set { targets = value; } }

    [SerializeField] private MyImageTarget currentActiveTarget;
    [SerializeField] private List<MyImageTarget> targets = new List<MyImageTarget>();

    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
        }
        else
        {
            Debug.LogError("Il y a plus d'une instance de MyTargetsManager dans cette scène !");
        }

        Initialize();
    }

    public void Initialize()
    {
        targets.Clear();
        targets = FindObjectsByType<MyImageTarget>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
    }

    public void ChangeCurrentTarget(MyImageTarget target)
    {
        CurrentActiveTarget = target;
    }

    private void DisableEveryTarget()
    {
        foreach (var t in Targets)
        {
            t.DisableImage();
        }
    }
}
