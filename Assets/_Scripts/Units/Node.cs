using System.Collections;
using UnityEngine;

public class Node : MonoBehaviour {

    public GridType nodeType { get; private set; }

    [SerializeField]
    private int _rollSpeed = 1;

    public Node(GridType type) {
        this.nodeType = type;
    }

    private void Awake() {
        bool tween = false;
        
        // Ah be abi
        if (transform.CompareTag("Player")) {
            nodeType = GridType.Player;
        } else if (transform.CompareTag("Enemy1")) {
            nodeType = GridType.Enemy1;
        } else if (transform.CompareTag("Enemy2")) {
            nodeType = GridType.Enemy2;
        } else if (transform.CompareTag("Enemy3")) {
            nodeType = GridType.Enemy3;
        } else if (transform.CompareTag("Filled")) {
            nodeType = GridType.Filled;
            tween = true;
        } else if (transform.CompareTag("Empty")) {
            nodeType = GridType.Empty;
            tween = true;
        }
        if (!tween)
            StartCoroutine(ScaleUpDown(1f, 1.5f));

    }

    IEnumerator ScaleUpDown(float upScale, float duration) {
        Vector3 initialScale = transform.localScale;
        while (true) {

            for (float time = 0; time < duration * 2; time += Time.deltaTime) {
                float progress = Mathf.PingPong(time, duration) / duration;
                transform.localScale = Vector3.Lerp(initialScale, new Vector3(1, upScale, 1), progress);
                yield return null;
            }
            transform.localScale = initialScale;
            yield return null;
        }
    }

    public void StartRoll() {

        int x = (int)transform.position.x;
        int z = (int)transform.position.z;

        bool isRolled = false;
        if (GridManager.Instance.IsCellEmpty(x, -(z + 1))) { // Up
            var newNodeTransform = GridManager.Instance.AddNewCell(nodeType, x, z, x, z + 1);
            Assemble(newNodeTransform, Vector3.forward);
            isRolled = true;
        }

        if (GridManager.Instance.IsCellEmpty(x, -(z - 1))) { // Down
            var newNodeTransform = GridManager.Instance.AddNewCell(nodeType, x, z, x, z - 1);
            Assemble(newNodeTransform, Vector3.back);
            isRolled = true;
        }

        if (GridManager.Instance.IsCellEmpty(x + 1, -z)) { // Right
            var newNodeTransform = GridManager.Instance.AddNewCell(nodeType, x, z, x+1, z);
            Assemble(newNodeTransform, Vector3.right);
            isRolled = true;
        }


        if (GridManager.Instance.IsCellEmpty(x - 1, -z)) { // Left
            var newNodeTransform = GridManager.Instance.AddNewCell(nodeType, x, z, x-1, z);
            Assemble(newNodeTransform, Vector3.left);
            isRolled = true;
        }


        if (!isRolled) {
            GridManager.Instance.CheckComplete();
        }
    }

    private void Assemble(Transform obj, Vector3 dir) {
        var anchor = obj.transform.position + dir * 0.5f;
        var axis = Vector3.Cross(Vector3.up, dir);


        StartCoroutine(Roll(obj, anchor, axis, dir));

    }

    IEnumerator Roll(Transform obj, Vector3 anchor, Vector3 axis, Vector3 dir) {
        AudioSystem.Instance.PlayPop();
        if (nodeType != GridType.Player)
            yield return new WaitForSeconds(0.01f);
        for (int i = 0; i < (180 / _rollSpeed); i++) {
            obj.transform.RotateAround(anchor, axis, _rollSpeed);
            yield return new WaitForSeconds(0.01f);
        }


        obj.transform.rotation = Quaternion.identity;
        obj.transform.position = transform.position + dir;
        obj.GetComponent<Node>().StartRoll();
    }
}
