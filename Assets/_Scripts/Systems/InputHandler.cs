using UnityEngine;

public class InputHandler : MonoBehaviour {

    public bool isActive = false;

    void Awake() => isActive = false;

    void Update() {
        if (!isActive) return;


#if UNITY_EDITOR || UNITY_WIN_STANDALONE
        if (Input.GetMouseButtonDown(0)) {
            Vector3 inputPosition = Input.mousePosition;

            SendPosition(inputPosition);
        }
#else
        if (Input.touchCount > 0) {
            Vector3 inputPosition = Input.GetTouch(0).position;
            Debug.Log($"inputPosition: {inputPosition}");

            SendPosition(inputPosition);
        }
#endif

    }

    private void SendPosition(Vector3 pos) {

        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000)) {
            if (!GridManager.Instance.SelectCell((int)hit.transform.position.x, (int) -hit.transform.position.z)) return; //If there is no empty cell on the grid,


            isActive = false;
            GameManager.Instance.ChangeState(GameManager.GameState.Running);
        }



    }
}
