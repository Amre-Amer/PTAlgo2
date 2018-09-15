using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PTAlgo : MonoBehaviour {
    bool ynDown = false;
    Vector3 curPos;
    string curUid;
    GameObject goPT;
    GameObject goSP;
    List<SPListItem> spList;

	// Use this for initialization
	void Start () {
        spList = new List<SPListItem>();
        //
        goPT = GameObject.CreatePrimitive(PrimitiveType.Cube);
        goPT.transform.position = new Vector3(0, .84f, 0);
        goPT.transform.localScale = new Vector3(18.9f, 10.1f, .63f);
        goPT.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture2D>("pt_layout");
        //
        //
        curUid = "abc";
        //
        InvokeRepeating("HeartBeat", 1, 1);
    }
	
    void HeartBeat() {
        ShowSpList();
    }
    
    void AddSpListItem() {
        SPListItem spItem = new SPListItem(curUid, curPos);
        goSP = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        goSP.transform.localScale = new Vector3(.5f, .5f, .5f);
        goSP.transform.Rotate(90, 0, 0);
        goSP.GetComponent<Renderer>().material.color = Color.black;
        goSP.transform.position = curPos;
        spList.Add(spItem);
    }

    void AddTrail()
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.localScale = new Vector3(.5f, .5f, .5f);
        go.transform.Rotate(90, 0, 0);
        go.GetComponent<Renderer>().material.color = Color.black;
        go.transform.position = curPos;
        Destroy(go, 1.5f);
    }

    void MoveListItem() {
        for (int n = 0; n < spList.Count; n++)
        {
            SPListItem spItem = spList[n];
            if (spItem.uid == curUid)
            {
                if (goSP != null)
                {
                    spItem.pos = curPos;
                    goSP.transform.position = curPos;
                    return;
                }
            }
        }
    }

    void DeleteListItem() {
        for (int n = 0; n < spList.Count; n++) {
            SPListItem spItem = spList[n];
            if (spItem.uid == curUid) {
                if (goSP != null) {
                    DestroyImmediate(goSP);
                }
                spList.Remove(spItem);
                return;
            }
        }
    }

    void ShowSpList() {
        Debug.Log("SpList.....................\n");
        foreach(SPListItem spItem in spList) {
            Debug.Log(spItem.uid + ":" + spItem.pos.x + ", " + spItem.pos.y + "\n");
        }
        Debug.Log("\n");
    }

	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject == goPT)
            {
                curPos = hit.point;
            }
        }
        if (Input.GetMouseButtonDown(0)) {
            AddSpListItem();
            ynDown = true;
            //Debug.Log("touch down\n");
        }
        if (Input.GetMouseButtonUp(0))
        {
            DeleteListItem();
            ynDown = false;
            //Debug.Log("touch up\n");
        }
        if (ynDown == true) {
            MoveListItem();
            //Debug.Log("touch drag\n");
        }
        AddTrail();
    }

    public struct SPListItem
    {
        public Vector3 pos;
        public string uid;
        public SPListItem(string uid0, Vector3 pos0) {
            pos = pos0;
            uid = uid0;
        }
    }
}
