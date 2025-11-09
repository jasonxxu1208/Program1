using UnityEngine;

public class OutOfBoards : MonoBehaviour
{
    [Header("Scene Bounds")]
    public float xLimit = 20f;  // slightly bigger than border
    public float yLimit = 16f;

    void Update()
    {
        Vector3 pos = transform.position;

        // deactivate when outside the borders
        if (Mathf.Abs(pos.x) > xLimit || Mathf.Abs(pos.y) > yLimit)
        {
            gameObject.SetActive(false);
        }
    }

    // optional: visualize area in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(xLimit * 2, yLimit * 2, 0));
    }
}

