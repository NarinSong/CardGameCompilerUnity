using System.Collections.Generic;
using UnityEngine;

public class editorBlockManager : MonoBehaviour
{
    public List<block> blockList;
    public List<block> variableBlockList;
    public List<block> labelBlockList;
    public List<block> locationBlockList;
    public Transform blockParent;
    public GameObject mainBlockPrefab;
    public GameObject variableBlockPrefab;
    public GameObject buttonBlockPrefab;
    public GameObject pileBlockPrefab;
    public GameObject counterBlockPrefab;
    public GameObject stepBlockPrefab;
    public GameObject phaseBlockPrefab;
    public GameObject actionRoleBlockPrefab;
    public GameObject playerRoleBlockPrefab;
    public GameObject locationBlockPrefab;
    public float y;

    //all blocks are literal except main and location

    public void setBlockList(List<block> bL)
    {
        blockList = bL;
        drawBlocks();
    }

    public void drawBlocks()
    {
        y = 0;
        foreach(block b in blockList)
        {
            GameObject newBlock = Instantiate(mainBlockPrefab, blockParent.position + new Vector3(0,y,0), Quaternion.identity, blockParent);
            newBlock.GetComponent<blockController>().Init(b.name,b.displayName,b.returnType,b.arguments);
            y -= 0.75f;
        }
    }
}
