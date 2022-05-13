using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newField : MonoBehaviour
{
    public Field[] fields;
    public GameObject landPrefab;
    public GameObject landParents;

    public List<Field> actualLands;
    public List<Mesh> meshList; 
    void Start()
    {
        landPrefab = (GameObject)Resources.Load("newField");

        actualLands = new List<Field>();

        ReadLandJson();

    }
    public void CreateLand() 
    {
        List<Land> cLands = new List<Land>();
        cLands = FieldManager.instance.allLandList;
        for (int i = 0; i < cLands.Count; i++)
        {
            if (cLands == null || cLands.Count == 0)
            {
                return;
            }
            Vector3 pos = new Vector3(cLands[i].landPos.x, cLands[i].landPos.y, cLands[i].landPos.z);
            GameObject go = Instantiate(landPrefab, pos,landPrefab.transform.rotation );
            go.transform.parent = landParents.transform;
            Field field = go.GetComponent<Field>();
            if (field == null) 
            {
                continue;
            }
            field.SetLandData(cLands[i]);
        }
    }
    public void ReadLandJson()
    {
        JsonManager.instance.ReadLandData();
        CreateLand();
        fields = GetComponentsInChildren<Field>();
        for (int i = 0; i < fields.Length; i++)
        {
            foreach (var item in fields)
            {
                actualLands.Add(item);
            }
        }

    }
    public void SaveLandJson()
    {
        JsonManager.instance.SaveLandData();
    }

}
