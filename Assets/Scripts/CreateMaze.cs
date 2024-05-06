using UnityEngine;

public class CreateMaze : MonoBehaviour
{
    public Transform mazePivotPoint;
    public GameObject wall;
    public Transform wallParent;

    public void Create(int size, float scale, bool[,] hwalls, bool[,] vwalls)
    {
        for (int i = 0; i < size + 1; i++)
        {
            for (int j = 0; j < size + 1; j++)
            {
                if (hwalls[i, j] && i < size)
                {
                    var prefab = Instantiate(wall, new Vector3(i * scale, j * scale - scale / 2, 0) + mazePivotPoint.position, Quaternion.identity, wallParent);
                    prefab.transform.localScale = new Vector3(scale, scale / 4, scale / 4);
                    prefab.name = "hwall_{" + i.ToString() + "," + j.ToString() + "}";
                }
                if (vwalls[i, j] && j < size)
                {
                    var prefab = Instantiate(wall, new Vector3(i * scale - scale / 2, j * scale, 0) + mazePivotPoint.position, Quaternion.identity, wallParent);
                    prefab.transform.localScale = new Vector3(scale / 4, scale, scale / 4);
                    prefab.name = "vwall_{" + i.ToString() + "," + j.ToString() + "}";
                }
            }
        }
    }
}
