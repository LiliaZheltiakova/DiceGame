using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Token : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Camera m_camera;
    private Vector3 m_pointerPositionBeforeDrag;
    private Vector3 m_positionBeforeDrag;
    private int[] m_dragSpace;
    private int m_tokenType;
    // Start is called before the first frame update
    void Start()
    {
        m_camera = Camera.main;
        AlignOnGrid();
        m_tokenType = UnityEngine.Random.Range(0, Controller.Instance.Level.TokenTypes);
        Material myMaterial = gameObject.GetComponent<Renderer>().material;
        myMaterial.SetColor("_Color", Controller.Instance.TokenColors[m_tokenType]);
        Controller.Instance.TokensByTypes[m_tokenType].Add(this);
        this.transform.SetParent(Controller.Instance.field.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_pointerPositionBeforeDrag = m_camera.ScreenToWorldPoint(eventData.position);
        m_positionBeforeDrag = this.transform.position;

        GetDragSpace();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mouseWorldPosition = m_camera.ScreenToWorldPoint(eventData.position);
        //mouse offset relative to the point where we drag the dice
        Vector3 totalDrag = mouseWorldPosition - m_pointerPositionBeforeDrag;

        if(Mathf.Abs(totalDrag.x) > Mathf.Abs(totalDrag.y)) //check for the movement direction
        {
            //movement in empty raws only
            float posX = Mathf.Clamp(mouseWorldPosition.x, m_pointerPositionBeforeDrag.x - m_dragSpace[1], m_positionBeforeDrag.x + m_dragSpace[0]);
            //horizontal movement
            this.transform.position = new Vector3(posX, m_pointerPositionBeforeDrag.y, this.transform.position.z);
        }
        else
        {
            //movement in empty columns only
            float posY = Mathf.Clamp(mouseWorldPosition.y, m_pointerPositionBeforeDrag.y - m_dragSpace[3], m_positionBeforeDrag.y + m_dragSpace[2]); 
            //vertical movement
            this.transform.position = new Vector3(m_pointerPositionBeforeDrag.x, posY, this.transform.position.z);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        AlignOnGrid();
        Controller.Instance.TurnDone();
    }

    private void AlignOnGrid()
    {
        Vector3 alignedPosition = this.transform.position;
        alignedPosition.x = Mathf.Round(this.transform.position.x);
        alignedPosition.y = Mathf.Round(this.transform.position.y);
        this.transform.position = alignedPosition;
    }

    private void GetDragSpace()
    {
        int OddEven = 1;
        if(Controller.Instance.Level.FieldSize % 2 != 0)
        {
            OddEven = 0;
        }
        m_dragSpace = new int[] {0, 0, 0, 0};
        int halfField = (Controller.Instance.Level.FieldSize - 1) / 2;

        m_dragSpace[0] = CheckSpace(Vector2.right);

        if(m_dragSpace[0] == -1)
        {
            m_dragSpace[0] = halfField + (int)this.transform.position.x + OddEven;
        }

        m_dragSpace[1] = CheckSpace(Vector2.left);

        if(m_dragSpace[1] == -1)
        {
            m_dragSpace[1] = halfField - (int)this.transform.position.x;
        }

        m_dragSpace[2] = CheckSpace(Vector2.up);

        if(m_dragSpace[2] == -1)
        {
            m_dragSpace[2] = halfField - (int)this.transform.position.y + OddEven;
        }

        m_dragSpace[3] = CheckSpace(Vector2.down);

        if(m_dragSpace[3] == -1)
        {
            m_dragSpace[3] = halfField + (int)this.transform.position.y;
        }
    }

    private int CheckSpace(Vector2 direction)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.transform.position, direction);

        for(int i = 0; i < hits.Length; i++)
        {
            if(hits[i].collider.gameObject != gameObject)
            {
                return Mathf.FloorToInt(hits[i].distance);
            }
        }

        return -1;
    }
}
