using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private static Controller m_instance;
    public static Controller Instance 
    {
        get 
        {
            if(m_instance == null)
            {
                var ctrl = Instantiate(Resources.Load("Prefabs/Controller")) as GameObject;
                m_instance = ctrl.GetComponent<Controller>();
            }

            return m_instance;
        }
    }

    [SerializeField] private int m_fieldSize;
    public int FieldSize 
    {
        get { return m_fieldSize; }
        set { m_fieldSize = value; }
    }
    [SerializeField] private int m_emptySquares;
    [SerializeField] private int m_tokenTypes;
    public int TokenTypes 
    {
        get { return m_tokenTypes; }
        set { m_tokenTypes = value; }
    }
    [SerializeField] private Color[] m_tokenColors;
    public Color[] TokenColors 
    {
        get { return m_tokenColors; }
        set { m_tokenColors = value; }
    }

    private List<List<Token>> m_tokensByTypes;

    public List<List<Token>> TokensByTypes 
    {
        get { return m_tokensByTypes; }
        set { m_tokensByTypes = value; }
    }
    // Start is called before the first frame update
    void Awake()
    {
        if(m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            if(m_instance != this)
                Destroy(this.gameObject);
        }

        m_tokenColors = MakeColor(m_tokenTypes);

        TokensByTypes = new List<List<Token>>();

        for(int i = 0; i < TokenTypes; i++)
        {
            TokensByTypes.Add(new List<Token>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Color[] MakeColor(int count)
    {
        Color[] result = new Color[count];
        float colorStep = 1f / (count + 1);
        float hue = 0f;
        float saturation = .5f;
        float value = 1f;

        for(int i = 0; i < count; i++)
        {
            float newHue = hue + (colorStep * i);

            result[i] = Color.HSVToRGB(newHue, saturation, value);
        }

        return result;
    }

    public void TurnDone()
    {
        if(IsAllTokensConnected())
        {
            Debug.Log("Win!");
        }
        else
        {
            Debug.Log("Continue...");
        }
    }

    public bool IsAllTokensConnected()
    {
        for(int i = 0; i < TokensByTypes.Count; i++)
        {
            if(IsTokensConnected(TokensByTypes[i]) == false)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsTokensConnected(List<Token> tokens)
    {
        if(tokens.Count == 0)
        {
            return true;
        }

        List<Token> connectedTokens = new List<Token>();
        connectedTokens.Add(tokens[0]);

        bool moved = true;

        while(moved)
        {
            moved = false;

            for(int i = 0; i < connectedTokens.Count; i++)
            {
                for(int j = 0; j < tokens.Count; j++)
                {
                    if(IsTokenNear(tokens[j], connectedTokens[i]))
                    {
                        if(connectedTokens.Contains(tokens[j]) == false)
                        {
                            connectedTokens.Add(tokens[j]);
                            moved = true;
                        }
                    }
                }
            }
        }

        if(tokens.Count == connectedTokens.Count)
        {
            return true;
        }

        return false;
    }

    private bool IsTokenNear(Token first, Token second)
    {
        if((int)first.transform.position.x == (int)second.transform.position.x + 1 || (int)first.transform.position.x == (int)second.transform.position.x - 1)
        {
            if((int)first.transform.position.y == (int)second.transform.position.y)
            {
                return true;
            }
        }

        if((int)first.transform.position.y == (int)second.transform.position.y + 1 || (int)first.transform.position.y == (int)second.transform.position.y - 1)
        {
            if((int)first.transform.position.x == (int)second.transform.position.x)
            {
                return true;
            }
        }

        return false;
    }
}
