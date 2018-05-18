using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
[InitializeOnLoad]
public class EpicPrefsEditor : EditorWindow
{
    #region PrefDictionaries
    static EpicPrefsEditor window;
    private static bool showWindow = false;
    private static bool subscribed = false;
    public Font passedFont;
    private static string passPhrase,initVector;
    private static Font font;
    private int selected = 0;
    private Texture2D deleteButton, addButton, editButton, checkedButton, uncheckedButton,applyButton,cancelButton,logo,exportButton;
    private static bool stylesInitialized = false;
    public static Color backgroundColor = new Color(51f / 255f, 77f / 255f, 92f / 255f);
    private Vector2 horizontalScroll, verticalScroll;
    private static Texture2D tex,tex2,textFieldBG;
    static Dictionary<string, string> IntDict, FloatDict,
        BoolDict, StringDict, DoubleDict, LongDict,
        ArrayIntDict, ArrayStringDict, ArrayFloatDict,
        ArrayDoubleDict,
        ListStringDict, ListIntDict, ListBoolDict,
        ListFloatDict, ListDoubleDict, ListLongDict,
        DictStringDict, DictIntDict, DictBoolDict,
        DictFloatDict, DictDoubleDict, DictLongDict,
        TransformDict, QuaternionDict,
        Vector2Dict, Vector3Dict, Vector4Dict, ColorDict;
    static Dictionary<string, int> IntDictValues;
    static Dictionary<string, float> FloatDictValues;
    static Dictionary<string, bool> BoolDictValues; 
    static Dictionary<string, string> StringDictValues;
    static Dictionary<string, double> DoubleDictValues;
    static Dictionary<string, long> LongDictValues;
    static Dictionary<string, int[]> ArrayIntDictValues;
    static Dictionary<string, string[]> ArrayStringDictValues;
    static Dictionary<string, float[]> ArrayFloatDictValues;
    static Dictionary<string, double[]> ArrayDoubleDictValues;
    static Dictionary<string, List<string>> ListStringDictValues;
    static Dictionary<string, List<int>> ListIntDictValues;
    static Dictionary<string, List<bool>> ListBoolDictValues;
    static Dictionary<string, List<float>> ListFloatDictValues;
    static Dictionary<string, List<double>> ListDoubleDictValues;
    static Dictionary<string, List<long>> ListLongDictValues;
    static Dictionary<string, Dictionary<string,string>> DictStringDictValues;
    static Dictionary<string, Dictionary<string,int>> DictIntDictValues;
    static Dictionary<string, Dictionary<string,bool>> DictBoolDictValues;
    static Dictionary<string, Dictionary<string,float>> DictFloatDictValues;
    static Dictionary<string, Dictionary<string,double>> DictDoubleDictValues;
    static Dictionary<string, Dictionary<string,long>> DictLongDictValues;
    static Dictionary<string, Transform> TransformDictValues;
    static Dictionary<string, Quaternion> QuaternionDictValues;
    static Dictionary<string, Vector2> Vector2DictValues;
    static Dictionary<string, Vector3> Vector3DictValues;
    static Dictionary<string, Vector4> Vector4DictValues;
    static Dictionary<string, Color> ColorDictValues;
    bool showInt, editInt, showString, editString, addNewValue,
        showFloat, editFloat,editLong,showLong,editBool,
        showBool,editDouble,
        editTransform, editQuaternion, editVector2,
        editVector3, editVector4, 
        showDouble, showColor, editColor,
        showListS,showListI,showListB,
        showListD,showListF,showListL,
        showArrayI,showArrayF, showArrayS, showArrayD,
        showQuaternion,showVector2,
        showVector3, showVector4,
        showDictS, showDictI, showDictB, 
        showDictF, showDictD, showDictL  = false;
    static bool[] showSubDictS, showSubDictI, showSubDictB,
        showSubDictF, showSubDictD, showSubDictL,
        showSubListS, showSubListI, showSubListB,
        showSubListF, showSubListD, showSubListL,
        showSubArrayS, showSubArrayI, showSubArrayD, showSubArrayF,
        showSubQuaternion, showSubVector2,
        showSubVector3, showSubVector4;
    static bool[] editSubDictS, editSubDictI, editSubDictB,
        editSubDictF, editSubDictD, editSubDictL,
        editSubListS, editSubListI, editSubListB,
        editSubArrayS, editSubArrayI, editSubArrayD, editSubArrayF,
        editSubListF, editSubListD, editSubListL, 
        editSubQuaternion, editSubVector2,
        editSubVector3, editSubVector4;
    static Dictionary<string, string> stringEditor,stringStringEditor;
    static Dictionary<string, int> intEditor;
    static Dictionary<string, long> longEditor;
    static Dictionary<string, bool> boolEditor;
    static Dictionary<string, double> doubleEditor;
    static Dictionary<string, float> floatEditor;
    static Dictionary<string, Color> colorEditor;
    static Dictionary<string, string> listsAndArrays,vectorsAndUnityTypes;
    private static GUIStyle line,text,titleStyle,textBox,foldout,toggleStyle;
    private GUIContent dB, eB, aB, cM, uCM,apB,cB,exB;
    private static bool repaint = false;
    private string newName="";
    private string newValue = "";
    private static bool startupInitialized = false;
    #endregion
    static EpicPrefsEditor()
    {
        startupInitialized = false;
    }
    public static void SetupStyles()
    {
        textFieldBG = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        textFieldBG.SetPixel(0, 0, new Color(223f / 255f, 73f / 255f, 73f / 255f));
        textFieldBG.Apply();
        line = new GUIStyle(GUI.skin.box);
        line.border.top = line.border.bottom = 1;
        line.margin.top = line.margin.bottom = 1;
        line.padding.top = line.padding.bottom = 1;
        line.normal.background = textFieldBG;
        text = new GUIStyle(EditorStyles.whiteLabel);
        text.font = font;
        text.normal.textColor = Color.white;
        text.hover.textColor = Color.white;
        text.focused.textColor = Color.white;
        text.active.textColor = Color.white;
        text.alignment = TextAnchor.MiddleLeft;
        titleStyle = new GUIStyle(EditorStyles.boldLabel);
        titleStyle.normal.textColor = Color.white;
        titleStyle.font = font;
        titleStyle.fontStyle = FontStyle.Bold;
        textBox = new GUIStyle(EditorStyles.whiteLabel);
        textBox.normal.textColor = Color.white;
        textBox.hover.textColor = Color.white;
        textBox.focused.textColor = Color.white;
        textBox.active.textColor = Color.white;
        textBox.normal.background = textFieldBG;
        textBox.hover.background = textFieldBG;
        textBox.focused.background = textFieldBG;
        textBox.active.background = textFieldBG;
        textBox.font = font;
        foldout = new GUIStyle(EditorStyles.foldout);
        foldout.normal.textColor = Color.white;
        foldout.hover.textColor = Color.white;
        foldout.focused.textColor = Color.white;
        foldout.active.textColor = Color.white;
        foldout.onNormal.textColor = Color.white;
        foldout.onHover.textColor = Color.white;
        foldout.onFocused.textColor = Color.white;
        foldout.onActive.textColor = Color.white;
        foldout.font = font;
        tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        tex.SetPixel(0, 0, new Color(223f / 255f, 73f / 255f, 73f / 255f));
        tex.Apply();
        tex2 = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        tex2.SetPixel(0, 0, backgroundColor);
        tex2.Apply();

        stylesInitialized = true;
    }
    private static void Subscribe()
    {
        if (!subscribed)
        {
            subscribed = true;
            Serializer.PrefEvent += OnPrefsChanged;
        }
    }
    private static void UnSubscribe()
    {
        if (subscribed)
        {
            subscribed = false;
            Serializer.PrefEvent -= OnPrefsChanged;
        }
    }
    public static void Separator()
    {
        GUILayout.Space(5);
        GUILayout.Box(GUIContent.none, line, GUILayout.ExpandWidth(true), GUILayout.Height(1f));
        GUILayout.Space(5);
    }

   private void Initializer()
    {
        addButton = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/HotTotemAssets/EpicPrefs/Editor/Graphics/add-button.png", typeof(Texture2D));
        editButton = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/HotTotemAssets/EpicPrefs/Editor/Graphics/new-file.png", typeof(Texture2D));
        deleteButton = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/HotTotemAssets/EpicPrefs/Editor/Graphics/removebutton.png", typeof(Texture2D));
        checkedButton = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/HotTotemAssets/EpicPrefs/Editor/Graphics/check-square.png", typeof(Texture2D));
        uncheckedButton = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/HotTotemAssets/EpicPrefs/Editor/Graphics/uncheck-square.png", typeof(Texture2D));
        applyButton = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/HotTotemAssets/EpicPrefs/Editor/Graphics/next-page.png", typeof(Texture2D));
        cancelButton = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/HotTotemAssets/EpicPrefs/Editor/Graphics/close.png", typeof(Texture2D));
        logo = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/HotTotemAssets/EpicPrefs/Editor/Graphics/logo.png", typeof(Texture2D));
        exportButton = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/HotTotemAssets/EpicPrefs/Editor/Graphics/arrows.png", typeof(Texture2D));
        font = (Font)AssetDatabase.LoadAssetAtPath("Assets/HotTotemAssets/EpicPrefs/Editor/Graphics/Calibri.ttf", typeof(Font));
        aB = new GUIContent(GUIContent.none);
        eB = new GUIContent(GUIContent.none);
        dB = new GUIContent(GUIContent.none);
        cB = new GUIContent(GUIContent.none);
        cM = new GUIContent(GUIContent.none);
        uCM = new GUIContent(GUIContent.none);
        apB = new GUIContent(GUIContent.none);
        exB = new GUIContent(GUIContent.none);
        aB.image = addButton;        
        aB.tooltip = "Add a new entry";
        eB.image = editButton;
        eB.tooltip = "Edit entry";
        dB.image = deleteButton;
        dB.tooltip = "Delete entry";
        cM.image = checkedButton;
        uCM.image = uncheckedButton;
        apB.image = applyButton;
        apB.tooltip = "Apply changes";
        exB.image = exportButton;
        exB.tooltip = "Export EpicPrefs to build";
        cB.image = cancelButton;
        cB.tooltip = "Cancel";
        toggleStyle = new GUIStyle(EditorStyles.toggle);
        toggleStyle.normal.textColor = Color.white;
        toggleStyle.hover.textColor = Color.white;
        toggleStyle.focused.textColor = Color.white;
        toggleStyle.active.textColor = Color.white;
        toggleStyle.onNormal.textColor = Color.white;
        toggleStyle.onHover.textColor = Color.white;
        toggleStyle.onFocused.textColor = Color.white;
        toggleStyle.onActive.textColor = Color.white;
        toggleStyle.normal.background = uncheckedButton;
        toggleStyle.onNormal.background = checkedButton;
        toggleStyle.font = font;

    }

    [MenuItem("Window/HotTotemAssets/EpicPrefs/EpicPrefsEditor")]
    static void Init()
    {
        window = (EpicPrefsEditor)EditorWindow.GetWindow(typeof(EpicPrefsEditor));
        showWindow = true;
    }
	[MenuItem("Window/HotTotemAssets/EpicPrefs/Show documentation...")]
    private static void showDocu()
    {
        Application.OpenURL(Application.dataPath + "/HotTotemAssets/EpicPrefs/Documentation/EpicPrefs Documentation.pdf");
    }
    [MenuItem("Window/HotTotemAssets/EpicPrefs/Help...")]
    private static void help()
    {
        Application.OpenURL("mailto:support@hot-totem.com?subject=EpicPrefs Help");
    }

