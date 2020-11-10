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

    private Field m_field;
    public Field field
    {
        get { return m_field; }
        set { m_field = value; }
    }

    [SerializeField] private LevelParameters m_level;
    public LevelParameters Level 
    {
        get { return m_level; }
    }

    private int m_currentLevel;
    public int CurrentLevel 
    {
        get { return m_currentLevel; }
        set { m_currentLevel = value; }
    }

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

        m_tokenColors = MakeColor(Level.TokenTypes);

        TokensByTypes = new List<List<Token>>();

        for(int i = 0; i < Level.TokenTypes; i++)
        {
            TokensByTypes.Add(new List<Token>());
        }

        InitializeLevel();
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
            m_currentLevel++;
            Destroy(m_field.gameObject);
            InitializeLevel();
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

    public void InitializeLevel()
    {
        m_level = new LevelParameters(m_currentLevel);

        TokenColors = MakeColor(Level.TokenTypes);

        TokensByTypes = new List<List<Token>>();

        for(int i = 0; i < Level.TokenTypes; i++)
        {
            TokensByTypes.Add(new List<Token>());
        }

        m_field = Field.Create(Level.FieldSize, Level.FreeSpace);
    }
}