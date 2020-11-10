using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Field Create(int size, int emptySquares)
    {
        Vector3 fieldPosition = Vector3.zero;

        if(size % 2 == 0)
        {
            fieldPosition = new Vector3(.5f, .5f, 0f);
        }

        var field = Instantiate(Resources.Load("Prefabs/Field") as GameObject, fieldPosition, Quaternion.identity);

        Vector3 scale = Vector3.one * size;
        scale.z = 1f;

        field.transform.localScale = scale;

        Vector3 cameraPosition = field.transform.position;
        cameraPosition.z = -10f;

        Camera.main.transform.position = cameraPosition;

        Camera.main.orthographicSize = (float)size * .7f;

        field.GetComponent<Renderer>().material.mainTextureScale = Vector2.one * size;

        field.gameObject.GetComponent<Field>().CreateTokens(size, emptySquares);

        return field.gameObject.GetComponent<Field>();;
    }

    private void CreateTokens(int size, int emptySquares)
    {
        var offset = (size - 1f) / 2f;

        var startPosition = new Vector3(this.transform.position.x - offset, this.transform.position.y - offset, this.transform.position.z - 2f);

        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                if((i * size) + j >= (size * size) - emptySquares)
                {
                    emptySquares--;
                }

                else
                {
                    if(emptySquares == 0 || Random.Range(0, size * size / emptySquares) > 0)
                    {
                        Token newToken = Instantiate(Resources.Load("Prefabs/Dice"), new Vector3(startPosition.x + i, startPosition.y + j, startPosition.z), Quaternion.identity) as Token;
                    }

                    else
                    {
                        emptySquares--;
                    }
                }
            }
        }
    }
}