    static void StartUp()
    {
        IntDict = EpicPrefs.GetEditorPrefs("Integer");
        if (IntDict == null)
            IntDict = new Dictionary<string, string>();
        FloatDict = EpicPrefs.GetEditorPrefs("Float");
        if (FloatDict == null)
            FloatDict = new Dictionary<string, string>();
        StringDict = EpicPrefs.GetEditorPrefs("String");
        if (StringDict == null)
            StringDict = new Dictionary<string, string>();
        BoolDict = EpicPrefs.GetEditorPrefs("Bool");
        if (BoolDict == null)
            BoolDict = new Dictionary<string, string>();
        DoubleDict = EpicPrefs.GetEditorPrefs("Double");
        if (DoubleDict == null)
            DoubleDict = new Dictionary<string, string>();
        LongDict = EpicPrefs.GetEditorPrefs("Long");
        if (LongDict == null)
            LongDict = new Dictionary<string, string>();
        ArrayIntDict = EpicPrefs.GetEditorPrefs("ArrayInt");
        if (ArrayIntDict == null)
            ArrayIntDict = new Dictionary<string, string>();
        ArrayStringDict = EpicPrefs.GetEditorPrefs("ArrayString");
        if (ArrayStringDict == null)
            ArrayStringDict = new Dictionary<string, string>();
        ArrayFloatDict = EpicPrefs.GetEditorPrefs("ArrayFloat");
        if (ArrayFloatDict == null)
            ArrayFloatDict = new Dictionary<string, string>();
        ArrayDoubleDict = EpicPrefs.GetEditorPrefs("ArrayDouble");
        if (ArrayDoubleDict == null)
            ArrayDoubleDict = new Dictionary<string, string>();
        ListStringDict = EpicPrefs.GetEditorPrefs("ListS");
        if (ListStringDict == null)
            ListStringDict = new Dictionary<string, string>();
        ListFloatDict = EpicPrefs.GetEditorPrefs("ListF");
        if (ListFloatDict == null)
            ListFloatDict = new Dictionary<string, string>();
        ListBoolDict = EpicPrefs.GetEditorPrefs("ListB");
        if (ListBoolDict == null)
            ListBoolDict = new Dictionary<string, string>();
        ListDoubleDict = EpicPrefs.GetEditorPrefs("ListD");
        if (ListDoubleDict == null)
            ListDoubleDict = new Dictionary<string, string>();
        ListIntDict = EpicPrefs.GetEditorPrefs("ListI");
        if (ListIntDict == null)
            ListIntDict = new Dictionary<string, string>();
        ListLongDict = EpicPrefs.GetEditorPrefs("ListL");
        if (ListLongDict == null)
            ListLongDict = new Dictionary<string, string>();
        DictStringDict = EpicPrefs.GetEditorPrefs("DictS");
        if (DictStringDict == null)
            DictStringDict = new Dictionary<string, string>();
        DictIntDict = EpicPrefs.GetEditorPrefs("DictI");
        if (DictIntDict == null)
            DictIntDict = new Dictionary<string, string>();
        DictBoolDict = EpicPrefs.GetEditorPrefs("DictB");
        if (DictBoolDict == null)
            DictBoolDict = new Dictionary<string, string>();
        DictFloatDict = EpicPrefs.GetEditorPrefs("DictF");
        if (DictFloatDict == null)
            DictFloatDict = new Dictionary<string, string>();
        DictDoubleDict = EpicPrefs.GetEditorPrefs("DictD");
        if (DictDoubleDict == null)
            DictDoubleDict = new Dictionary<string, string>();
        DictLongDict = EpicPrefs.GetEditorPrefs("DictL");
        if (DictLongDict == null)
            DictLongDict = new Dictionary<string, string>();
        TransformDict = EpicPrefs.GetEditorPrefs("Transform");
        if (TransformDict == null)
            TransformDict = new Dictionary<string, string>();
        QuaternionDict = EpicPrefs.GetEditorPrefs("Quaternion");
        if (QuaternionDict == null)
            QuaternionDict = new Dictionary<string, string>();
        Vector2Dict = EpicPrefs.GetEditorPrefs("Vector2");
        if (Vector2Dict == null)
            Vector2Dict = new Dictionary<string, string>();
        Vector3Dict = EpicPrefs.GetEditorPrefs("Vector3");
        if (Vector3Dict == null)
            Vector3Dict = new Dictionary<string, string>();
        Vector4Dict = EpicPrefs.GetEditorPrefs("Vector4");
        if (Vector4Dict == null)
            Vector4Dict = new Dictionary<string, string>();
        ColorDict = EpicPrefs.GetEditorPrefs("Color");
        if (ColorDict == null)
            ColorDict = new Dictionary<string, string>();
        stringEditor = new Dictionary<string, string>();
        intEditor = new Dictionary<string, int>();
        floatEditor = new Dictionary<string, float>();
        stringStringEditor = new Dictionary<string, string>();
        boolEditor = new Dictionary<string, bool>();
        doubleEditor = new Dictionary<string, double>();
        longEditor = new Dictionary<string, long>();
        listsAndArrays = new Dictionary<string, string>();
        vectorsAndUnityTypes = new Dictionary<string, string>();
        colorEditor = new Dictionary<string, Color>();
        showSubDictS = new bool[DictStringDict.Count];
        showSubDictI = new bool[DictIntDict.Count];
        showSubDictB = new bool[DictBoolDict.Count];
        showSubDictF = new bool[DictFloatDict.Count];
        showSubDictD = new bool[DictDoubleDict.Count];
        showSubDictL = new bool[DictLongDict.Count];
        editSubDictS = new bool[DictStringDict.Count];
        editSubDictI = new bool[DictIntDict.Count];
        editSubDictB = new bool[DictBoolDict.Count];
        editSubDictF = new bool[DictFloatDict.Count];
        editSubDictD = new bool[DictDoubleDict.Count];
        editSubDictL = new bool[DictLongDict.Count];
        showSubListS = new bool[ListStringDict.Count];
        showSubListI = new bool[ListIntDict.Count];
        showSubListB = new bool[ListBoolDict.Count];
        showSubListF = new bool[ListFloatDict.Count];
        showSubListD = new bool[ListDoubleDict.Count];
        showSubListL = new bool[ListLongDict.Count];
        editSubListS = new bool[ListStringDict.Count];
        editSubListI = new bool[ListIntDict.Count];
        editSubListB = new bool[ListBoolDict.Count];
        editSubListF = new bool[ListFloatDict.Count];
        editSubListD = new bool[ListDoubleDict.Count];
        editSubListL = new bool[ListLongDict.Count];
        editSubArrayF = new bool[ArrayFloatDict.Count];
        editSubArrayI = new bool[ArrayIntDict.Count];
        editSubArrayS = new bool[ArrayStringDict.Count];
        editSubArrayD = new bool[ArrayDoubleDict.Count];
        showSubArrayF = new bool[ArrayFloatDict.Count];
        showSubArrayI = new bool[ArrayIntDict.Count];
        showSubArrayS = new bool[ArrayStringDict.Count];
        showSubArrayD = new bool[ArrayDoubleDict.Count];
        showSubQuaternion = new bool[QuaternionDict.Count];
        showSubVector2 = new bool[Vector2Dict.Count];
        showSubVector3 = new bool[Vector3Dict.Count];
        showSubVector4 = new bool[Vector4Dict.Count];
        editSubQuaternion = new bool[QuaternionDict.Count];
        editSubVector2 = new bool[Vector2Dict.Count];
        editSubVector3 = new bool[Vector3Dict.Count];
        editSubVector4 = new bool[Vector4Dict.Count];
        fillValues();
        passPhrase = EpicPrefs.getPassPhrase();
        initVector = EpicPrefs.getInitVector();
        Subscribe();
        startupInitialized = true;
    }
    static void fillValues(){
        if(IntDictValues == null)
            IntDictValues = new Dictionary<string,int>();
        foreach(KeyValuePair<string,string> pair in IntDict){
            if(!IntDictValues.ContainsKey(pair.Key))
                IntDictValues[pair.Key] = EpicPrefs.GetInt(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(FloatDictValues == null)
            FloatDictValues = new Dictionary<string,float>();
        foreach(KeyValuePair<string,string> pair in FloatDict){
            if(!FloatDictValues.ContainsKey(pair.Key))
                FloatDictValues[pair.Key] = EpicPrefs.GetFloat(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(StringDictValues == null)
            StringDictValues = new Dictionary<string,string>();
        foreach(KeyValuePair<string,string> pair in StringDict){
            if(!StringDictValues.ContainsKey(pair.Key))
                StringDictValues[pair.Key] = EpicPrefs.GetString(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(BoolDictValues == null)
            BoolDictValues = new Dictionary<string,bool>();
        foreach(KeyValuePair<string,string> pair in BoolDict){
            if(!BoolDictValues.ContainsKey(pair.Key))
                BoolDictValues[pair.Key] = EpicPrefs.GetBool(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(DoubleDictValues == null)
            DoubleDictValues = new Dictionary<string,double>();
        foreach(KeyValuePair<string,string> pair in DoubleDict){
            if(!DoubleDictValues.ContainsKey(pair.Key))
                DoubleDictValues[pair.Key] = EpicPrefs.GetDouble(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(LongDictValues == null)
            LongDictValues = new Dictionary<string,long>();
        foreach(KeyValuePair<string,string> pair in LongDict){
            if(!LongDictValues.ContainsKey(pair.Key))
                LongDictValues[pair.Key] = EpicPrefs.GetLong(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(ListStringDictValues == null)
            ListStringDictValues = new Dictionary<string,List<string>>();
        foreach(KeyValuePair<string,string> pair in ListStringDict){
            if(!ListStringDictValues.ContainsKey(pair.Key))
                ListStringDictValues[pair.Key] = EpicPrefs.GetListString(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(ListIntDictValues == null)
            ListIntDictValues = new Dictionary<string,List<int>>();
        foreach(KeyValuePair<string,string> pair in ListIntDict){
            if(!ListIntDictValues.ContainsKey(pair.Key))
                ListIntDictValues[pair.Key] = EpicPrefs.GetListInt(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(ListLongDictValues == null)
            ListLongDictValues = new Dictionary<string,List<long>>();
        foreach(KeyValuePair<string,string> pair in ListLongDict){
            if(!ListLongDictValues.ContainsKey(pair.Key))
                ListLongDictValues[pair.Key] = EpicPrefs.GetListLong(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(ListDoubleDictValues == null)
            ListDoubleDictValues = new Dictionary<string,List<double>>();
        foreach(KeyValuePair<string,string> pair in ListDoubleDict){
            if(!ListDoubleDictValues.ContainsKey(pair.Key))
                ListDoubleDictValues[pair.Key] = EpicPrefs.GetListDouble(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(ListFloatDictValues == null)
            ListFloatDictValues = new Dictionary<string,List<float>>();
        foreach(KeyValuePair<string,string> pair in ListFloatDict){
            if(!ListFloatDictValues.ContainsKey(pair.Key))
                ListFloatDictValues[pair.Key] = EpicPrefs.GetListFloat(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(ListBoolDictValues == null)
            ListBoolDictValues = new Dictionary<string,List<bool>>();
        foreach(KeyValuePair<string,string> pair in ListBoolDict){
            if(!ListBoolDictValues.ContainsKey(pair.Key))
                ListBoolDictValues[pair.Key] = EpicPrefs.GetListBool(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(ArrayIntDictValues == null)
            ArrayIntDictValues = new Dictionary<string,int[]>();
        foreach(KeyValuePair<string,string> pair in ArrayIntDict){
            if(!ArrayIntDictValues.ContainsKey(pair.Key))
                ArrayIntDictValues[pair.Key] = EpicPrefs.GetArrayInt(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(ArrayFloatDictValues == null)
            ArrayFloatDictValues = new Dictionary<string,float[]>();
        foreach(KeyValuePair<string,string> pair in ArrayFloatDict){
            if(!ArrayFloatDictValues.ContainsKey(pair.Key))
                ArrayFloatDictValues[pair.Key] = EpicPrefs.GetArrayFloat(pair.Key,Operators.ToBool(pair.Value));
        }

        if (ArrayDoubleDictValues == null)
            ArrayDoubleDictValues = new Dictionary<string, double[]>();
        foreach (KeyValuePair<string, string> pair in ArrayDoubleDict)
        {
            if (!ArrayDoubleDictValues.ContainsKey(pair.Key))
                ArrayDoubleDictValues[pair.Key] = EpicPrefs.GetArrayDouble(pair.Key, Operators.ToBool(pair.Value));
        }

        if (ArrayStringDictValues == null)
            ArrayStringDictValues = new Dictionary<string,string[]>();
        foreach(KeyValuePair<string,string> pair in ArrayStringDict){
            if(!ArrayStringDictValues.ContainsKey(pair.Key))
                ArrayStringDictValues[pair.Key] = EpicPrefs.GetArrayString(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(DictStringDictValues == null)
            DictStringDictValues = new Dictionary<string,Dictionary<string,string>>();
        foreach(KeyValuePair<string,string> pair in DictStringDict){
            if(!DictStringDictValues.ContainsKey(pair.Key))
                DictStringDictValues[pair.Key] = EpicPrefs.GetDictStringString(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(DictIntDictValues == null)
            DictIntDictValues = new Dictionary<string,Dictionary<string,int>>();
        foreach(KeyValuePair<string,string> pair in DictIntDict){
            if(!DictIntDictValues.ContainsKey(pair.Key))
                DictIntDictValues[pair.Key] = EpicPrefs.GetDictStringInt(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(DictFloatDictValues == null)
            DictFloatDictValues = new Dictionary<string,Dictionary<string,float>>();
        foreach(KeyValuePair<string,string> pair in DictFloatDict){
            if(!DictFloatDictValues.ContainsKey(pair.Key))
                DictFloatDictValues[pair.Key] = EpicPrefs.GetDictStringFloat(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(DictDoubleDictValues == null)
            DictDoubleDictValues = new Dictionary<string,Dictionary<string,double>>();
        foreach(KeyValuePair<string,string> pair in DictDoubleDict){
            if(!DictDoubleDictValues.ContainsKey(pair.Key))
                DictDoubleDictValues[pair.Key] = EpicPrefs.GetDictStringDouble(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(DictBoolDictValues == null)
            DictBoolDictValues = new Dictionary<string,Dictionary<string,bool>>();
        foreach(KeyValuePair<string,string> pair in DictBoolDict){
            if(!DictBoolDictValues.ContainsKey(pair.Key))
                DictBoolDictValues[pair.Key] = EpicPrefs.GetDictStringBool(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(DictLongDictValues == null)
            DictLongDictValues = new Dictionary<string,Dictionary<string,long>>();
        foreach(KeyValuePair<string,string> pair in DictLongDict){
            if(!DictLongDictValues.ContainsKey(pair.Key))
                DictLongDictValues[pair.Key] = EpicPrefs.GetDictStringLong(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(QuaternionDictValues == null)
            QuaternionDictValues = new Dictionary<string,Quaternion>();
        foreach(KeyValuePair<string,string> pair in QuaternionDict){
            if(!QuaternionDictValues.ContainsKey(pair.Key))
                QuaternionDictValues[pair.Key] = EpicPrefs.GetQuaternion(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(Vector2DictValues == null)
            Vector2DictValues = new Dictionary<string,Vector2>();
        foreach(KeyValuePair<string,string> pair in Vector2Dict){
            if(!Vector2DictValues.ContainsKey(pair.Key))
                Vector2DictValues[pair.Key] = EpicPrefs.GetVector2(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(Vector3DictValues == null)
            Vector3DictValues = new Dictionary<string,Vector3>();
        foreach(KeyValuePair<string,string> pair in Vector3Dict){
            if(!Vector3DictValues.ContainsKey(pair.Key))
                Vector3DictValues[pair.Key] = EpicPrefs.GetVector3(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(Vector4DictValues == null)
            Vector4DictValues = new Dictionary<string,Vector4>();
        foreach(KeyValuePair<string,string> pair in Vector4Dict){
            if(!Vector4DictValues.ContainsKey(pair.Key))
                Vector4DictValues[pair.Key] = EpicPrefs.GetVector4(pair.Key,Operators.ToBool(pair.Value));
        }
        
        if(ColorDictValues == null)
            ColorDictValues = new Dictionary<string,Color>();
        foreach(KeyValuePair<string,string> pair in ColorDict){
            if(!ColorDictValues.ContainsKey(pair.Key))
                ColorDictValues[pair.Key] = EpicPrefs.GetColor(pair.Key,Operators.ToBool(pair.Value));
        }
    }
    static void deleteEncrypted()
    {
        List<Dictionary<string, string>> allDicts = new List<Dictionary<string, string>>();
        #region setup
        allDicts.Add(IntDict);
        allDicts.Add(FloatDict);
        allDicts.Add(BoolDict);
        allDicts.Add(StringDict);
        allDicts.Add(LongDict);
        allDicts.Add(DoubleDict);
        allDicts.Add(ArrayIntDict);
        allDicts.Add(ArrayStringDict);
        allDicts.Add(ArrayFloatDict);
        allDicts.Add(ListStringDict);
        allDicts.Add(ListIntDict);
        allDicts.Add(ListBoolDict);
        allDicts.Add(ListFloatDict);
        allDicts.Add(ListDoubleDict);
        allDicts.Add(ListLongDict);
        allDicts.Add(DictStringDict);
        allDicts.Add(DictIntDict);
        allDicts.Add(DictBoolDict);
        allDicts.Add(DictFloatDict);
        allDicts.Add(DictDoubleDict);
        allDicts.Add(DictLongDict);
        allDicts.Add(TransformDict);
        allDicts.Add(QuaternionDict);
        allDicts.Add(Vector2Dict);
        allDicts.Add(Vector3Dict);
        allDicts.Add(Vector4Dict);
        allDicts.Add(ColorDict);
        #endregion
        for (int i =0; i<allDicts.Count;i++)
        {
            Dictionary<string, string> noEncryp = new Dictionary<string, string>();
            foreach(KeyValuePair<string,string> pair in allDicts[i])
            {
                if (!Operators.ToBool(pair.Value))
                    noEncryp[pair.Key] = pair.Value;
            }
            allDicts[i] = noEncryp;
        }
        #region apply changes
        IntDict = allDicts[0];
        FloatDict = allDicts[1];
        BoolDict = allDicts[2];
        StringDict = allDicts[3];
        LongDict = allDicts[4];
        DoubleDict = allDicts[5];
        ArrayIntDict = allDicts[6];
        ArrayStringDict = allDicts[7];
        ArrayFloatDict = allDicts[8];
        ListStringDict = allDicts[9];
        ListIntDict = allDicts[10];
        ListBoolDict = allDicts[11];
        ListFloatDict = allDicts[12];
        ListDoubleDict = allDicts[13];
        ListLongDict = allDicts[14];
        DictStringDict = allDicts[15];
        DictIntDict = allDicts[16];
        DictBoolDict = allDicts[17];
        DictFloatDict = allDicts[18];
        DictDoubleDict = allDicts[19];
        DictLongDict = allDicts[20];
        TransformDict = allDicts[21];
        QuaternionDict = allDicts[22];
        Vector2Dict = allDicts[23];
        Vector3Dict = allDicts[24];
        Vector4Dict = allDicts[25];
        ColorDict = allDicts[26];
        #endregion
    }
    private Dictionary<string,string> GetAllListDicts()
    {
        Dictionary<string, string> nDict = new Dictionary<string, string>();
        foreach (KeyValuePair<string, string> k in ListStringDict)
            nDict.Add(k.Key, k.Value);
        foreach (KeyValuePair<string, string> k in ListIntDict)
            nDict.Add(k.Key, k.Value);
        foreach (KeyValuePair<string, string> k in ListBoolDict)
            nDict.Add(k.Key, k.Value);
        foreach (KeyValuePair<string, string> k in ListFloatDict)
            nDict.Add(k.Key, k.Value);
        foreach (KeyValuePair<string, string> k in ListDoubleDict)
            nDict.Add(k.Key, k.Value);
        foreach (KeyValuePair<string, string> k in ListLongDict)
            nDict.Add(k.Key, k.Value);
        return nDict;
    }
    private Dictionary<string, string> GetAllDictsDict()
    {
        Dictionary<string, string> nDict = new Dictionary<string, string>();
        foreach (KeyValuePair<string, string> k in DictBoolDict)
            nDict.Add(k.Key, k.Value);
        foreach (KeyValuePair<string, string> k in DictStringDict)
            nDict.Add(k.Key, k.Value);
        foreach (KeyValuePair<string, string> k in DictDoubleDict)
            nDict.Add(k.Key, k.Value);
        foreach (KeyValuePair<string, string> k in DictFloatDict)
            nDict.Add(k.Key, k.Value);
        foreach (KeyValuePair<string, string> k in DictLongDict)
            nDict.Add(k.Key, k.Value);
        foreach (KeyValuePair<string, string> k in DictIntDict)
            nDict.Add(k.Key, k.Value);
        return nDict;
    }
    private static void OnPrefsChanged(Serializer.PrefEventArgs e)
    {
        if (!startupInitialized)
        {
            StartUp();
        }
        passPhrase = EpicPrefs.getPassPhrase();
        initVector = EpicPrefs.getInitVector();
        switch (e.Type)
        {
            case Serializer.SerializationTypes.ArrayFloat:
                if (e.Deleted){
                    ArrayFloatDict.Remove(e.Name);
                    ArrayFloatDictValues.Remove(e.Name);
                }
                else{
                    ArrayFloatDict[e.Name] = e.Encrpyted.ToString();
                    ArrayFloatDictValues[e.Name] = EpicPrefs.GetArrayFloat(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), ArrayFloatDict);
                if (showSubArrayF.Length != ArrayFloatDict.Count)
                {
                    showSubArrayF = new bool[ArrayFloatDict.Count];
                    editSubArrayF = new bool[ArrayFloatDict.Count];
                }
                break;
            case Serializer.SerializationTypes.ArrayDouble:
                if (e.Deleted)
                {
                    ArrayDoubleDict.Remove(e.Name);
                    ArrayDoubleDictValues.Remove(e.Name);
                }
                else {
                    ArrayDoubleDict[e.Name] = e.Encrpyted.ToString();
                    ArrayDoubleDictValues[e.Name] = EpicPrefs.GetArrayDouble(e.Name, e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), ArrayDoubleDict);
                if (showSubArrayD.Length != ArrayDoubleDict.Count)
                {
                    showSubArrayD = new bool[ArrayDoubleDict.Count];
                    editSubArrayD = new bool[ArrayDoubleDict.Count];
                }
                break;
            case Serializer.SerializationTypes.ArrayInt:
                if (e.Deleted){
                    ArrayIntDict.Remove(e.Name);
                    IntDictValues.Remove(e.Name);
                }
                else{
                    ArrayIntDict[e.Name] = e.Encrpyted.ToString();
                    ArrayIntDictValues[e.Name] = EpicPrefs.GetArrayInt(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), ArrayIntDict);
                if (showSubArrayI.Length != ArrayIntDict.Count)
                {
                    showSubArrayI = new bool[ArrayIntDict.Count];
                    editSubArrayI = new bool[ArrayIntDict.Count];
                }
                break;
            case Serializer.SerializationTypes.ArrayString:
                if (e.Deleted){
                    ArrayStringDict.Remove(e.Name);
                    ArrayStringDictValues.Remove(e.Name);
                }
                else{
                    ArrayStringDict[e.Name] = e.Encrpyted.ToString();
                    ArrayStringDictValues[e.Name] = EpicPrefs.GetArrayString(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), ArrayStringDict);
                if (showSubArrayS.Length != ArrayStringDict.Count)
                {
                    showSubArrayS = new bool[ArrayStringDict.Count];
                    editSubArrayS = new bool[ArrayStringDict.Count];
                }
                break;
            case Serializer.SerializationTypes.Integer:
                if (e.Deleted){
                    IntDict.Remove(e.Name);
                    IntDictValues.Remove(e.Name);
                }
                else{
                    IntDict[e.Name] = e.Encrpyted.ToString();
                    IntDictValues[e.Name] = EpicPrefs.GetInt(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), IntDict);                
                break;
            case Serializer.SerializationTypes.String:
                if (e.Deleted){
                    StringDict.Remove(e.Name);
                    StringDictValues.Remove(e.Name);
                }
                else{
                    StringDict[e.Name] = e.Encrpyted.ToString();
                    StringDictValues[e.Name] = EpicPrefs.GetString(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), StringDict);
                break;
            case Serializer.SerializationTypes.Bool:
                if (e.Deleted){
                    BoolDict.Remove(e.Name);
                    BoolDictValues.Remove(e.Name);
                }
                else{
                    BoolDict[e.Name] = e.Encrpyted.ToString();
                    BoolDictValues[e.Name] = EpicPrefs.GetBool(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), BoolDict);
                break;
            case Serializer.SerializationTypes.Float:
                if (e.Deleted){
                    FloatDict.Remove(e.Name);
                    FloatDictValues.Remove(e.Name);
                }
                else{
                    FloatDict[e.Name] = e.Encrpyted.ToString();
                    FloatDictValues[e.Name] = EpicPrefs.GetFloat(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), FloatDict);
                break;
            case Serializer.SerializationTypes.Double:
                if (e.Deleted){
                    DoubleDict.Remove(e.Name);
                    DoubleDictValues.Remove(e.Name);
                }
                else{
                    DoubleDict[e.Name] = e.Encrpyted.ToString();
                    DoubleDictValues[e.Name] = EpicPrefs.GetDouble(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), DoubleDict);
                break;
            case Serializer.SerializationTypes.Long:
                if (e.Deleted){
                    LongDict.Remove(e.Name);
                    LongDictValues.Remove(e.Name);
                }
                else{
                    LongDict[e.Name] = e.Encrpyted.ToString();
                    LongDictValues[e.Name] = EpicPrefs.GetLong(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), LongDict);
                break;
            case Serializer.SerializationTypes.ListS:
                if (e.Deleted){
                    ListStringDict.Remove(e.Name);
                    ListStringDictValues.Remove(e.Name);
                }
                else{
                    ListStringDict[e.Name] = e.Encrpyted.ToString();
                    ListStringDictValues[e.Name] = EpicPrefs.GetListString(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), ListStringDict);
                if (showSubListS.Length != ListStringDict.Count)
                {
                    showSubListS = new bool[ListStringDict.Count];
                    editSubListS = new bool[ListStringDict.Count];
                }
                break;
            case Serializer.SerializationTypes.ListI:
                if (e.Deleted){
                    ListIntDict.Remove(e.Name);
                    ListIntDictValues.Remove(e.Name);
                }
                else{
                    ListIntDict[e.Name] = e.Encrpyted.ToString();
                    ListIntDictValues[e.Name] = EpicPrefs.GetListInt(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), ListIntDict);
                if (showSubListI.Length != ListIntDict.Count)
                {
                    showSubListI = new bool[ListIntDict.Count];
                    editSubListI = new bool[ListIntDict.Count];
                }
                break;
            case Serializer.SerializationTypes.ListF:
                if (e.Deleted){
                    ListFloatDict.Remove(e.Name);
                    ListFloatDictValues.Remove(e.Name);
                }
                else{
                    ListFloatDict[e.Name] = e.Encrpyted.ToString();
                    ListFloatDictValues[e.Name] = EpicPrefs.GetListFloat(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), ListFloatDict);
                if (showSubListF.Length != ListFloatDict.Count)
                {
                    showSubListF = new bool[ListFloatDict.Count];
                    editSubListF = new bool[ListFloatDict.Count];
                }
                break;
            case Serializer.SerializationTypes.ListD:
                if (e.Deleted){
                    ListDoubleDict.Remove(e.Name);
                    ListDoubleDictValues.Remove(e.Name);
                }
                else{
                    ListDoubleDict[e.Name] = e.Encrpyted.ToString();
                    ListDoubleDictValues[e.Name] = EpicPrefs.GetListDouble(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), ListDoubleDict);
                if (showSubListD.Length != ListDoubleDict.Count)
                {
                    showSubListD = new bool[ListDoubleDict.Count];
                    editSubListD = new bool[ListDoubleDict.Count];
                }
                break;
            case Serializer.SerializationTypes.ListB:
                if (e.Deleted){
                    ListBoolDict.Remove(e.Name);
                    ListBoolDictValues.Remove(e.Name);
                }
                else{
                    ListBoolDict[e.Name] = e.Encrpyted.ToString();
                    ListBoolDictValues[e.Name] = EpicPrefs.GetListBool(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), ListBoolDict);
                if (showSubListB.Length != ListBoolDict.Count)
                {
                    showSubListB = new bool[ListBoolDict.Count];
                    editSubListB = new bool[ListBoolDict.Count];
                }
                break;
            case Serializer.SerializationTypes.ListL:
                if (e.Deleted){
                    ListLongDict.Remove(e.Name);
                    ListLongDictValues.Remove(e.Name);
                }
                else{
                    ListLongDict[e.Name] = e.Encrpyted.ToString();
                    ListLongDictValues[e.Name] = EpicPrefs.GetListLong(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), ListLongDict);
                if (showSubListL.Length != ListLongDict.Count)
                {
                    showSubListL = new bool[ListLongDict.Count];
                    editSubListL = new bool[ListLongDict.Count];
                }
                break;
            case Serializer.SerializationTypes.DictS:
                if (e.Deleted)
                {
                    DictStringDict.Remove(e.Name);
                    DictStringDictValues.Remove(e.Name);
                }
                else{
                    DictStringDict[e.Name] = e.Encrpyted.ToString();
                    DictStringDictValues[e.Name] = EpicPrefs.GetDictStringString(e.Name,e.Encrpyted);
                    if(DictStringDictValues[e.Name] == null)
                        Debug.Log("ADDED A NULL");
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), DictStringDict);
                if (showSubDictS.Length != DictStringDict.Count)
                {
                    showSubDictS = new bool[DictStringDict.Count];
                    editSubDictS = new bool[DictStringDict.Count];
                }
                break;
            case Serializer.SerializationTypes.DictI:
                if (e.Deleted){
                    DictIntDict.Remove(e.Name);
                    DictIntDictValues.Remove(e.Name);
                }
                else{
                    DictIntDict[e.Name] = e.Encrpyted.ToString();
                    DictIntDictValues[e.Name] = EpicPrefs.GetDictStringInt(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), DictIntDict);
                if (showSubDictI.Length != DictIntDict.Count)
                {
                    showSubDictI = new bool[DictIntDict.Count];
                    editSubDictI = new bool[DictIntDict.Count];
                }
                break;
            case Serializer.SerializationTypes.DictB:
                if (e.Deleted){
                    DictBoolDict.Remove(e.Name);
                    DictBoolDictValues.Remove(e.Name);
                }
                else{
                    DictBoolDict[e.Name] = e.Encrpyted.ToString();
                    DictBoolDictValues[e.Name] = EpicPrefs.GetDictStringBool(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), DictBoolDict);
                if (showSubDictB.Length != DictBoolDict.Count)
                {
                    showSubDictB = new bool[DictBoolDict.Count];
                    editSubDictB = new bool[DictBoolDict.Count];
                }
                break;
            case Serializer.SerializationTypes.DictF:
                if (e.Deleted){
                    DictFloatDict.Remove(e.Name);
                    DictFloatDictValues.Remove(e.Name);
                }
                else{
                    DictFloatDict[e.Name] = e.Encrpyted.ToString();
                    DictFloatDictValues[e.Name] = EpicPrefs.GetDictStringFloat(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), DictFloatDict);
                if (showSubDictF.Length != DictFloatDict.Count)
                {
                    showSubDictF = new bool[DictFloatDict.Count];
                    editSubDictF = new bool[DictFloatDict.Count];
                }
                break;
            case Serializer.SerializationTypes.DictL:
                if (e.Deleted){
                    DictLongDict.Remove(e.Name);
                    DictLongDictValues.Remove(e.Name);
                }
                else{
                    DictLongDict[e.Name] = e.Encrpyted.ToString();
                    DictLongDictValues[e.Name] = EpicPrefs.GetDictStringLong(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), DictLongDict);
                if (showSubDictL.Length != DictLongDict.Count)
                {
                    showSubDictL = new bool[DictLongDict.Count];
                    editSubDictL = new bool[DictLongDict.Count];
                }
                break;
            case Serializer.SerializationTypes.DictD:
                if (e.Deleted){
                    DictDoubleDict.Remove(e.Name);
                    DictDoubleDictValues.Remove(e.Name);
                }
                else{
                    DictDoubleDict[e.Name] = e.Encrpyted.ToString();
                    DictDoubleDictValues[e.Name] = EpicPrefs.GetDictStringDouble(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), DictDoubleDict);
                if (showSubDictD.Length != DictDoubleDict.Count)
                {
                    showSubDictD = new bool[DictDoubleDict.Count];
                    editSubDictD = new bool[DictDoubleDict.Count];
                }
                break;
            case Serializer.SerializationTypes.Color:
                if (e.Deleted){
                    ColorDict.Remove(e.Name);
                    ColorDictValues.Remove(e.Name);
                }
                else{
                    ColorDict[e.Name] = e.Encrpyted.ToString();
                    ColorDictValues[e.Name] = EpicPrefs.GetColor(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), ColorDict);
                break;
            case Serializer.SerializationTypes.Transform:
                /*if (e.Deleted)
                    ArrayFloatDict.Remove(e.Name);
                else{
                    TransformDict[e.Name] = e.Encrpyted.ToString();
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), TransformDict);
                if (showSubTransform.Length != TransformDict.Count)
                {
                    showSubTransform = new bool[TransformDict.Count];
                    editSubTransform = new bool[TransformDict.Count];
                }*/
                break;
            case Serializer.SerializationTypes.Quaternion:
                if (e.Deleted){
                    QuaternionDict.Remove(e.Name);
                    QuaternionDictValues.Remove(e.Name);
                }
                else{
                    QuaternionDict[e.Name] = e.Encrpyted.ToString();
                    QuaternionDictValues[e.Name] = EpicPrefs.GetQuaternion(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), QuaternionDict);
                if (showSubQuaternion.Length != QuaternionDict.Count) {                 
                    showSubQuaternion = new bool[QuaternionDict.Count];
                    editSubQuaternion = new bool[QuaternionDict.Count];
                }
                break;
            case Serializer.SerializationTypes.Vector2:
                if (e.Deleted){
                    Vector2Dict.Remove(e.Name);
                    Vector2DictValues.Remove(e.Name);
                }
                else{
                    Vector2Dict[e.Name] = e.Encrpyted.ToString();
                    Vector2DictValues[e.Name] = EpicPrefs.GetVector2(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), Vector2Dict);
                if (showSubVector2.Length != Vector2Dict.Count)
                {
                    showSubVector2 = new bool[Vector2Dict.Count];
                    editSubVector2 = new bool[Vector2Dict.Count];
                }
                break;
            case Serializer.SerializationTypes.Vector3:
                if (e.Deleted){
                    Vector3Dict.Remove(e.Name);
                    Vector3DictValues.Remove(e.Name);
                }
                else{
                    Vector3Dict[e.Name] = e.Encrpyted.ToString();
                    Vector3DictValues[e.Name] = EpicPrefs.GetVector3(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), Vector3Dict);
                if (showSubVector3.Length != Vector3Dict.Count)
                {
                    showSubVector3 = new bool[Vector3Dict.Count];
                    editSubVector3 = new bool[Vector3Dict.Count];
                }
                break;
            case Serializer.SerializationTypes.Vector4:
                if (e.Deleted){
                    Vector4Dict.Remove(e.Name);
                    Vector4DictValues.Remove(e.Name);
                }
                else{
                    Vector4Dict[e.Name] = e.Encrpyted.ToString();
                    Vector4DictValues[e.Name] = EpicPrefs.GetVector4(e.Name,e.Encrpyted);
                }
                EpicPrefs.SetEditorPrefs(e.Type.ToString(), Vector4Dict);
                if (showSubVector4.Length != Vector4Dict.Count)
                {
                    showSubVector4 = new bool[Vector4Dict.Count];
                    editSubVector4 = new bool[Vector4Dict.Count];
                }
                break;
        }
        repaint = true;              
    }

    void OnGUI()
    {
        #region Layout Design #1
        Subscribe();
        if (!stylesInitialized)
        {
            if(Event.current.type == EventType.Layout)
            {
                Repaint();
                return;
            } else
            {
                Initializer();
                SetupStyles();
                Repaint();
                return;
            }
        }
        if (!startupInitialized)
        {
            if (Event.current.type == EventType.Layout)
            {
                Repaint();
                return;
            }
            else
            {
                StartUp();
                Repaint();
                return;
            }
        }
        if (tex == null || eB == null )
        {
            stylesInitialized = false;
            Repaint();
            return;
        }
        if(stringEditor == null)
        {
            startupInitialized = false;
            Repaint();
            return;
        }
        GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), tex);
        GUI.DrawTexture(new Rect(5, 5, maxSize.x - 5, maxSize.y - 5), tex2);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10);
        EditorGUILayout.BeginVertical();
        GUILayout.Space(10);
        GUILayout.Label("EpicPrefs Editor", titleStyle);
        EditorGUILayout.BeginHorizontal();
        GUI.DrawTexture(new Rect(15, 35, 80, 80), logo);
        GUILayout.Space(100);
        EditorGUILayout.BeginVertical();
        GUILayout.Label("You can export EpicPrefs to release builds.",text);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("This means every EpicPref you set at Editor time will also exist in your release build.",text);
        if(GUILayout.Button(exB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20))){
            Operators.DirectoryCopy("HotTotem/EpicPrefs/" , "HotTotemAssets/EpicPrefs/",true);
            AssetDatabase.Refresh();          
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Label("To do so simply click the export button, once you added all your EpicPrefs.",text); 
        EditorGUILayout.EndVertical();      
        EditorGUILayout.EndHorizontal();
        Separator();
        if (!stringEditor.ContainsKey("editAESInit"))
        {
            stringEditor["editAESInit"] = (false).ToString();
        }
        if (Convert.ToBoolean(stringEditor["editAESInit"]))
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Encryption initialization. Set these to ensure a unique encryption.", text);
            if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
            {
                if (System.Text.ASCIIEncoding.ASCII.GetByteCount(stringEditor["AESINITVECTOR"]) != 16)
                {
                    if (EditorUtility.DisplayDialog("Error", "The initialization vector MUST be 16 bytes long.", "Ok, got it!"))
                    {
                        return;
                    }
                }
                else {
                    if (passPhrase != stringEditor["AESINITPASSPHRASE"] || initVector != stringEditor["AESINITVECTOR"])
                        deleteEncrypted();
                    EpicPrefs.setPassPhrase(stringEditor["AESINITPASSPHRASE"]);
                    passPhrase = stringEditor["AESINITPASSPHRASE"];
                    EpicPrefs.setInitVector(stringEditor["AESINITVECTOR"]);
                    initVector = stringEditor["AESINITVECTOR"];
                    stringEditor["editAESInit"] = (false).ToString();
                    Repaint();
                    return;
                }

            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            stringEditor["AESINITPASSPHRASE"] = myTextField("Encryption Passphrase : ", stringEditor["AESINITPASSPHRASE"]);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Initialization Vector(needs to be 16 bytes long) : ", text);
            stringEditor["AESINITVECTOR"] = myTextField(stringEditor["AESINITVECTOR"]);
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Encryption initialization. Set these to ensure a unique encryption.", text);
            if (GUILayout.Button(eB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
            {
                if (EditorUtility.DisplayDialog("Encryption change", "Are you sure you want to edit the AES encryption settings ? Doing " +
                    "so will PERMANENTLY DELETE all previously encrypted EpicPrefs.", "Yes", "No"))
                {
                    passPhrase = EpicPrefs.getPassPhrase();
                    initVector = EpicPrefs.getInitVector();
                    stringEditor["AESINITPASSPHRASE"] = passPhrase;
                    stringEditor["AESINITVECTOR"] = initVector;
                    stringEditor["editAESInit"] = (true).ToString();
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            myLabelField("Encryption Passphrase : ", passPhrase);            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Initialization Vector(needs to be 16 bytes long) : ", text);
            GUILayout.Label(initVector, text);
            EditorGUILayout.EndHorizontal();
        }
        horizontalScroll =
                EditorGUILayout.BeginScrollView(horizontalScroll);
        verticalScroll =
                EditorGUILayout.BeginScrollView(verticalScroll);
        Separator();

        #endregion
        #region Add New 
        if (!addNewValue)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(60);
            GUILayout.Label("Add new EpicPrefs ", titleStyle);
            if (GUILayout.Button(aB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
            {
                addNewValue = true;
            }
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(60);
            GUILayout.Label("Add new EpicPrefs ", titleStyle);
            if (GUILayout.Button(cB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
            {
                addNewValue = false;
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(20);
            string[] options = new string[]
             {
                 "Integer", "String", "Float","Double","Long","Bool",
                 "List<int>", "List<string>","List<float>","List<double>","List<long>","List<bool>",
                 "Dictionary<string,int>", "Dictionary<string,string>","Dictionary<string,float>",
                 "Dictionary<string,double>","Dictionary<string,long>","Dictionary<string,bool>",
                 "Vector2","Vector3","Vector4","Quaternion","Color","string[]","int[]","float[]","double[]"
             };
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Select a type : ", text, GUILayout.Height(20));
            selected = EditorGUILayout.Popup(selected, options,textBox,GUILayout.Height(20));
            EditorGUILayout.EndHorizontal();
            #region Apply Changes
            switch (selected)
            {
                case 0:
                    #region int
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : " ,newName, textBox);
                    newValue = myTextField("Value : ",newValue, textBox);
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        EpicPrefs.SetInt(newName, Operators.ToInt(newValue), false);
                        addNewValue = false;
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                    #endregion
                    break;
                case 1:
                    #region string
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : ", newName, textBox);
                    newValue = myTextField("Value : ", newValue, textBox);
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        EpicPrefs.SetString(newName, newValue, false);
                        addNewValue = false;
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                    #endregion
                    break;
                case 2:
                    #region float
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : ", newName, textBox);
                    newValue = myTextField("Value : ", newValue, textBox);
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        EpicPrefs.SetFloat(newName, Operators.ToFloat(newValue), false);
                        addNewValue = false;
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                    #endregion
                    break;
                case 3:
                    #region double
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : ", newName, textBox);
                    newValue = myTextField("Value : ", newValue, textBox);
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        EpicPrefs.SetDouble(newName, Operators.ToDouble(newValue), false);
                        addNewValue = false;
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                    #endregion
                    break;
                case 4:
                    #region long
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : ", newName, textBox);
                    newValue = myTextField("Value : ", newValue, textBox);
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        EpicPrefs.SetLong(newName, Operators.ToLong(newValue), false);
                        addNewValue = false;
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                    #endregion
                    break;
                case 5:
                    #region bool
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : ", newName, textBox);
                    newValue = myTextField("Value : ", newValue, textBox);
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        EpicPrefs.SetBool(newName, Operators.ToBool(newValue), false);
                        addNewValue = false;
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                    #endregion
                    break;
                case 6:
                    #region intList
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : ", newName, textBox);
                    if (!listsAndArrays.ContainsKey("[newIL]newEntryCounter"))
                        listsAndArrays["[newIL]newEntryCounter"] = "1";
                    int newCounter = Convert.ToInt32(listsAndArrays["[newIL]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        if (!listsAndArrays.ContainsKey("[newIL]newEntryValue" + n))
                            listsAndArrays["[newIL]newEntryValue" + n] = "";

                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Space(20);
                    myLabelField("Index 0 : ");
                    listsAndArrays["[newIL]newEntryValue0"] = myTextField(listsAndArrays["[newIL]newEntryValue0"], textBox);
                    GUILayout.EndHorizontal();
                    for (int n = 1; n < newCounter; n++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        myLabelField("Index " + (n).ToString());
                        listsAndArrays["[newIL]newEntryValue" + n] = myTextField(listsAndArrays["[newIL]newEntryValue" + n], textBox);
                        GUILayout.EndHorizontal();
                    }
                    if (listsAndArrays["[newIL]newEntryValue" + (newCounter - 1)] != "")
                    {
                        listsAndArrays["[newIL]newEntryCounter"] = (newCounter + 1).ToString();
                    }
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        if (!GetAllListDicts().ContainsKey(newName))
                        {
                            newCounter = Convert.ToInt32(listsAndArrays["[newIL]newEntryCounter"]);
                            List<int> tmpIL = new List<int>();
                            for (int n = 0; n < newCounter; n++)
                            {
                                newValue = listsAndArrays["[newIL]newEntryValue" + n].Trim();
                                if (newValue != "")
                                {
                                    tmpIL.Add(Operators.ToInt(newValue));
                                }
                            }
                            EpicPrefs.SetList(newName, tmpIL, false);
                            addNewValue = false;
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "A list with that name is already existing. Please chose a new one.", "OK");

                        }
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    #endregion
                    break;
                case 7:
                    #region stringlist
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : ", newName, textBox);
                    if (!listsAndArrays.ContainsKey("[newSL]newEntryCounter"))
                        listsAndArrays["[newSL]newEntryCounter"] = "1";
                    newCounter = Convert.ToInt32(listsAndArrays["[newSL]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        if (!listsAndArrays.ContainsKey("[newSL]newEntryValue" + n))
                            listsAndArrays["[newSL]newEntryValue" + n] = "";

                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Space(20);
                    myLabelField("Index 0 : ");
                    listsAndArrays["[newSL]newEntryValue0"] = myTextField(listsAndArrays["[newSL]newEntryValue0"], textBox);
                    GUILayout.EndHorizontal();
                    for (int n = 1; n < newCounter; n++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        myLabelField("Index " + (n).ToString());
                        listsAndArrays["[newSL]newEntryValue" + n] = myTextField(listsAndArrays["[newSL]newEntryValue" + n], textBox);
                        GUILayout.EndHorizontal();
                    }
                    if (listsAndArrays["[newSL]newEntryValue" + (newCounter - 1)] != "")
                    {
                        listsAndArrays["[newSL]newEntryCounter"] = (newCounter + 1).ToString();
                    }
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        if (!GetAllListDicts().ContainsKey(newName))
                        {
                            List<string> tmpSL = new List<string>();
                            for (int n = 0; n < newCounter; n++)
                            {
                                if (listsAndArrays["[newSL]newEntryValue" + n].Trim() != "")
                                    tmpSL.Add(listsAndArrays["[newSL]newEntryValue" + n]);
                            }
                            EpicPrefs.SetList(newName, tmpSL, false);
                            addNewValue = false;
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "A list with that name is already existing. Please chose a new one.", "OK");
                        }                        
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    #endregion
                    break;
                case 8:
                    #region floatList
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : ", newName, textBox);
                    if (!listsAndArrays.ContainsKey("[newFL]newEntryCounter"))
                        listsAndArrays["[newFL]newEntryCounter"] = "1";
                    newCounter = Convert.ToInt32(listsAndArrays["[newFL]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        if (!listsAndArrays.ContainsKey("[newFL]newEntryValue" + n))
                            listsAndArrays["[newFL]newEntryValue" + n] = "";

                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Space(20);
                    myLabelField("Index 0 : ");
                    listsAndArrays["[newFL]newEntryValue0"] = myTextField(listsAndArrays["[newFL]newEntryValue0"], textBox);
                    GUILayout.EndHorizontal();
                    for (int n = 1; n < newCounter; n++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        myLabelField("Index " + (n).ToString());
                        listsAndArrays["[newFL]newEntryValue" + n] = myTextField(listsAndArrays["[newFL]newEntryValue" + n], textBox);
                        GUILayout.EndHorizontal();
                    }
                    if (listsAndArrays["[newFL]newEntryValue" + (newCounter - 1)] != "")
                    {
                        listsAndArrays["[newFL]newEntryCounter"] = (newCounter + 1).ToString();
                    }
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        if (!GetAllListDicts().ContainsKey(newName))
                        {
                            List<float> tmpIL = new List<float>();
                            for (int n = 0; n < newCounter; n++)
                            {
                                newValue = listsAndArrays["[newFL]newEntryValue" + n].Trim();
                                if (newValue != "")
                                {
                                    tmpIL.Add(Operators.ToFloat(newValue));
                                }
                            }
                            EpicPrefs.SetList(newName, tmpIL, false);
                            addNewValue = false;
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "A list with that name is already existing. Please chose a new one.", "OK");
                        }                        
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    #endregion
                    break;
                case 9:
                    #region doubleList
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : ", newName, textBox);
                    if (!listsAndArrays.ContainsKey("[newDL]newEntryCounter"))
                        listsAndArrays["[newDL]newEntryCounter"] = "1";
                    newCounter = Convert.ToInt32(listsAndArrays["[newDL]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        if (!listsAndArrays.ContainsKey("[newDL]newEntryValue" + n))
                            listsAndArrays["[newDL]newEntryValue" + n] = "";

                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Space(20);
                    myLabelField("Index 0 : ");
                    listsAndArrays["[newDL]newEntryValue0"] = myTextField(listsAndArrays["[newDL]newEntryValue0"], textBox);
                    GUILayout.EndHorizontal();
                    for (int n = 1; n < newCounter; n++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        myLabelField("Index " + (n).ToString());
                        listsAndArrays["[newDL]newEntryValue" + n] = myTextField(listsAndArrays["[newDL]newEntryValue" + n], textBox);
                        GUILayout.EndHorizontal();
                    }
                    if (listsAndArrays["[newDL]newEntryValue" + (newCounter - 1)] != "")
                    {
                        listsAndArrays["[newDL]newEntryCounter"] = (newCounter + 1).ToString();
                    }
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        if (!GetAllListDicts().ContainsKey(newName))
                        {
                            List<double> tmpIL = new List<double>();
                            for (int n = 0; n < newCounter; n++)
                            {
                                newValue = listsAndArrays["[newDL]newEntryValue" + n].Trim();
                                if (newValue != "")
                                {
                                    tmpIL.Add(Operators.ToDouble(newValue));
                                }
                            }
                            EpicPrefs.SetList(newName, tmpIL, false);
                            addNewValue = false;
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "A list with that name is already existing. Please chose a new one.", "OK");
                        }
                        
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    #endregion
                    break;
                case 10:
                    #region longList
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : ", newName, textBox);
                    if (!listsAndArrays.ContainsKey("[newLL]newEntryCounter"))
                        listsAndArrays["[newLL]newEntryCounter"] = "1";
                    newCounter = Convert.ToInt32(listsAndArrays["[newLL]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        if (!listsAndArrays.ContainsKey("[newLL]newEntryValue" + n))
                            listsAndArrays["[newLL]newEntryValue" + n] = "";

                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Space(20);
                    myLabelField("Index 0 : ");
                    listsAndArrays["[newLL]newEntryValue0"] = myTextField(listsAndArrays["[newLL]newEntryValue0"], textBox);
                    GUILayout.EndHorizontal();
                    for (int n = 1; n < newCounter; n++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        myLabelField("Index " + (n).ToString());
                        listsAndArrays["[newLL]newEntryValue" + n] = myTextField(listsAndArrays["[newLL]newEntryValue" + n], textBox);
                        GUILayout.EndHorizontal();
                    }
                    if (listsAndArrays["[newLL]newEntryValue" + (newCounter - 1)] != "")
                    {
                        listsAndArrays["[newLL]newEntryCounter"] = (newCounter + 1).ToString();
                    }
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        if (!GetAllListDicts().ContainsKey(newName))
                        {
                            List<long> tmpIL = new List<long>();
                            for (int n = 0; n < newCounter; n++)
                            {
                                newValue = listsAndArrays["[newLL]newEntryValue" + n].Trim();
                                if (newValue != "")
                                {
                                    tmpIL.Add(Operators.ToLong(newValue));
                                }
                            }
                            EpicPrefs.SetList(newName, tmpIL, false);
                            addNewValue = false;
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "A list with that name is already existing. Please chose a new one.", "OK");
                        }
                        
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    #endregion
                    break;
                case 11:
                    #region boolList
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : ", newName, textBox);
                    if (!listsAndArrays.ContainsKey("[newBL]newEntryCounter"))
                        listsAndArrays["[newBL]newEntryCounter"] = "1";
                    newCounter = Convert.ToInt32(listsAndArrays["[newBL]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        if (!listsAndArrays.ContainsKey("[newBL]newEntryValue" + n))
                            listsAndArrays["[newBL]newEntryValue" + n] = "";

                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Space(20);
                    myLabelField("Index 0 : ");
                    listsAndArrays["[newBL]newEntryValue0"] = myTextField(listsAndArrays["[newBL]newEntryValue0"], textBox);
                    GUILayout.EndHorizontal();
                    for (int n = 1; n < newCounter; n++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        myLabelField("Index " + (n).ToString());
                        listsAndArrays["[newBL]newEntryValue" + n] = myTextField(listsAndArrays["[newBL]newEntryValue" + n], textBox);
                        GUILayout.EndHorizontal();
                    }
                    if (listsAndArrays["[newBL]newEntryValue" + (newCounter - 1)] != "")
                    {
                        listsAndArrays["[newBL]newEntryCounter"] = (newCounter + 1).ToString();
                    }
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        if (!GetAllListDicts().ContainsKey(newName))
                        {
                            List<bool> tmpIL = new List<bool>();
                            for (int n = 0; n < newCounter; n++)
                            {
                                tmpIL.Add(Operators.ToBool(listsAndArrays["[newBL]newEntryValue" + n]));
                            }
                            EpicPrefs.SetList(newName, tmpIL, false);
                            addNewValue = false;
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "A list with that name is already existing. Please chose a new one.", "OK");
                        }
                        
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    #endregion
                    break;
                case 12:
                    #region dict string int
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : ", newName, textBox);
                    if (!listsAndArrays.ContainsKey("[newID]newEntryCounter"))
                        listsAndArrays["[newID]newEntryCounter"] = "1";
                    newCounter = Convert.ToInt32(listsAndArrays["[newID]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        if (!listsAndArrays.ContainsKey("[newID]newEntryKey" + n))
                            listsAndArrays["[newID]newEntryKey" + n] = "";
                        if (!listsAndArrays.ContainsKey("[newID]newEntryValue" + n))
                            listsAndArrays["[newID]newEntryValue" + n] = "";
                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Space(20);
                    listsAndArrays["[newID]newEntryKey0"] = myTextField(listsAndArrays["[newID]newEntryKey0"], textBox);
                    listsAndArrays["[newID]newEntryValue0"] = myTextField(listsAndArrays["[newID]newEntryValue0"], textBox);
                    GUILayout.EndHorizontal();
                    for (int n = 1; n < newCounter; n++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        listsAndArrays["[newID]newEntryKey" +n] = myTextField(listsAndArrays["[newID]newEntryKey" +n], textBox);
                        listsAndArrays["[newID]newEntryValue" + n] = myTextField(listsAndArrays["[newID]newEntryValue" + n], textBox);
                        GUILayout.EndHorizontal();
                    }
                    if (listsAndArrays["[newID]newEntryKey" + (newCounter - 1)] != "" && listsAndArrays["[newID]newEntryValue" + (newCounter - 1)] != "")
                    {
                        listsAndArrays["[newID]newEntryCounter"] = (newCounter + 1).ToString();
                    }
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        if (!GetAllDictsDict().ContainsKey(newName))
                        {
                            Dictionary<string,int> tmpSL = new Dictionary<string, int>();
                            for (int n = 0; n < newCounter; n++)
                            {
                                if (listsAndArrays["[newID]newEntryKey" + n].Trim() != "")
                                    tmpSL[listsAndArrays["[newID]newEntryKey" + n]] = Operators.ToInt(listsAndArrays["[newID]newEntryValue" + n]);
                            }
                            EpicPrefs.SetDict(newName, tmpSL, false);
                            addNewValue = false;
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "A list with that name is already existing. Please chose a new one.", "OK");
                        }
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    #endregion
                    break;
                case 13:
                    #region dict string string
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : ", newName, textBox);
                    if (!listsAndArrays.ContainsKey("[newSD]newEntryCounter"))
                        listsAndArrays["[newSD]newEntryCounter"] = "1";
                    newCounter = Convert.ToInt32(listsAndArrays["[newSD]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        if (!listsAndArrays.ContainsKey("[newSD]newEntryKey" + n))
                            listsAndArrays["[newSD]newEntryKey" + n] = "";
                        if (!listsAndArrays.ContainsKey("[newSD]newEntryValue" + n))
                            listsAndArrays["[newSD]newEntryValue" + n] = "";
                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Space(20);
                    listsAndArrays["[newSD]newEntryKey0"] = myTextField(listsAndArrays["[newSD]newEntryKey0"], textBox);
                    listsAndArrays["[newSD]newEntryValue0"] = myTextField(listsAndArrays["[newSD]newEntryValue0"], textBox);
                    GUILayout.EndHorizontal();
                    for (int n = 1; n < newCounter; n++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        listsAndArrays["[newSD]newEntryKey" + n] = myTextField(listsAndArrays["[newSD]newEntryKey" + n], textBox);
                        listsAndArrays["[newSD]newEntryValue" + n] = myTextField(listsAndArrays["[newSD]newEntryValue" + n], textBox);
                        GUILayout.EndHorizontal();
                    }
                    if (listsAndArrays["[newSD]newEntryKey" + (newCounter - 1)] != "" && listsAndArrays["[newSD]newEntryValue" + (newCounter - 1)] != "")
                    {
                        listsAndArrays["[newSD]newEntryCounter"] = (newCounter + 1).ToString();
                    }
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        if (!GetAllDictsDict().ContainsKey(newName))
                        {
                            Dictionary<string, string> tmpSL = new Dictionary<string, string>();
                            for (int n = 0; n < newCounter; n++)
                            {
                                if (listsAndArrays["[newSD]newEntryKey" + n].Trim() != "")
                                    tmpSL[listsAndArrays["[newSD]newEntryKey" + n]] = listsAndArrays["[newSD]newEntryValue" + n];
                            }
                            EpicPrefs.SetDict(newName, tmpSL, false);
                            addNewValue = false;
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "A list with that name is already existing. Please chose a new one.", "OK");
                        }
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    #endregion
                    break;
                case 14:
                    #region dict string float
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : ", newName, textBox);
                    if (!listsAndArrays.ContainsKey("[newFD]newEntryCounter"))
                        listsAndArrays["[newFD]newEntryCounter"] = "1";
                    newCounter = Convert.ToInt32(listsAndArrays["[newFD]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        if (!listsAndArrays.ContainsKey("[newFD]newEntryKey" + n))
                            listsAndArrays["[newFD]newEntryKey" + n] = "";
                        if (!listsAndArrays.ContainsKey("[newFD]newEntryValue" + n))
                            listsAndArrays["[newFD]newEntryValue" + n] = "";
                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Space(20);
                    listsAndArrays["[newFD]newEntryKey0"] = myTextField(listsAndArrays["[newFD]newEntryKey0"], textBox);
                    listsAndArrays["[newFD]newEntryValue0"] = myTextField(listsAndArrays["[newFD]newEntryValue0"], textBox);
                    GUILayout.EndHorizontal();
                    for (int n = 1; n < newCounter; n++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        listsAndArrays["[newFD]newEntryKey" + n] = myTextField(listsAndArrays["[newFD]newEntryKey" + n], textBox);
                        listsAndArrays["[newFD]newEntryValue" + n] = myTextField(listsAndArrays["[newFD]newEntryValue" + n], textBox);
                        GUILayout.EndHorizontal();
                    }
                    if (listsAndArrays["[newFD]newEntryKey" + (newCounter - 1)] != "" && listsAndArrays["[newFD]newEntryValue" + (newCounter - 1)] != "")
                    {
                        listsAndArrays["[newFD]newEntryCounter"] = (newCounter + 1).ToString();
                    }
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        if (!GetAllDictsDict().ContainsKey(newName))
                        {
                            Dictionary<string, float> tmpSL = new Dictionary<string, float>();
                            for (int n = 0; n < newCounter; n++)
                            {
                                if (listsAndArrays["[newFD]newEntryKey" + n].Trim() != "")
                                    tmpSL[listsAndArrays["[newFD]newEntryKey" + n]] = Operators.ToFloat(listsAndArrays["[newFD]newEntryValue" + n]);
                            }
                            EpicPrefs.SetDict(newName, tmpSL, false);
                            addNewValue = false;
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "A list with that name is already existing. Please chose a new one.", "OK");
                        }
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    #endregion
                    break;
                case 15:
                    #region dict string double
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : ", newName, textBox);
                    if (!listsAndArrays.ContainsKey("[newDD]newEntryCounter"))
                        listsAndArrays["[newDD]newEntryCounter"] = "1";
                    newCounter = Convert.ToInt32(listsAndArrays["[newDD]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        if (!listsAndArrays.ContainsKey("[newDD]newEntryKey" + n))
                            listsAndArrays["[newDD]newEntryKey" + n] = "";
                        if (!listsAndArrays.ContainsKey("[newDD]newEntryValue" + n))
                            listsAndArrays["[newDD]newEntryValue" + n] = "";
                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Space(20);
                    listsAndArrays["[newDD]newEntryKey0"] = myTextField(listsAndArrays["[newDD]newEntryKey0"], textBox);
                    listsAndArrays["[newDD]newEntryValue0"] = myTextField(listsAndArrays["[newDD]newEntryValue0"], textBox);
                    GUILayout.EndHorizontal();
                    for (int n = 1; n < newCounter; n++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        listsAndArrays["[newDD]newEntryKey" + n] = myTextField(listsAndArrays["[newDD]newEntryKey" + n], textBox);
                        listsAndArrays["[newDD]newEntryValue" + n] = myTextField(listsAndArrays["[newDD]newEntryValue" + n], textBox);
                        GUILayout.EndHorizontal();
                    }
                    if (listsAndArrays["[newDD]newEntryKey" + (newCounter - 1)] != "" && listsAndArrays["[newDD]newEntryValue" + (newCounter - 1)] != "")
                    {
                        listsAndArrays["[newDD]newEntryCounter"] = (newCounter + 1).ToString();
                    }
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        if (!GetAllDictsDict().ContainsKey(newName))
                        {
                            Dictionary<string, double> tmpSL = new Dictionary<string, double>();
                            for (int n = 0; n < newCounter; n++)
                            {
                                if (listsAndArrays["[newDD]newEntryKey" + n].Trim() != "")
                                    tmpSL[listsAndArrays["[newDD]newEntryKey" + n]] = Operators.ToDouble(listsAndArrays["[newDD]newEntryValue" + n]);
                            }
                            EpicPrefs.SetDict(newName, tmpSL, false);
                            addNewValue = false;
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "A list with that name is already existing. Please chose a new one.", "OK");
                        }
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    #endregion
                    break;
                case 16:
                    #region dict string long
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : ", newName, textBox);
                    if (!listsAndArrays.ContainsKey("[newLD]newEntryCounter"))
                        listsAndArrays["[newLD]newEntryCounter"] = "1";
                    newCounter = Convert.ToInt32(listsAndArrays["[newLD]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        if (!listsAndArrays.ContainsKey("[newLD]newEntryKey" + n))
                            listsAndArrays["[newLD]newEntryKey" + n] = "";
                        if (!listsAndArrays.ContainsKey("[newLD]newEntryValue" + n))
                            listsAndArrays["[newLD]newEntryValue" + n] = "";
                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Space(20);
                    listsAndArrays["[newLD]newEntryKey0"] = myTextField(listsAndArrays["[newLD]newEntryKey0"], textBox);
                    listsAndArrays["[newLD]newEntryValue0"] = myTextField(listsAndArrays["[newLD]newEntryValue0"], textBox);
                    GUILayout.EndHorizontal();
                    for (int n = 1; n < newCounter; n++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        listsAndArrays["[newLD]newEntryKey" + n] = myTextField(listsAndArrays["[newLD]newEntryKey" + n], textBox);
                        listsAndArrays["[newLD]newEntryValue" + n] = myTextField(listsAndArrays["[newLD]newEntryValue" + n], textBox);
                        GUILayout.EndHorizontal();
                    }
                    if (listsAndArrays["[newLD]newEntryKey" + (newCounter - 1)] != "" && listsAndArrays["[newLD]newEntryValue" + (newCounter - 1)] != "")
                    {
                        listsAndArrays["[newLD]newEntryCounter"] = (newCounter + 1).ToString();
                    }
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        if (!GetAllDictsDict().ContainsKey(newName))
                        {
                            Dictionary<string, long> tmpSL = new Dictionary<string, long>();
                            for (int n = 0; n < newCounter; n++)
                            {
                                if (listsAndArrays["[newLD]newEntryKey" + n].Trim() != "")
                                    tmpSL[listsAndArrays["[newLD]newEntryKey" + n]] = Operators.ToLong(listsAndArrays["[newLD]newEntryValue" + n]);
                            }
                            EpicPrefs.SetDict(newName, tmpSL, false);
                            addNewValue = false;
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "A list with that name is already existing. Please chose a new one.", "OK");
                        }
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    #endregion
                    break;
                case 17:
                    #region dict string bool
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : ", newName, textBox);
                    if (!listsAndArrays.ContainsKey("[newBD]newEntryCounter"))
                        listsAndArrays["[newBD]newEntryCounter"] = "1";
                    newCounter = Convert.ToInt32(listsAndArrays["[newBD]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        if (!listsAndArrays.ContainsKey("[newBD]newEntryKey" + n))
                            listsAndArrays["[newBD]newEntryKey" + n] = "";
                        if (!listsAndArrays.ContainsKey("[newBD]newEntryValue" + n))
                            listsAndArrays["[newBD]newEntryValue" + n] = "";
                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Space(20);
                    listsAndArrays["[newBD]newEntryKey0"] = myTextField(listsAndArrays["[newBD]newEntryKey0"], textBox);
                    listsAndArrays["[newBD]newEntryValue0"] = myTextField(listsAndArrays["[newBD]newEntryValue0"], textBox);
                    GUILayout.EndHorizontal();
                    for (int n = 1; n < newCounter; n++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        listsAndArrays["[newBD]newEntryKey" + n] = myTextField(listsAndArrays["[newBD]newEntryKey" + n], textBox);
                        listsAndArrays["[newBD]newEntryValue" + n] = myTextField(listsAndArrays["[newBD]newEntryValue" + n], textBox);
                        GUILayout.EndHorizontal();
                    }
                    if (listsAndArrays["[newBD]newEntryKey" + (newCounter - 1)] != "" && listsAndArrays["[newBD]newEntryValue" + (newCounter - 1)] != "")
                    {
                        listsAndArrays["[newBD]newEntryCounter"] = (newCounter + 1).ToString();
                    }
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        if (!GetAllDictsDict().ContainsKey(newName))
                        {
                            Dictionary<string, bool> tmpSL = new Dictionary<string, bool>();
                            for (int n = 0; n < newCounter; n++)
                            {
                                if (listsAndArrays["[newBD]newEntryKey" + n].Trim() != "")
                                    tmpSL[listsAndArrays["[newBD]newEntryKey" + n]] = Operators.ToBool(listsAndArrays["[newBD]newEntryValue" + n]);
                            }
                            EpicPrefs.SetDict(newName, tmpSL, false);
                            addNewValue = false;
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "A list with that name is already existing. Please chose a new one.", "OK");
                        }
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    #endregion
                    break;
                case 18:
                    #region vector2
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : ", newName, textBox);
                    if (!listsAndArrays.ContainsKey("[newV2]newEntryCounter"))
                        listsAndArrays["[newV2]newEntryCounter"] = "1";
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(50);
                    if (!vectorsAndUnityTypes.ContainsKey("[newV2]XnewEntryValue0"))
                        vectorsAndUnityTypes["[newV2]XnewEntryValue0"] = "";
                    vectorsAndUnityTypes["[newV2]XnewEntryValue0"] = myTextField("X: ", vectorsAndUnityTypes["[newV2]XnewEntryValue0"], true);
                    if (!vectorsAndUnityTypes.ContainsKey("[newV2]YnewEntryValue0"))
                        vectorsAndUnityTypes["[newV2]YnewEntryValue0"] = "";
                    vectorsAndUnityTypes["[newV2]YnewEntryValue0"] = myTextField("Y: ", vectorsAndUnityTypes["[newV2]YnewEntryValue0"], true);
                    EditorGUILayout.EndHorizontal();

                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        if (!Vector2Dict.ContainsKey(newName))
                        {
                            #region floatCheck
                            if (vectorsAndUnityTypes["[newV2]XnewEntryValue0"] == "" || vectorsAndUnityTypes["[newV2]XnewEntryValue0"] == "-")
                            {
                                EditorUtility.DisplayDialog("Error", "Fill out all fields.", "OK");
                                break;
                            }
                            if (vectorsAndUnityTypes["[newV2]YnewEntryValue0"] == "" || vectorsAndUnityTypes["[newV2]YnewEntryValue0"] == "-")
                            {
                                EditorUtility.DisplayDialog("Error", "Fill out all fields.", "OK");
                                break;
                            }
                            #endregion
                            EpicPrefs.SetVector2(newName, new Vector2(Operators.ToFloat(vectorsAndUnityTypes["[newV2]XnewEntryValue0"]), Operators.ToFloat(vectorsAndUnityTypes["[newV2]YnewEntryValue0"])), false);
                            addNewValue = false;
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "A list with that name is already existing. Please chose a new one.", "OK");
                        }

                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    #endregion
                    break;
                case 19:
                    #region vector3
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : ", newName, textBox);
                    if (!listsAndArrays.ContainsKey("[newV3]newEntryCounter"))
                        listsAndArrays["[newV3]newEntryCounter"] = "1";
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(50);
                    if (!vectorsAndUnityTypes.ContainsKey("[newV3]XnewEntryValue0"))
                        vectorsAndUnityTypes["[newV3]XnewEntryValue0"] = "";
                    vectorsAndUnityTypes["[newV3]XnewEntryValue0"] = myTextField("X: ", vectorsAndUnityTypes["[newV3]XnewEntryValue0"], true);
                    if (!vectorsAndUnityTypes.ContainsKey("[newV3]YnewEntryValue0"))
                        vectorsAndUnityTypes["[newV3]YnewEntryValue0"] = "";
                    vectorsAndUnityTypes["[newV3]YnewEntryValue0"] = myTextField("Y: ", vectorsAndUnityTypes["[newV3]YnewEntryValue0"], true);
                    if (!vectorsAndUnityTypes.ContainsKey("[newV3]ZnewEntryValue0"))
                        vectorsAndUnityTypes["[newV3]ZnewEntryValue0"] = "";
                    vectorsAndUnityTypes["[newV3]ZnewEntryValue0"] = myTextField("Y: ", vectorsAndUnityTypes["[newV3]ZnewEntryValue0"], true);
                    EditorGUILayout.EndHorizontal();

                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        if (!Vector2Dict.ContainsKey(newName))
                        {
                            #region floatCheck
                            if (vectorsAndUnityTypes["[newV3]XnewEntryValue0"] == "" || vectorsAndUnityTypes["[newV3]XnewEntryValue0"] == "-")
                            {
                                EditorUtility.DisplayDialog("Error", "Fill out all fields.", "OK");
                                break;
                            }
                            if (vectorsAndUnityTypes["[newV3]YnewEntryValue0"] == "" || vectorsAndUnityTypes["[newV3]YnewEntryValue0"] == "-")
                            {
                                EditorUtility.DisplayDialog("Error", "Fill out all fields.", "OK");
                                break;
                            }
                            if (vectorsAndUnityTypes["[newV3]ZnewEntryValue0"] == "" || vectorsAndUnityTypes["[newV3]ZnewEntryValue0"] == "-")
                            {
                                EditorUtility.DisplayDialog("Error", "Fill out all fields.", "OK");
                                break;
                            }
                            #endregion
                            EpicPrefs.SetVector3(newName, new Vector3(Operators.ToFloat(vectorsAndUnityTypes["[newV3]XnewEntryValue0"]), Operators.ToFloat(vectorsAndUnityTypes["[newV3]YnewEntryValue0"]), Operators.ToFloat(vectorsAndUnityTypes["[newV3]ZnewEntryValue0"])), false);
                            addNewValue = false;
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "A list with that name is already existing. Please chose a new one.", "OK");
                        }

                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    #endregion
                    break;
                case 20:
                    #region vector4
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : ", newName, textBox);
                    if (!listsAndArrays.ContainsKey("[newV4]newEntryCounter"))
                        listsAndArrays["[newV4]newEntryCounter"] = "1";
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(50);
                    if (!vectorsAndUnityTypes.ContainsKey("[newV4]XnewEntryValue0"))
                        vectorsAndUnityTypes["[newV4]XnewEntryValue0"] = "";
                    vectorsAndUnityTypes["[newV4]XnewEntryValue0"] = myTextField("X: ", vectorsAndUnityTypes["[newV4]XnewEntryValue0"], true);
                    if (!vectorsAndUnityTypes.ContainsKey("[newV4]YnewEntryValue0"))
                        vectorsAndUnityTypes["[newV4]YnewEntryValue0"] = "";
                    vectorsAndUnityTypes["[newV4]YnewEntryValue0"] = myTextField("Y: ", vectorsAndUnityTypes["[newV4]YnewEntryValue0"], true);
                    if (!vectorsAndUnityTypes.ContainsKey("[newV4]ZnewEntryValue0"))
                        vectorsAndUnityTypes["[newV4]ZnewEntryValue0"] = "";
                    vectorsAndUnityTypes["[newV4]ZnewEntryValue0"] = myTextField("Y: ", vectorsAndUnityTypes["[newV4]ZnewEntryValue0"], true);
                    if (!vectorsAndUnityTypes.ContainsKey("[newV4]WnewEntryValue0"))
                        vectorsAndUnityTypes["[newV4]WnewEntryValue0"] = "";
                    vectorsAndUnityTypes["[newV4]WnewEntryValue0"] = myTextField("Y: ", vectorsAndUnityTypes["[newV4]WnewEntryValue0"], true);
                    EditorGUILayout.EndHorizontal();

                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        if (!Vector2Dict.ContainsKey(newName))
                        {
                            #region floatCheck
                            if (vectorsAndUnityTypes["[newV4]XnewEntryValue0"] == "" || vectorsAndUnityTypes["[newV4]XnewEntryValue0"] == "-")
                            {
                                EditorUtility.DisplayDialog("Error", "Fill out all fields.", "OK");
                                break;
                            }
                            if (vectorsAndUnityTypes["[newV4]YnewEntryValue0"] == "" || vectorsAndUnityTypes["[newV4]YnewEntryValue0"] == "-")
                            {
                                EditorUtility.DisplayDialog("Error", "Fill out all fields.", "OK");
                                break;
                            }
                            if (vectorsAndUnityTypes["[newV4]ZnewEntryValue0"] == "" || vectorsAndUnityTypes["[newV4]ZnewEntryValue0"] == "-")
                            {
                                EditorUtility.DisplayDialog("Error", "Fill out all fields.", "OK");
                                break;
                            }
                            if (vectorsAndUnityTypes["[newV4]WnewEntryValue0"] == "" || vectorsAndUnityTypes["[newV4]WnewEntryValue0"] == "-")
                            {
                                EditorUtility.DisplayDialog("Error", "Fill out all fields.", "OK");
                                break;
                            }
                            #endregion
                            EpicPrefs.SetVector4(newName, new Vector4(Operators.ToFloat(vectorsAndUnityTypes["[newV4]XnewEntryValue0"]), Operators.ToFloat(vectorsAndUnityTypes["[newV4]YnewEntryValue0"]), Operators.ToFloat(vectorsAndUnityTypes["[newV4]ZnewEntryValue0"]), Operators.ToFloat(vectorsAndUnityTypes["[newV4]WnewEntryValue0"])), false);
                            addNewValue = false;
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "A list with that name is already existing. Please chose a new one.", "OK");
                        }

                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    #endregion
                    break;
                case 21:
                    #region quaternion
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : ", newName, textBox);
                    if (!listsAndArrays.ContainsKey("[newV4]newEntryCounter"))
                        listsAndArrays["[quat]newEntryCounter"] = "1";
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(50);
                    if (!vectorsAndUnityTypes.ContainsKey("[quat]XnewEntryValue0"))
                        vectorsAndUnityTypes["[quat]XnewEntryValue0"] = "";
                    vectorsAndUnityTypes["[quat]XnewEntryValue0"] = myTextField("X: ", vectorsAndUnityTypes["[quat]XnewEntryValue0"], true);
                    if (!vectorsAndUnityTypes.ContainsKey("[quat]YnewEntryValue0"))
                        vectorsAndUnityTypes["[quat]YnewEntryValue0"] = "";
                    vectorsAndUnityTypes["[quat]YnewEntryValue0"] = myTextField("Y: ", vectorsAndUnityTypes["[quat]YnewEntryValue0"], true);
                    if (!vectorsAndUnityTypes.ContainsKey("[quat]ZnewEntryValue0"))
                        vectorsAndUnityTypes["[quat]ZnewEntryValue0"] = "";
                    vectorsAndUnityTypes["[quat]ZnewEntryValue0"] = myTextField("Y: ", vectorsAndUnityTypes["[quat]ZnewEntryValue0"], true);
                    if (!vectorsAndUnityTypes.ContainsKey("[quat]WnewEntryValue0"))
                        vectorsAndUnityTypes["[quat]WnewEntryValue0"] = "";
                    vectorsAndUnityTypes["[quat]WnewEntryValue0"] = myTextField("Y: ", vectorsAndUnityTypes["[quat]WnewEntryValue0"], true);
                    EditorGUILayout.EndHorizontal();

                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        if (!Vector2Dict.ContainsKey(newName))
                        {
                            #region floatCheck
                            if (vectorsAndUnityTypes["[quat]XnewEntryValue0"] == "" || vectorsAndUnityTypes["[quat]XnewEntryValue0"] == "-")
                            {
                                EditorUtility.DisplayDialog("Error", "Fill out all fields.", "OK");
                                break;
                            }
                            if (vectorsAndUnityTypes["[quat]YnewEntryValue0"] == "" || vectorsAndUnityTypes["[quat]YnewEntryValue0"] == "-")
                            {
                                EditorUtility.DisplayDialog("Error", "Fill out all fields.", "OK");
                                break;
                            }
                            if (vectorsAndUnityTypes["[quat]ZnewEntryValue0"] == "" || vectorsAndUnityTypes["[quat]ZnewEntryValue0"] == "-")
                            {
                                EditorUtility.DisplayDialog("Error", "Fill out all fields.", "OK");
                                break;
                            }
                            if (vectorsAndUnityTypes["[quat]WnewEntryValue0"] == "" || vectorsAndUnityTypes["[quat]WnewEntryValue0"] == "-")
                            {
                                EditorUtility.DisplayDialog("Error", "Fill out all fields.", "OK");
                                break;
                            }
                            #endregion
                            EpicPrefs.SetQuaternion(newName, new Quaternion(Operators.ToFloat(vectorsAndUnityTypes["[quat]XnewEntryValue0"]), Operators.ToFloat(vectorsAndUnityTypes["[quat]YnewEntryValue0"]), Operators.ToFloat(vectorsAndUnityTypes["[quat]ZnewEntryValue0"]), Operators.ToFloat(vectorsAndUnityTypes["[quat]WnewEntryValue0"])), false);
                            addNewValue = false;
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "A list with that name is already existing. Please chose a new one.", "OK");
                        }

                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    #endregion
                    break;
                case 22:
                    #region color
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : ", newName, textBox);
                    Color col = Color.white;
                    col = Operators.StringToColor(newValue);
                    EditorGUILayout.BeginHorizontal();
                    myLabelField("Color : ");
                    newValue = Operators.ColorToString(EditorGUILayout.ColorField(col));
                    EditorGUILayout.EndHorizontal();
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
						col = Operators.StringToColor(newValue);
						EpicPrefs.SetColor(newName, col, false);
                        addNewValue = false;
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                    #endregion
                    break;
                case 23:
                    #region string[]
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    EditorGUILayout.BeginHorizontal();
                    newName = myTextField("Name : ", newName, textBox);
                    if (!listsAndArrays.ContainsKey("[newSA]newEntryCounter"))
                        listsAndArrays["[newSA]newEntryCounter"] = "0";
                    listsAndArrays["[newSA]newEntryCounter"] = myTextField("Size : ", listsAndArrays["[newSA]newEntryCounter"], textBox);
                    EditorGUILayout.EndHorizontal();
                    newCounter = Operators.ToInt(listsAndArrays["[newSA]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        if (!listsAndArrays.ContainsKey("[newSA]newEntryValue" + n))
                            listsAndArrays["[newSA]newEntryValue" + n] = "";
                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Space(20);
                    GUILayout.EndHorizontal();
                    for (int n = 0; n < newCounter; n++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        myLabelField("Index " + (n).ToString());
                        listsAndArrays["[newSA]newEntryValue" + n] = myTextField(listsAndArrays["[newSA]newEntryValue" + n], textBox);
                        GUILayout.EndHorizontal();
                    }
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        if (!GetAllListDicts().ContainsKey(newName))
                        {
                            string[] tmpA = new string[Operators.ToInt(listsAndArrays["[newSA]newEntryCounter"])];
                            for (int n = 0; n < newCounter; n++)
                            {
                                tmpA[n] = listsAndArrays["[newSA]newEntryValue" + n];
                            }
                            EpicPrefs.SetArray(newName, tmpA, false);
                            addNewValue = false;
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "An array with that name is already existing. Please chose a new one.", "OK");
                        }

                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    #endregion
                    break;
                case 24:
                    #region int[]
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    EditorGUILayout.BeginHorizontal();
                    newName = myTextField("Name : ", newName, textBox);
                    if (!listsAndArrays.ContainsKey("[newIA]newEntryCounter"))
                        listsAndArrays["[newIA]newEntryCounter"] = "0";
                    listsAndArrays["[newIA]newEntryCounter"] = myTextField("Size : ", listsAndArrays["[newIA]newEntryCounter"], textBox);
                    EditorGUILayout.EndHorizontal();
                    newCounter = Operators.ToInt(listsAndArrays["[newIA]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        if (!listsAndArrays.ContainsKey("[newIA]newEntryValue" + n))
                            listsAndArrays["[newIA]newEntryValue" + n] = "";
                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Space(20);
                    GUILayout.EndHorizontal();
                    for (int n = 0; n < newCounter; n++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        myLabelField("Index " + (n).ToString());
                        listsAndArrays["[newIA]newEntryValue" + n] = myTextField(listsAndArrays["[newIA]newEntryValue" + n], textBox);
                        GUILayout.EndHorizontal();
                    }
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        if (!GetAllListDicts().ContainsKey(newName))
                        {
                            int[] tmpA = new int[Operators.ToInt(listsAndArrays["[newIA]newEntryCounter"])];
                            for (int n = 0; n < newCounter; n++)
                            {
                                tmpA[n] = Operators.ToInt(listsAndArrays["[newIA]newEntryValue" + n]);
                            }
                            EpicPrefs.SetArray(newName, tmpA, false);
                            addNewValue = false;
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "An array with that name is already existing. Please chose a new one.", "OK");
                        }

                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    #endregion
                    break;
                case 25:
                    #region float[]
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    EditorGUILayout.BeginHorizontal();
                    newName = myTextField("Name : ", newName, textBox);
                    if (!listsAndArrays.ContainsKey("[newFA]newEntryCounter"))
                        listsAndArrays["[newFA]newEntryCounter"] = "0";
                    listsAndArrays["[newFA]newEntryCounter"] = myTextField("Size : ", listsAndArrays["[newFA]newEntryCounter"], textBox);
                    EditorGUILayout.EndHorizontal();
                    if (!listsAndArrays.ContainsKey("[newFA]newEntryCounter"))
                        listsAndArrays["[newFA]newEntryCounter"] = "1";
                    newCounter = Operators.ToInt(listsAndArrays["[newFA]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        if (!listsAndArrays.ContainsKey("[newFA]newEntryValue" + n))
                            listsAndArrays["[newFA]newEntryValue" + n] = "";
                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Space(20);
                    GUILayout.EndHorizontal();
                    for (int n = 0; n < newCounter; n++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        myLabelField("Index " + (n).ToString());
                        listsAndArrays["[newFA]newEntryValue" + n] = myTextField(listsAndArrays["[newFA]newEntryValue" + n], textBox);
                        GUILayout.EndHorizontal();
                    }
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        if (!GetAllListDicts().ContainsKey(newName))
                        {
                            float[] tmpA = new float[Operators.ToInt(listsAndArrays["[newFA]newEntryCounter"])];
                            for (int n = 0; n < newCounter; n++)
                            {
                                tmpA[n] = Operators.ToFloat(listsAndArrays["[newFA]newEntryValue" + n]);
                            }
                            EpicPrefs.SetArray(newName, tmpA, false);
                            addNewValue = false;
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "An array with that name is already existing. Please chose a new one.", "OK");
                        }

                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    #endregion
                    break;
                case 26:
                    #region double[]
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    EditorGUILayout.BeginHorizontal();
                    newName = myTextField("Name : ", newName, textBox);
                    if (!listsAndArrays.ContainsKey("[newDA]newEntryCounter"))
                        listsAndArrays["[newDA]newEntryCounter"] = "0";
                    listsAndArrays["[newDA]newEntryCounter"] = myTextField("Size : ", listsAndArrays["[newDA]newEntryCounter"], textBox);
                    EditorGUILayout.EndHorizontal();
                    if (!listsAndArrays.ContainsKey("[newDA]newEntryCounter"))
                        listsAndArrays["[newDA]newEntryCounter"] = "1";
                    newCounter = Operators.ToInt(listsAndArrays["[newDA]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        if (!listsAndArrays.ContainsKey("[newDA]newEntryValue" + n))
                            listsAndArrays["[newDA]newEntryValue" + n] = "";
                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Space(20);
                    GUILayout.EndHorizontal();
                    for (int n = 0; n < newCounter; n++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        myLabelField("Index " + (n).ToString());
                        listsAndArrays["[newDA]newEntryValue" + n] = myTextField(listsAndArrays["[newDA]newEntryValue" + n], textBox);
                        GUILayout.EndHorizontal();
                    }
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        if (!GetAllListDicts().ContainsKey(newName))
                        {
                            double[] tmpA = new double[Operators.ToInt(listsAndArrays["[newDA]newEntryCounter"])];
                            for (int n = 0; n < newCounter; n++)
                            {
                                tmpA[n] = Operators.ToDouble(listsAndArrays["[newDA]newEntryValue" + n]);
                            }
                            EpicPrefs.SetArray(newName, tmpA, false);
                            addNewValue = false;
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "An array with that name is already existing. Please chose a new one.", "OK");
                        }

                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    #endregion
                    break;
                case 27:
                    #region transform
                    /*EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(5);
                    newName = myTextField("Name : ", newName, textBox);
                    if (!listsAndArrays.ContainsKey("[newBL]newEntryCounter"))
                        listsAndArrays["[newBL]newEntryCounter"] = "1";
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(40);
                    myLabelField("Position");
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(50);
                    if (!vectorsAndUnityTypes.ContainsKey("[newTransform]pXnewEntryValue0"))
                        vectorsAndUnityTypes["[newTransform]pXnewEntryValue0"] = "";
                    vectorsAndUnityTypes["[newTransform]pXnewEntryValue0"] = myTextField("X: ", vectorsAndUnityTypes["[newTransform]pXnewEntryValue0"], true);
                    if (!vectorsAndUnityTypes.ContainsKey("[newTransform]pYnewEntryValue0"))
                        vectorsAndUnityTypes["[newTransform]pYnewEntryValue0"] = "";
                    vectorsAndUnityTypes["[newTransform]pYnewEntryValue0"] = myTextField("Y: ", vectorsAndUnityTypes["[newTransform]pYnewEntryValue0"], true);
                    if (!vectorsAndUnityTypes.ContainsKey("[newTransform]pZnewEntryValue0"))
                        vectorsAndUnityTypes["[newTransform]pZnewEntryValue0"] = "";
                    vectorsAndUnityTypes["[newTransform]pZnewEntryValue0"] = myTextField("Z: ", vectorsAndUnityTypes["[newTransform]pZnewEntryValue0"], true);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(40);
                    myLabelField("Rotation");
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(50);
                    if (!vectorsAndUnityTypes.ContainsKey("[newTransform]rXnewEntryValue0"))
                        vectorsAndUnityTypes["[newTransform]rXnewEntryValue0"] = "";
                    vectorsAndUnityTypes["[newTransform]rXnewEntryValue0"] = myTextField("X: ", vectorsAndUnityTypes["[newTransform]rXnewEntryValue0"], true);
                    if (!vectorsAndUnityTypes.ContainsKey("[newTransform]rYnewEntryValue0"))
                        vectorsAndUnityTypes["[newTransform]rYnewEntryValue0"] = "";
                    vectorsAndUnityTypes["[newTransform]rYnewEntryValue0"] = myTextField("Y: ", vectorsAndUnityTypes["[newTransform]rYnewEntryValue0"], true);
                    if (!vectorsAndUnityTypes.ContainsKey("[newTransform]rZnewEntryValue0"))
                        vectorsAndUnityTypes["[newTransform]rZnewEntryValue0"] = "";
                    vectorsAndUnityTypes["[newTransform]rZnewEntryValue0"] = myTextField("Z: ", vectorsAndUnityTypes["[newTransform]rZnewEntryValue0"], true);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(40);
                    myLabelField("Scale");
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(50);
                    if (!vectorsAndUnityTypes.ContainsKey("[newTransform]sXnewEntryValue0"))
                        vectorsAndUnityTypes["[newTransform]sXnewEntryValue0"] = "";
                    vectorsAndUnityTypes["[newTransform]sXnewEntryValue0"] = myTextField("X: ", vectorsAndUnityTypes["[newTransform]sXnewEntryValue0"], true);
                    if (!vectorsAndUnityTypes.ContainsKey("[newTransform]sYnewEntryValue0"))
                        vectorsAndUnityTypes["[newTransform]sYnewEntryValue0"] = "";
                    vectorsAndUnityTypes["[newTransform]sYnewEntryValue0"] = myTextField("Y: ", vectorsAndUnityTypes["[newTransform]sYnewEntryValue0"], true);
                    if (!vectorsAndUnityTypes.ContainsKey("[newTransform]sZnewEntryValue0"))
                        vectorsAndUnityTypes["[newTransform]sZnewEntryValue0"] = "";
                    vectorsAndUnityTypes["[newTransform]sZnewEntryValue0"] = myTextField("Z: ", vectorsAndUnityTypes["[newTransform]sZnewEntryValue0"], true);
                    EditorGUILayout.EndHorizontal();
                    if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        if (!TransformDict.ContainsKey(newName))
                        {
                            GameObject temp = new GameObject();
                            Transform tf = temp.transform;
                            tf.name = newName;
                            #region floatCheck
                            if (vectorsAndUnityTypes["[newTransform]pXnewEntryValue0"] == "" || vectorsAndUnityTypes["[newTransform]pXnewEntryValue0"] == "-")
                            {
                                EditorUtility.DisplayDialog("Error", "Fill out all fields.", "OK");
                                DestroyImmediate(temp);
                                break;
                            }
                            if (vectorsAndUnityTypes["[newTransform]pYnewEntryValue0"] == "" || vectorsAndUnityTypes["[newTransform]pYnewEntryValue0"] == "-")
                            {
                                EditorUtility.DisplayDialog("Error", "Fill out all fields.", "OK");
                                DestroyImmediate(temp);
                                break;
                            }
                            if (vectorsAndUnityTypes["[newTransform]pZnewEntryValue0"] == "" || vectorsAndUnityTypes["[newTransform]pZnewEntryValue0"] == "-")
                            {
                                EditorUtility.DisplayDialog("Error", "Fill out all fields.", "OK");
                                DestroyImmediate(temp);
                                break;
                            }
                            #endregion
                            tf.position = new Vector3(Operators.ToFloat(vectorsAndUnityTypes["[newTransform]pXnewEntryValue0"]), Operators.ToFloat(vectorsAndUnityTypes["[newTransform]pYnewEntryValue0"]), Operators.ToFloat(vectorsAndUnityTypes["[newTransform]pZnewEntryValue0"]));
                            #region floatCheck
                            if (vectorsAndUnityTypes["[newTransform]rXnewEntryValue0"] == "" || vectorsAndUnityTypes["[newTransform]rXnewEntryValue0"] == "-")
                            {
                                EditorUtility.DisplayDialog("Error", "Fill out all fields.", "OK");
                                DestroyImmediate(temp);
                                break;
                            }
                            if (vectorsAndUnityTypes["[newTransform]rYnewEntryValue0"] == "" || vectorsAndUnityTypes["[newTransform]rYnewEntryValue0"] == "-")
                            {
                                EditorUtility.DisplayDialog("Error", "Fill out all fields.", "OK");
                                DestroyImmediate(temp);
                                break;
                            }
                            if (vectorsAndUnityTypes["[newTransform]rZnewEntryValue0"] == "" || vectorsAndUnityTypes["[newTransform]rZnewEntryValue0"] == "-")
                            {
                                EditorUtility.DisplayDialog("Error", "Fill out all fields.", "OK");
                                DestroyImmediate(temp);
                                break;
                            }
                            #endregion
                            tf.rotation = Quaternion.Euler(new Vector3(Operators.ToFloat(vectorsAndUnityTypes["[newTransform]rXnewEntryValue0"]), Operators.ToFloat(vectorsAndUnityTypes["[newTransform]rYnewEntryValue0"]), Operators.ToFloat(vectorsAndUnityTypes["[newTransform]rZnewEntryValue0"])));
                            #region floatCheck
                            if (vectorsAndUnityTypes["[newTransform]sXnewEntryValue0"] == "" || vectorsAndUnityTypes["[newTransform]sXnewEntryValue0"] == "-")
                            {
                                EditorUtility.DisplayDialog("Error", "Fill out all fields.", "OK");
                                DestroyImmediate(temp);
                                break;
                            }
                            if (vectorsAndUnityTypes["[newTransform]sYnewEntryValue0"] == "" || vectorsAndUnityTypes["[newTransform]sYnewEntryValue0"] == "-")
                            {
                                EditorUtility.DisplayDialog("Error", "Fill out all fields.", "OK");
                                DestroyImmediate(temp);
                                break;
                            }
                            if (vectorsAndUnityTypes["[newTransform]sZnewEntryValue0"] == "" || vectorsAndUnityTypes["[newTransform]sZnewEntryValue0"] == "-")
                            {
                                EditorUtility.DisplayDialog("Error", "Fill out all fields.", "OK");
                                DestroyImmediate(temp);
                                break;
                            }
                            #endregion
                            tf.localScale = new Vector3(Operators.ToFloat(vectorsAndUnityTypes["[newTransform]sXnewEntryValue0"]), Operators.ToFloat(vectorsAndUnityTypes["[newTransform]sYnewEntryValue0"]), Operators.ToFloat(vectorsAndUnityTypes["[newTransform]sZnewEntryValue0"]));
                            EpicPrefs.SetTransform(newName, tf, false);
                            DestroyImmediate(temp);
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "A list with that name is already existing. Please chose a new one.", "OK");
                        }

                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();*/
                    #endregion
                    break;
            }
            #endregion
        }
        Separator();
        #endregion
        #region Integer Prefs
        if (IntDict.Count > 0)
        {
            EditorGUILayout.BeginHorizontal();
            showInt = EditorGUILayout.Foldout(showInt, "Integer Prefs", foldout);
            if (editInt)
            {
                if (!stringEditor.ContainsKey("[int]newEntry"))
                    stringEditor["[int]newEntry"] = "false";
                if (!stringEditor.ContainsKey("[int]newEntryCounter"))
                    stringEditor["[int]newEntryCounter"] = "0";
                if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    editInt = false;
                    foreach (KeyValuePair<string, int> i in intEditor)
                    {
                        if (!i.Key.Contains("[encryption]"))
                        {
                            EpicPrefs.SetInt(i.Key, i.Value, Convert.ToBoolean(intEditor["[encryption]" + i.Key]));
                        }
                    }
                    if (stringEditor["[int]newEntry"] == "true")
                    {
                        stringEditor["[int]newEntry"] = "false";
                        int newCounter = Convert.ToInt32(stringEditor["[int]newEntryCounter"]);
                        for (int n = 0; n < newCounter; n++)
                        {
                            stringEditor["[int]newEntryValue" + n] = Operators.ToInt(stringEditor["[int]newEntryValue" + n]).ToString();
                            if (stringEditor["[int]newEntryValue" + n] == "")
                                stringEditor["[int]newEntryValue" + n] = "0";
                            if (stringEditor["[int]newEntryKey" + n] != "")
                                EpicPrefs.SetInt(stringEditor["[int]newEntryKey" + n], Convert.ToInt32(stringEditor["[int]newEntryValue" + n]), Convert.ToBoolean(stringEditor["[int]newEntryEncryp" + n]));
                            stringEditor["[int]newEntryKey" + n] = "";
                            stringEditor["[int]newEntryValue" + n] = "";
                            stringEditor["[int]newEntryEncryp" + n] = "false";
                        }
                        stringEditor["[int]newEntryCounter"] = "0";
                    }
                    return;
                }
                if (GUILayout.Button(cB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    editInt = false;
                    stringEditor["[int]newEntry"] = "false";
                    int newCounter = Convert.ToInt32(stringEditor["[int]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        stringEditor["[int]newEntryKey" + n] = "";
                        stringEditor["[int]newEntryValue" + n] = "";
                        stringEditor["[int]newEntryEncryp" + n] = "false";
                    }
                    stringEditor["[int]newEntryCounter"] = "0";
                    intEditor = new Dictionary<string, int>();
                    return;
                }
            }
            else
            {
                if (GUILayout.Button(eB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    editInt = true;
                    showInt = true;
                }
            }
            EditorGUILayout.EndHorizontal();
            if (showInt)
            {
                GUILayout.Space(5);
                foreach (KeyValuePair<string, string> t in IntDict)
                {
                    if (!stringEditor.ContainsKey("[int]" + t.Key + "encrypt"))
                        stringEditor["[int]" + t.Key + "encrypt"] = t.Value;
                    if (!stringEditor.ContainsKey("[int]" + t.Key + "name"))
                        stringEditor["[int]" + t.Key + "name"] = t.Key;
                    if (!editInt)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        myLabelField(t.Key, IntDictValues[t.Key].ToString(), text);
                        if (GUILayout.Button(dB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            EpicPrefs.DeleteInt(t.Key, Convert.ToBoolean(t.Value));
                            return;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    else {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        if (!intEditor.ContainsKey(t.Key))
                            intEditor[t.Key] = IntDictValues[t.Key];
                        if (!intEditor.ContainsKey("[encryption]" + t.Key))
                            intEditor["[encryption]" + t.Key] = Convert.ToInt32(Convert.ToBoolean(t.Value));
                        intEditor[t.Key] = Operators.ToInt(myTextField(t.Key, intEditor[t.Key].ToString(), textBox));
                        EditorGUILayout.LabelField("AES Encryption : ", text, GUILayout.Width(100), GUILayout.Height(20));
                        GUIContent toggleControl;
                        if (Convert.ToBoolean(intEditor["[encryption]" + t.Key]))
                            toggleControl = new GUIContent(cM);
                        else
                            toggleControl = new GUIContent(uCM);
                        if (GUILayout.Button(toggleControl, GUIStyle.none, GUILayout.Width(15), GUILayout.Height(15)))
                        {
                            if (toggleControl.image == checkedButton)
                            {
                                toggleControl.image = uncheckedButton;
                                intEditor["[encryption]" + t.Key] = Convert.ToInt32(false);
                            }
                            else
                            {
                                toggleControl.image = checkedButton;
                                intEditor["[encryption]" + t.Key] = Convert.ToInt32(true);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                if (!stringEditor.ContainsKey("[int]newEntry"))
                    stringEditor["[int]newEntry"] = "false";
                if (stringEditor["[int]newEntry"] == "true")
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Space(20);
                    int newCounter = Convert.ToInt32(stringEditor["[int]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        if (!stringEditor.ContainsKey("[int]newEntryKey" + n))
                            stringEditor["[int]newEntryKey" + n] = "";
                        if (!stringEditor.ContainsKey("[int]newEntryValue" + n))
                            stringEditor["[int]newEntryValue" + n] = "";
                        if (!stringEditor.ContainsKey("[int]newEntryEncryp" + n))
                            stringEditor["[int]newEntryEncryp" + n] = "false";
                    }
                    stringEditor["[int]newEntryKey0"] = myTextField(stringEditor["[int]newEntryKey0"], textBox);
                    stringEditor["[int]newEntryValue0"] = myTextField(stringEditor["[int]newEntryValue0"], textBox);
                    stringEditor["[int]newEntryEncryp0"] = EncryptionToggle(stringEditor["[int]newEntryEncryp0"]);
                    GUILayout.EndHorizontal();
                    for (int n = 1; n < newCounter; n++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        stringEditor["[int]newEntryKey" + n] = myTextField(stringEditor["[int]newEntryKey" + n], textBox);
                        if(n == newCounter-1)
                            GUI.SetNextControlName("newIntField");
                        stringEditor["[int]newEntryValue" + n] = myTextField(stringEditor["[int]newEntryValue" + n], textBox);
                        stringEditor["[int]newEntryEncryp" + n] = EncryptionToggle(stringEditor["[int]newEntryEncryp" + n]);
                        GUILayout.EndHorizontal();
                    }
                    if (stringEditor["[int]newEntryKey" + (newCounter - 1)] != "" && stringEditor["[int]newEntryValue" + (newCounter - 1)] != "")
                    {
                        stringEditor["[int]newEntryCounter"] = (newCounter + 1).ToString();
                    }
                }
                if (editInt)
                {
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(40);
                    if (stringEditor["[int]newEntry"] != "true")
                    {
                        if (GUILayout.Button(aB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            stringEditor["[int]newEntryCounter"] = "1";
                            stringEditor["[int]newEntry"] = "true";
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
            Separator();
        }
        #endregion
        #region Long Prefs
        if (LongDict.Count > 0)
        {
            EditorGUILayout.BeginHorizontal();
            showLong = EditorGUILayout.Foldout(showLong, "Long Prefs", foldout);
            if (editLong)
            {
                if (!stringEditor.ContainsKey("[long]newEntry"))
                    stringEditor["[long]newEntry"] = "false";
                if (!stringEditor.ContainsKey("[long]newEntryCounter"))
                    stringEditor["[long]newEntryCounter"] = "0";
                if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    editLong = false;
                    foreach (KeyValuePair<string, long> i in longEditor)
                    {
                        if (!i.Key.Contains("[encryption]"))
                        {
                            EpicPrefs.SetLong(i.Key, i.Value, Convert.ToBoolean(longEditor["[encryption]" + i.Key]));
                        }
                    }
                    if (stringEditor["[long]newEntry"] == "true")
                    {
                        stringEditor["[long]newEntry"] = "false";
                        int newCounter = Convert.ToInt32(stringEditor["[long]newEntryCounter"]);
                        for (int n = 0; n < newCounter; n++)
                        {
                            stringEditor["[long]newEntryValue" + n] = Operators.ToLong(stringEditor["[long]newEntryValue" + n]).ToString();
                            if (stringEditor["[long]newEntryValue" + n] == "")
                                stringEditor["[long]newEntryValue" + n] = "0";
                            if (stringEditor["[long]newEntryKey" + n] != "")
                                EpicPrefs.SetLong(stringEditor["[long]newEntryKey" + n], Convert.ToInt64(stringEditor["[long]newEntryValue" + n]), Convert.ToBoolean(stringEditor["[long]newEntryEncryp" + n]));
                            stringEditor["[long]newEntryKey" + n] = "";
                            stringEditor["[long]newEntryValue" + n] = "";
                            stringEditor["[long]newEntryEncryp" + n] = "false";
                        }
                        stringEditor["[long]newEntryCounter"] = "0";
                    }
                    return;
                }
                if (GUILayout.Button(cB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    editLong = false;
                    stringEditor["[long]newEntry"] = "false";
                    int newCounter = Convert.ToInt32(stringEditor["[long]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        stringEditor["[long]newEntryKey" + n] = "";
                        stringEditor["[long]newEntryValue" + n] = "";
                        stringEditor["[long]newEntryEncryp" + n] = "false";
                    }
                    stringEditor["[long]newEntryCounter"] = "0";
                    longEditor = new Dictionary<string, long>();
                    return;
                }
            }
            else
            {
                if (GUILayout.Button(eB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    editLong = true;
                    showLong = true;
                }
            }
            EditorGUILayout.EndHorizontal();
            if (showLong)
            {
                GUILayout.Space(5);
                foreach (KeyValuePair<string, string> t in LongDict)
                {
                    if (!stringEditor.ContainsKey("[long]" + t.Key + "encrypt"))
                        stringEditor["[long]" + t.Key + "encrypt"] = t.Value;
                    if (!stringEditor.ContainsKey("[long]" + t.Key + "name"))
                        stringEditor["[long]" + t.Key + "name"] = t.Key;
                    if (!editLong)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        myLabelField(t.Key,LongDictValues[t.Key].ToString(), text);
                        if (GUILayout.Button(dB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            EpicPrefs.DeleteLong(t.Key, Convert.ToBoolean(t.Value));
                            return;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    else {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        if (!longEditor.ContainsKey(t.Key))
                            longEditor[t.Key] = LongDictValues[t.Key];//EpicPrefs.GetLong(t.Key, Convert.ToBoolean(t.Value));
                        if (!longEditor.ContainsKey("[encryption]" + t.Key))
                            longEditor["[encryption]" + t.Key] = Convert.ToInt64(Convert.ToBoolean(t.Value));
                        longEditor[t.Key] = Operators.ToLong(myTextField(t.Key, longEditor[t.Key].ToString(), textBox));
                        EditorGUILayout.LabelField("AES Encryption : ", text, GUILayout.Width(100), GUILayout.Height(20));
                        GUIContent toggleControl;
                        if (Convert.ToBoolean(longEditor["[encryption]" + t.Key]))
                            toggleControl = new GUIContent(cM);
                        else
                            toggleControl = new GUIContent(uCM);
                        if (GUILayout.Button(toggleControl, GUIStyle.none, GUILayout.Width(15), GUILayout.Height(15)))
                        {
                            if (toggleControl.image == checkedButton)
                            {
                                toggleControl.image = uncheckedButton;
                                longEditor["[encryption]" + t.Key] = Convert.ToInt64(false);
                            }
                            else
                            {
                                toggleControl.image = checkedButton;
                                longEditor["[encryption]" + t.Key] = Convert.ToInt64(true);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                if (!stringEditor.ContainsKey("[long]newEntry"))
                    stringEditor["[long]newEntry"] = "false";
                if (stringEditor["[long]newEntry"] == "true")
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Space(20);
                    int newCounter = Convert.ToInt32(stringEditor["[long]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        if (!stringEditor.ContainsKey("[long]newEntryKey" + n))
                            stringEditor["[long]newEntryKey" + n] = "";
                        if (!stringEditor.ContainsKey("[long]newEntryValue" + n))
                            stringEditor["[long]newEntryValue" + n] = "";
                        if (!stringEditor.ContainsKey("[long]newEntryEncryp" + n))
                            stringEditor["[long]newEntryEncryp" + n] = "false";

                    }
                    stringEditor["[long]newEntryKey0"] = myTextField(stringEditor["[long]newEntryKey0"], textBox);
                    stringEditor["[long]newEntryValue0"] = myTextField(stringEditor["[long]newEntryValue0"], textBox);
                    stringEditor["[long]newEntryEncryp0"] = EncryptionToggle(stringEditor["[long]newEntryEncryp0"]);
                    GUILayout.EndHorizontal();
                    for (int n = 1; n < newCounter; n++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        stringEditor["[long]newEntryKey" + n] = myTextField(stringEditor["[long]newEntryKey" + n], textBox);
                        stringEditor["[long]newEntryValue" + n] = myTextField(stringEditor["[long]newEntryValue" + n], textBox);
                        stringEditor["[long]newEntryEncryp" + n] = EncryptionToggle(stringEditor["[long]newEntryEncryp" + n]);
                        GUILayout.EndHorizontal();
                    }
                    if (stringEditor["[long]newEntryKey" + (newCounter - 1)] != "" && stringEditor["[long]newEntryValue" + (newCounter - 1)] != "")
                    {
                        stringEditor["[long]newEntryCounter"] = (newCounter + 1).ToString();
                    }
                }
                if (editLong)
                {
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(40);
                    if (stringEditor["[long]newEntry"] != "true")
                    {
                        if (GUILayout.Button(aB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            stringEditor["[long]newEntryCounter"] = "1";
                            stringEditor["[long]newEntry"] = "true";
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
            Separator();
        }
        #endregion
        #region String Prefs
        if (StringDict.Count > 0)
        {
            EditorGUILayout.BeginHorizontal();
            showString = EditorGUILayout.Foldout(showString, "String Prefs", foldout);
            if (editString)
            {
                if (!stringEditor.ContainsKey("[string]newEntry"))
                    stringEditor["[string]newEntry"] = "false";
                if (!stringEditor.ContainsKey("[string]newEntryCounter"))
                    stringEditor["[string]newEntryCounter"] = "0";
                if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    editString = false;
                    foreach (KeyValuePair<string, string> i in stringStringEditor)
                    {
                        if (!i.Key.Contains("[encryption]"))
                        {
                            EpicPrefs.SetString(i.Key, i.Value, Convert.ToBoolean(stringStringEditor["[encryption]" + i.Key]));
                        }
                    }
                    if (stringEditor["[string]newEntry"] == "true")
                    {
                        stringEditor["[string]newEntry"] = "false";
                        int newCounter = Convert.ToInt32(stringEditor["[string]newEntryCounter"]);
                        for (int n = 0; n < newCounter; n++)
                        {
                            stringEditor["[string]newEntryValue" + n] = stringEditor["[string]newEntryValue" + n];
                            if (stringEditor["[string]newEntryValue" + n] == "")
                                stringEditor["[string]newEntryValue" + n] = "0";
                            if (stringEditor["[string]newEntryKey" + n] != "")
                                EpicPrefs.SetString(stringEditor["[string]newEntryKey" + n], stringEditor["[string]newEntryValue" + n], Convert.ToBoolean(stringEditor["[string]newEntryEncryp" + n]));
                            stringEditor["[string]newEntryKey" + n] = "";
                            stringEditor["[string]newEntryValue" + n] = "";
                            stringEditor["[string]newEntryEncryp" + n] = "false";
                        }
                        stringEditor["[string]newEntryCounter"] = "0";
                    }
                    return;
                }
                if (GUILayout.Button(cB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    editString = false;
                    stringEditor["[string]newEntry"] = "false";
                    int newCounter = Convert.ToInt32(stringEditor["[string]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        stringEditor["[string]newEntryKey" + n] = "";
                        stringEditor["[string]newEntryValue" + n] = "";
                        stringEditor["[string]newEntryEncryp" + n] = "false";
                    }
                    stringEditor["[string]newEntryCounter"] = "0";
                    stringStringEditor = new Dictionary<string, string>();
                    return;
                }
            }
            else
            {
                if (GUILayout.Button(eB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    editString = true;
                    showString = true;
                }
            }
            EditorGUILayout.EndHorizontal();
            if (showString)
            {
                GUILayout.Space(5);
                foreach (KeyValuePair<string, string> t in StringDict)
                {
                    if (!stringEditor.ContainsKey("[string]" + t.Key + "encrypt"))
                        stringEditor["[string]" + t.Key + "encrypt"] = t.Value;
                    if (!stringEditor.ContainsKey("[string]" + t.Key + "name"))
                        stringEditor["[string]" + t.Key + "name"] = t.Key;
                    if (!editString)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        myLabelField(t.Key, StringDictValues[t.Key], text);
                        if (GUILayout.Button(dB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            EpicPrefs.DeleteString(t.Key, Convert.ToBoolean(t.Value));
                            return;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    else {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        if (!stringStringEditor.ContainsKey(t.Key))
                            stringStringEditor[t.Key] = StringDictValues[t.Key];//EpicPrefs.GetString(t.Key, Convert.ToBoolean(t.Value));
                        if (!stringStringEditor.ContainsKey("[encryption]" + t.Key))
                            stringStringEditor["[encryption]" + t.Key] = Convert.ToString(Convert.ToBoolean(t.Value));
                        stringStringEditor[t.Key] = myTextField(t.Key, stringStringEditor[t.Key].ToString(), textBox);
                        EditorGUILayout.LabelField("AES Encryption : ", text, GUILayout.Width(100), GUILayout.Height(20));
                        GUIContent toggleControl;
                        if (Convert.ToBoolean(stringStringEditor["[encryption]" + t.Key]))
                            toggleControl = new GUIContent(cM);
                        else
                            toggleControl = new GUIContent(uCM);
                        if (GUILayout.Button(toggleControl, GUIStyle.none, GUILayout.Width(15), GUILayout.Height(15)))
                        {
                            if (toggleControl.image == checkedButton)
                            {
                                toggleControl.image = uncheckedButton;
                                stringStringEditor["[encryption]" + t.Key] = Convert.ToString(false);
                            }
                            else
                            {
                                toggleControl.image = checkedButton;
                                stringStringEditor["[encryption]" + t.Key] = Convert.ToString(true);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                if (!stringEditor.ContainsKey("[string]newEntry"))
                    stringEditor["[string]newEntry"] = "false";
                if (stringEditor["[string]newEntry"] == "true")
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Space(20);
                    int newCounter = Convert.ToInt32(stringEditor["[string]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        if (!stringEditor.ContainsKey("[string]newEntryKey" + n))
                            stringEditor["[string]newEntryKey" + n] = "";
                        if (!stringEditor.ContainsKey("[string]newEntryValue" + n))
                            stringEditor["[string]newEntryValue" + n] = "";
                        if (!stringEditor.ContainsKey("[string]newEntryEncryp" + n))
                            stringEditor["[string]newEntryEncryp" + n] = "false";

                    }
                    stringEditor["[string]newEntryKey0"] = myTextField(stringEditor["[string]newEntryKey0"], textBox);
                    stringEditor["[string]newEntryValue0"] = myTextField(stringEditor["[string]newEntryValue0"], textBox);
                    stringEditor["[string]newEntryEncryp0"] = EncryptionToggle(stringEditor["[string]newEntryEncryp0"]);
                    GUILayout.EndHorizontal();
                    for (int n = 1; n < newCounter; n++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        stringEditor["[string]newEntryKey" + n] = myTextField(stringEditor["[string]newEntryKey" + n], textBox);
                        stringEditor["[string]newEntryValue" + n] = myTextField(stringEditor["[string]newEntryValue" + n], textBox);
                        stringEditor["[string]newEntryEncryp" + n] = EncryptionToggle(stringEditor["[string]newEntryEncryp" + n]);
                        GUILayout.EndHorizontal();
                    }
                    if (stringEditor["[string]newEntryKey" + (newCounter - 1)] != "" && stringEditor["[string]newEntryValue" + (newCounter - 1)] != "")
                    {
                        stringEditor["[string]newEntryCounter"] = (newCounter + 1).ToString();
                    }
                }
                if (editString)
                {
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(40);
                    if (stringEditor["[string]newEntry"] != "true")
                    {
                        if (GUILayout.Button(aB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            stringEditor["[string]newEntryCounter"] = "1";
                            stringEditor["[string]newEntry"] = "true";
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
            Separator();
        }
        #endregion
        #region Bool Prefs
        if (BoolDict.Count > 0)
        {
            EditorGUILayout.BeginHorizontal();
            showBool = EditorGUILayout.Foldout(showBool, "Boolean Prefs", foldout);
            if (editBool)
            {
                if (!stringEditor.ContainsKey("[bool]newEntry"))
                    stringEditor["[bool]newEntry"] = "false";
                if (!stringEditor.ContainsKey("[bool]newEntryCounter"))
                    stringEditor["[bool]newEntryCounter"] = "0";
                if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    editBool = false;
                    foreach (KeyValuePair<string, bool> i in boolEditor)
                    {
                        if (!i.Key.Contains("[encryption]"))
                        {
                            EpicPrefs.SetBool(i.Key, i.Value, boolEditor["[encryption]" + i.Key]);
                        }
                    }
                    if (stringEditor["[bool]newEntry"] == "true")
                    {
                        stringEditor["[bool]newEntry"] = "false";
                        int newCounter = Convert.ToInt32(stringEditor["[bool]newEntryCounter"]);
                        for (int n = 0; n < newCounter; n++)
                        {
                            stringEditor["[bool]newEntryValue" + n] = stringEditor["[bool]newEntryValue" + n];
                            if (stringEditor["[bool]newEntryValue" + n] == "")
                                stringEditor["[bool]newEntryValue" + n] = "0";
                            if (stringEditor["[bool]newEntryKey" + n] != "")
                                EpicPrefs.SetBool(stringEditor["[bool]newEntryKey" + n], Operators.ToBool(stringEditor["[bool]newEntryValue" + n]), Convert.ToBoolean(stringEditor["[bool]newEntryEncryp" + n]));
                            stringEditor["[bool]newEntryKey" + n] = "";
                            stringEditor["[bool]newEntryValue" + n] = "";
                            stringEditor["[bool]newEntryEncryp" + n] = "false";
                        }
                        stringEditor["[bool]newEntryCounter"] = "0";
                    }
                    return;
                }
                if (GUILayout.Button(cB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    editBool = false;
                    stringEditor["[bool]newEntry"] = "false";
                    int newCounter = Convert.ToInt32(stringEditor["[bool]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        stringEditor["[bool]newEntryKey" + n] = "";
                        stringEditor["[bool]newEntryValue" + n] = "";
                        stringEditor["[bool]newEntryEncryp" + n] = "false";
                    }
                    stringEditor["[bool]newEntryCounter"] = "0";
                    boolEditor = new Dictionary<string, bool>();
                    return;
                }
            }
            else
            {
                if (GUILayout.Button(eB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    editBool = true;
                    showBool = true;
                }
            }
            EditorGUILayout.EndHorizontal();
            if (showBool)
            {
                GUILayout.Space(5);
                foreach (KeyValuePair<string, string> t in BoolDict)
                {
                    if (!stringEditor.ContainsKey("[bool]" + t.Key + "encrypt"))
                        stringEditor["[bool]" + t.Key + "encrypt"] = t.Value;
                    if (!stringEditor.ContainsKey("[bool]" + t.Key + "name"))
                        stringEditor["[bool]" + t.Key + "name"] = t.Key;
                    if (!editBool)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        myLabelField(t.Key, BoolDictValues[t.Key].ToString(), text);
                        if (GUILayout.Button(dB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            EpicPrefs.DeleteBool(t.Key, Convert.ToBoolean(t.Value));
                            return;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    else {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        if (!boolEditor.ContainsKey(t.Key))
                            boolEditor[t.Key] = BoolDictValues[t.Key];//EpicPrefs.GetBool(t.Key, Convert.ToBoolean(t.Value));
                        if (!boolEditor.ContainsKey("[encryption]" + t.Key))
                            boolEditor["[encryption]" + t.Key] = Convert.ToBoolean(t.Value);
                        boolEditor[t.Key] = Operators.ToBool(myTextField(t.Key, boolEditor[t.Key].ToString(), textBox));
                        EditorGUILayout.LabelField("AES Encryption : ", text, GUILayout.Width(100), GUILayout.Height(20));
                        GUIContent toggleControl;
                        if (Convert.ToBoolean(boolEditor["[encryption]" + t.Key]))
                            toggleControl = new GUIContent(cM);
                        else
                            toggleControl = new GUIContent(uCM);
                        if (GUILayout.Button(toggleControl, GUIStyle.none, GUILayout.Width(15), GUILayout.Height(15)))
                        {
                            if (toggleControl.image == checkedButton)
                            {
                                toggleControl.image = uncheckedButton;
                                boolEditor["[encryption]" + t.Key] = false;
                            }
                            else
                            {
                                toggleControl.image = checkedButton;
                                boolEditor["[encryption]" + t.Key] = true;
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                if (!stringEditor.ContainsKey("[bool]newEntry"))
                    stringEditor["[bool]newEntry"] = "false";
                if (stringEditor["[bool]newEntry"] == "true")
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Space(20);
                    int newCounter = Convert.ToInt32(stringEditor["[bool]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        if (!stringEditor.ContainsKey("[bool]newEntryKey" + n))
                            stringEditor["[bool]newEntryKey" + n] = "";
                        if (!stringEditor.ContainsKey("[bool]newEntryValue" + n))
                            stringEditor["[bool]newEntryValue" + n] = "";
                        if (!stringEditor.ContainsKey("[bool]newEntryEncryp" + n))
                            stringEditor["[bool]newEntryEncryp" + n] = "false";

                    }
                    stringEditor["[bool]newEntryKey0"] = myTextField(stringEditor["[bool]newEntryKey0"], textBox);
                    stringEditor["[bool]newEntryValue0"] = myTextField(stringEditor["[bool]newEntryValue0"], textBox);
                    stringEditor["[bool]newEntryEncryp0"] = EncryptionToggle(stringEditor["[bool]newEntryEncryp0"]);
                    GUILayout.EndHorizontal();
                    for (int n = 1; n < newCounter; n++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        stringEditor["[bool]newEntryKey" + n] = myTextField(stringEditor["[bool]newEntryKey" + n], textBox);
                        stringEditor["[bool]newEntryValue" + n] = myTextField(stringEditor["[bool]newEntryValue" + n], textBox);
                        stringEditor["[bool]newEntryEncryp" + n] = EncryptionToggle(stringEditor["[bool]newEntryEncryp" + n]);
                        GUILayout.EndHorizontal();
                    }
                    if (stringEditor["[bool]newEntryKey" + (newCounter - 1)] != "" && stringEditor["[bool]newEntryValue" + (newCounter - 1)] != "")
                    {
                        stringEditor["[bool]newEntryCounter"] = (newCounter + 1).ToString();
                    }
                }
                if (editBool)
                {
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(40);
                    if (stringEditor["[bool]newEntry"] != "true")
                    {
                        if (GUILayout.Button(aB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            stringEditor["[bool]newEntryCounter"] = "1";
                            stringEditor["[bool]newEntry"] = "true";
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
            Separator();
        }
        #endregion
        #region Float Prefs
        if (FloatDict.Count > 0)
        {
            EditorGUILayout.BeginHorizontal();
            showFloat = EditorGUILayout.Foldout(showFloat, "Float Prefs", foldout);
            if (editFloat)
            {
                if (!stringEditor.ContainsKey("[float]newEntry"))
                    stringEditor["[float]newEntry"] = "false";
                if (!stringEditor.ContainsKey("[float]newEntryCounter"))
                    stringEditor["[float]newEntryCounter"] = "0";
                if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    editFloat = false;
                    foreach (KeyValuePair<string, float> i in floatEditor)
                    {
                        if (!i.Key.Contains("[encryption]"))
                        {
                            EpicPrefs.SetFloat(i.Key, i.Value, Convert.ToBoolean(floatEditor["[encryption]" + i.Key]));
                        }
                    }
                    if (stringEditor["[float]newEntry"] == "true")
                    {
                        stringEditor["[float]newEntry"] = "false";
                        int newCounter = Convert.ToInt32(stringEditor["[float]newEntryCounter"]);
                        for (int n = 0; n < newCounter; n++)
                        {
                            stringEditor["[float]newEntryValue" + n] = stringEditor["[float]newEntryValue" + n];
                            if (stringEditor["[float]newEntryValue" + n] == "")
                                stringEditor["[float]newEntryValue" + n] = "0";
                            if (stringEditor["[float]newEntryKey" + n] != "")
                                EpicPrefs.SetFloat(stringEditor["[float]newEntryKey" + n], Operators.ToFloat(stringEditor["[float]newEntryValue" + n]), Convert.ToBoolean(stringEditor["[float]newEntryEncryp" + n]));
                            stringEditor["[float]newEntryKey" + n] = "";
                            stringEditor["[float]newEntryValue" + n] = "";
                            stringEditor["[float]newEntryEncryp" + n] = "false";
                        }
                        stringEditor["[float]newEntryCounter"] = "0";
                    }
                    return;
                }
                if (GUILayout.Button(cB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    editFloat = false;
                    stringEditor["[float]newEntry"] = "false";
                    int newCounter = Convert.ToInt32(stringEditor["[float]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        stringEditor["[float]newEntryKey" + n] = "";
                        stringEditor["[float]newEntryValue" + n] = "";
                        stringEditor["[float]newEntryEncryp" + n] = "false";
                    }
                    stringEditor["[float]newEntryCounter"] = "0";
                    floatEditor = new Dictionary<string, float>();
                    return;
                }
            }
            else
            {
                if (GUILayout.Button(eB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    editFloat = true;
                    showFloat = true;
                }
            }
            EditorGUILayout.EndHorizontal();
            if (showFloat)
            {
                GUILayout.Space(5);
                foreach (KeyValuePair<string, string> t in FloatDict)
                {
                    if (!stringEditor.ContainsKey("[float]" + t.Key + "encrypt"))
                        stringEditor["[float]" + t.Key + "encrypt"] = t.Value;
                    if (!stringEditor.ContainsKey("[float]" + t.Key + "name"))
                        stringEditor["[float]" + t.Key + "name"] = t.Key;
                    if (!editFloat)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        myLabelField(t.Key, FloatDictValues[t.Key].ToString(), text);
                        if (GUILayout.Button(dB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            EpicPrefs.DeleteFloat(t.Key, Convert.ToBoolean(t.Value));
                            return;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    else {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        if (!floatEditor.ContainsKey(t.Key))
                            floatEditor[t.Key] = FloatDictValues[t.Key];//EpicPrefs.GetFloat(t.Key, Convert.ToBoolean(t.Value));
                        if (!floatEditor.ContainsKey("[encryption]" + t.Key))
                            floatEditor["[encryption]" + t.Key] = Convert.ToSingle(Convert.ToBoolean(t.Value));
                        floatEditor[t.Key] = Operators.ToFloat(myTextField(t.Key, floatEditor[t.Key].ToString(), textBox));
                        EditorGUILayout.LabelField("AES Encryption : ", text, GUILayout.Width(100), GUILayout.Height(20));
                        GUIContent toggleControl;
                        if (Convert.ToBoolean(floatEditor["[encryption]" + t.Key]))
                            toggleControl = new GUIContent(cM);
                        else
                            toggleControl = new GUIContent(uCM);
                        if (GUILayout.Button(toggleControl, GUIStyle.none, GUILayout.Width(15), GUILayout.Height(15)))
                        {
                            if (toggleControl.image == checkedButton)
                            {
                                toggleControl.image = uncheckedButton;
                                floatEditor["[encryption]" + t.Key] = Convert.ToSingle(false);
                            }
                            else
                            {
                                toggleControl.image = checkedButton;
                                floatEditor["[encryption]" + t.Key] = Convert.ToSingle(true);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                if (!stringEditor.ContainsKey("[float]newEntry"))
                    stringEditor["[float]newEntry"] = "false";
                if (stringEditor["[float]newEntry"] == "true")
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Space(20);
                    int newCounter = Convert.ToInt32(stringEditor["[float]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        if (!stringEditor.ContainsKey("[float]newEntryKey" + n))
                            stringEditor["[float]newEntryKey" + n] = "";
                        if (!stringEditor.ContainsKey("[float]newEntryValue" + n))
                            stringEditor["[float]newEntryValue" + n] = "";
                        if (!stringEditor.ContainsKey("[float]newEntryEncryp" + n))
                            stringEditor["[float]newEntryEncryp" + n] = "false";

                    }
                    stringEditor["[float]newEntryKey0"] = myTextField(stringEditor["[float]newEntryKey0"], textBox);
                    stringEditor["[float]newEntryValue0"] = myTextField(stringEditor["[float]newEntryValue0"], textBox);
                    stringEditor["[float]newEntryEncryp0"] = EncryptionToggle(stringEditor["[float]newEntryEncryp0"]);
                    GUILayout.EndHorizontal();
                    for (int n = 1; n < newCounter; n++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        stringEditor["[float]newEntryKey" + n] = myTextField(stringEditor["[float]newEntryKey" + n], textBox);
                        stringEditor["[float]newEntryValue" + n] = myTextField(stringEditor["[float]newEntryValue" + n], textBox);
                        stringEditor["[float]newEntryEncryp" + n] = EncryptionToggle(stringEditor["[float]newEntryEncryp" + n]);
                        GUILayout.EndHorizontal();
                    }
                    if (stringEditor["[float]newEntryKey" + (newCounter - 1)] != "" && stringEditor["[float]newEntryValue" + (newCounter - 1)] != "")
                    {
                        stringEditor["[float]newEntryCounter"] = (newCounter + 1).ToString();
                    }
                }
                if (editFloat)
                {
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(40);
                    if (stringEditor["[float]newEntry"] != "true")
                    {
                        if (GUILayout.Button(aB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            stringEditor["[float]newEntryCounter"] = "1";
                            stringEditor["[float]newEntry"] = "true";
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
            Separator();
        }
        #endregion
        #region Double Prefs
        if (DoubleDict.Count > 0)
        {
            EditorGUILayout.BeginHorizontal();
            showDouble = EditorGUILayout.Foldout(showDouble, "Double Prefs", foldout);
            if (editDouble)
            {
                if (!stringEditor.ContainsKey("[double]newEntry"))
                    stringEditor["[double]newEntry"] = "false";
                if (!stringEditor.ContainsKey("[double]newEntryCounter"))
                    stringEditor["[double]newEntryCounter"] = "0";
                if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    editDouble = false;
                    foreach (KeyValuePair<string, double> i in doubleEditor)
                    {
                        if (!i.Key.Contains("[encryption]"))
                        {
                            EpicPrefs.SetDouble(i.Key, i.Value, Convert.ToBoolean(doubleEditor["[encryption]" + i.Key]));
                        }
                    }
                    if (stringEditor["[double]newEntry"] == "true")
                    {
                        stringEditor["[double]newEntry"] = "false";
                        int newCounter = Convert.ToInt32(stringEditor["[double]newEntryCounter"]);
                        for (int n = 0; n < newCounter; n++)
                        {
                            stringEditor["[double]newEntryValue" + n] = stringEditor["[double]newEntryValue" + n];
                            if (stringEditor["[double]newEntryValue" + n] == "")
                                stringEditor["[double]newEntryValue" + n] = "0";
                            if(stringEditor["[double]newEntryKey" + n] != "")
                                EpicPrefs.SetDouble(stringEditor["[double]newEntryKey" + n], Operators.ToDouble(stringEditor["[double]newEntryValue" + n]), Convert.ToBoolean(stringEditor["[double]newEntryEncryp" + n]));
                            stringEditor["[double]newEntryKey" + n] = "";
                            stringEditor["[double]newEntryValue" + n] = "";
                            stringEditor["[double]newEntryEncryp" + n] = "false";
                        }
                        stringEditor["[double]newEntryCounter"] = "0";
                    }
                    return;
                }
                if (GUILayout.Button(cB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    editDouble = false;
                    stringEditor["[double]newEntry"] = "false";
                    int newCounter = Convert.ToInt32(stringEditor["[double]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        stringEditor["[double]newEntryKey" + n] = "";
                        stringEditor["[double]newEntryValue" + n] = "";
                        stringEditor["[double]newEntryEncryp" + n] = "false";
                    }
                    stringEditor["[double]newEntryCounter"] = "0";
                    doubleEditor = new Dictionary<string, double>();
                    return;
                }
            }
            else
            {
                if (GUILayout.Button(eB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    editDouble = true;
                    showDouble = true;
                }
            }
            EditorGUILayout.EndHorizontal();
            if (showDouble)
            {
                GUILayout.Space(5);
                foreach (KeyValuePair<string, string> t in DoubleDict)
                {
                    if (!stringEditor.ContainsKey("[double]" + t.Key + "encrypt"))
                        stringEditor["[double]" + t.Key + "encrypt"] = t.Value;
                    if (!stringEditor.ContainsKey("[double]" + t.Key + "name"))
                        stringEditor["[double]" + t.Key + "name"] = t.Key;
                    if (!editDouble)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        myLabelField(t.Key, DoubleDictValues[t.Key].ToString(), text);
                        if (GUILayout.Button(dB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            EpicPrefs.DeleteDouble(t.Key, Convert.ToBoolean(t.Value));
                            return;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    else {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        if (!doubleEditor.ContainsKey(t.Key))
                            doubleEditor[t.Key] = DoubleDictValues[t.Key];//EpicPrefs.GetDouble(t.Key, Convert.ToBoolean(t.Value));
                        if (!doubleEditor.ContainsKey("[encryption]" + t.Key))
                            doubleEditor["[encryption]" + t.Key] = Operators.ToDouble(Convert.ToBoolean(t.Value).ToString());
                        stringEditor["[double]" + t.Key] = myTextField(t.Key, doubleEditor[t.Key].ToString(), textBox);
                        doubleEditor[t.Key] = Operators.ToDouble(stringEditor["[double]" + t.Key]);
                        EditorGUILayout.LabelField("AES Encryption : ", text, GUILayout.Width(100), GUILayout.Height(20));
                        GUIContent toggleControl;
                        if (Convert.ToBoolean(doubleEditor["[encryption]" + t.Key]))
                            toggleControl = new GUIContent(cM);
                        else
                            toggleControl = new GUIContent(uCM);
                        if (GUILayout.Button(toggleControl, GUIStyle.none, GUILayout.Width(15), GUILayout.Height(15)))
                        {
                            if (toggleControl.image == checkedButton)
                            {
                                toggleControl.image = uncheckedButton;
                                doubleEditor["[encryption]" + t.Key] = Convert.ToDouble(false);
                            }
                            else
                            {
                                toggleControl.image = checkedButton;
                                doubleEditor["[encryption]" + t.Key] = Convert.ToDouble(true);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                if (!stringEditor.ContainsKey("[double]newEntry"))
                    stringEditor["[double]newEntry"] = "false";
                if (stringEditor["[double]newEntry"] == "true")
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.Space(20);
                    int newCounter = Convert.ToInt32(stringEditor["[double]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        if (!stringEditor.ContainsKey("[double]newEntryKey" + n))
                            stringEditor["[double]newEntryKey" + n] = "";
                        if (!stringEditor.ContainsKey("[double]newEntryValue" + n))
                            stringEditor["[double]newEntryValue" + n] = "";
                        if (!stringEditor.ContainsKey("[double]newEntryEncryp" + n))
                            stringEditor["[double]newEntryEncryp" + n] = "false";

                    }
                    stringEditor["[double]newEntryKey0"] = myTextField(stringEditor["[double]newEntryKey0"], textBox);
                    stringEditor["[double]newEntryValue0"] = myTextField(stringEditor["[double]newEntryValue0"], textBox);
                    stringEditor["[double]newEntryEncryp0"] = EncryptionToggle(stringEditor["[double]newEntryEncryp0"]);
                    GUILayout.EndHorizontal();
                    for (int n = 1; n < newCounter; n++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        stringEditor["[double]newEntryKey" + n] = myTextField(stringEditor["[double]newEntryKey" + n], textBox);
                        stringEditor["[double]newEntryValue" + n] = myTextField(stringEditor["[double]newEntryValue" + n], textBox);
                        stringEditor["[double]newEntryEncryp" + n] = EncryptionToggle(stringEditor["[double]newEntryEncryp" + n]);
                        GUILayout.EndHorizontal();
                    }
                    if (stringEditor["[double]newEntryKey" + (newCounter - 1)] != "" && stringEditor["[double]newEntryValue" + (newCounter - 1)] != "")
                    {
                        stringEditor["[double]newEntryCounter"] = (newCounter + 1).ToString();
                    }
                }
                if (editDouble)
                {
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(40);
                    if (stringEditor["[double]newEntry"] != "true")
                    {
                        if (GUILayout.Button(aB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            stringEditor["[double]newEntryCounter"] = "1";
                            stringEditor["[double]newEntry"] = "true";
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
            Separator();
        }
        #endregion
        #region Int Dict Prefs
        if (DictIntDict.Count > 0)
        {
            showDictI = EditorGUILayout.Foldout(showDictI, "Integer Dictionary Prefs", foldout);
            if (showDictI)
            {
                GUILayout.Space(5);
                int counter = 0;
                foreach (KeyValuePair<string, string> t in DictIntDict)
                {
                    if (!stringEditor.ContainsKey(t.Key + "encrypt"))
                        stringEditor[t.Key + "encrypt"] = t.Value;
                    if (!stringEditor.ContainsKey(t.Key + "name"))
                        stringEditor[t.Key + "name"] = t.Key;
                    Dictionary<string, int> tDict = new Dictionary<string,int>(DictIntDictValues[t.Key]);//EpicPrefs.GetDictStringInt(t.Key, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                    Dictionary<string, int> nDict = new Dictionary<string,int>(DictIntDictValues[t.Key]);//EpicPrefs.GetDictStringInt(t.Key, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    bool applyDict = false;
                    if (!stringEditor.ContainsKey(t.Key + "newEntry"))
                        stringEditor[t.Key + "newEntry"] = "false";
                    showSubDictI[counter] = EditorGUILayout.Foldout(showSubDictI[counter], "");
                    if (editSubDictI[counter])
                    {
                        stringEditor[t.Key + "name"] = myTextField(stringEditor[t.Key + "name"], textBox);
                    }
                    else
                    {
                        myLabelField(t.Key);
                    }
                    bool encryptedCheck = Convert.ToBoolean(stringEditor[t.Key + "encrypt"]);
                    bool encryptionChanged = false;
                    //stringEditor[t.Key + "encrypt"] = EditorGUILayout.Toggle("AES Encryption : ", Convert.ToBoolean(stringEditor[t.Key + "encrypt"])).ToString();
                    stringEditor[t.Key + "encrypt"] = EncryptionToggle(stringEditor[t.Key + "encrypt"]);
                    if (Convert.ToBoolean(stringEditor[t.Key + "encrypt"]) != encryptedCheck)
                        if (EditorUtility.DisplayDialog("Change encryption",
                "Are you sure to change the encryption settings of " + t.Key
                + " ? You will lose all unsaved changes.", "Yes", "No"))
                        {
                            encryptionChanged = true;
                        }
                        else
                        {
                            stringEditor[t.Key + "encrypt"] = encryptedCheck.ToString();
                        }
                    if (editSubDictI[counter])
                    {
                        if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            applyDict = true;
                        }
                        if (GUILayout.Button(cB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            editSubDictI[counter] = false;
                            stringEditor[t.Key + "newEntry"] = "false";
                            if (!stringEditor.ContainsKey(t.Key + "newEntryCounter"))
                                stringEditor[t.Key + "newEntryCounter"] = "0";
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                //if (stringEditor[t.Key + "newEntryKey" + n] != "")
                                //    nDict[stringEditor[t.Key + "newEntryKey" + n]] = Convert.ToInt32(stringEditor[t.Key + "newEntryValue" + n]);
                                stringEditor[t.Key + "newEntryKey" + n] = "";
                                stringEditor[t.Key + "newEntryValue" + n] = "";
                            }
                            stringEditor[t.Key + "newEntryCounter"] = "0";
                            return;
                        }
                    }
                    else
                    {
                        if (GUILayout.Button(eB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            editSubDictI[counter] = true;
                            showSubDictI[counter] = true;
                        }
                        if (GUILayout.Button(dB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            if (EditorUtility.DisplayDialog("Delete",
                "Are you sure to delete " + t.Key
                + " permanently ?", "Yes", "No"))
                                EpicPrefs.DeleteEditorPrefs(t.Key, Serializer.SerializationTypes.DictI, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            return;
                        }
                    }
                    GUILayout.EndHorizontal();
                    if (showSubDictI[counter])
                    {
                        foreach (KeyValuePair<string, int> pair in tDict)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.Space(20);
                            if (editSubDictI[counter])
                            {
                                GUILayout.Label(pair.Key, text);
                                if (!stringEditor.ContainsKey(t.Key + pair.Key))
                                    stringEditor[t.Key + pair.Key] = pair.Value.ToString();
                                string _s = myTextField(stringEditor[t.Key + pair.Key], textBox);
                                if (_s != "")
                                {
                                    stringEditor[t.Key + pair.Key] = Operators.ToInt(_s).ToString();
                                }
                                if (stringEditor[t.Key + pair.Key] == "")
                                    stringEditor[t.Key + pair.Key] = "0";
                                if (Operators.IsInteger(stringEditor[t.Key + pair.Key]))
                                    nDict[pair.Key] = Convert.ToInt32(stringEditor[t.Key + pair.Key]);
                            }
                            else
                            {
                                myLabelField(pair.Key, pair.Value.ToString());
                                if (GUILayout.Button(dB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                                {
                                    nDict.Remove(pair.Key);
                                    applyDict = true;
                                }
                            }
                            GUILayout.EndHorizontal();
                        }
                        if (stringEditor[t.Key + "newEntry"] == "true")
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.Space(20);
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                if (!stringEditor.ContainsKey(t.Key + "newEntryKey" + n))
                                    stringEditor[t.Key + "newEntryKey" + n] = "";
                                if (!stringEditor.ContainsKey(t.Key + "newEntryValue" + n))
                                    stringEditor[t.Key + "newEntryValue" + n] = "";

                            }
                            stringEditor[t.Key + "newEntryKey0"] = myTextField(stringEditor[t.Key + "newEntryKey0"], textBox);
                            stringEditor[t.Key + "newEntryValue0"] = myTextField(stringEditor[t.Key + "newEntryValue0"], textBox);
                            GUILayout.EndHorizontal();
                            for (int n = 1; n < newCounter; n++)
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Space(20);
                                GUILayout.Space(20);
                                stringEditor[t.Key + "newEntryKey" + n] = myTextField(stringEditor[t.Key + "newEntryKey" + n], textBox);
                                stringEditor[t.Key + "newEntryValue" + n] = myTextField(stringEditor[t.Key + "newEntryValue" + n], textBox);
                                GUILayout.EndHorizontal();
                            }
                            if (stringEditor[t.Key + "newEntryKey" + (newCounter - 1)] != "" && stringEditor[t.Key + "newEntryValue" + (newCounter - 1)] != "")
                            {
                                stringEditor[t.Key + "newEntryCounter"] = (newCounter + 1).ToString();
                            }
                        }
                        if (encryptionChanged)
                        {
                            editSubDictI[counter] = false;
                            stringEditor[t.Key + "newEntry"] = "false";
                            if (!stringEditor.ContainsKey(t.Key + "newEntryCounter"))
                                stringEditor[t.Key + "newEntryCounter"] = "0";
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                stringEditor[t.Key + "newEntryKey" + n] = "";
                                stringEditor[t.Key + "newEntryValue" + n] = "";
                            }
                            stringEditor[t.Key + "newEntryCounter"] = "0";
                            //EpicPrefs.DeleteEditorPrefs(t.Key, Serializer.SerializationTypes.DictI, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            EpicPrefs.SetDict(t.Key, tDict, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            return;
                        }
                        if (applyDict)
                        {
                            //EpicPrefs.DeleteEditorPrefs(t.Key, Serializer.SerializationTypes.DictI, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            if (!stringEditor.ContainsKey(t.Key + "newEntryCounter"))
                                stringEditor[t.Key + "newEntryCounter"] = "0";
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                if (stringEditor[t.Key + "newEntryKey" + n] != "")
                                {
                                    stringEditor[t.Key + "newEntryValue" + n] = Operators.ToInt(stringEditor[t.Key + "newEntryValue" + n]).ToString();
                                    if (stringEditor[t.Key + "newEntryValue" + n] == "")
                                        stringEditor[t.Key + "newEntryValue" + n] = "0";
                                    if (Operators.IsInteger(stringEditor[t.Key + "newEntryValue" + n]))
                                        nDict[stringEditor[t.Key + "newEntryKey" + n]] = Convert.ToInt32(stringEditor[t.Key + "newEntryValue" + n]);
                                }
                                stringEditor[t.Key + "newEntryKey" + n] = "";
                                stringEditor[t.Key + "newEntryValue" + n] = "";
                            }
                            stringEditor[t.Key + "newEntry"] = "false";
                            stringEditor[t.Key + "newEntryCounter"] = "0";
                            EpicPrefs.SetDict(stringEditor[t.Key + "name"], nDict, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            editSubDictI[counter] = false;
                            return;
                        }
                    }
                    if (editSubDictI[counter])
                    {

                        GUILayout.Space(10);
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(40);
                        if (stringEditor[t.Key + "newEntry"] != "true")
                        {
                            if (GUILayout.Button(aB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                            {
                                stringEditor[t.Key + "newEntryCounter"] = "1";
                                stringEditor[t.Key + "newEntry"] = "true";
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                    counter++;
                }
            }
            Separator();
        }
        #endregion
        #region Long Dict Prefs
        if (DictLongDict.Count > 0)
        {
            showDictL = EditorGUILayout.Foldout(showDictL, "Long Dictionary Prefs", foldout);
            if (showDictL)
            {
                GUILayout.Space(5);
                int counter = 0;
                foreach (KeyValuePair<string, string> t in DictLongDict)
                {
                    if (!stringEditor.ContainsKey(t.Key + "encrypt"))
                        stringEditor[t.Key + "encrypt"] = t.Value;
                    if (!stringEditor.ContainsKey(t.Key + "name"))
                        stringEditor[t.Key + "name"] = t.Key;
                    Dictionary<string, long> tDict = new Dictionary<string,long>(DictLongDictValues[t.Key]);//EpicPrefs.GetDictStringLong(t.Key, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                    Dictionary<string, long> nDict = new Dictionary<string,long>(DictLongDictValues[t.Key]);//EpicPrefs.GetDictStringLong(t.Key, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    bool applyDict = false;
                    if (!stringEditor.ContainsKey(t.Key + "newEntry"))
                        stringEditor[t.Key + "newEntry"] = "false";
                    showSubDictL[counter] = EditorGUILayout.Foldout(showSubDictL[counter], "");
                    if (editSubDictL[counter])
                    {
                        stringEditor[t.Key + "name"] = myTextField(stringEditor[t.Key + "name"], textBox);
                    }
                    else
                    {
                        myLabelField(t.Key);
                    }
                    bool encryptedCheck = Convert.ToBoolean(stringEditor[t.Key + "encrypt"]);
                    bool encryptionChanged = false;
                    //stringEditor[t.Key + "encrypt"] = EditorGUILayout.Toggle("AES Encryption : ", Convert.ToBoolean(stringEditor[t.Key + "encrypt"])).ToString();
                    stringEditor[t.Key + "encrypt"] = EncryptionToggle(stringEditor[t.Key + "encrypt"]);
                    if (Convert.ToBoolean(stringEditor[t.Key + "encrypt"]) != encryptedCheck)
                        if (EditorUtility.DisplayDialog("Change encryption",
                "Are you sure to change the encryption settings of " + t.Key
                + " ? You will lose all unsaved changes.", "Yes", "No"))
                        {
                            encryptionChanged = true;
                        }
                        else
                        {
                            stringEditor[t.Key + "encrypt"] = encryptedCheck.ToString();
                        }
                    if (editSubDictL[counter])
                    {
                        if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            applyDict = true;
                        }
                        if (GUILayout.Button(cB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            editSubDictL[counter] = false;
                            stringEditor[t.Key + "newEntry"] = "false";
                            if (!stringEditor.ContainsKey(t.Key + "newEntryCounter"))
                                stringEditor[t.Key + "newEntryCounter"] = "0";
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                //if (stringEditor[t.Key + "newEntryKey" + n] != "")
                                //  nDict[stringEditor[t.Key + "newEntryKey" + n]] = Convert.ToInt64(stringEditor[t.Key + "newEntryValue" + n]);
                                stringEditor[t.Key + "newEntryKey" + n] = "";
                                stringEditor[t.Key + "newEntryValue" + n] = "";
                            }
                            stringEditor[t.Key + "newEntryCounter"] = "0";
                            return;
                        }
                    }
                    else
                    {
                        if (GUILayout.Button(eB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            editSubDictL[counter] = true;
                            showSubDictL[counter] = true;
                        }
                        if (GUILayout.Button(dB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            if (EditorUtility.DisplayDialog("Delete",
                "Are you sure to delete " + t.Key
                + " permanently ?", "Yes", "No"))
                                EpicPrefs.DeleteEditorPrefs(t.Key, Serializer.SerializationTypes.DictL, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            return;
                        }
                    }
                    GUILayout.EndHorizontal();
                    if (showSubDictL[counter])
                    {
                        foreach (KeyValuePair<string, long> pair in tDict)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.Space(20);
                            if (editSubDictL[counter])
                            {
                                GUILayout.Label(pair.Key, text);
                                if (!stringEditor.ContainsKey(t.Key + pair.Key))
                                    stringEditor[t.Key + pair.Key] = pair.Value.ToString();
                                string _s = myTextField(stringEditor[t.Key + pair.Key], textBox);
                                if (_s != "")
                                {
                                    stringEditor[t.Key + pair.Key] = Operators.ToLong(_s).ToString(); ;
                                }
                                if (stringEditor[t.Key + pair.Key] == "")
                                    stringEditor[t.Key + pair.Key] = "0";
                                if (Operators.IsInteger(stringEditor[t.Key + pair.Key]))
                                    nDict[pair.Key] = Convert.ToInt64(stringEditor[t.Key + pair.Key]);
                            }
                            else
                            {
                                myLabelField(pair.Key, pair.Value.ToString());
                                if (GUILayout.Button(dB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                                {
                                    nDict.Remove(pair.Key);
                                    applyDict = true;
                                }
                            }
                            GUILayout.EndHorizontal();
                        }
                        if (stringEditor[t.Key + "newEntry"] == "true")
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.Space(20);
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                if (!stringEditor.ContainsKey(t.Key + "newEntryKey" + n))
                                    stringEditor[t.Key + "newEntryKey" + n] = "";
                                if (!stringEditor.ContainsKey(t.Key + "newEntryValue" + n))
                                    stringEditor[t.Key + "newEntryValue" + n] = "";

                            }
                            stringEditor[t.Key + "newEntryKey0"] = myTextField(stringEditor[t.Key + "newEntryKey0"], textBox);
                            stringEditor[t.Key + "newEntryValue0"] = myTextField(stringEditor[t.Key + "newEntryValue0"], textBox);
                            GUILayout.EndHorizontal();
                            for (int n = 1; n < newCounter; n++)
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Space(20);
                                GUILayout.Space(20);
                                stringEditor[t.Key + "newEntryKey" + n] = myTextField(stringEditor[t.Key + "newEntryKey" + n], textBox);
                                stringEditor[t.Key + "newEntryValue" + n] = myTextField(stringEditor[t.Key + "newEntryValue" + n], textBox);
                                GUILayout.EndHorizontal();
                            }
                            if (stringEditor[t.Key + "newEntryKey" + (newCounter - 1)] != "" && stringEditor[t.Key + "newEntryValue" + (newCounter - 1)] != "")
                            {
                                stringEditor[t.Key + "newEntryCounter"] = (newCounter + 1).ToString();
                            }
                        }
                        if (encryptionChanged)
                        {
                            editSubDictL[counter] = false;
                            stringEditor[t.Key + "newEntry"] = "false";
                            if (!stringEditor.ContainsKey(t.Key + "newEntryCounter"))
                                stringEditor[t.Key + "newEntryCounter"] = "0";
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                stringEditor[t.Key + "newEntryKey" + n] = "";
                                stringEditor[t.Key + "newEntryValue" + n] = "";
                            }
                            stringEditor[t.Key + "newEntryCounter"] = "0";
                            //EpicPrefs.DeleteEditorPrefs(t.Key, Serializer.SerializationTypes.DictL, !Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            EpicPrefs.SetDict(t.Key, tDict, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            return;
                        }
                        if (applyDict)
                        {
                            //EpicPrefs.DeleteEditorPrefs(t.Key, Serializer.SerializationTypes.DictL, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            if (!stringEditor.ContainsKey(t.Key + "newEntryCounter"))
                                stringEditor[t.Key + "newEntryCounter"] = "0";
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                if (stringEditor[t.Key + "newEntryKey" + n] != "")
                                {
                                    stringEditor[t.Key + "newEntryValue" + n] = Operators.ToLong(stringEditor[t.Key + "newEntryValue" + n]).ToString();
                                    if (stringEditor[t.Key + "newEntryValue" + n] == "")
                                        stringEditor[t.Key + "newEntryValue" + n] = "0";
                                    if (Operators.IsInteger(stringEditor[t.Key + "newEntryValue" + n]))
                                        nDict[stringEditor[t.Key + "newEntryKey" + n]] = Convert.ToInt64(stringEditor[t.Key + "newEntryValue" + n]);
                                }
                                stringEditor[t.Key + "newEntryKey" + n] = "";
                                stringEditor[t.Key + "newEntryValue" + n] = "";
                            }
                            stringEditor[t.Key + "newEntry"] = "false";
                            stringEditor[t.Key + "newEntryCounter"] = "0";
                            EpicPrefs.SetDict(stringEditor[t.Key + "name"], nDict, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            editSubDictL[counter] = false;
                            return;
                        }
                    }
                    if (editSubDictL[counter])
                    {
                        GUILayout.Space(10);
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(40);
                        if (stringEditor[t.Key + "newEntry"] != "true")
                        {
                            if (GUILayout.Button(aB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                            {
                                stringEditor[t.Key + "newEntryCounter"] = "1";
                                stringEditor[t.Key + "newEntry"] = "true";
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                    counter++;
                }
            }
            Separator();
        }
        #endregion
        #region String Dict Prefs
        if (DictStringDict.Count > 0)
        {
            showDictS = EditorGUILayout.Foldout(showDictS, "String Dictionary Prefs", foldout);
            if (showDictS)
            {
                GUILayout.Space(5);
                int counter = 0;
                foreach (KeyValuePair<string, string> t in DictStringDict)
                {
                    if (!stringEditor.ContainsKey(t.Key + "encrypt"))
                        stringEditor[t.Key + "encrypt"] = t.Value;
                    if (!stringEditor.ContainsKey(t.Key + "name"))
                        stringEditor[t.Key + "name"] = t.Key;
                    Dictionary<string, string> tDict = new Dictionary<string,string>(DictStringDictValues[t.Key]);//EpicPrefs.GetDictStringString(t.Key, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                    Dictionary<string, string> nDict = new Dictionary<string,string>(DictStringDictValues[t.Key]);//EpicPrefs.GetDictStringString(t.Key, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    bool applyDict = false;
                    if (!stringEditor.ContainsKey(t.Key + "newEntry"))
                        stringEditor[t.Key + "newEntry"] = "false";
                    showSubDictS[counter] = EditorGUILayout.Foldout(showSubDictS[counter], "", foldout);
                    if (editSubDictS[counter])
                    {
                        stringEditor[t.Key + "name"] = myTextField(stringEditor[t.Key + "name"], textBox);
                    }
                    else
                    {
                        myLabelField(t.Key);
                    }
                    bool encryptedCheck = Convert.ToBoolean(stringEditor[t.Key + "encrypt"]);
                    bool encryptionChanged = false;
                    //stringEditor[t.Key + "encrypt"] = EditorGUILayout.Toggle("AES Encryption : ", Convert.ToBoolean(stringEditor[t.Key + "encrypt"])).ToString();
                    stringEditor[t.Key + "encrypt"] = EncryptionToggle(stringEditor[t.Key + "encrypt"]);
                    if (Convert.ToBoolean(stringEditor[t.Key + "encrypt"]) != encryptedCheck)
                        if (EditorUtility.DisplayDialog("Change encryption",
                "Are you sure to change the encryption settings of " + t.Key
                + " ? You will lose all unsaved changes.", "Yes", "No"))
                        {
                            encryptionChanged = true;
                        }
                        else
                        {
                            stringEditor[t.Key + "encrypt"] = encryptedCheck.ToString();
                        }
                    if (editSubDictS[counter])
                    {
                        if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            applyDict = true;
                        }
                        if (GUILayout.Button(cB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            editSubDictS[counter] = false;
                            stringEditor[t.Key + "newEntry"] = "false";
                            if (!stringEditor.ContainsKey(t.Key + "newEntryCounter"))
                                stringEditor[t.Key + "newEntryCounter"] = "0";
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                //if (stringEditor[t.Key + "newEntryKey" + n] != "")
                                //   nDict[stringEditor[t.Key + "newEntryKey" + n]] = stringEditor[t.Key + "newEntryValue" + n];
                                stringEditor[t.Key + "newEntryKey" + n] = "";
                                stringEditor[t.Key + "newEntryValue" + n] = "";
                            }
                            stringEditor[t.Key + "newEntryCounter"] = "0";
                            return;
                        }
                    }
                    else
                    {
                        if (GUILayout.Button(eB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            editSubDictS[counter] = true;
                            showSubDictS[counter] = true;
                        }
                        if (GUILayout.Button(dB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            if (EditorUtility.DisplayDialog("Delete",
                "Are you sure to delete " + t.Key
                + " permanently ?", "Yes", "No"))
                                EpicPrefs.DeleteEditorPrefs(t.Key, Serializer.SerializationTypes.DictS, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            return;
                        }
                    }
                    GUILayout.EndHorizontal();
                    if (showSubDictS[counter])
                    {
                        foreach (KeyValuePair<string, string> pair in tDict)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.Space(20);
                            if (editSubDictS[counter])
                            {
                                GUILayout.Label(pair.Key, text);
                                if (!stringEditor.ContainsKey(t.Key + pair.Key))
                                    stringEditor[t.Key + pair.Key] = pair.Value;
                                stringEditor[t.Key + pair.Key] = myTextField(stringEditor[t.Key + pair.Key], textBox);
                                nDict[pair.Key] = stringEditor[t.Key + pair.Key];
                            }
                            else
                            {
                                myLabelField(pair.Key, pair.Value);
                                if (GUILayout.Button(dB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                                {
                                    nDict.Remove(pair.Key);
                                    applyDict = true;
                                }
                            }
                            GUILayout.EndHorizontal();
                        }
                        if (stringEditor[t.Key + "newEntry"] == "true")
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.Space(20);
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                if (!stringEditor.ContainsKey(t.Key + "newEntryKey" + n))
                                    stringEditor[t.Key + "newEntryKey" + n] = "";
                                if (!stringEditor.ContainsKey(t.Key + "newEntryValue" + n))
                                    stringEditor[t.Key + "newEntryValue" + n] = "";

                            }
                            stringEditor[t.Key + "newEntryKey0"] = myTextField(stringEditor[t.Key + "newEntryKey0"], textBox);
                            stringEditor[t.Key + "newEntryValue0"] = myTextField(stringEditor[t.Key + "newEntryValue0"], textBox);
                            GUILayout.EndHorizontal();
                            for (int n = 1; n < newCounter; n++)
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Space(20);
                                GUILayout.Space(20);
                                stringEditor[t.Key + "newEntryKey" + n] = myTextField(stringEditor[t.Key + "newEntryKey" + n], textBox);
                                stringEditor[t.Key + "newEntryValue" + n] = myTextField(stringEditor[t.Key + "newEntryValue" + n], textBox);
                                GUILayout.EndHorizontal();
                            }
                            if (stringEditor[t.Key + "newEntryKey" + (newCounter - 1)] != "" && stringEditor[t.Key + "newEntryValue" + (newCounter - 1)] != "")
                            {
                                stringEditor[t.Key + "newEntryCounter"] = (newCounter + 1).ToString();
                            }
                        }
                        if (encryptionChanged)
                        {
                            editSubDictS[counter] = false;
                            stringEditor[t.Key + "newEntry"] = "false";
                            if (!stringEditor.ContainsKey(t.Key + "newEntryCounter"))
                                stringEditor[t.Key + "newEntryCounter"] = "0";
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                stringEditor[t.Key + "newEntryKey" + n] = "";
                                stringEditor[t.Key + "newEntryValue" + n] = "";
                            }
                            stringEditor[t.Key + "newEntryCounter"] = "0";
                            //EpicPrefs.DeleteEditorPrefs(t.Key, Serializer.SerializationTypes.DictS, !Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            EpicPrefs.SetDict(t.Key, tDict, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            return;
                        }
                        if (applyDict)
                        {
                            //EpicPrefs.DeleteEditorPrefs(t.Key, Serializer.SerializationTypes.DictS, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            if (!stringEditor.ContainsKey(t.Key + "newEntryCounter"))
                                stringEditor[t.Key + "newEntryCounter"] = "0";
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                if (stringEditor[t.Key + "newEntryKey" + n] != "")
                                    nDict[stringEditor[t.Key + "newEntryKey" + n]] = stringEditor[t.Key + "newEntryValue" + n];
                                stringEditor[t.Key + "newEntryKey" + n] = "";
                                stringEditor[t.Key + "newEntryValue" + n] = "";
                            }
                            stringEditor[t.Key + "newEntry"] = "false";
                            stringEditor[t.Key + "newEntryCounter"] = "0";
                            EpicPrefs.SetDict(stringEditor[t.Key + "name"], nDict, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            editSubDictS[counter] = false;
                            return;
                        }
                    }
                    if (editSubDictS[counter])
                    {
                        GUILayout.Space(10);
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(40);
                        if (stringEditor[t.Key + "newEntry"] != "true")
                        {
                            if (GUILayout.Button(aB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                            {
                                stringEditor[t.Key + "newEntryCounter"] = "1";
                                stringEditor[t.Key + "newEntry"] = "true";
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                    counter++;
                }
            }
            Separator();
        }
        #endregion
        #region Bool Dict Prefs
        if (DictBoolDict.Count > 0)
        {
            showDictB = EditorGUILayout.Foldout(showDictB, "Bool Dictionary Prefs", foldout);
            if (showDictB)
            {
                GUILayout.Space(5);
                int counter = 0;
                foreach (KeyValuePair<string, string> t in DictBoolDict)
                {
                    if (!stringEditor.ContainsKey(t.Key + "encrypt"))
                        stringEditor[t.Key + "encrypt"] = t.Value;
                    if (!stringEditor.ContainsKey(t.Key + "name"))
                        stringEditor[t.Key + "name"] = t.Key;
                    Dictionary<string, bool> tDict = new Dictionary<string,bool>(DictBoolDictValues[t.Key]);//EpicPrefs.GetDictStringBool(t.Key, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                    Dictionary<string, bool> nDict = new Dictionary<string,bool>(DictBoolDictValues[t.Key]);//EpicPrefs.GetDictStringBool(t.Key, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    bool applyDict = false;
                    if (!stringEditor.ContainsKey(t.Key + "newEntry"))
                        stringEditor[t.Key + "newEntry"] = "false";
                    showSubDictB[counter] = EditorGUILayout.Foldout(showSubDictB[counter], "");
                    if (editSubDictB[counter])
                    {
                        stringEditor[t.Key + "name"] = myTextField(stringEditor[t.Key + "name"], textBox);
                    }
                    else
                    {
                        myLabelField(t.Key);
                    }
                    bool encryptedCheck = Convert.ToBoolean(stringEditor[t.Key + "encrypt"]);
                    bool encryptionChanged = false;
                    //stringEditor[t.Key + "encrypt"] = EditorGUILayout.Toggle("AES Encryption : ", Convert.ToBoolean(stringEditor[t.Key + "encrypt"])).ToString();
                    stringEditor[t.Key + "encrypt"] = EncryptionToggle(stringEditor[t.Key + "encrypt"]);
                    if (Convert.ToBoolean(stringEditor[t.Key + "encrypt"]) != encryptedCheck)
                        if (EditorUtility.DisplayDialog("Change encryption",
                "Are you sure to change the encryption settings of " + t.Key
                + " ? You will lose all unsaved changes.", "Yes", "No"))
                        {
                            encryptionChanged = true;
                        }
                        else
                        {
                            stringEditor[t.Key + "encrypt"] = encryptedCheck.ToString();
                        }
                    if (editSubDictB[counter])
                    {
                        if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            applyDict = true;
                        }
                        if (GUILayout.Button(cB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            editSubDictB[counter] = false;
                            stringEditor[t.Key + "newEntry"] = "false";
                            if (!stringEditor.ContainsKey(t.Key + "newEntryCounter"))
                                stringEditor[t.Key + "newEntryCounter"] = "0";
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                // if (stringEditor[t.Key + "newEntryKey" + n] != "")
                                //     nDict[stringEditor[t.Key + "newEntryKey" + n]] = Convert.Operators.ToBool(stringEditor[t.Key + "newEntryValue" + n]);
                                stringEditor[t.Key + "newEntryKey" + n] = "";
                                stringEditor[t.Key + "newEntryValue" + n] = "";
                            }
                            stringEditor[t.Key + "newEntryCounter"] = "0";
                            return;
                        }
                    }
                    else
                    {
                        if (GUILayout.Button(eB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            editSubDictB[counter] = true;
                            showSubDictB[counter] = true;
                        }
                        if (GUILayout.Button(dB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            if (EditorUtility.DisplayDialog("Delete",
                "Are you sure to delete " + t.Key
                + " permanently ?", "Yes", "No"))
                                EpicPrefs.DeleteEditorPrefs(t.Key, Serializer.SerializationTypes.DictB, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            return;
                        }
                    }
                    GUILayout.EndHorizontal();
                    if (showSubDictB[counter])
                    {
                        foreach (KeyValuePair<string, bool> pair in tDict)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.Space(20);
                            if (editSubDictB[counter])
                            {
                                GUILayout.Label(pair.Key, text);
                                if (!stringEditor.ContainsKey(t.Key + pair.Key))
                                    stringEditor[t.Key + pair.Key] = pair.Value.ToString();
                                stringEditor[t.Key + pair.Key] = myTextField(stringEditor[t.Key + pair.Key], textBox);
                                stringEditor[t.Key + pair.Key] = Operators.ToBool(stringEditor[t.Key + pair.Key]).ToString();
                                if (stringEditor[t.Key + pair.Key] == "")
                                    stringEditor[t.Key + pair.Key] = "0";
                                nDict[pair.Key] = Convert.ToBoolean(stringEditor[t.Key + pair.Key]);
                            }
                            else
                            {
                                myLabelField(pair.Key, pair.Value.ToString());
                                if (GUILayout.Button(dB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                                {
                                    nDict.Remove(pair.Key);
                                    applyDict = true;
                                }
                            }
                            GUILayout.EndHorizontal();
                        }
                        if (stringEditor[t.Key + "newEntry"] == "true")
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.Space(20);
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                if (!stringEditor.ContainsKey(t.Key + "newEntryKey" + n))
                                    stringEditor[t.Key + "newEntryKey" + n] = "";
                                if (!stringEditor.ContainsKey(t.Key + "newEntryValue" + n))
                                    stringEditor[t.Key + "newEntryValue" + n] = "";

                            }
                            stringEditor[t.Key + "newEntryKey0"] = myTextField(stringEditor[t.Key + "newEntryKey0"], textBox);
                            stringEditor[t.Key + "newEntryValue0"] = myTextField(stringEditor[t.Key + "newEntryValue0"], textBox);
                            GUILayout.EndHorizontal();
                            for (int n = 1; n < newCounter; n++)
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Space(20);
                                GUILayout.Space(20);
                                stringEditor[t.Key + "newEntryKey" + n] = myTextField(stringEditor[t.Key + "newEntryKey" + n], textBox);
                                stringEditor[t.Key + "newEntryValue" + n] = myTextField(stringEditor[t.Key + "newEntryValue" + n], textBox);
                                GUILayout.EndHorizontal();
                            }
                            if (stringEditor[t.Key + "newEntryKey" + (newCounter - 1)] != "" && stringEditor[t.Key + "newEntryValue" + (newCounter - 1)] != "")
                            {
                                stringEditor[t.Key + "newEntryCounter"] = (newCounter + 1).ToString();
                            }
                        }
                        if (encryptionChanged)
                        {
                            editSubDictB[counter] = false;
                            stringEditor[t.Key + "newEntry"] = "false";
                            if (!stringEditor.ContainsKey(t.Key + "newEntryCounter"))
                                stringEditor[t.Key + "newEntryCounter"] = "0";
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                stringEditor[t.Key + "newEntryKey" + n] = "";
                                stringEditor[t.Key + "newEntryValue" + n] = "";
                            }
                            stringEditor[t.Key + "newEntryCounter"] = "0";
                            //EpicPrefs.DeleteEditorPrefs(t.Key, Serializer.SerializationTypes.DictB, !Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            EpicPrefs.SetDict(t.Key, tDict, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            return;
                        }
                        if (applyDict)
                        {
                            //EpicPrefs.DeleteEditorPrefs(t.Key, Serializer.SerializationTypes.DictB, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            if (!stringEditor.ContainsKey(t.Key + "newEntryCounter"))
                                stringEditor[t.Key + "newEntryCounter"] = "0";
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                if (stringEditor[t.Key + "newEntryKey" + n] != "")
                                {
                                    stringEditor[t.Key + "newEntryValue" + n] = Operators.ToBool(stringEditor[t.Key + "newEntryValue" + n]).ToString();
                                    if (stringEditor[t.Key + "newEntryValue" + n] == "")
                                        stringEditor[t.Key + "newEntryValue" + n] = "0";
                                    nDict[stringEditor[t.Key + "newEntryKey" + n]] = Convert.ToBoolean(stringEditor[t.Key + "newEntryValue" + n]);
                                }
                                stringEditor[t.Key + "newEntryKey" + n] = "";
                                stringEditor[t.Key + "newEntryValue" + n] = "";
                            }
                            stringEditor[t.Key + "newEntry"] = "false";
                            stringEditor[t.Key + "newEntryCounter"] = "0";
                            EpicPrefs.SetDict(stringEditor[t.Key + "name"], nDict, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            editSubDictB[counter] = false;
                            return;
                        }
                    }
                    if (editSubDictB[counter])
                    {
                        GUILayout.Space(10);
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(40);
                        if (stringEditor[t.Key + "newEntry"] != "true")
                        {
                            if (GUILayout.Button(aB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                            {
                                stringEditor[t.Key + "newEntryCounter"] = "1";
                                stringEditor[t.Key + "newEntry"] = "true";
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                    counter++;
                }
            }
            Separator();
        }
        #endregion
        #region Float Dict Prefs
        if (DictFloatDict.Count > 0)
        {
            showDictF = EditorGUILayout.Foldout(showDictF, "Float Dictionary Prefs", foldout);
            if (showDictF)
            {
                GUILayout.Space(5);
                int counter = 0;
                foreach (KeyValuePair<string, string> t in DictFloatDict)
                {
                    if (!stringEditor.ContainsKey(t.Key + "encrypt"))
                        stringEditor[t.Key + "encrypt"] = t.Value;
                    if (!stringEditor.ContainsKey(t.Key + "name"))
                        stringEditor[t.Key + "name"] = t.Key;
                    Dictionary<string, float> tDict = new Dictionary<string,float>(new Dictionary<string,float>(DictFloatDictValues[t.Key]));//EpicPrefs.GetDictStringFloat(t.Key, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                    Dictionary<string, float> nDict = new Dictionary<string,float>(new Dictionary<string,float>(DictFloatDictValues[t.Key]));//EpicPrefs.GetDictStringFloat(t.Key, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    bool applyDict = false;
                    if (!stringEditor.ContainsKey(t.Key + "newEntry"))
                        stringEditor[t.Key + "newEntry"] = "false";
                    showSubDictF[counter] = EditorGUILayout.Foldout(showSubDictF[counter], "");
                    if (editSubDictF[counter])
                    {
                        stringEditor[t.Key + "name"] = myTextField(stringEditor[t.Key + "name"], textBox);
                    }
                    else
                    {
                        myLabelField(t.Key);
                    }
                    bool encryptedCheck = Convert.ToBoolean(stringEditor[t.Key + "encrypt"]);
                    bool encryptionChanged = false;
                    //stringEditor[t.Key + "encrypt"] = EditorGUILayout.Toggle("AES Encryption : ", Convert.ToBoolean(stringEditor[t.Key + "encrypt"])).ToString();
                    stringEditor[t.Key + "encrypt"] = EncryptionToggle(stringEditor[t.Key + "encrypt"]);
                    if (Convert.ToBoolean(stringEditor[t.Key + "encrypt"]) != encryptedCheck)
                        if (EditorUtility.DisplayDialog("Change encryption",
                "Are you sure to change the encryption settings of " + t.Key
                + " ? You will lose all unsaved changes.", "Yes", "No"))
                        {
                            encryptionChanged = true;
                        }
                        else
                        {
                            stringEditor[t.Key + "encrypt"] = encryptedCheck.ToString();
                        }
                    if (editSubDictF[counter])
                    {
                        if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            applyDict = true;
                        }
                        if (GUILayout.Button(cB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            editSubDictF[counter] = false;
                            stringEditor[t.Key + "newEntry"] = "false";
                            if (!stringEditor.ContainsKey(t.Key + "newEntryCounter"))
                                stringEditor[t.Key + "newEntryCounter"] = "0";
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                //if (stringEditor[t.Key + "newEntryKey" + n] != "")
                                //  nDict[stringEditor[t.Key + "newEntryKey" + n]] = Convert.ToSingle(stringEditor[t.Key + "newEntryValue" + n]);
                                stringEditor[t.Key + "newEntryKey" + n] = "";
                                stringEditor[t.Key + "newEntryValue" + n] = "";
                            }
                            stringEditor[t.Key + "newEntryCounter"] = "0";
                            return;
                        }
                    }
                    else
                    {
                        if (GUILayout.Button(eB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            editSubDictF[counter] = true;
                            showSubDictF[counter] = true;
                        }
                        if (GUILayout.Button(dB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            if (EditorUtility.DisplayDialog("Delete",
                "Are you sure to delete " + t.Key
                + " permanently ?", "Yes", "No"))
                                EpicPrefs.DeleteEditorPrefs(t.Key, Serializer.SerializationTypes.DictF, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            return;
                        }
                    }
                    GUILayout.EndHorizontal();
                    if (showSubDictF[counter])
                    {
                        foreach (KeyValuePair<string, float> pair in tDict)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.Space(20);
                            if (editSubDictF[counter])
                            {
                                GUILayout.Label(pair.Key, text);
                                if (!stringEditor.ContainsKey(t.Key + pair.Key))
                                    stringEditor[t.Key + pair.Key] = pair.Value.ToString();
                                stringEditor[t.Key + pair.Key] = myTextField(stringEditor[t.Key + pair.Key], textBox);
                                stringEditor[t.Key + pair.Key] = Operators.ToFloat(stringEditor[t.Key + pair.Key]).ToString();
                                if (stringEditor[t.Key + pair.Key] == "")
                                    stringEditor[t.Key + pair.Key] = "0";
                                if (Operators.IsFloat(stringEditor[t.Key + pair.Key]))
                                    nDict[pair.Key] = Convert.ToSingle(stringEditor[t.Key + pair.Key]);
                            }
                            else
                            {
                                myLabelField(pair.Key, pair.Value.ToString());
                                if (GUILayout.Button(dB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                                {
                                    nDict.Remove(pair.Key);
                                    applyDict = true;
                                }
                            }
                            GUILayout.EndHorizontal();
                        }
                        if (stringEditor[t.Key + "newEntry"] == "true")
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.Space(20);
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                if (!stringEditor.ContainsKey(t.Key + "newEntryKey" + n))
                                    stringEditor[t.Key + "newEntryKey" + n] = "";
                                if (!stringEditor.ContainsKey(t.Key + "newEntryValue" + n))
                                    stringEditor[t.Key + "newEntryValue" + n] = "";

                            }
                            stringEditor[t.Key + "newEntryKey0"] = myTextField(stringEditor[t.Key + "newEntryKey0"], textBox);
                            stringEditor[t.Key + "newEntryValue0"] = myTextField(stringEditor[t.Key + "newEntryValue0"], textBox);
                            GUILayout.EndHorizontal();
                            for (int n = 1; n < newCounter; n++)
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Space(20);
                                GUILayout.Space(20);
                                stringEditor[t.Key + "newEntryKey" + n] = myTextField(stringEditor[t.Key + "newEntryKey" + n], textBox);
                                stringEditor[t.Key + "newEntryValue" + n] = myTextField(stringEditor[t.Key + "newEntryValue" + n], textBox);
                                GUILayout.EndHorizontal();
                            }
                            if (stringEditor[t.Key + "newEntryKey" + (newCounter - 1)] != "" && stringEditor[t.Key + "newEntryValue" + (newCounter - 1)] != "")
                            {
                                stringEditor[t.Key + "newEntryCounter"] = (newCounter + 1).ToString();
                            }
                        }
                        if (encryptionChanged)
                        {
                            editSubDictF[counter] = false;
                            stringEditor[t.Key + "newEntry"] = "false";
                            if (!stringEditor.ContainsKey(t.Key + "newEntryCounter"))
                                stringEditor[t.Key + "newEntryCounter"] = "0";
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                stringEditor[t.Key + "newEntryKey" + n] = "";
                                stringEditor[t.Key + "newEntryValue" + n] = "";
                            }
                            stringEditor[t.Key + "newEntryCounter"] = "0";
                            //EpicPrefs.DeleteEditorPrefs(t.Key, Serializer.SerializationTypes.DictF, !Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            EpicPrefs.SetDict(t.Key, tDict, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            return;
                        }
                        if (applyDict)
                        {
                            //EpicPrefs.DeleteEditorPrefs(t.Key, Serializer.SerializationTypes.DictF, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            if (!stringEditor.ContainsKey(t.Key + "newEntryCounter"))
                                stringEditor[t.Key + "newEntryCounter"] = "0";
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                if (stringEditor[t.Key + "newEntryKey" + n] != "")
                                {
                                    stringEditor[t.Key + "newEntryValue" + n] = Operators.ToFloat(stringEditor[t.Key + "newEntryValue" + n]).ToString();
                                    if (stringEditor[t.Key + "newEntryValue" + n] == "")
                                        stringEditor[t.Key + "newEntryValue" + n] = "0";
                                    if (Operators.IsFloat(stringEditor[t.Key + "newEntryValue" + n]))
                                        nDict[stringEditor[t.Key + "newEntryKey" + n]] = Convert.ToSingle(stringEditor[t.Key + "newEntryValue" + n]);
                                }
                                stringEditor[t.Key + "newEntryKey" + n] = "";
                                stringEditor[t.Key + "newEntryValue" + n] = "";
                            }
                            stringEditor[t.Key + "newEntry"] = "false";
                            stringEditor[t.Key + "newEntryCounter"] = "0";
                            EpicPrefs.SetDict(stringEditor[t.Key + "name"], nDict, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            editSubDictF[counter] = false;
                            return;
                        }
                    }
                    if (editSubDictF[counter])
                    {
                        GUILayout.Space(10);
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(40);
                        if (stringEditor[t.Key + "newEntry"] != "true")
                        {
                            if (GUILayout.Button(aB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                            {
                                stringEditor[t.Key + "newEntryCounter"] = "1";
                                stringEditor[t.Key + "newEntry"] = "true";
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                    counter++;
                }
            }
            Separator();
        }
        #endregion
        #region Double Dict Prefs
        if (DictDoubleDict.Count > 0)
        {
            showDictD = EditorGUILayout.Foldout(showDictD, "Double Dictionary Prefs", foldout);
            if (showDictD)
            {
                GUILayout.Space(5);
                int counter = 0;
                foreach (KeyValuePair<string, string> t in DictDoubleDict)
                {
                    if (!stringEditor.ContainsKey(t.Key + "encrypt"))
                        stringEditor[t.Key + "encrypt"] = t.Value;
                    if (!stringEditor.ContainsKey(t.Key + "name"))
                        stringEditor[t.Key + "name"] = t.Key;
                    Dictionary<string, double> tDict = new Dictionary<string,double>(DictDoubleDictValues[t.Key]);//EpicPrefs.GetDictStringDouble(t.Key, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                    Dictionary<string, double> nDict = new Dictionary<string,double>(DictDoubleDictValues[t.Key]);//EpicPrefs.GetDictStringDouble(t.Key, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    bool applyDict = false;
                    if (!stringEditor.ContainsKey(t.Key + "newEntry"))
                        stringEditor[t.Key + "newEntry"] = "false";
                    showSubDictD[counter] = EditorGUILayout.Foldout(showSubDictD[counter], "");
                    if (editSubDictD[counter])
                    {
                        stringEditor[t.Key + "name"] = myTextField(stringEditor[t.Key + "name"], textBox);
                    }
                    else
                    {
                        myLabelField(t.Key);
                    }
                    bool encryptedCheck = Convert.ToBoolean(stringEditor[t.Key + "encrypt"]);
                    bool encryptionChanged = false;
                    //stringEditor[t.Key + "encrypt"] = EditorGUILayout.Toggle("AES Encryption : ", Convert.ToBoolean(stringEditor[t.Key + "encrypt"])).ToString();
                    stringEditor[t.Key + "encrypt"] = EncryptionToggle(stringEditor[t.Key + "encrypt"]);
                    if (Convert.ToBoolean(stringEditor[t.Key + "encrypt"]) != encryptedCheck)
                        if (EditorUtility.DisplayDialog("Change encryption",
                "Are you sure to change the encryption settings of " + t.Key
                + " ? You will lose all unsaved changes.", "Yes", "No"))
                        {
                            encryptionChanged = true;
                        }
                        else
                        {
                            stringEditor[t.Key + "encrypt"] = encryptedCheck.ToString();
                        }
                    if (editSubDictD[counter])
                    {
                        if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            applyDict = true;
                        }
                        if (GUILayout.Button(cB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            editSubDictD[counter] = false;
                            stringEditor[t.Key + "newEntry"] = "false";
                            if (!stringEditor.ContainsKey(t.Key + "newEntryCounter"))
                                stringEditor[t.Key + "newEntryCounter"] = "0";
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                // if (stringEditor[t.Key + "newEntryKey" + n] != "")
                                //     nDict[stringEditor[t.Key + "newEntryKey" + n]] = Convert.Operators.ToDouble(stringEditor[t.Key + "newEntryValue" + n]);
                                stringEditor[t.Key + "newEntryKey" + n] = "";
                                stringEditor[t.Key + "newEntryValue" + n] = "";
                            }
                            stringEditor[t.Key + "newEntryCounter"] = "0";
                            return;
                        }
                    }
                    else
                    {
                        if (GUILayout.Button(eB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            editSubDictD[counter] = true;
                            showSubDictD[counter] = true;
                        }
                        if (GUILayout.Button(dB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            if (EditorUtility.DisplayDialog("Delete",
                "Are you sure to delete " + t.Key
                + " permanently ?", "Yes", "No"))
                                EpicPrefs.DeleteEditorPrefs(t.Key, Serializer.SerializationTypes.DictD, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            return;
                        }
                    }
                    GUILayout.EndHorizontal();
                    if (showSubDictD[counter])
                    {
                        foreach (KeyValuePair<string, double> pair in tDict)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.Space(20);
                            if (editSubDictD[counter])
                            {
                                GUILayout.Label(pair.Key, text);
                                if (!stringEditor.ContainsKey(t.Key + pair.Key))
                                    stringEditor[t.Key + pair.Key] = pair.Value.ToString();
                                stringEditor[t.Key + pair.Key] = myTextField(stringEditor[t.Key + pair.Key], textBox);
                                stringEditor[t.Key + pair.Key] = Operators.ToDouble(stringEditor[t.Key + pair.Key]).ToString();
                                if (stringEditor[t.Key + pair.Key] == "")
                                    stringEditor[t.Key + pair.Key] = "0";
                                if (Operators.IsFloat(stringEditor[t.Key + pair.Key]))
                                    nDict[pair.Key] = Operators.ToDouble(stringEditor[t.Key + pair.Key]);
                            }
                            else
                            {
                                myLabelField(pair.Key, pair.Value.ToString());
                                if (GUILayout.Button(dB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                                {
                                    nDict.Remove(pair.Key);
                                    applyDict = true;
                                }
                            }
                            GUILayout.EndHorizontal();
                        }
                        if (stringEditor[t.Key + "newEntry"] == "true")
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.Space(20);
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                if (!stringEditor.ContainsKey(t.Key + "newEntryKey" + n))
                                    stringEditor[t.Key + "newEntryKey" + n] = "";
                                if (!stringEditor.ContainsKey(t.Key + "newEntryValue" + n))
                                    stringEditor[t.Key + "newEntryValue" + n] = "";

                            }
                            stringEditor[t.Key + "newEntryKey0"] = myTextField(stringEditor[t.Key + "newEntryKey0"], textBox);
                            stringEditor[t.Key + "newEntryValue0"] = myTextField(stringEditor[t.Key + "newEntryValue0"], textBox);
                            GUILayout.EndHorizontal();
                            for (int n = 1; n < newCounter; n++)
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Space(20);
                                GUILayout.Space(20);
                                stringEditor[t.Key + "newEntryKey" + n] = myTextField(stringEditor[t.Key + "newEntryKey" + n], textBox);
                                stringEditor[t.Key + "newEntryValue" + n] = myTextField(stringEditor[t.Key + "newEntryValue" + n], textBox);
                                GUILayout.EndHorizontal();
                            }
                            if (stringEditor[t.Key + "newEntryKey" + (newCounter - 1)] != "" && stringEditor[t.Key + "newEntryValue" + (newCounter - 1)] != "")
                            {
                                stringEditor[t.Key + "newEntryCounter"] = (newCounter + 1).ToString();
                            }
                        }
                        if (encryptionChanged)
                        {
                            editSubDictD[counter] = false;
                            stringEditor[t.Key + "newEntry"] = "false";
                            if (!stringEditor.ContainsKey(t.Key + "newEntryCounter"))
                                stringEditor[t.Key + "newEntryCounter"] = "0";
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                stringEditor[t.Key + "newEntryKey" + n] = "";
                                stringEditor[t.Key + "newEntryValue" + n] = "";
                            }
                            stringEditor[t.Key + "newEntryCounter"] = "0";
                            //EpicPrefs.DeleteEditorPrefs(t.Key, Serializer.SerializationTypes.DictD, !Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            EpicPrefs.SetDict(t.Key, tDict, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            return;
                        }
                        if (applyDict)
                        {
                            //EpicPrefs.DeleteEditorPrefs(t.Key, Serializer.SerializationTypes.DictD, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            if (!stringEditor.ContainsKey(t.Key + "newEntryCounter"))
                                stringEditor[t.Key + "newEntryCounter"] = "0";
                            int newCounter = Convert.ToInt32(stringEditor[t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                if (stringEditor[t.Key + "newEntryKey" + n] != "")
                                {
                                    stringEditor[t.Key + "newEntryValue" + n] = Operators.ToDouble(stringEditor[t.Key + "newEntryValue" + n]).ToString();
                                    if (stringEditor[t.Key + "newEntryValue" + n] == "")
                                        stringEditor[t.Key + "newEntryValue" + n] = "0";
                                    if (Operators.IsFloat(stringEditor[t.Key + "newEntryValue" + n]))
                                        nDict[stringEditor[t.Key + "newEntryKey" + n]] = Operators.ToDouble(stringEditor[t.Key + "newEntryValue" + n]);
                                }
                                stringEditor[t.Key + "newEntryKey" + n] = "";
                                stringEditor[t.Key + "newEntryValue" + n] = "";
                            }
                            stringEditor[t.Key + "newEntry"] = "false";
                            stringEditor[t.Key + "newEntryCounter"] = "0";
                            EpicPrefs.SetDict(stringEditor[t.Key + "name"], nDict, Convert.ToBoolean(stringEditor[t.Key + "encrypt"]));
                            editSubDictD[counter] = false;
                            return;
                        }
                    }
                    if (editSubDictD[counter])
                    {
                        GUILayout.Space(10);
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(40);
                        if (stringEditor[t.Key + "newEntry"] != "true")
                        {
                            if (GUILayout.Button(aB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                            {
                                stringEditor[t.Key + "newEntryCounter"] = "1";
                                stringEditor[t.Key + "newEntry"] = "true";
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                    counter++;
                }
            }
            Separator();
        }
        #endregion
        #region Lists
        ListDisplay(Serializer.SerializationTypes.ListI);
        ListDisplay(Serializer.SerializationTypes.ListL);
        ListDisplay(Serializer.SerializationTypes.ListS);
        ListDisplay(Serializer.SerializationTypes.ListB);
        ListDisplay(Serializer.SerializationTypes.ListF);
        ListDisplay(Serializer.SerializationTypes.ListD);
        #endregion
        #region Arrays
        ListDisplay(Serializer.SerializationTypes.ArrayInt);
        ListDisplay(Serializer.SerializationTypes.ArrayString);
        ListDisplay(Serializer.SerializationTypes.ArrayFloat);
        ListDisplay(Serializer.SerializationTypes.ArrayDouble);
        #endregion
        #region Vectors
        VectorDisplay(Serializer.SerializationTypes.Vector2);
        VectorDisplay(Serializer.SerializationTypes.Vector3);
        VectorDisplay(Serializer.SerializationTypes.Vector4);
        VectorDisplay(Serializer.SerializationTypes.Quaternion);
        VectorDisplay(Serializer.SerializationTypes.Transform);
        #endregion
        #region Color Prefs
        if (ColorDict.Count > 0)
        {
            EditorGUILayout.BeginHorizontal();
            showColor = EditorGUILayout.Foldout(showColor, "Color Prefs", foldout);
            if (editColor)
            {
                if (!stringEditor.ContainsKey("[color]newEntry"))
                    stringEditor["[color]newEntry"] = "false";
                if (!stringEditor.ContainsKey("[color]newEntryCounter"))
                    stringEditor["[color]newEntryCounter"] = "0";
                if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    editColor = false;
                    for(int m=0; m < Operators.ToInt(stringEditor["[color]newEntryCounter"]); m++)
                    {
                        if (stringEditor["[color]newEntryKey" + m].Trim() != "") {
                            Color tmpCol = new Color();
                            tmpCol = Operators.StringToColor(stringEditor["[color]newEntryValue" + m]);
                            colorEditor[stringEditor["[color]newEntryKey" + m]] = tmpCol;
                        }
                    }
                    foreach (KeyValuePair<string, Color> i in colorEditor)
                    {
                        EpicPrefs.SetColor(i.Key, i.Value);
                    }
                    stringEditor["[color]newEntryCounter"] = "0";
                    return;
                }
                if (GUILayout.Button(cB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    editColor = false;
                    colorEditor = new Dictionary<string, Color>();
                    stringEditor["[color]newEntryCounter"] = "0";
                    return;
                }
            }
            else
            {
                if (GUILayout.Button(eB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    editColor = true;
                    showColor = true;
                }
            }
            EditorGUILayout.EndHorizontal();
            if (showColor)
            {
                GUILayout.Space(5);
                foreach (KeyValuePair<string, string> t in ColorDict)
                {
                   if (!stringEditor.ContainsKey("[color]" + t.Key + "name"))
                        stringEditor["[color]" + t.Key + "name"] = t.Key;
                    if (!editColor)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        myLabelField(t.Key);
                        Color newCol = ColorDictValues[t.Key];//EpicPrefs.GetColor(t.Key);
                        Debug.Log(newCol.ToString());
                        GUIStyle newColStyle = new GUIStyle(text);
                        newColStyle.normal.textColor = newCol;
                        EditorGUILayout.LabelField(newCol.ToString(), newColStyle, GUILayout.Width(250), GUILayout.Height(20));
                        if (GUILayout.Button(dB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            EpicPrefs.DeleteColor(t.Key, Convert.ToBoolean(t.Value));
                            return;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    else {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        myLabelField(t.Key);
                        if (!colorEditor.ContainsKey(t.Key))
                            colorEditor[t.Key] = ColorDictValues[t.Key];//EpicPrefs.GetColor(t.Key);
                        colorEditor[t.Key] = EditorGUILayout.ColorField(colorEditor[t.Key]);                        
                        EditorGUILayout.EndHorizontal();
                    }
                }
                if (!stringEditor.ContainsKey("[color]newEntry"))
                    stringEditor["[color]newEntry"] = "false";
                if (stringEditor["[color]newEntry"] == "true")
                {
                    int newCounter = Convert.ToInt32(stringEditor["[color]newEntryCounter"]);
                    for (int n = 0; n < newCounter; n++)
                    {
                        if (!stringEditor.ContainsKey("[color]newEntryKey" + n))
                            stringEditor["[color]newEntryKey" + n] = "";
                        if (!stringEditor.ContainsKey("[color]newEntryValue" + n))
                            stringEditor["[color]newEntryValue" + n] = Operators.ColorToString(Color.white);                    
                    }
                    for (int n = 0; n < newCounter; n++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        GUILayout.Space(20);
                        stringEditor["[color]newEntryKey" + n] = myTextField(stringEditor["[color]newEntryKey" + n], textBox);
                        Color newCol = Color.white;
                        newCol = Operators.StringToColor(stringEditor["[color]newEntryValue" + n]);
						stringEditor["[color]newEntryValue" + n] = Operators.ColorToString(EditorGUILayout.ColorField(newCol));
                        GUILayout.EndHorizontal();
                    }                    
                }
                if (editColor)
                {
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(40);
                    if (GUILayout.Button(aB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        stringEditor["[color]newEntryCounter"] = (Operators.ToInt(stringEditor["[color]newEntryCounter"]) + 1).ToString();
                        stringEditor["[color]newEntry"] = "true";                    
                    }
                    GUILayout.EndHorizontal();
                }
            }
            Separator();
        }
        #endregion
        #region Layout Design #2
        GUILayout.Space(20);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndHorizontal();
        #endregion
    }
    public void OnInspectorUpdate()
    {
        if (showWindow)
        {
            showWindow = false;
            window.Show();
        }
        if (repaint)
        {
            repaint = false;
            Repaint();
        }
    }
    private void VectorDisplay(Serializer.SerializationTypes type)
    {
        string tag = "";
        string title = "";
        Dictionary<string, string> usedDict = new Dictionary<string, string>();
        bool[] editDetails = new bool[0];
        bool[] showSubDetails = new bool[0];
        bool showDetails = false;
        #region Selection
        switch (type)
        {
            case Serializer.SerializationTypes.Transform:
                /*tag = "[transform]";
                title = "Transform Prefs";
                usedDict = TransformDict;
                showDetails = showTransform;
                showSubDetails = showSubTransform;
                editDetails = editSubTransform;*/
                break;
            case Serializer.SerializationTypes.Vector2:
                tag = "[vector2]";
                title = "Vector2 Prefs";
                usedDict = Vector2Dict;
                showDetails = showVector2;
                showSubDetails = showSubVector2;
                editDetails = editSubVector2;
                break;
            case Serializer.SerializationTypes.Vector3:
                tag = "[vector3]";
                title = "Vector3 Prefs";
                usedDict = Vector3Dict;
                showDetails = showVector3;
                showSubDetails = showSubVector3;
                editDetails = editSubVector3;
                break;
            case Serializer.SerializationTypes.Vector4:
                tag = "[vector4]";
                title = "Vector4 Prefs";
                usedDict = Vector4Dict;
                showDetails = showVector4;
                showSubDetails = showSubVector4;
                editDetails = editSubVector4;
                break;
            case Serializer.SerializationTypes.Quaternion:
                tag = "[quaternion]";
                title = "Quaternion Prefs";
                usedDict = QuaternionDict;
                showDetails = showQuaternion;
                showSubDetails = showSubQuaternion;
                editDetails = editSubQuaternion;
                break;
        }
        #endregion
        #region GUI
        if (usedDict.Count > 0)
        {
            showDetails = EditorGUILayout.Foldout(showDetails, title, foldout);
            if (showDetails)
            {
                GUILayout.Space(5);
                int counter = 0;
                foreach (KeyValuePair<string, string> t in usedDict)
                {
                    if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "encrypt"))
                        vectorsAndUnityTypes[tag + t.Key + "encrypt"] = t.Value;
                    if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "name"))
                        vectorsAndUnityTypes[tag + t.Key + "name"] = t.Key;
                    Dictionary<string, float> tList = new Dictionary<string, float>();
                    Dictionary<string, float> nList = new Dictionary<string, float>();
                    #region Creation
                    if (type == Serializer.SerializationTypes.Transform)
                    {
                        /*GameObject nObj = new GameObject();
                        Transform newTrans = EpicPrefs.GetTransform(t.Key, Convert.ToBoolean(vectorsAndUnityTypes[tag + t.Key + "encrypt"]));
                        tList[tag + t.Key + "fX"] = newTrans.forward.x;
                        tList[tag + t.Key + "fY"] = newTrans.forward.y;
                        tList[tag + t.Key + "fZ"] = newTrans.forward.z;

                        tList[tag + t.Key + "rX"] = newTrans.right.x;
                        tList[tag + t.Key + "rY"] = newTrans.right.y;
                        tList[tag + t.Key + "rZ"] = newTrans.right.z;

                        tList[tag + t.Key + "pX"] = newTrans.position.x;
                        tList[tag + t.Key + "pY"] = newTrans.position.y;
                        tList[tag + t.Key + "pZ"] = newTrans.position.z;

                        tList[tag + t.Key + "qX"] = newTrans.rotation.x;
                        tList[tag + t.Key + "qY"] = newTrans.rotation.y;
                        tList[tag + t.Key + "qZ"] = newTrans.rotation.z;
                        tList[tag + t.Key + "qW"] = newTrans.rotation.w;

                        tList[tag + t.Key + "sX"] = newTrans.localScale.x;
                        tList[tag + t.Key + "sY"] = newTrans.localScale.y;
                        tList[tag + t.Key + "sZ"] = newTrans.localScale.z;

                        nList[tag + t.Key + "fX"] = newTrans.forward.x;
                        nList[tag + t.Key + "fY"] = newTrans.forward.y;
                        nList[tag + t.Key + "fZ"] = newTrans.forward.z;

                        nList[tag + t.Key + "rX"] = newTrans.right.x;
                        nList[tag + t.Key + "rY"] = newTrans.right.y;
                        nList[tag + t.Key + "rZ"] = newTrans.right.z;

                        nList[tag + t.Key + "pX"] = newTrans.position.x;
                        nList[tag + t.Key + "pY"] = newTrans.position.y;
                        nList[tag + t.Key + "pZ"] = newTrans.position.z;

                        nList[tag + t.Key + "qX"] = newTrans.rotation.x;
                        nList[tag + t.Key + "qY"] = newTrans.rotation.y;
                        nList[tag + t.Key + "qZ"] = newTrans.rotation.z;
                        nList[tag + t.Key + "qW"] = newTrans.rotation.w;

                        nList[tag + t.Key + "sX"] = newTrans.localScale.x;
                        nList[tag + t.Key + "sY"] = newTrans.localScale.y;
                        nList[tag + t.Key + "sZ"] = newTrans.localScale.z;*/
                    }
                    else
                        if (type == Serializer.SerializationTypes.Vector2)
                        {
                            Vector2 newVec = Vector2DictValues[t.Key];//EpicPrefs.GetVector2(t.Key, Convert.ToBoolean(vectorsAndUnityTypes[tag + t.Key + "encrypt"]));
                            tList[tag + t.Key + "X"] = newVec.x;
                            tList[tag + t.Key + "Y"] = newVec.y;
                            nList[tag + t.Key + "X"] = newVec.x;
                            nList[tag + t.Key + "Y"] = newVec.y;
                        }
                        else
                                if (type == Serializer.SerializationTypes.Vector3)
                                {

                                    Vector3 newVec = Vector3DictValues[t.Key];//EpicPrefs.GetVector3(t.Key, Convert.ToBoolean(vectorsAndUnityTypes[tag + t.Key + "encrypt"]));
                                    tList[tag + t.Key + "X"] = newVec.x;
                                    tList[tag + t.Key + "Y"] = newVec.y;
                                    tList[tag + t.Key + "Z"] = newVec.z;

                                    nList[tag + t.Key + "X"] = newVec.x; 
                                    nList[tag + t.Key + "Y"] = newVec.y;
                                    nList[tag + t.Key + "Z"] = newVec.z;
                                }
                                else {
                                    if (type == Serializer.SerializationTypes.Vector4)
                                    {
                                        Vector4 newVec = Vector4DictValues[t.Key];//EpicPrefs.GetVector4(t.Key, Convert.ToBoolean(vectorsAndUnityTypes[tag + t.Key + "encrypt"]));
                                        tList[tag + t.Key + "X"] = newVec.x;
                                        tList[tag + t.Key + "Y"] = newVec.y;
                                        tList[tag + t.Key + "Z"] = newVec.z;
                                        tList[tag + t.Key + "W"] = newVec.w;

                                        nList[tag + t.Key + "X"] = newVec.x;
                                        nList[tag + t.Key + "Y"] = newVec.y;
                                        nList[tag + t.Key + "Z"] = newVec.z;
                                        nList[tag + t.Key + "W"] = newVec.w;
                                    }
                                    else
                                    {
                                        if (type == Serializer.SerializationTypes.Quaternion)
                                        {
                                            Quaternion newQuat = QuaternionDictValues[t.Key];//EpicPrefs.GetQuaternion(t.Key, Convert.ToBoolean(vectorsAndUnityTypes[tag + t.Key + "encrypt"]));
                                            tList[tag + t.Key + "X"] = newQuat.x;
                                            tList[tag + t.Key + "Y"] = newQuat.y;
                                            tList[tag + t.Key + "Z"] = newQuat.z;
                                            tList[tag + t.Key + "W"] = newQuat.w;

                                            nList[tag + t.Key + "X"] = newQuat.x;
                                            nList[tag + t.Key + "Y"] = newQuat.y;
                                            nList[tag + t.Key + "Z"] = newQuat.z;
                                            nList[tag + t.Key + "W"] = newQuat.w;
                                        }
                                    }
                                }
                    #endregion
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    bool applyDict = false;
                    if (!vectorsAndUnityTypes.ContainsKey(tag  + "newEntry"))
                        vectorsAndUnityTypes[tag  + "newEntry"] = "false";
                    showSubDetails[counter] = EditorGUILayout.Foldout(showSubDetails[counter], "");
                    if (editDetails[counter])
                    {
                        vectorsAndUnityTypes[tag + t.Key + "name"] = myTextField(vectorsAndUnityTypes[tag + t.Key + "name"], textBox);
                    }
                    else
                    {
                        myLabelField(t.Key);
                    }
                    #region Todo Encryption
                    //bool encryptedCheck = Convert.ToBoolean(vectorsAndUnityTypes[tag + t.Key + "encrypt"]);
                    //bool encryptionChanged = false;
                    /*vectorsAndUnityTypes[tag + t.Key + "encrypt"] = EncryptionToggle(vectorsAndUnityTypes[tag + t.Key + "encrypt"]);
                    if (Convert.ToBoolean(vectorsAndUnityTypes[tag + t.Key + "encrypt"]) != encryptedCheck)
                        if (EditorUtility.DisplayDialog("Change encryption",
                "Are you sure to change the encryption settings of " + t.Key
                + " ? You will lose all unsaved changes.", "Yes", "No"))
                        {
                            encryptionChanged = true;
                        }
                        else
                        {
                            vectorsAndUnityTypes[tag + t.Key + "encrypt"] = encryptedCheck.ToString();
                        }*/
                    #endregion
                    if (editDetails[counter])
                    {
                        if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            applyDict = true;
                        }
                        if (GUILayout.Button(cB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            editDetails[counter] = false;
                            vectorsAndUnityTypes[tag  + "newEntry"] = "false";
                            if (!vectorsAndUnityTypes.ContainsKey(tag  + "newEntryCounter"))
                                vectorsAndUnityTypes[tag  + "newEntryCounter"] = "0";
                            vectorsAndUnityTypes[tag + t.Key + "newEntryCounter"] = "0";
                            return;
                        }
                    }
                    else
                    {
                        if (GUILayout.Button(eB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            editDetails[counter] = true;
                            editDetails[counter] = true;
                        }
                        if (GUILayout.Button(dB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            if (EditorUtility.DisplayDialog("Delete",
                "Are you sure to delete " + t.Key
                + " permanently ?", "Yes", "No"))
                                EpicPrefs.DeleteEditorPrefs(t.Key, type, Convert.ToBoolean(vectorsAndUnityTypes[tag + t.Key + "encrypt"]));
                            return;
                        }
                    }
                    GUILayout.EndHorizontal();
                    if (showSubDetails[counter])
                    {
                        if (editDetails[counter])
                        {
                            EditorGUILayout.BeginVertical();
                            #region Edit
                            if (type == Serializer.SerializationTypes.Transform)
                            {
                                #region transform
                                /*EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(40);
                                myLabelField("Position");
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(50);
                                if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "pX"))
                                    vectorsAndUnityTypes[tag + t.Key + "pX"] = nList[tag + t.Key + "pX"].ToString();
                                vectorsAndUnityTypes[tag + t.Key + "pX"] = myTextField("X: ", vectorsAndUnityTypes[tag + t.Key + "pX"],true);
                                string _tempX = Operators.ToFloat(vectorsAndUnityTypes[tag + t.Key + "pX"]).ToString();
                                if (_tempX != "" && _tempX != "-")
                                    nList[tag + t.Key + "pX"] = Convert.ToSingle(_tempX);

                                if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "pY"))
                                    vectorsAndUnityTypes[tag + t.Key + "pY"] = nList[tag + t.Key + "pY"].ToString();
                                vectorsAndUnityTypes[tag + t.Key + "pY"] = myTextField("Y: ", vectorsAndUnityTypes[tag + t.Key + "pY"], true);
                                string _tempY = Operators.ToFloat(vectorsAndUnityTypes[tag + t.Key + "pY"]).ToString();
                                if (_tempY != "" && _tempY != "-")
                                    nList[tag + t.Key + "pY"] = Convert.ToSingle(_tempY);

                                if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "pZ"))
                                    vectorsAndUnityTypes[tag + t.Key + "pZ"] = nList[tag + t.Key + "pZ"].ToString();
                                vectorsAndUnityTypes[tag + t.Key + "pZ"] = myTextField("Z: ", vectorsAndUnityTypes[tag + t.Key + "pZ"], true);
                                string _tempZ = Operators.ToFloat(vectorsAndUnityTypes[tag + t.Key + "pZ"]).ToString();
                                if (_tempZ != "" && _tempZ != "-")
                                    nList[tag + t.Key + "pZ"] = Convert.ToSingle(_tempZ);
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(40);
                                myLabelField("Rotation");
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(50);
                                if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "rX"))
                                    vectorsAndUnityTypes[tag + t.Key + "rX"] = nList[tag + t.Key + "rX"].ToString();
                                vectorsAndUnityTypes[tag + t.Key + "rX"] = myTextField("X: ", vectorsAndUnityTypes[tag + t.Key + "rX"], true);
                                string _temprX = Operators.ToFloat(vectorsAndUnityTypes[tag + t.Key + "rX"]).ToString();
                                if (_temprX != "" && _temprX != "-")
                                    nList[tag + t.Key + "rX"] = Convert.ToSingle(_temprX);

                                if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "rY"))
                                    vectorsAndUnityTypes[tag + t.Key + "rY"] = nList[tag + t.Key + "rY"].ToString();
                                vectorsAndUnityTypes[tag + t.Key + "rY"] = myTextField("Y: ", vectorsAndUnityTypes[tag + t.Key + "rY"], true);
                                string _temprY = Operators.ToFloat(vectorsAndUnityTypes[tag + t.Key + "rY"]).ToString();
                                if (_temprY != "" && _temprY != "-")
                                    nList[tag + t.Key + "rY"] = Convert.ToSingle(_temprY);

                                if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "rZ"))
                                    vectorsAndUnityTypes[tag + t.Key + "rZ"] = nList[tag + t.Key + "rZ"].ToString();
                                vectorsAndUnityTypes[tag + t.Key + "rZ"] = myTextField("Z: ", vectorsAndUnityTypes[tag + t.Key + "rZ"], true);
                                string _temprZ = Operators.ToFloat(vectorsAndUnityTypes[tag + t.Key + "rZ"]).ToString();
                                if (_temprZ != "" && _temprZ != "-")
                                    nList[tag + t.Key + "rZ"] = Convert.ToSingle(_temprZ);
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(40);
                                myLabelField("Scale");
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(50);
                                if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "sX"))
                                    vectorsAndUnityTypes[tag + t.Key + "sX"] = nList[tag + t.Key + "sX"].ToString();
                                vectorsAndUnityTypes[tag + t.Key + "sX"] = myTextField("X: ", vectorsAndUnityTypes[tag + t.Key + "sX"], true);
                                string _tempsX = Operators.ToFloat(vectorsAndUnityTypes[tag + t.Key + "sX"]).ToString();
                                if (_tempsX != "" && _tempsX != "-")
                                    nList[tag + t.Key + "sX"] = Convert.ToSingle(_tempsX);

                                if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "sY"))
                                    vectorsAndUnityTypes[tag + t.Key + "sY"] = nList[tag + t.Key + "sY"].ToString();
                                vectorsAndUnityTypes[tag + t.Key + "sY"] = myTextField("Y: ", vectorsAndUnityTypes[tag + t.Key + "sY"], true);
                                string _tempsY = Operators.ToFloat(vectorsAndUnityTypes[tag + t.Key + "sY"]).ToString();
                                if (_tempsY != "" && _tempsY != "-")
                                    nList[tag + t.Key + "sY"] = Convert.ToSingle(_tempsY);

                                if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "sZ"))
                                    vectorsAndUnityTypes[tag + t.Key + "sZ"] = nList[tag + t.Key + "sZ"].ToString();
                                vectorsAndUnityTypes[tag + t.Key + "sZ"] = myTextField("Z: ", vectorsAndUnityTypes[tag + t.Key + "sZ"], true);
                                string _tempsZ = Operators.ToFloat(vectorsAndUnityTypes[tag + t.Key + "sZ"]).ToString();
                                if (_tempsZ != "" && _tempsZ != "-")
                                    nList[tag + t.Key + "sZ"] = Convert.ToSingle(_tempsZ);
                                EditorGUILayout.EndHorizontal();*/
                                #endregion
                            }
                            else {
                                if (type == Serializer.SerializationTypes.Vector2)
                                {
                                    #region vector2                                        
                                    EditorGUILayout.BeginHorizontal();
                                    GUILayout.Space(40);
                                    if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "X"))
                                        vectorsAndUnityTypes[tag + t.Key + "X"] = nList[tag + t.Key + "X"].ToString();
                                    vectorsAndUnityTypes[tag + t.Key + "X"] = myTextField("X: ", vectorsAndUnityTypes[tag + t.Key + "X"], true);
                                    string _tempX = Operators.ToFloat(vectorsAndUnityTypes[tag + t.Key + "X"]).ToString();
                                    if (_tempX != "" && _tempX != "-")
                                        nList[tag + t.Key + "X"] = Convert.ToSingle(_tempX);
                                    if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "Y"))
                                        vectorsAndUnityTypes[tag + t.Key + "Y"] = nList[tag + t.Key + "Y"].ToString();
                                    vectorsAndUnityTypes[tag + t.Key + "Y"] = myTextField("Y: ", vectorsAndUnityTypes[tag + t.Key + "Y"], true);
                                    string _tempY = Operators.ToFloat(vectorsAndUnityTypes[tag + t.Key + "Y"]).ToString();
                                    if (_tempY != "" && _tempY != "-")
                                        nList[tag + t.Key + "Y"] = Convert.ToSingle(_tempY);

                                    EditorGUILayout.EndHorizontal();
                                    #endregion
                                }
                                else {
                                    if (type == Serializer.SerializationTypes.Vector3)
                                    {
                                        #region vector3                                    
                                        EditorGUILayout.BeginHorizontal();
                                        GUILayout.Space(40);
                                        if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "X"))
                                            vectorsAndUnityTypes[tag + t.Key + "X"] = nList[tag + t.Key + "X"].ToString();
                                        vectorsAndUnityTypes[tag + t.Key + "X"] = myTextField("X: ", vectorsAndUnityTypes[tag + t.Key + "X"], true);
                                        string _tempX = Operators.ToFloat(vectorsAndUnityTypes[tag + t.Key + "X"]).ToString();
                                        if (_tempX != "" && _tempX != "-")
                                            nList[tag + t.Key + "X"] = Convert.ToSingle(_tempX);

                                        if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "Y"))
                                            vectorsAndUnityTypes[tag + t.Key + "Y"] = nList[tag + t.Key + "Y"].ToString();
                                        vectorsAndUnityTypes[tag + t.Key + "Y"] = myTextField("Y: ", vectorsAndUnityTypes[tag + t.Key + "Y"], true);
                                        string _tempY = Operators.ToFloat(vectorsAndUnityTypes[tag + t.Key + "Y"]).ToString();
                                        if (_tempY != "" && _tempY != "-")
                                            nList[tag + t.Key + "Y"] = Convert.ToSingle(_tempY);

                                        if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "Z"))
                                            vectorsAndUnityTypes[tag + t.Key + "Z"] = nList[tag + t.Key + "Z"].ToString();
                                        vectorsAndUnityTypes[tag + t.Key + "Z"] = myTextField("Z: ", vectorsAndUnityTypes[tag + t.Key + "Z"], true);
                                        string _tempZ = Operators.ToFloat(vectorsAndUnityTypes[tag + t.Key + "Z"]).ToString();
                                        if (_tempZ != "" && _tempZ != "-")
                                            nList[tag + t.Key + "Z"] = Convert.ToSingle(_tempZ);
                                        EditorGUILayout.EndHorizontal();
                                        #endregion
                                    }
                                    else {
                                        if (type == Serializer.SerializationTypes.Vector4)
                                        {
                                            #region vector4                         
                                            EditorGUILayout.BeginHorizontal();
                                            GUILayout.Space(40);
                                            if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "X"))
                                                vectorsAndUnityTypes[tag + t.Key + "X"] = nList[tag + t.Key + "X"].ToString();
                                            vectorsAndUnityTypes[tag + t.Key + "X"] = myTextField("X: ", vectorsAndUnityTypes[tag + t.Key + "X"], true);
                                            string _tempX = Operators.ToFloat(vectorsAndUnityTypes[tag + t.Key + "X"]).ToString();
                                            if (_tempX != "" && _tempX != "-")
                                                nList[tag + t.Key + "X"] = Convert.ToSingle(_tempX);
                                            if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "Y"))

                                                vectorsAndUnityTypes[tag + t.Key + "Y"] = nList[tag + t.Key + "Y"].ToString();
                                            vectorsAndUnityTypes[tag + t.Key + "Y"] = myTextField("Y: ", vectorsAndUnityTypes[tag + t.Key + "Y"], true);
                                            string _tempY = Operators.ToFloat(vectorsAndUnityTypes[tag + t.Key + "Y"]).ToString();
                                            if (_tempY != "" && _tempY != "-")
                                                nList[tag + t.Key + "Y"] = Convert.ToSingle(_tempY);

                                            if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "Z"))
                                                vectorsAndUnityTypes[tag + t.Key + "Z"] = nList[tag + t.Key + "Z"].ToString();
                                            vectorsAndUnityTypes[tag + t.Key + "Z"] = myTextField("Z: ", vectorsAndUnityTypes[tag + t.Key + "Z"], true);
                                            string _tempZ = Operators.ToFloat(vectorsAndUnityTypes[tag + t.Key + "Z"]).ToString();
                                            if (_tempZ != "" && _tempZ != "-")
                                                nList[tag + t.Key + "Z"] = Convert.ToSingle(_tempZ);

                                            if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "W"))
                                                vectorsAndUnityTypes[tag + t.Key + "W"] = nList[tag + t.Key + "W"].ToString();
                                            vectorsAndUnityTypes[tag + t.Key + "W"] = myTextField("W: ", vectorsAndUnityTypes[tag + t.Key + "W"], true);
                                            string _tempW = Operators.ToFloat(vectorsAndUnityTypes[tag + t.Key + "W"]).ToString();
                                            if (_tempW != "" && _tempW != "-")
                                                nList[tag + t.Key + "W"] = Convert.ToSingle(_tempW);
                                            EditorGUILayout.EndHorizontal();
                                            #endregion
                                        }
                                        else
                                        {
                                            if (type == Serializer.SerializationTypes.Quaternion)
                                            {
                                                #region quaternion                        
                                                EditorGUILayout.BeginHorizontal();
                                                GUILayout.Space(40);
                                                if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "X"))
                                                    vectorsAndUnityTypes[tag + t.Key + "X"] = nList[tag + t.Key + "X"].ToString();
                                                vectorsAndUnityTypes[tag + t.Key + "X"] = myTextField("X: ", vectorsAndUnityTypes[tag + t.Key + "X"], true);
                                                string _tempX = Operators.ToFloat(vectorsAndUnityTypes[tag + t.Key + "X"]).ToString();
                                                if (_tempX != "" && _tempX != "-")
                                                    nList[tag + t.Key + "X"] = Convert.ToSingle(_tempX);
                                                if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "Y"))

                                                    vectorsAndUnityTypes[tag + t.Key + "Y"] = nList[tag + t.Key + "Y"].ToString();
                                                vectorsAndUnityTypes[tag + t.Key + "Y"] = myTextField("Y: ", vectorsAndUnityTypes[tag + t.Key + "Y"], true);
                                                string _tempY = Operators.ToFloat(vectorsAndUnityTypes[tag + t.Key + "Y"]).ToString();
                                                if (_tempY != "" && _tempY != "-")
                                                    nList[tag + t.Key + "Y"] = Convert.ToSingle(_tempY);

                                                if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "Z"))
                                                    vectorsAndUnityTypes[tag + t.Key + "Z"] = nList[tag + t.Key + "Z"].ToString();
                                                vectorsAndUnityTypes[tag + t.Key + "Z"] = myTextField("Z: ", vectorsAndUnityTypes[tag + t.Key + "Z"], true);
                                                string _tempZ = Operators.ToFloat(vectorsAndUnityTypes[tag + t.Key + "Z"]).ToString();
                                                if (_tempZ != "" && _tempZ != "-")
                                                    nList[tag + t.Key + "Z"] = Convert.ToSingle(_tempZ);

                                                if (!vectorsAndUnityTypes.ContainsKey(tag + t.Key + "W"))
                                                    vectorsAndUnityTypes[tag + t.Key + "W"] = nList[tag + t.Key + "W"].ToString();
                                                vectorsAndUnityTypes[tag + t.Key + "W"] = myTextField("W: ", vectorsAndUnityTypes[tag + t.Key + "W"], true);
                                                string _tempW = Operators.ToFloat(vectorsAndUnityTypes[tag + t.Key + "W"]).ToString();
                                                if (_tempW != "" && _tempW != "-")
                                                    nList[tag + t.Key + "W"] = Convert.ToSingle(_tempW);
                                                EditorGUILayout.EndHorizontal();
                                                #endregion
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                            EditorGUILayout.EndVertical();
                        }
                        else
                        {
                            EditorGUILayout.BeginVertical();
                            #region Display
                            if (type == Serializer.SerializationTypes.Transform)
                            {
                                #region transform
                                /*EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(40);
                                myLabelField("Position");
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(50);
                                myLabelField("X: ", tList[tag + t.Key + "pX"].ToString(),true);
                                myLabelField("Y: ", tList[tag + t.Key + "pY"].ToString(),true);
                                myLabelField("Z: ", tList[tag + t.Key + "pZ"].ToString(),true);
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(40);
                                myLabelField("Rotation");
                                EditorGUILayout.EndHorizontal();
                                Quaternion rotQ = new Quaternion(tList[tag + t.Key + "rX"], tList[tag + t.Key + "rY"], tList[tag + t.Key + "rZ"], tList[tag + t.Key + "rW"]);
                                Vector3 rotV = rotQ.eulerAngles;
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(50);
                                myLabelField("X: ", rotV.x.ToString(), true);
                                myLabelField("Y: ", rotV.y.ToString(), true);
                                myLabelField("Z: ", rotV.z.ToString(), true);
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(40);
                                myLabelField("Scale");
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(50);
                                myLabelField("X: ", tList[tag + t.Key + "sX"].ToString(), true);
                                myLabelField("Y: ", tList[tag + t.Key + "sY"].ToString(), true);
                                myLabelField("Z: ", tList[tag + t.Key + "sZ"].ToString(), true);
                                EditorGUILayout.EndHorizontal();*/
                                #endregion
                            }
                            else {
                                if (type == Serializer.SerializationTypes.Vector2)
                                {
                                    #region vector2                                        
                                    EditorGUILayout.BeginHorizontal();
                                    GUILayout.Space(40);
                                    myLabelField("X: ", tList[tag + t.Key + "X"].ToString(), true);
                                    myLabelField("Y: ", tList[tag + t.Key + "Y"].ToString(), true);
                                    EditorGUILayout.EndHorizontal();
                                    #endregion
                                }
                                else {
                                    if (type == Serializer.SerializationTypes.Vector3)
                                    {
                                        #region vector3                                        
                                        EditorGUILayout.BeginHorizontal();
                                        GUILayout.Space(40);
                                        myLabelField("X: ", tList[tag + t.Key + "X"].ToString(), true);
                                        myLabelField("Y: ", tList[tag + t.Key + "Y"].ToString(), true);
                                        myLabelField("Z: ", tList[tag + t.Key + "Z"].ToString(), true);
                                        EditorGUILayout.EndHorizontal();
                                        #endregion
                                    }
                                    else {
                                        if (type == Serializer.SerializationTypes.Vector4)
                                        {
                                            #region vector4                                        
                                            EditorGUILayout.BeginHorizontal();
                                            GUILayout.Space(40);
                                            myLabelField("X: ", tList[tag + t.Key + "X"].ToString(), true);
                                            myLabelField("Y: ", tList[tag + t.Key + "Y"].ToString(), true);
                                            myLabelField("Z: ", tList[tag + t.Key + "Z"].ToString(), true);
                                            myLabelField("W: ", tList[tag + t.Key + "W"].ToString(), true);
                                            EditorGUILayout.EndHorizontal();
                                            #endregion
                                        }
                                        else
                                        {
                                            if (type == Serializer.SerializationTypes.Quaternion)
                                            {
                                                #region quaternion                                        
                                                EditorGUILayout.BeginHorizontal();
                                                GUILayout.Space(40);
                                                myLabelField("X: ", tList[tag + t.Key + "X"].ToString(), true);
                                                myLabelField("Y: ", tList[tag + t.Key + "Y"].ToString(), true);
                                                myLabelField("Z: ", tList[tag + t.Key + "Z"].ToString(), true);
                                                myLabelField("W: ", tList[tag + t.Key + "W"].ToString(), true);
                                                EditorGUILayout.EndHorizontal();
                                                #endregion
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                            EditorGUILayout.EndVertical();
                        }
                        #region Todo Encryption
                        /*                        
                        if (encryptionChanged)
                        {
                            editDetails[counter] = false;
                            vectorsAndUnityTypes[tag + "newEntry"] = "false";
                            if (!vectorsAndUnityTypes.ContainsKey(tag + "newEntryCounter"))
                                vectorsAndUnityTypes[tag + "newEntryCounter"] = "0";
                            #region INIT
                            int newCounter = Convert.ToInt32(vectorsAndUnityTypes[tag + t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                vectorsAndUnityTypes[tag + t.Key + "pXnewEntryValue" + n] = "";
                                vectorsAndUnityTypes[tag + t.Key + "pYnewEntryValue" + n] = "";
                                vectorsAndUnityTypes[tag + t.Key + "pZnewEntryValue" + n] = "";

                                vectorsAndUnityTypes[tag + t.Key + "rXnewEntryValue" + n] = "";
                                vectorsAndUnityTypes[tag + t.Key + "rYnewEntryValue" + n] = "";
                                vectorsAndUnityTypes[tag + t.Key + "rZnewEntryValue" + n] = "";

                                vectorsAndUnityTypes[tag + t.Key + "sXnewEntryValue" + n] = "";
                                vectorsAndUnityTypes[tag + t.Key + "sYnewEntryValue" + n] = "";
                                vectorsAndUnityTypes[tag + t.Key + "sZnewEntryValue" + n] = "";

                                vectorsAndUnityTypes[tag + t.Key + "XnewEntryValue" + n] = "";
                                vectorsAndUnityTypes[tag + t.Key + "YnewEntryValue" + n] = "";
                                vectorsAndUnityTypes[tag + t.Key + "YnewEntryValue" + n] = "";
                            }
                            #endregion
                            vectorsAndUnityTypes[tag  + "newEntryCounter"] = "0";
                            EpicPrefs.DeleteEditorPrefs(t.Key, type, !Convert.ToBoolean(vectorsAndUnityTypes[tag  + "encrypt"]));
                            #region Save 
                            switch (type)
                            {
                                case Serializer.SerializationTypes.Vector2:
                                    Vector2 vec2 = new Vector2(tList[tag + t.Key + "X"], tList[tag + t.Key + "Y"]);
                                    EpicPrefs.DeleteVector2(t.Key, !Convert.ToBoolean(vectorsAndUnityTypes[tag + t.Key + "encrypt"]));
                                    EpicPrefs.SetVector2(t.Key, vec2, Convert.ToBoolean(vectorsAndUnityTypes[tag + t.Key + "encrypt"]));
                                    break;
                                case Serializer.SerializationTypes.Vector3:
                                    Vector3 vec3 = new Vector3(tList[tag + t.Key + "X"], tList[tag + t.Key + "Y"], tList[tag + t.Key + "Z"]);
                                    EpicPrefs.DeleteVector3(t.Key, !Convert.ToBoolean(vectorsAndUnityTypes[tag + t.Key + "encrypt"]));
                                    EpicPrefs.SetVector3(t.Key, vec3, Convert.ToBoolean(vectorsAndUnityTypes[tag + t.Key + "encrypt"]));
                                    break;
                                case Serializer.SerializationTypes.Vector4:
                                    Vector4 vec4 = new Vector4(tList[tag + t.Key + "X"], tList[tag + t.Key + "Y"], tList[tag + t.Key + "Z"], tList[tag + t.Key + "W"]);
                                    EpicPrefs.DeleteVector4(t.Key, !Convert.ToBoolean(vectorsAndUnityTypes[tag + t.Key + "encrypt"]));
                                    EpicPrefs.SetVector4(t.Key, vec4, Convert.ToBoolean(vectorsAndUnityTypes[tag + t.Key + "encrypt"]));                            
                                    break;
                                case Serializer.SerializationTypes.Quaternion:
                                    Quaternion quat = new Quaternion(tList[tag + t.Key + "X"], tList[tag + t.Key + "Y"], tList[tag + t.Key + "Z"], tList[tag + t.Key + "W"]);
                                    EpicPrefs.DeleteQuaternion(t.Key, !Convert.ToBoolean(vectorsAndUnityTypes[tag + t.Key + "encrypt"]));
                                    EpicPrefs.SetQuaternion(t.Key, quat, Convert.ToBoolean(vectorsAndUnityTypes[tag + t.Key + "encrypt"]));
                                    break;
                                case Serializer.SerializationTypes.Transform:
                                    /*GameObject newObj = new GameObject();
                                    Transform tempTrans = newObj.transform;
                                    tempTrans.name = t.Key;
                                    tempTrans.position = new Vector3(tList[tag + t.Key + "pX"], tList[tag + t.Key + "pY"], tList[tag + t.Key + "pZ"]);
                                    tempTrans.rotation = new Quaternion(tList[tag + t.Key + "rX"], tList[tag + t.Key + "rY"], tList[tag + t.Key + "rZ"], tList[tag + t.Key + "rW"]);
                                    tempTrans.localScale = new Vector3(tList[tag + t.Key + "sX"], tList[tag + t.Key + "sY"], tList[tag + t.Key + "sZ"]);
                                    EpicPrefs.DeleteTransform(t.Key, !Convert.ToBoolean(vectorsAndUnityTypes[tag + t.Key + "encrypt"]));
                                    EpicPrefs.SetTransform(t.Key, tempTrans, Convert.ToBoolean(vectorsAndUnityTypes[tag + t.Key + "encrypt"]));
                                break;
                            }
                            #endregion
                            return;

                        }*/
                        #endregion
                        if (vectorsAndUnityTypes[tag  + "newEntry"] == "true")
                        {
                            #region INIT
                            int newCounter = Convert.ToInt32(vectorsAndUnityTypes[tag  + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                if (!vectorsAndUnityTypes.ContainsKey(tag  + "pXnewEntryValue" + n))
                                    vectorsAndUnityTypes[tag  + "pXnewEntryValue" + n] = "";
                                if (!vectorsAndUnityTypes.ContainsKey(tag  + "pYnewEntryValue" + n))
                                    vectorsAndUnityTypes[tag  + "pYnewEntryValue" + n] = "";
                                if (!vectorsAndUnityTypes.ContainsKey(tag  + "pZnewEntryValue" + n))
                                    vectorsAndUnityTypes[tag  + "pZnewEntryValue" + n] = "";

                                if (!vectorsAndUnityTypes.ContainsKey(tag  + "rXnewEntryValue" + n))
                                    vectorsAndUnityTypes[tag  + "rXnewEntryValue" + n] = "";
                                if (!vectorsAndUnityTypes.ContainsKey(tag  + "rYnewEntryValue" + n))
                                    vectorsAndUnityTypes[tag  + "rYnewEntryValue" + n] = "";
                                if (!vectorsAndUnityTypes.ContainsKey(tag  + "rZnewEntryValue" + n))
                                    vectorsAndUnityTypes[tag  + "rZnewEntryValue" + n] = "";

                                if (!vectorsAndUnityTypes.ContainsKey(tag  + "sXnewEntryValue" + n))
                                    vectorsAndUnityTypes[tag  + "sXnewEntryValue" + n] = "";
                                if (!vectorsAndUnityTypes.ContainsKey(tag  + "sYnewEntryValue" + n))
                                    vectorsAndUnityTypes[tag  + "sYnewEntryValue" + n] = "";
                                if (!vectorsAndUnityTypes.ContainsKey(tag  + "sZnewEntryValue" + n))
                                    vectorsAndUnityTypes[tag  + "sZnewEntryValue" + n] = "";

                                if (!vectorsAndUnityTypes.ContainsKey(tag  + "XnewEntryValue" + n))
                                    vectorsAndUnityTypes[tag  + "XnewEntryValue" + n] = "";
                                if (!vectorsAndUnityTypes.ContainsKey(tag  + "YnewEntryValue" + n))
                                    vectorsAndUnityTypes[tag  + "YnewEntryValue" + n] = "";
                                if (!vectorsAndUnityTypes.ContainsKey(tag  + "ZnewEntryValue" + n))
                                    vectorsAndUnityTypes[tag  + "ZnewEntryValue" + n] = "";
                            }
                            #endregion
                            EditorGUILayout.BeginVertical();
                            if(!vectorsAndUnityTypes.ContainsKey(tag + "NameNewEntryValue0"))
                                vectorsAndUnityTypes[tag + "NameNewEntryValue0"] = "";
                            vectorsAndUnityTypes[tag + "NameNewEntryValue0"] = myTextField("Name : ",vectorsAndUnityTypes[tag + "NameNewEntryValue0"], true);
                            #region Add
                            if (type == Serializer.SerializationTypes.Transform)
                            {
                                #region transform
                                /*EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(40);
                                myLabelField("Position");
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(50);
                                if (!vectorsAndUnityTypes.ContainsKey(tag + "pXnewEntryValue0"))
                                    vectorsAndUnityTypes[tag + "pXnewEntryValue0"] = "";
                                vectorsAndUnityTypes[tag + "pXnewEntryValue0"] = myTextField("X: ", vectorsAndUnityTypes[tag + "pXnewEntryValue0"], true);
                                if (!vectorsAndUnityTypes.ContainsKey(tag + "pYnewEntryValue0"))
                                    vectorsAndUnityTypes[tag + "pYnewEntryValue0"] = "";
                                vectorsAndUnityTypes[tag + "pYnewEntryValue0"] = myTextField("Y: ", vectorsAndUnityTypes[tag + "pYnewEntryValue0"], true);
                                if (!vectorsAndUnityTypes.ContainsKey(tag + "pZnewEntryValue0"))
                                    vectorsAndUnityTypes[tag + "pZnewEntryValue0"] = "";
                                vectorsAndUnityTypes[tag + "pZnewEntryValue0"] = myTextField("Z: ", vectorsAndUnityTypes[tag + "pZnewEntryValue0"], true);
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(40);
                                myLabelField("Rotation");
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(50);
                                if (!vectorsAndUnityTypes.ContainsKey(tag + "rXnewEntryValue0"))
                                    vectorsAndUnityTypes[tag + "rXnewEntryValue0"] = "";
                                vectorsAndUnityTypes[tag + "rXnewEntryValue0"] = myTextField("X: ", vectorsAndUnityTypes[tag + "rXnewEntryValue0"], true);
                                if (!vectorsAndUnityTypes.ContainsKey(tag + "rYnewEntryValue0"))
                                    vectorsAndUnityTypes[tag + "rYnewEntryValue0"] = "";
                                vectorsAndUnityTypes[tag + "rYnewEntryValue0"] = myTextField("Y: ", vectorsAndUnityTypes[tag + "rYnewEntryValue0"], true);
                                if (!vectorsAndUnityTypes.ContainsKey(tag + "rZnewEntryValue0"))
                                    vectorsAndUnityTypes[tag + "rZnewEntryValue0"] = "";
                                vectorsAndUnityTypes[tag + "rZnewEntryValue0"] = myTextField("Z: ", vectorsAndUnityTypes[tag + "rZnewEntryValue0"], true);
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(40);
                                myLabelField("Scale");
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(50);
                                if (!vectorsAndUnityTypes.ContainsKey(tag + "sXnewEntryValue0"))
                                    vectorsAndUnityTypes[tag + "sXnewEntryValue0"] = "";
                                vectorsAndUnityTypes[tag + "sXnewEntryValue0"] = myTextField("X: ", vectorsAndUnityTypes[tag + "sXnewEntryValue0"], true);
                                if (!vectorsAndUnityTypes.ContainsKey(tag + "sYnewEntryValue0"))
                                    vectorsAndUnityTypes[tag + "sYnewEntryValue0"] = "";
                                vectorsAndUnityTypes[tag + "sYnewEntryValue0"] = myTextField("Y: ", vectorsAndUnityTypes[tag + "sYnewEntryValue0"], true);
                                if (!vectorsAndUnityTypes.ContainsKey(tag + "sZnewEntryValue0"))
                                    vectorsAndUnityTypes[tag + "sZnewEntryValue0"] = "";
                                vectorsAndUnityTypes[tag + "sZnewEntryValue0"] = myTextField("Z: ", vectorsAndUnityTypes[tag + "sZnewEntryValue0"], true);
                                EditorGUILayout.EndHorizontal();*/
                                #endregion
                            }
                            else {
                                if (type == Serializer.SerializationTypes.Vector2)
                                {
                                    #region vector2                                        
                                    EditorGUILayout.BeginHorizontal();
                                    GUILayout.Space(40);
                                    if (!vectorsAndUnityTypes.ContainsKey(tag + "XnewEntryValue0"))
                                        vectorsAndUnityTypes[tag + "XnewEntryValue0"] = "";
                                    vectorsAndUnityTypes[tag + "XnewEntryValue0"] = myTextField("X: ", vectorsAndUnityTypes[tag + "XnewEntryValue0"], true);
                                    if (!vectorsAndUnityTypes.ContainsKey(tag + "YnewEntryValue0"))
                                        vectorsAndUnityTypes[tag + "YnewEntryValue0"] = "";
                                    vectorsAndUnityTypes[tag + "YnewEntryValue0"] = myTextField("Y: ", vectorsAndUnityTypes[tag + "YnewEntryValue0"], true);
                                    EditorGUILayout.EndHorizontal();
                                    #endregion
                                }
                                else {
                                    if (type == Serializer.SerializationTypes.Vector3)
                                    {
                                        #region vector3       
                                        EditorGUILayout.BeginHorizontal();
                                        GUILayout.Space(40);
                                        if (!vectorsAndUnityTypes.ContainsKey(tag + "XnewEntryValue0"))
                                            vectorsAndUnityTypes[tag + "XnewEntryValue0"] = "";
                                        vectorsAndUnityTypes[tag + "XnewEntryValue0"] = myTextField("X: ", vectorsAndUnityTypes[tag + "XnewEntryValue0"], true);
                                        if (!vectorsAndUnityTypes.ContainsKey(tag + "YnewEntryValue0"))
                                            vectorsAndUnityTypes[tag + "YnewEntryValue0"] = "";
                                        vectorsAndUnityTypes[tag + "YnewEntryValue0"] = myTextField("Y: ", vectorsAndUnityTypes[tag + "YnewEntryValue0"], true);
                                        if (!vectorsAndUnityTypes.ContainsKey(tag + "ZnewEntryValue0"))
                                            vectorsAndUnityTypes[tag + "ZnewEntryValue0"] = "";
                                        vectorsAndUnityTypes[tag + "ZnewEntryValue0"] = myTextField("Z: ", vectorsAndUnityTypes[tag + "ZnewEntryValue0"], true);
                                        EditorGUILayout.EndHorizontal();
                                        #endregion
                                    }
                                    else {
                                        if (type == Serializer.SerializationTypes.Vector4)
                                        {
                                            #region vector4  
                                            EditorGUILayout.BeginHorizontal();
                                            GUILayout.Space(40);
                                            if (!vectorsAndUnityTypes.ContainsKey(tag + "XnewEntryValue0"))
                                                vectorsAndUnityTypes[tag + "XnewEntryValue0"] = "";
                                            vectorsAndUnityTypes[tag + "XnewEntryValue0"] = myTextField("X: ", vectorsAndUnityTypes[tag + "XnewEntryValue0"], true);
                                            if (!vectorsAndUnityTypes.ContainsKey(tag + "YnewEntryValue0"))
                                                vectorsAndUnityTypes[tag + "YnewEntryValue0"] = "";
                                            vectorsAndUnityTypes[tag + "YnewEntryValue0"] = myTextField("Y: ", vectorsAndUnityTypes[tag + "YnewEntryValue0"], true);
                                            if (!vectorsAndUnityTypes.ContainsKey(tag + "ZnewEntryValue0"))
                                                vectorsAndUnityTypes[tag + "ZnewEntryValue0"] = "";
                                            vectorsAndUnityTypes[tag + "ZnewEntryValue0"] = myTextField("Z: ", vectorsAndUnityTypes[tag + "ZnewEntryValue0"], true);
                                            if (!vectorsAndUnityTypes.ContainsKey(tag + "WnewEntryValue0"))
                                                vectorsAndUnityTypes[tag + "WnewEntryValue0"] = "";
                                            vectorsAndUnityTypes[tag + "WnewEntryValue0"] = myTextField("W: ", vectorsAndUnityTypes[tag + "WnewEntryValue0"], true);
                                            EditorGUILayout.EndHorizontal();
                                            #endregion
                                        }
                                        else
                                        {
                                            if (type == Serializer.SerializationTypes.Quaternion)
                                            {
                                                #region quaternion 
                                                EditorGUILayout.BeginHorizontal();
                                                GUILayout.Space(40);
                                                if (!vectorsAndUnityTypes.ContainsKey(tag + "XnewEntryValue0"))
                                                    vectorsAndUnityTypes[tag + "XnewEntryValue0"] = "";
                                                vectorsAndUnityTypes[tag + "XnewEntryValue0"] = myTextField("X: ", vectorsAndUnityTypes[tag + "XnewEntryValue0"], true);
                                                if (!vectorsAndUnityTypes.ContainsKey(tag + "YnewEntryValue0"))
                                                    vectorsAndUnityTypes[tag + "YnewEntryValue0"] = "";
                                                vectorsAndUnityTypes[tag + "YnewEntryValue0"] = myTextField("Y: ", vectorsAndUnityTypes[tag + "YnewEntryValue0"], true);
                                                if (!vectorsAndUnityTypes.ContainsKey(tag + "ZnewEntryValue0"))
                                                    vectorsAndUnityTypes[tag + "ZnewEntryValue0"] = "";
                                                vectorsAndUnityTypes[tag + "ZnewEntryValue0"] = myTextField("Z: ", vectorsAndUnityTypes[tag + "ZnewEntryValue0"], true);
                                                if (!vectorsAndUnityTypes.ContainsKey(tag + "WnewEntryValue0"))
                                                    vectorsAndUnityTypes[tag + "WnewEntryValue0"] = "";
                                                EditorGUILayout.EndHorizontal();
                                                #endregion
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                            EditorGUILayout.EndVertical();
                            for (int n = 1; n < newCounter; n++)
                            {
                                EditorGUILayout.BeginVertical();
                                if (!vectorsAndUnityTypes.ContainsKey(tag + "NameNewEntryValue"+n))
                                    vectorsAndUnityTypes[tag + "NameNewEntryValue"+n] = "";
                                vectorsAndUnityTypes[tag + "NameNewEntryValue"+n] = myTextField("Name : ", vectorsAndUnityTypes[tag + "NameNewEntryValue"+n]);
                                #region Add
                                if (type == Serializer.SerializationTypes.Transform)
                                {
                                    #region transform
                                    /*EditorGUILayout.BeginHorizontal();
                                    GUILayout.Space(40);
                                    myLabelField("Position");
                                    EditorGUILayout.EndHorizontal();
                                    EditorGUILayout.BeginHorizontal();
                                    GUILayout.Space(50);
                                    if (!vectorsAndUnityTypes.ContainsKey(tag + "pXnewEntryValue" + n))
                                        vectorsAndUnityTypes[tag + "pXnewEntryValue" + n] = "";
                                    vectorsAndUnityTypes[tag + "pXnewEntryValue" + n] = myTextField("X: ", vectorsAndUnityTypes[tag + "pXnewEntryValue" + n], true);
                                    if (!vectorsAndUnityTypes.ContainsKey(tag + "pYnewEntryValue" + n))
                                        vectorsAndUnityTypes[tag + "pYnewEntryValue" + n] = "";
                                    vectorsAndUnityTypes[tag + "pYnewEntryValue" + n] = myTextField("Y: ", vectorsAndUnityTypes[tag + "pYnewEntryValue" + n], true);
                                    if (!vectorsAndUnityTypes.ContainsKey(tag + "pZnewEntryValue" + n))
                                        vectorsAndUnityTypes[tag + "pZnewEntryValue" + n] = "";
                                    vectorsAndUnityTypes[tag + "pZnewEntryValue" + n] = myTextField("Z: ", vectorsAndUnityTypes[tag + "pZnewEntryValue" + n], true);
                                    EditorGUILayout.EndHorizontal();

                                    EditorGUILayout.BeginHorizontal();
                                    GUILayout.Space(40);
                                    myLabelField("Rotation");
                                    EditorGUILayout.EndHorizontal();
                                    EditorGUILayout.BeginHorizontal();
                                    GUILayout.Space(50);
                                    if (!vectorsAndUnityTypes.ContainsKey(tag + "rXnewEntryValue" + n))
                                        vectorsAndUnityTypes[tag + "rXnewEntryValue" + n] = "";
                                    vectorsAndUnityTypes[tag + "rXnewEntryValue" + n] = myTextField("X: ", vectorsAndUnityTypes[tag + "rXnewEntryValue" + n], true);
                                    if (!vectorsAndUnityTypes.ContainsKey(tag + "rYnewEntryValue" + n))
                                        vectorsAndUnityTypes[tag + "rYnewEntryValue" + n] = "";
                                    vectorsAndUnityTypes[tag + "rYnewEntryValue" + n] = myTextField("Y: ", vectorsAndUnityTypes[tag + "rYnewEntryValue" + n], true);
                                    if (!vectorsAndUnityTypes.ContainsKey(tag + "rZnewEntryValue" + n))
                                        vectorsAndUnityTypes[tag + "rZnewEntryValue" + n] = "";
                                    vectorsAndUnityTypes[tag + "rZnewEntryValue" + n] = myTextField("Z: ", vectorsAndUnityTypes[tag + "rZnewEntryValue" + n], true);
                                    EditorGUILayout.EndHorizontal();

                                    EditorGUILayout.BeginHorizontal();
                                    GUILayout.Space(40);
                                    myLabelField("Scale");
                                    EditorGUILayout.EndHorizontal();
                                    EditorGUILayout.BeginHorizontal();
                                    GUILayout.Space(50);
                                    if (!vectorsAndUnityTypes.ContainsKey(tag + "sXnewEntryValue" + n))
                                        vectorsAndUnityTypes[tag + "sXnewEntryValue" + n] = "";
                                    vectorsAndUnityTypes[tag + "sXnewEntryValue" + n] = myTextField("X: ", vectorsAndUnityTypes[tag + "sXnewEntryValue" + n], true);
                                    if (!vectorsAndUnityTypes.ContainsKey(tag + "sYnewEntryValue" + n))
                                        vectorsAndUnityTypes[tag + "sYnewEntryValue" + n] = "";
                                    vectorsAndUnityTypes[tag + "sYnewEntryValue" + n] = myTextField("Y: ", vectorsAndUnityTypes[tag + "sYnewEntryValue" + n], true);
                                    if (!vectorsAndUnityTypes.ContainsKey(tag + "sZnewEntryValue" + n))
                                        vectorsAndUnityTypes[tag + "sZnewEntryValue" + n] = "";
                                    vectorsAndUnityTypes[tag + "sZnewEntryValue" + n] = myTextField("Z: ", vectorsAndUnityTypes[tag + "sZnewEntryValue" + n], true);
                                    EditorGUILayout.EndHorizontal();*/
                                    #endregion
                                }
                                else {
                                    if (type == Serializer.SerializationTypes.Vector2)
                                    {
                                        #region vector2                                        
                                        EditorGUILayout.BeginHorizontal();
                                        GUILayout.Space(40);
                                        if (!vectorsAndUnityTypes.ContainsKey(tag + "XnewEntryValue" + n))
                                            vectorsAndUnityTypes[tag + "XnewEntryValue" + n] = "";
                                        vectorsAndUnityTypes[tag + "XnewEntryValue" + n] = myTextField("X: ", vectorsAndUnityTypes[tag + "XnewEntryValue" + n], true);
                                        if (!vectorsAndUnityTypes.ContainsKey(tag + "YnewEntryValue" + n))
                                            vectorsAndUnityTypes[tag + "YnewEntryValue" + n] = "";
                                        vectorsAndUnityTypes[tag + "YnewEntryValue" + n] = myTextField("Y: ", vectorsAndUnityTypes[tag + "YnewEntryValue" + n], true);
                                        EditorGUILayout.EndHorizontal();
                                        #endregion
                                    }
                                    else {
                                        if (type == Serializer.SerializationTypes.Vector3)
                                        {
                                            #region vector3       
                                            EditorGUILayout.BeginHorizontal();
                                            GUILayout.Space(40);
                                            if (!vectorsAndUnityTypes.ContainsKey(tag + "XnewEntryValue" + n))
                                                vectorsAndUnityTypes[tag + "XnewEntryValue" + n] = "";
                                            vectorsAndUnityTypes[tag + "XnewEntryValue" + n] = myTextField("X: ", vectorsAndUnityTypes[tag + "XnewEntryValue" + n], true);
                                            if (!vectorsAndUnityTypes.ContainsKey(tag + "YnewEntryValue" + n))
                                                vectorsAndUnityTypes[tag + "YnewEntryValue" + n] = "";
                                            vectorsAndUnityTypes[tag + "YnewEntryValue" + n] = myTextField("Y: ", vectorsAndUnityTypes[tag + "YnewEntryValue" + n], true);
                                            if (!vectorsAndUnityTypes.ContainsKey(tag + "ZnewEntryValue" + n))
                                                vectorsAndUnityTypes[tag + "ZnewEntryValue" + n] = "";
                                            vectorsAndUnityTypes[tag + "ZnewEntryValue" + n] = myTextField("Z: ", vectorsAndUnityTypes[tag + "ZnewEntryValue" + n], true);
                                            EditorGUILayout.EndHorizontal();
                                            #endregion
                                        }
                                        else {
                                            if (type == Serializer.SerializationTypes.Vector4)
                                            {
                                                #region vector4  
                                                EditorGUILayout.BeginHorizontal();
                                                GUILayout.Space(40);
                                                if (!vectorsAndUnityTypes.ContainsKey(tag + "XnewEntryValue" + n))
                                                    vectorsAndUnityTypes[tag + "XnewEntryValue" + n] = "";
                                                vectorsAndUnityTypes[tag + "XnewEntryValue" + n] = myTextField("X: ", vectorsAndUnityTypes[tag + "XnewEntryValue" + n], true);
                                                if (!vectorsAndUnityTypes.ContainsKey(tag + "YnewEntryValue" + n))
                                                    vectorsAndUnityTypes[tag + "YnewEntryValue" + n] = "";
                                                vectorsAndUnityTypes[tag + "YnewEntryValue" + n] = myTextField("Y: ", vectorsAndUnityTypes[tag + "YnewEntryValue" + n], true);
                                                if (!vectorsAndUnityTypes.ContainsKey(tag + "ZnewEntryValue" + n))
                                                    vectorsAndUnityTypes[tag + "ZnewEntryValue" + n] = "";
                                                vectorsAndUnityTypes[tag + "ZnewEntryValue" + n] = myTextField("Z: ", vectorsAndUnityTypes[tag + "ZnewEntryValue" + n], true);
                                                if (!vectorsAndUnityTypes.ContainsKey(tag + "WnewEntryValue" + n))
                                                    vectorsAndUnityTypes[tag + "WnewEntryValue" + n] = "";
                                                vectorsAndUnityTypes[tag + "WnewEntryValue" + n] = myTextField("W: ", vectorsAndUnityTypes[tag + "WnewEntryValue" + n], true);
                                                EditorGUILayout.EndHorizontal();
                                                #endregion
                                            }
                                            else
                                            {
                                                if (type == Serializer.SerializationTypes.Quaternion)
                                                {
                                                    #region quaternion 
                                                    EditorGUILayout.BeginHorizontal();
                                                    GUILayout.Space(40);
                                                    if (!vectorsAndUnityTypes.ContainsKey(tag + "XnewEntryValue" + n))
                                                        vectorsAndUnityTypes[tag + "XnewEntryValue" + n] = "";
                                                    vectorsAndUnityTypes[tag + "XnewEntryValue" + n] = myTextField("X: ", vectorsAndUnityTypes[tag + "XnewEntryValue" + n], true);
                                                    if (!vectorsAndUnityTypes.ContainsKey(tag + "YnewEntryValue" + n))
                                                        vectorsAndUnityTypes[tag + "YnewEntryValue" + n] = "";
                                                    vectorsAndUnityTypes[tag + "YnewEntryValue" + n] = myTextField("Y: ", vectorsAndUnityTypes[tag + "YnewEntryValue" + n], true);
                                                    if (!vectorsAndUnityTypes.ContainsKey(tag + "ZnewEntryValue" + n))
                                                        vectorsAndUnityTypes[tag + "ZnewEntryValue" + n] = "";
                                                    vectorsAndUnityTypes[tag + "ZnewEntryValue" + n] = myTextField("Z: ", vectorsAndUnityTypes[tag + "ZnewEntryValue" + n], true);
                                                    if (!vectorsAndUnityTypes.ContainsKey(tag + "WnewEntryValue" + n))
                                                        vectorsAndUnityTypes[tag + "WnewEntryValue" + n] = "";
                                                    EditorGUILayout.EndHorizontal();
                                                    #endregion
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                                EditorGUILayout.EndVertical();
                            }
                            #region showMoreAddFields
                            switch (type)
                            {
                                case Serializer.SerializationTypes.Vector2:
                                    if (vectorsAndUnityTypes[tag + "XnewEntryValue" + (newCounter - 1)] != "" && vectorsAndUnityTypes[tag + "YnewEntryValue" + (newCounter - 1)] != "")
                                    {
                                        vectorsAndUnityTypes[tag + "newEntryCounter"] = (newCounter + 1).ToString();
                                    }
                                    break;
                                case Serializer.SerializationTypes.Vector3:
                                    if (vectorsAndUnityTypes[tag + "XnewEntryValue" + (newCounter - 1)] != "" && vectorsAndUnityTypes[tag + "YnewEntryValue" + (newCounter - 1)] != "" && vectorsAndUnityTypes[tag + "ZnewEntryValue" + (newCounter - 1)] != "")
                                    {
                                        vectorsAndUnityTypes[tag + "newEntryCounter"] = (newCounter + 1).ToString();
                                    }
                                    break;
                                case Serializer.SerializationTypes.Vector4:
                                    if (vectorsAndUnityTypes[tag + "XnewEntryValue" + (newCounter - 1)] != "" && vectorsAndUnityTypes[tag + "YnewEntryValue" + (newCounter - 1)] != "" && vectorsAndUnityTypes[tag + "ZnewEntryValue" + (newCounter - 1)] != "" && vectorsAndUnityTypes[tag + "WnewEntryValue" + (newCounter - 1)] != "")
                                    {
                                        vectorsAndUnityTypes[tag + "newEntryCounter"] = (newCounter + 1).ToString();
                                    }
                                    break;
                                case Serializer.SerializationTypes.Quaternion:
                                    if (vectorsAndUnityTypes[tag + "XnewEntryValue" + (newCounter - 1)] != "" && vectorsAndUnityTypes[tag + "YnewEntryValue" + (newCounter - 1)] != "" && vectorsAndUnityTypes[tag + "ZnewEntryValue" + (newCounter - 1)] != "" && vectorsAndUnityTypes[tag + "WnewEntryValue" + (newCounter - 1)] != "")
                                    {
                                        vectorsAndUnityTypes[tag + "newEntryCounter"] = (newCounter + 1).ToString();
                                    }
                                    break;
                                case Serializer.SerializationTypes.Transform:
                                    /*if (vectorsAndUnityTypes[tag + "pXnewEntryValue" + (newCounter - 1)] != "" && vectorsAndUnityTypes[tag + "pYnewEntryValue" + (newCounter - 1)] != "" && vectorsAndUnityTypes[tag + "pZnewEntryValue" + (newCounter - 1)] != "")
                                    {
                                        if (vectorsAndUnityTypes[tag + "rXnewEntryValue" + (newCounter - 1)] != "" && vectorsAndUnityTypes[tag + "rYnewEntryValue" + (newCounter - 1)] != "" && vectorsAndUnityTypes[tag + "rZnewEntryValue" + (newCounter - 1)] != "")
                                        {
                                            if (vectorsAndUnityTypes[tag + "sXnewEntryValue" + (newCounter - 1)] != "" && vectorsAndUnityTypes[tag + "sYnewEntryValue" + (newCounter - 1)] != "" && vectorsAndUnityTypes[tag + "sZnewEntryValue" + (newCounter - 1)] != "")
                                            {
                                                vectorsAndUnityTypes[tag + "newEntryCounter"] = (newCounter + 1).ToString();
                                            }
                                        }
                                    }*/
                                    break;
                            }
                            #endregion
                        }
                        if (applyDict)
                        {
                            if (!vectorsAndUnityTypes.ContainsKey(tag + "newEntryCounter"))
                                vectorsAndUnityTypes[tag + "newEntryCounter"] = "0";
                            int newCounter = Convert.ToInt32(vectorsAndUnityTypes[tag + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                #region SaveNew
                                if (vectorsAndUnityTypes[tag + "NameNewEntryValue" + n].Trim() != "")
                                {
                                    switch (type)
                                    {
                                        case Serializer.SerializationTypes.Vector2:
                                            if (vectorsAndUnityTypes[tag + "XnewEntryValue" + n] == "" || vectorsAndUnityTypes[tag + "XnewEntryValue" +n] == "-")
                                                break;
                                            if (vectorsAndUnityTypes[tag + "YnewEntryValue" + n] == "" || vectorsAndUnityTypes[tag + "YnewEntryValue" + n] == "-")
                                                break;
                                            Vector2 vec2 = new Vector2(Convert.ToSingle(vectorsAndUnityTypes[tag + "XnewEntryValue" + n]), Convert.ToSingle(vectorsAndUnityTypes[tag + "YnewEntryValue" + n]));
                                            EpicPrefs.SetVector2(vectorsAndUnityTypes[tag + "NameNewEntryValue"+n], vec2, false);
                                            break;
                                        case Serializer.SerializationTypes.Vector3:
                                            if (vectorsAndUnityTypes[tag + "XnewEntryValue" + n] == "" || vectorsAndUnityTypes[tag + "XnewEntryValue" + n] == "-")
                                                break;
                                            if (vectorsAndUnityTypes[tag + "YnewEntryValue" + n] == "" || vectorsAndUnityTypes[tag + "YnewEntryValue" + n] == "-")
                                                break;
                                            if (vectorsAndUnityTypes[tag + "ZnewEntryValue" + n] == "" || vectorsAndUnityTypes[tag + "ZnewEntryValue" + n] == "-")
                                                break;
                                            Vector3 vec3 = new Vector3(Convert.ToSingle(vectorsAndUnityTypes[tag + "XnewEntryValue" + n]), Convert.ToSingle(vectorsAndUnityTypes[tag + "YnewEntryValue" + n]), Convert.ToSingle(vectorsAndUnityTypes[tag + "ZnewEntryValue" + n]));
                                            EpicPrefs.SetVector3(vectorsAndUnityTypes[tag + "NameNewEntryValue"+n], vec3, false);
                                            break;
                                        case Serializer.SerializationTypes.Vector4:
                                            if (vectorsAndUnityTypes[tag + "XnewEntryValue" + n] == "" || vectorsAndUnityTypes[tag + "XnewEntryValue" + n] == "-")
                                                break;
                                            if (vectorsAndUnityTypes[tag + "YnewEntryValue" + n] == "" || vectorsAndUnityTypes[tag + "YnewEntryValue" + n] == "-")
                                                break;
                                            if (vectorsAndUnityTypes[tag + "ZnewEntryValue" + n] == "" || vectorsAndUnityTypes[tag + "ZnewEntryValue" + n] == "-")
                                                break;
                                            if (vectorsAndUnityTypes[tag + "WnewEntryValue" + n] == "" || vectorsAndUnityTypes[tag + "WnewEntryValue" + n] == "-")
                                                break;
                                            Vector4 vec4 = new Vector4(Convert.ToSingle(vectorsAndUnityTypes[tag + "XnewEntryValue" + n]), Convert.ToSingle(vectorsAndUnityTypes[tag + "YnewEntryValue" + n]), Convert.ToSingle(vectorsAndUnityTypes[tag + "ZnewEntryValue" + n]), Convert.ToSingle(vectorsAndUnityTypes[tag + "WnewEntryValue" + n]));
                                            EpicPrefs.SetVector4(vectorsAndUnityTypes[tag + "NameNewEntryValue"+n], vec4, false);
                                            break;
                                        case Serializer.SerializationTypes.Quaternion:
                                            if (vectorsAndUnityTypes[tag + "XnewEntryValue" + n] == "" || vectorsAndUnityTypes[tag + "XnewEntryValue" + n] == "-")
                                                break;
                                            if (vectorsAndUnityTypes[tag + "YnewEntryValue" + n] == "" || vectorsAndUnityTypes[tag + "YnewEntryValue" + n] == "-")
                                                break;
                                            if (vectorsAndUnityTypes[tag + "ZnewEntryValue" + n] == "" || vectorsAndUnityTypes[tag + "ZnewEntryValue" + n] == "-")
                                                break;
                                            if (vectorsAndUnityTypes[tag + "WnewEntryValue" + n] == "" || vectorsAndUnityTypes[tag + "WnewEntryValue" + n] == "-")
                                                break;
                                            Quaternion quat = new Quaternion(Convert.ToSingle(vectorsAndUnityTypes[tag + "XnewEntryValue" + n]), Convert.ToSingle(vectorsAndUnityTypes[tag + "YnewEntryValue" + n]), Convert.ToSingle(vectorsAndUnityTypes[tag + "ZnewEntryValue" + n]), Convert.ToSingle(vectorsAndUnityTypes[tag + "WnewEntryValue" + n]));
                                            EpicPrefs.SetQuaternion(vectorsAndUnityTypes[tag + "NameNewEntryValue"+n], quat, false);
                                            break;
                                        case Serializer.SerializationTypes.Transform:
                                            /*GameObject newObj = new GameObject();
                                            Transform tempTrans = newObj.transform;
                                            tempTrans.name = vectorsAndUnityTypes[tag + "NameNewEntryValue"+n];
                                            if (vectorsAndUnityTypes[tag + "pXnewEntryValue" + n] == "" || vectorsAndUnityTypes[tag + "pXnewEntryValue" + n] == "-")
                                                break;
                                            if (vectorsAndUnityTypes[tag + "pYnewEntryValue" + n] == "" || vectorsAndUnityTypes[tag + "pYnewEntryValue" + n] == "-")
                                                break;
                                            if (vectorsAndUnityTypes[tag + "pZnewEntryValue" + n] == "" || vectorsAndUnityTypes[tag + "pZnewEntryValue" + n] == "-")
                                                break;
                                            tempTrans.position = new Vector3(Convert.ToSingle(vectorsAndUnityTypes[tag + "pXnewEntryValue" + n]), Convert.ToSingle(vectorsAndUnityTypes[tag + "pYnewEntryValue" + n]), Convert.ToSingle(vectorsAndUnityTypes[tag + "pZnewEntryValue" + n]));
                                            if (vectorsAndUnityTypes[tag + "rXnewEntryValue" + n] == "" || vectorsAndUnityTypes[tag + "rXnewEntryValue" + n] == "-")
                                                break;
                                            if (vectorsAndUnityTypes[tag + "rYnewEntryValue" + n] == "" || vectorsAndUnityTypes[tag + "rYnewEntryValue" + n] == "-")
                                                break;
                                            if (vectorsAndUnityTypes[tag + "rZnewEntryValue" + n] == "" || vectorsAndUnityTypes[tag + "rZnewEntryValue" + n] == "-")
                                                break;
                                            tempTrans.rotation = Quaternion.Euler(new Vector3(Convert.ToSingle(vectorsAndUnityTypes[tag + "rXnewEntryValue" + n]), Convert.ToSingle(vectorsAndUnityTypes[tag + "rYnewEntryValue" + n]), Convert.ToSingle(vectorsAndUnityTypes[tag + "rZnewEntryValue" + n])));
                                            if (vectorsAndUnityTypes[tag + "sXnewEntryValue" + n] == "" || vectorsAndUnityTypes[tag + "sXnewEntryValue" + n] == "-")
                                                break;
                                            if (vectorsAndUnityTypes[tag + "sYnewEntryValue" + n] == "" || vectorsAndUnityTypes[tag + "sYnewEntryValue" + n] == "-")
                                                break;
                                            if (vectorsAndUnityTypes[tag + "sZnewEntryValue" + n] == "" || vectorsAndUnityTypes[tag + "sZnewEntryValue" + n] == "-")
                                                break;
                                            tempTrans.localScale = new Vector3(Convert.ToSingle(vectorsAndUnityTypes[tag + "sXnewEntryValue" + n]), Convert.ToSingle(vectorsAndUnityTypes[tag + "sYnewEntryValue" + n]), Convert.ToSingle(vectorsAndUnityTypes[tag + "sZnewEntryValue" + n]));
                                            EpicPrefs.SetTransform(vectorsAndUnityTypes[tag + "NameNewEntryValue"+n], tempTrans, Convert.ToBoolean(vectorsAndUnityTypes[tag  + "encrypt"]));
                                            DestroyImmediate(newObj);*/
                                            break;
                                    }
                                }
                                #endregion
                            }
                            vectorsAndUnityTypes[tag  + "newEntry"] = "false";
                            vectorsAndUnityTypes[tag  + "newEntryCounter"] = "0";
                            #region Save 
                            float _x = 0;
                            float _y = 0;
                            float _z = 0;
                            float _w = 0;

                            switch (type)
                            {
                                case Serializer.SerializationTypes.Vector2:
                                    if (vectorsAndUnityTypes[tag + t.Key + "X"] != "" && vectorsAndUnityTypes[tag + t.Key + "X"] != "-")
                                        _x = Convert.ToSingle(vectorsAndUnityTypes[tag + t.Key + "X"]);
                                    if (vectorsAndUnityTypes[tag + t.Key + "Y"] != "" && vectorsAndUnityTypes[tag + t.Key + "Y"] != "-")
                                        _y = Convert.ToSingle(vectorsAndUnityTypes[tag + t.Key + "Y"]);
                                    Vector2 vec2 = new Vector2(_x,_y);
                                    EpicPrefs.SetVector2(t.Key, vec2, Convert.ToBoolean(vectorsAndUnityTypes[tag + t.Key + "encrypt"]));
                                    break;
                                case Serializer.SerializationTypes.Vector3:
                                    if (vectorsAndUnityTypes[tag + t.Key + "X"] != "" && vectorsAndUnityTypes[tag + t.Key + "X"] != "-")
                                        _x = Convert.ToSingle(vectorsAndUnityTypes[tag + t.Key + "X"]);
                                    if (vectorsAndUnityTypes[tag + t.Key + "Y"] != "" && vectorsAndUnityTypes[tag + t.Key + "Y"] != "-")
                                        _y = Convert.ToSingle(vectorsAndUnityTypes[tag + t.Key + "Y"]);
                                    if (vectorsAndUnityTypes[tag + t.Key + "Z"] != "" && vectorsAndUnityTypes[tag + t.Key + "Z"] != "-")
                                        _z = Convert.ToSingle(vectorsAndUnityTypes[tag + t.Key + "Z"]);
                                    Vector3 vec3 = new Vector3(_x, _y, _z);
                                    EpicPrefs.SetVector3(t.Key, vec3, Convert.ToBoolean(vectorsAndUnityTypes[tag + t.Key + "encrypt"]));
                                    break;
                                case Serializer.SerializationTypes.Vector4:
                                    if (vectorsAndUnityTypes[tag + t.Key + "X"] != "" && vectorsAndUnityTypes[tag + t.Key + "X"] != "-")
                                        _x = Convert.ToSingle(vectorsAndUnityTypes[tag + t.Key + "X"]);
                                    if (vectorsAndUnityTypes[tag + t.Key + "Y"] != "" && vectorsAndUnityTypes[tag + t.Key + "Y"] != "-")
                                        _y = Convert.ToSingle(vectorsAndUnityTypes[tag + t.Key + "Y"]);
                                    if (vectorsAndUnityTypes[tag + t.Key + "Z"] != "" && vectorsAndUnityTypes[tag + t.Key + "Z"] != "-")
                                        _z = Convert.ToSingle(vectorsAndUnityTypes[tag + t.Key + "Z"]);
                                    if (vectorsAndUnityTypes[tag + t.Key + "W"] != "" && vectorsAndUnityTypes[tag + t.Key + "W"] != "-")
                                        _w = Convert.ToSingle(vectorsAndUnityTypes[tag + t.Key + "W"]);
                                    Vector4 vec4 = new Vector4(_x, _y, _z,_w);
                                    EpicPrefs.SetVector4(t.Key, vec4, Convert.ToBoolean(vectorsAndUnityTypes[tag + t.Key + "encrypt"]));
                                    break;
                                case Serializer.SerializationTypes.Quaternion:
                                    if (vectorsAndUnityTypes[tag + t.Key + "X"] != "" && vectorsAndUnityTypes[tag + t.Key + "X"] != "-")
                                        _x = Convert.ToSingle(vectorsAndUnityTypes[tag + t.Key + "X"]);
                                    if (vectorsAndUnityTypes[tag + t.Key + "Y"] != "" && vectorsAndUnityTypes[tag + t.Key + "Y"] != "-")
                                        _y = Convert.ToSingle(vectorsAndUnityTypes[tag + t.Key + "Y"]);
                                    if (vectorsAndUnityTypes[tag + t.Key + "Z"] != "" && vectorsAndUnityTypes[tag + t.Key + "Z"] != "-")
                                        _z = Convert.ToSingle(vectorsAndUnityTypes[tag + t.Key + "Z"]);
                                    if (vectorsAndUnityTypes[tag + t.Key + "W"] != "" && vectorsAndUnityTypes[tag + t.Key + "W"] != "-")
                                        _w = Convert.ToSingle(vectorsAndUnityTypes[tag + t.Key + "W"]);
                                    Quaternion quat = new Quaternion(_x,_y,_z,_w);
                                    EpicPrefs.SetQuaternion(t.Key, quat, Convert.ToBoolean(vectorsAndUnityTypes[tag + t.Key + "encrypt"]));
                                    break;
                                case Serializer.SerializationTypes.Transform:
                                    /*GameObject newObj = new GameObject();
                                    Transform tempTrans = newObj.transform;
                                    tempTrans.name = t.Key;
                                    if (vectorsAndUnityTypes[tag + t.Key + "pX"] != "" && vectorsAndUnityTypes[tag + t.Key + "pX"] != "-")
                                        _x = Convert.ToSingle(vectorsAndUnityTypes[tag + t.Key + "pX"]);
                                    if (vectorsAndUnityTypes[tag + t.Key + "pY"] != "" && vectorsAndUnityTypes[tag + t.Key + "pY"] != "-")
                                        _y = Convert.ToSingle(vectorsAndUnityTypes[tag + t.Key + "pY"]);
                                    if (vectorsAndUnityTypes[tag + t.Key + "pZ"] != "" && vectorsAndUnityTypes[tag + t.Key + "pZ"] != "-")
                                        _z = Convert.ToSingle(vectorsAndUnityTypes[tag + t.Key + "pZ"]);
                                    tempTrans.position = new Vector3(_x,_y,_z);
                                    if (vectorsAndUnityTypes[tag + t.Key + "rX"] != "" && vectorsAndUnityTypes[tag + t.Key + "rX"] != "-")
                                        _x = Convert.ToSingle(vectorsAndUnityTypes[tag + t.Key + "rX"]);
                                    if (vectorsAndUnityTypes[tag + t.Key + "rY"] != "" && vectorsAndUnityTypes[tag + t.Key + "rY"] != "-")
                                        _y = Convert.ToSingle(vectorsAndUnityTypes[tag + t.Key + "rY"]);
                                    if (vectorsAndUnityTypes[tag + t.Key + "rZ"] != "" && vectorsAndUnityTypes[tag + t.Key + "rZ"] != "-")
                                        _z = Convert.ToSingle(vectorsAndUnityTypes[tag + t.Key + "rZ"]);
                                    tempTrans.rotation = Quaternion.Euler(_x, _y, _z);
                                    if (vectorsAndUnityTypes[tag + t.Key + "sX"] != "" && vectorsAndUnityTypes[tag + t.Key + "sX"] != "-")
                                        _x = Convert.ToSingle(vectorsAndUnityTypes[tag + t.Key + "sX"]);
                                    if (vectorsAndUnityTypes[tag + t.Key + "sY"] != "" && vectorsAndUnityTypes[tag + t.Key + "sY"] != "-")
                                        _y = Convert.ToSingle(vectorsAndUnityTypes[tag + t.Key + "sY"]);
                                    if (vectorsAndUnityTypes[tag + t.Key + "sZ"] != "" && vectorsAndUnityTypes[tag + t.Key + "sZ"] != "-")
                                        _z = Convert.ToSingle(vectorsAndUnityTypes[tag + t.Key + "sZ"]);
                                    tempTrans.localScale = new Vector3(_x,_y,_z);
                                    EpicPrefs.SetTransform(t.Key, tempTrans, Convert.ToBoolean(vectorsAndUnityTypes[tag + t.Key + "encrypt"]));*/
                                    break;
                            }
                            #endregion
                            vectorsAndUnityTypes[tag + "newEntryCounter"] = "0";
                            vectorsAndUnityTypes[tag + "newEntry"] = "false";
                            editDetails[counter] = false;
                            return;
                        }
                    }
                    if (editDetails[counter])
                    {
                        GUILayout.Space(10);
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(40);
                        if (vectorsAndUnityTypes[tag + "newEntry"] != "true")
                        {
                            if (GUILayout.Button(aB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                            {
                                vectorsAndUnityTypes[tag  + "newEntryCounter"] = "1";
                                vectorsAndUnityTypes[tag  + "newEntry"] = "true";
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                    counter++;
                }
            }
            Separator();
            #endregion
            #region Submission
            switch (type)
            {
                case Serializer.SerializationTypes.Transform:
                    /*showTransform = showDetails;
                    showSubTransform = showSubDetails;
                    editSubTransform = editDetails;*/
                    break;
                case Serializer.SerializationTypes.Vector2:
                    showVector2 = showDetails;
                    showSubVector2 = showSubDetails;
                    editSubVector2 = editDetails;
                    break;
                case Serializer.SerializationTypes.Vector3:
                    showVector3 = showDetails;
                    showSubVector3 = showSubDetails;
                    editSubVector3 = editDetails;
                    break;
                case Serializer.SerializationTypes.Vector4:
                    showVector4 = showDetails;
                    showSubVector4 = showSubDetails;
                    editSubVector4 = editDetails;
                    break;
                case Serializer.SerializationTypes.Quaternion:
                    showQuaternion = showDetails;
                    showSubQuaternion = showSubDetails;
                    editSubQuaternion = editDetails;
                    break;
            }
            #endregion
        }
    }
    private void ListDisplay(Serializer.SerializationTypes type)
    {
        string tag = "";
        string title = "";
        Dictionary<string, string> usedDict = new Dictionary<string, string>();
        bool[] editDetails = new bool[0];
        bool[] showSubDetails = new bool[0];
        bool showDetails=false;
        #region Selection
        switch (type)
        {
            case Serializer.SerializationTypes.ListI:
                tag = "[intList]";
                title = "Integer List";
                usedDict = ListIntDict;
                showDetails = showListI;
                showSubDetails = showSubListI;
                editDetails = editSubListI;
                break;
            case Serializer.SerializationTypes.ListS:
                tag = "[stringList]";
                title = "String List";
                usedDict = ListStringDict;
                showDetails = showListS;
                showSubDetails = showSubListS;
                editDetails = editSubListS;
                break;
            case Serializer.SerializationTypes.ListB:
                tag = "[boolList]";
                title = "Boolean List";
                usedDict = ListBoolDict;
                showDetails = showListB;
                showSubDetails = showSubListB;
                editDetails = editSubListB;
                break;
            case Serializer.SerializationTypes.ListF:
                tag = "[floatList]";
                title = "Float List";
                usedDict = ListFloatDict;
                showDetails = showListF;
                showSubDetails = showSubListF;
                editDetails = editSubListF;
                break;
            case Serializer.SerializationTypes.ListD:
                tag = "[doubleList]";
                title = "Double List";
                usedDict = ListDoubleDict;
                showDetails = showListD;
                showSubDetails = showSubListD;
                editDetails = editSubListD;
                break;
            case Serializer.SerializationTypes.ListL:
                tag = "[longList]";
                title = "Long List";
                usedDict = ListLongDict;
                showDetails = showListL;
                showSubDetails = showSubListL;
                editDetails = editSubListL;
                break;
            case Serializer.SerializationTypes.ArrayFloat:
                tag = "[floatArray]";
                title = "Float Array";
                usedDict = ArrayFloatDict;
                showDetails = showArrayF;
                showSubDetails = showSubArrayF;
                editDetails = editSubArrayF;
                break;
            case Serializer.SerializationTypes.ArrayInt:
                tag = "[intArray]";
                title = "Integer Array";
                usedDict = ArrayIntDict;
                showDetails = showArrayI;
                showSubDetails = showSubArrayI;
                editDetails = editSubArrayI;
                break;
            case Serializer.SerializationTypes.ArrayString:
                tag = "[stringArray]";
                title = "String Array";
                usedDict = ArrayStringDict;
                showDetails = showArrayS;
                showSubDetails = showSubArrayS;
                editDetails = editSubArrayS;
                break;
            case Serializer.SerializationTypes.ArrayDouble:
                tag = "[doubleArray]";
                title = "Double Array";
                usedDict = ArrayDoubleDict;
                showDetails = showArrayD;
                showSubDetails = showSubArrayD;
                editDetails = editSubArrayD;
                break;
        }
        #endregion
        #region GUI
        if (usedDict.Count > 0)
        {
            showDetails = EditorGUILayout.Foldout(showDetails,title +" Prefs", foldout);
            if (showDetails)
            {
                GUILayout.Space(5);
                int counter = 0;
                foreach (KeyValuePair<string, string> t in usedDict)
                {
                    if (!listsAndArrays.ContainsKey(tag + t.Key + "encrypt"))
                        listsAndArrays[tag +t.Key + "encrypt"] = t.Value;
                    if (!listsAndArrays.ContainsKey(tag + t.Key + "name"))
                        listsAndArrays[tag + t.Key + "name"] = t.Key;
                    List<string> tList = new List<string>();
                    List<string> nList = new List<string>();
                    if (type == Serializer.SerializationTypes.ArrayFloat)
                    {
                        float[] tmpArr = ArrayFloatDictValues[t.Key];//EpicPrefs.GetArrayFloat(t.Key, Convert.ToBoolean(t.Value));
                        foreach (float _s in tmpArr)
                        {
                            tList.Add(_s.ToString());
                            nList.Add(_s.ToString());
                        }
                    }
                    else {
                        if (type == Serializer.SerializationTypes.ArrayInt)
                        {
                            int[] tmpArr = ArrayIntDictValues[t.Key];//EpicPrefs.GetArrayInt(t.Key, Convert.ToBoolean(t.Value));
                            foreach (int _s in tmpArr)
                            {
                                tList.Add(_s.ToString());
                                nList.Add(_s.ToString());
                            }
                        }
                        else {
                            if (type == Serializer.SerializationTypes.ArrayString)
                            {
                                string[] tmpArr = ArrayStringDictValues[t.Key];//EpicPrefs.GetArrayString(t.Key, Convert.ToBoolean(t.Value));
                                foreach (string _s in tmpArr)
                                {
                                    tList.Add(_s);
                                    nList.Add(_s);
                                }
                            }
                            else {
                                if (type == Serializer.SerializationTypes.ListI)
                                {
                                    List<int> nL = ListIntDictValues[t.Key];//EpicPrefs.GetListInt(t.Key,def, Convert.ToBoolean(t.Value));
                                    tList = nL.ConvertAll(obj => obj.ToString());
                                    nList = nL.ConvertAll(obj => obj.ToString());
                                }
                                else
                                {
                                    if (type == Serializer.SerializationTypes.ListB)
                                    {
                                        List<bool> nL = ListBoolDictValues[t.Key];//EpicPrefs.GetListBool(t.Key, Convert.ToBoolean(t.Value));
                                        tList = nL.ConvertAll(obj => obj.ToString());
                                        nList = nL.ConvertAll(obj => obj.ToString());
                                    }
                                    else
                                    {
                                        if (type == Serializer.SerializationTypes.ListD)
                                        {
                                            List<double> nL = ListDoubleDictValues[t.Key];//EpicPrefs.GetListDouble(t.Key,def, Convert.ToBoolean(t.Value));
                                            tList = nL.ConvertAll(obj => obj.ToString());
                                            nList = nL.ConvertAll(obj => obj.ToString());
                                        }
                                        else
                                        {
                                            if (type == Serializer.SerializationTypes.ListL)
                                            {
                                                List<long> nL = ListLongDictValues[t.Key];//EpicPrefs.GetListLong(t.Key, Convert.ToBoolean(t.Value));
                                                tList = nL.ConvertAll(obj => obj.ToString());
                                                nList = nL.ConvertAll(obj => obj.ToString());
                                            }
                                            else
                                            {
                                                if (type == Serializer.SerializationTypes.ListS)
                                                {
                                                    tList = ListStringDictValues[t.Key];//EpicPrefs.GetListString(t.Key, Convert.ToBoolean(t.Value));
                                                    nList = ListStringDictValues[t.Key];//EpicPrefs.GetListString(t.Key, Convert.ToBoolean(t.Value));
                                                }
                                                else
                                                {
                                                    if (type == Serializer.SerializationTypes.ListF)
                                                    {
                                                        List<float> nL = ListFloatDictValues[t.Key];//EpicPrefs.GetListFloat(t.Key, Convert.ToBoolean(t.Value));
                                                        tList = nL.ConvertAll(obj => obj.ToString());
                                                        nList = nL.ConvertAll(obj => obj.ToString());
                                                    }
                                                    else
                                                    {
                                                        if (type == Serializer.SerializationTypes.ArrayDouble)
                                                        {
                                                            double[] tmpArr = ArrayDoubleDictValues[t.Key];//EpicPrefs.GetArrayFloat(t.Key, Convert.ToBoolean(t.Value));
                                                            foreach (float _s in tmpArr)
                                                            {
                                                                tList.Add(_s.ToString());
                                                                nList.Add(_s.ToString());
                                                            }
                                                        }
                                                    }
                                                }                                                
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    bool applyDict = false;
                    if (!listsAndArrays.ContainsKey(tag + t.Key + "newEntry"))
                        listsAndArrays[tag + t.Key + "newEntry"] = "false";
                    showSubDetails[counter] = EditorGUILayout.Foldout(showSubDetails[counter], "");
                    if (editDetails[counter])
                    {
                        listsAndArrays[tag + t.Key + "name"] = myTextField(listsAndArrays[tag + t.Key + "name"], textBox);
                    }
                    else
                    {
                        myLabelField(t.Key);
                    }
                    #region Encryption
                    bool encryptedCheck = Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]);
                    listsAndArrays[tag + t.Key + "encrypt"] = EncryptionToggle(listsAndArrays[tag + t.Key + "encrypt"]);
                    if (Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]) != encryptedCheck)
                        if (EditorUtility.DisplayDialog("Change encryption",
                "Are you sure to change the encryption settings of " + t.Key
                + " ? You will lose all unsaved changes.", "Yes", "No"))
                        {
                            editDetails[counter] = false;
                            listsAndArrays[tag + t.Key + "newEntry"] = "false";
                            if (!listsAndArrays.ContainsKey(tag + t.Key + "newEntryCounter"))
                                listsAndArrays[tag + t.Key + "newEntryCounter"] = "0";
                            int newCounter = Convert.ToInt32(listsAndArrays[tag + t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                listsAndArrays[tag + t.Key + "newEntryValue" + n] = "";
                            }
                            listsAndArrays[tag + t.Key + "newEntryCounter"] = "0";
                            if (type == Serializer.SerializationTypes.ArrayFloat)
                            {
                                EpicPrefs.SetArray(t.Key, EpicPrefs.GetArrayFloat(t.Key,!Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"])), Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                //EpicPrefs.DeleteEditorPrefs(t.Key, type, !Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                            }
                            else {
                                if (type == Serializer.SerializationTypes.ArrayInt)
                                {
                                    int[] nI = EpicPrefs.GetArrayInt(t.Key, !Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                    //EpicPrefs.DeleteEditorPrefs(t.Key, type, !Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                    EpicPrefs.SetArray(t.Key,nI, Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                }
                                else {
                                    if (type == Serializer.SerializationTypes.ArrayString)
                                    {
                                        string[] nI = EpicPrefs.GetArrayString(t.Key, !Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                        //EpicPrefs.DeleteEditorPrefs(t.Key, type, !Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                        EpicPrefs.SetArray(t.Key, nI, Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                    }
                                    else {
                                        if (type == Serializer.SerializationTypes.ListI)
                                        {
                                            List<int> nI = EpicPrefs.GetListInt(t.Key, !Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                            //EpicPrefs.DeleteEditorPrefs(t.Key, type, !Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                            EpicPrefs.SetList(t.Key, nI, Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                        }
                                        else
                                        {
                                            if (type == Serializer.SerializationTypes.ListB)
                                            {
                                                List<bool> nI = EpicPrefs.GetListBool(t.Key, !Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                                //EpicPrefs.DeleteEditorPrefs(t.Key, type, !Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                                EpicPrefs.SetList(t.Key, nI, Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                            }
                                            else
                                            {
                                                if (type == Serializer.SerializationTypes.ListD)
                                                {
                                                    List<double> nI = EpicPrefs.GetListDouble(t.Key, !Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                                    //EpicPrefs.DeleteEditorPrefs(t.Key, type, !Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                                    EpicPrefs.SetList(t.Key, nI, Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                                }
                                                else
                                                {
                                                    if (type == Serializer.SerializationTypes.ListL)
                                                    {
                                                        List<long> nI = EpicPrefs.GetListLong(t.Key, !Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                                        //EpicPrefs.DeleteEditorPrefs(t.Key, type, !Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                                        EpicPrefs.SetList(t.Key, nI, Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                                    }
                                                    else
                                                    {
                                                        if (type == Serializer.SerializationTypes.ListS)
                                                        {
                                                            List<string> nI = EpicPrefs.GetListString(t.Key, !Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                                            //EpicPrefs.DeleteEditorPrefs(t.Key, type, !Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                                            EpicPrefs.SetList(t.Key, nI, Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                                        }
                                                        else
                                                        {
                                                            if (type == Serializer.SerializationTypes.ListF)
                                                            {
                                                                List<float> nI = EpicPrefs.GetListFloat(t.Key, !Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                                                //EpicPrefs.DeleteEditorPrefs(t.Key, type, !Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                                                EpicPrefs.SetList(t.Key, nI, Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                                            }
                                                            else
                                                            {
                                                                if (type == Serializer.SerializationTypes.ArrayDouble)
                                                                {
                                                                    EpicPrefs.SetArray(t.Key, EpicPrefs.GetArrayDouble(t.Key, !Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"])), Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                                                    //EpicPrefs.DeleteEditorPrefs(t.Key, type, !Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            return;
                        }
                        else
                        {
                            listsAndArrays[tag + t.Key + "encrypt"] = encryptedCheck.ToString();
                        }
                    #endregion
                    if (editDetails[counter])
                    {
                        if (GUILayout.Button(apB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            applyDict = true;
                        }
                        if (GUILayout.Button(cB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            editDetails[counter] = false;
                            listsAndArrays[tag + t.Key + "newEntry"] = "false";
                            if (!listsAndArrays.ContainsKey(tag + t.Key + "newEntryCounter"))
                                listsAndArrays[tag + t.Key + "newEntryCounter"] = "0";
                            int newCounter = Convert.ToInt32(listsAndArrays[tag + t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                listsAndArrays[tag + t.Key + "newEntryValue" + n] = "";
                            }
                            listsAndArrays[tag + t.Key + "newEntryCounter"] = "0";
                            return;
                        }
                    }
                    else
                    {
                        if (GUILayout.Button(eB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            editDetails[counter] = true;
                            editDetails[counter] = true;
                        }
                        if (GUILayout.Button(dB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                        {
                            if (EditorUtility.DisplayDialog("Delete",
                "Are you sure to delete " + t.Key
                + " permanently ?", "Yes", "No"))
                                EpicPrefs.DeleteEditorPrefs(t.Key, type, Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                            return;
                        }
                    }
                    GUILayout.EndHorizontal();
                    if (showSubDetails[counter])
                    {
                        for (int n=0;n<tList.Count;n++)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.Space(20);
                            if (editDetails[counter])
                            {
                                GUILayout.Label("Index "+n.ToString(), text);
                                if (!listsAndArrays.ContainsKey(tag + t.Key + n.ToString()))
                                    listsAndArrays[tag + t.Key + n.ToString()] = tList[n];
                                listsAndArrays[tag + t.Key + n.ToString()] = myTextField(listsAndArrays[tag + t.Key + n.ToString()], textBox);
                                nList[n] = listsAndArrays[tag + t.Key + n.ToString()];
                            }
                            else
                            {
                                myLabelField("Index " + n.ToString(), tList[n]);
                                if (GUILayout.Button(dB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                                {
                                    nList.RemoveAt(n);
                                    applyDict = true;
                                }
                            }
                            GUILayout.EndHorizontal();
                        }
                        if (listsAndArrays[tag + t.Key + "newEntry"] == "true")
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            GUILayout.Space(20);
                            int newCounter = Convert.ToInt32(listsAndArrays[tag + t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                if (!listsAndArrays.ContainsKey(tag + t.Key + "newEntryValue" + n))
                                    listsAndArrays[tag + t.Key + "newEntryValue" + n] = "";

                            }
                            myLabelField("Index " + tList.Count.ToString());
                            listsAndArrays[tag + t.Key + "newEntryValue0"] = myTextField(listsAndArrays[tag + t.Key + "newEntryValue0"], textBox);
                            GUILayout.EndHorizontal();
                            for (int n = 1; n < newCounter; n++)
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Space(20);
                                GUILayout.Space(20);
                                myLabelField("Index " + (tList.Count + n).ToString());
                                listsAndArrays[tag + t.Key + "newEntryValue" + n] = myTextField(listsAndArrays[tag + t.Key + "newEntryValue" + n], textBox);
                                GUILayout.EndHorizontal();
                            }
                            if (listsAndArrays[tag + t.Key + "newEntryValue" + (newCounter - 1)] != "")
                            {
                                listsAndArrays[tag + t.Key + "newEntryCounter"] = (newCounter + 1).ToString();
                            }
                        }
                        if (applyDict)
                        {   
                            if (!listsAndArrays.ContainsKey(tag + t.Key + "newEntryCounter"))
                                listsAndArrays[tag + t.Key + "newEntryCounter"] = "0";
                            int newCounter = Convert.ToInt32(listsAndArrays[tag + t.Key + "newEntryCounter"]);
                            for (int n = 0; n < newCounter; n++)
                            {
                                if (listsAndArrays[tag + t.Key + "newEntryValue" + n] != "")
                                {
                                    nList.Add(listsAndArrays[tag + t.Key + "newEntryValue" + n]);
                                }
                                listsAndArrays[tag + t.Key + "newEntryValue" + n] = "";
                            }
                            listsAndArrays[tag + t.Key + "newEntry"] = "false";
                            listsAndArrays[tag + t.Key + "newEntryCounter"] = "0";
                            switch (type)
                            {
                                case Serializer.SerializationTypes.ListI:
                                    List<int> tempI = new List<int>();
                                    foreach (string _s in nList)
                                    {
                                        tempI.Add(Operators.ToInt(_s));
                                    }
                                    EpicPrefs.SetList(listsAndArrays[tag + t.Key + "name"], tempI, Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                    break;
                                case Serializer.SerializationTypes.ListF:
                                    List<float> tempF = new List<float>();
                                    foreach (string _s in nList)
                                    {
                                        tempF.Add(Operators.ToFloat(_s));
                                    }
                                    EpicPrefs.SetList(listsAndArrays[tag + t.Key + "name"], tempF, Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                    break;
                                case Serializer.SerializationTypes.ListD:
                                    List<double> tempD = new List<double>();
                                    foreach (string _s in nList)
                                    {
                                        tempD.Add(Operators.ToDouble(_s));
                                    }
                                    EpicPrefs.SetList(listsAndArrays[tag + t.Key + "name"], tempD, Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                    break;
                                case Serializer.SerializationTypes.ListL:
                                    List<long> tempL = new List<long>();
                                    foreach (string _s in nList)
                                    {
                                        tempL.Add(Operators.ToLong(_s));
                                    }
                                    EpicPrefs.SetList(listsAndArrays[tag + t.Key + "name"], tempL, Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                    break;
                                case Serializer.SerializationTypes.ListB:
                                    List<bool> tempB = new List<bool>();
                                    foreach (string _s in nList)
                                    {
                                        tempB.Add(Operators.ToBool(_s));
                                    }
                                    EpicPrefs.SetList(listsAndArrays[tag + t.Key + "name"], tempB, Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                    break;
                                case Serializer.SerializationTypes.ListS:
                                    EpicPrefs.SetList(listsAndArrays[tag + t.Key + "name"], nList, Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                    break;
                                case Serializer.SerializationTypes.ArrayFloat:
                                    tempF = new List<float>();
                                    foreach (string _s in nList)
                                    {
                                        tempF.Add(Operators.ToFloat(_s));
                                    }
                                    float[] tempArrayF = new float[tempF.Count];
                                    for (int m = 0; m < tempArrayF.Length; m++)
                                        tempArrayF[m] = tempF[m];
                                    EpicPrefs.SetArray(listsAndArrays[tag + t.Key + "name"], tempArrayF, Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                    break;
                                case Serializer.SerializationTypes.ArrayDouble:
                                    tempD = new List<double>();
                                    foreach (string _s in nList)
                                    {
                                        tempD.Add(Operators.ToDouble(_s));
                                    }
                                    double[] tempArrayD = new double[tempD.Count];
                                    for (int m = 0; m < tempArrayD.Length; m++)
                                        tempArrayD[m] = tempD[m];
                                    EpicPrefs.SetArray(listsAndArrays[tag + t.Key + "name"], tempArrayD, Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                    break;
                                case Serializer.SerializationTypes.ArrayInt:
                                    tempI = new List<int>();
                                    foreach (string _s in nList)
                                    {
                                        tempI.Add(Operators.ToInt(_s));
                                    }
                                    int[] tempArrayI = new int[tempI.Count];
                                    for (int m = 0; m < tempArrayI.Length; m++)
                                        tempArrayI[m] = tempI[m];
                                    EpicPrefs.SetArray(listsAndArrays[tag + t.Key + "name"], tempArrayI, Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                    break;
                                case Serializer.SerializationTypes.ArrayString:
                                    string[] tempArrayS = new string[nList.Count];
                                    for (int m = 0; m < tempArrayS.Length; m++)
                                        tempArrayS[m] = nList[m];
                                    EpicPrefs.SetArray(listsAndArrays[tag + t.Key + "name"], tempArrayS, Convert.ToBoolean(listsAndArrays[tag + t.Key + "encrypt"]));
                                    break;
                            }                            
                            editDetails[counter] = false;
                            return;
                        }
                    }
                    if (editDetails[counter])
                    {
                        GUILayout.Space(10);
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(40);
                        if (listsAndArrays[tag + t.Key + "newEntry"] != "true")
                        {
                            if (GUILayout.Button(aB, GUIStyle.none, GUILayout.Width(20), GUILayout.Height(20)))
                            {
                                listsAndArrays[tag + t.Key + "newEntryCounter"] = "1";
                                listsAndArrays[tag + t.Key + "newEntry"] = "true";
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                    counter++;
                }
            }
            Separator();
#endregion
            #region Submission
            switch (type)
            {
                case Serializer.SerializationTypes.ListI:
                    showListI = showDetails;
                    showSubListI = showSubDetails;
                    editSubListI = editDetails;
                    break;
                case Serializer.SerializationTypes.ListS:
                    showListS = showDetails;
                    showSubListS = showSubDetails;
                    editSubListS = editDetails;
                    break;
                case Serializer.SerializationTypes.ListB:
                    showListB = showDetails;
                    showSubListB = showSubDetails;
                    editSubListB = editDetails;
                    break;
                case Serializer.SerializationTypes.ListF:
                    showListF = showDetails;
                    showSubListF = showSubDetails;
                    editSubListF = editDetails;
                    break;
                case Serializer.SerializationTypes.ListD:
                    showListD = showDetails;
                    showSubListD = showSubDetails;
                    editSubListD = editDetails;
                    break;
                case Serializer.SerializationTypes.ListL:
                    showListL = showDetails;
                    showSubListL = showSubDetails;
                    editSubListL = editDetails;
                    break;
                case Serializer.SerializationTypes.ArrayFloat:
                    showArrayF = showDetails;
                    showSubArrayF = showSubDetails;
                    editSubArrayF = editDetails;
                    break;
                case Serializer.SerializationTypes.ArrayDouble:
                    showArrayD = showDetails;
                    showSubArrayD = showSubDetails;
                    editSubArrayD = editDetails;
                    break;
                case Serializer.SerializationTypes.ArrayInt:
                    showArrayI = showDetails;
                    showSubArrayI = showSubDetails;
                    editSubArrayI = editDetails;
                    break;
                case Serializer.SerializationTypes.ArrayString:
                    showArrayS = showDetails;
                    showSubArrayS = showSubDetails;
                    editSubArrayS = editDetails;
                    break;
            }
            #endregion
        }
    }
    private string EncryptionToggle(string value)
    {
        EditorGUILayout.LabelField("AES Encryption : ", text, GUILayout.Width(100), GUILayout.Height(20));
        GUIContent toggleControlN;
        if (Convert.ToBoolean(value))
            toggleControlN = new GUIContent(cM);
        else
            toggleControlN = new GUIContent(uCM);
        if (GUILayout.Button(toggleControlN, GUIStyle.none, GUILayout.Width(15), GUILayout.Height(15)))
        {
            if (toggleControlN.image == checkedButton)
            {
                toggleControlN.image = uncheckedButton;
                return Convert.ToString(false);
            }
            else
            {
                toggleControlN.image = checkedButton;
                return Convert.ToString(true);
            }
        } else
        {
            return value;
        }
    }
    private void myLabelField(string label1)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label1, text, GUILayout.Width(250), GUILayout.Height(20));
        EditorGUILayout.EndHorizontal();
    }
    private void myLabelField(string label1,bool adaptive)
    {
        if (!adaptive)
            myLabelField(label1);
        else {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label1, text, GUILayout.Width(label1.Length*15), GUILayout.Height(20));
            EditorGUILayout.EndHorizontal();
        }
    }
    private void myLabelField(string label1, string label2)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label1, text, GUILayout.Width(250), GUILayout.Height(20));
        EditorGUILayout.LabelField(label2, text, GUILayout.Width(250), GUILayout.Height(20));
        EditorGUILayout.EndHorizontal();
    }
    private void myLabelField(string label1, string label2, bool adaptive)
    {
        if (!adaptive)
            myLabelField(label1, label2);
        else {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label1, text, GUILayout.Width(label1.Length*20), GUILayout.Height(20));
            EditorGUILayout.LabelField(label2, text, GUILayout.Width(label2.Length * 20), GUILayout.Height(20));
            EditorGUILayout.EndHorizontal();
        }
    }
    private void myLabelField(string label1,string label2,GUIStyle style)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label1, style,GUILayout.Width(250),GUILayout.Height(20));
        EditorGUILayout.LabelField(label2, style,GUILayout.Width(250),GUILayout.Height(20));
        EditorGUILayout.EndHorizontal();
    }
    private string myTextField(string passedText)
    {
        string newText = passedText;
        EditorGUILayout.BeginHorizontal();
        newText = EditorGUILayout.TextField(newText, textBox);
        EditorGUILayout.EndHorizontal();
        return newText;
    }
    private string myTextField(string passedText, GUIStyle style)
    {
        string newText = passedText;
        EditorGUILayout.BeginHorizontal();
        newText = EditorGUILayout.TextField(newText, style);
        EditorGUILayout.EndHorizontal();
        return newText;
    }
    private string myTextField(string label, string passedText)
    {
        string newText = passedText;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, text, GUILayout.Width(250), GUILayout.Height(20));
        newText = EditorGUILayout.TextField(newText, textBox);
        EditorGUILayout.EndHorizontal();
        return newText;
    }
    private string myTextField(string label, string passedText,bool adaptive)
    {
        if (!adaptive)
            return myTextField(label, passedText);
        else {
            string newText = passedText;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, text, GUILayout.Width(label.Length*20), GUILayout.Height(20));
            newText = EditorGUILayout.TextField(newText, textBox);
            EditorGUILayout.EndHorizontal();
            return newText;
        }
    }
    private string myTextField(string label, string passedText, GUIStyle style)
    {
        string newText = passedText;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, text, GUILayout.Width(250), GUILayout.Height(20));
        newText = EditorGUILayout.TextField(newText, style);
        EditorGUILayout.EndHorizontal();
        return newText;
    }
    
    void OnDestroy()
    {
        UnSubscribe();
    }
}
