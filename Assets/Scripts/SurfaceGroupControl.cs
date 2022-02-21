using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceGroupControl : MonoBehaviour
{
    // maintain a list of surfaces
    public List<Surface_single> group = new List<Surface_single>();
    private float spreadThreashold;

    void Start()
    {
        foreach(Transform child in transform)
        {
            group.Add(child.gameObject.GetComponent<Surface_single>());
        }
        spreadThreashold = group[0].unintended_threshold;
    }

    // Update is called once per frame
    void Update()
    {
        // then for each infected surface, check if it's infected time has reached the threashold
        for(int i = 0; i < group.Count; ++i)
        {
            if(group[i].time_hasBeenInfected >= spreadThreashold - 0.1)
            {
                // spread the virus to near by surfaces
                // mind the edge case: leftmost surface only spread to right, rightmost surface only spread to left
                // surfaces in the middle spread to both sides
                if(i == 0)
                {
                    if(!group[i + 1].isInfected)
                    {
                        group[i + 1].isInfected = true;
                    }
                }
                else if(i == group.Count - 1)
                {
                    if (!group[i - 1].isInfected)
                    {
                        group[i - 1].isInfected = true;
                    }
                }
                else
                {
                    if (!group[i - 1].isInfected)
                    {
                        group[i - 1].isInfected = true;
                    }

                    if (!group[i + 1].isInfected)
                    {
                        group[i + 1].isInfected = true;
                    }

                }
            }
        }
    }


}
