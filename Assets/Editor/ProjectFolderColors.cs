#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ColoredProjectView
{
    const string MenuPath = "NXLab/ColoredProjectView";
    const string PrefKey = "NXLab_ColoredProjectView_Enabled";

    [MenuItem(MenuPath)]
    static void ToggleEnabled()
    {
        bool isEnabled = !Menu.GetChecked(MenuPath);
        Menu.SetChecked(MenuPath, isEnabled);
        EditorPrefs.SetBool(PrefKey, isEnabled);
        EditorApplication.RepaintProjectWindow();
    }

    [InitializeOnLoadMethod]
    static void Initialize()
    {
        SetEvent();
        bool isEnabled = EditorPrefs.GetBool(PrefKey, true);  // �����l��true�ɐݒ�
        Menu.SetChecked(MenuPath, isEnabled);
    }

    static void SetEvent()
    {
        EditorApplication.projectWindowItemOnGUI += OnGUI;
    }

    static void OnGUI(string guid, Rect selectionRect)
    {
        if (!Menu.GetChecked(MenuPath))
        {
            return;
        }

        string assetPath = AssetDatabase.GUIDToAssetPath(guid);
        int depth = assetPath.Split('/').Length - 1;
        Color colorToUse = GetColorByDepth(depth);

        // �A���t�@�l��ݒ肵�ĐF��K�p
        colorToUse.a = GetAlphaByDepth(depth);
        EditorGUI.DrawRect(selectionRect, colorToUse);
    }

    private static Color GetColorByDepth(int depth)
    {
        // �w�肳�ꂽ�K�w�Ɋ�Â��ĐF��Ԃ�
        switch (depth)
        {
            case 1: return Color.red;
            case 2: return Color.green;
            case 3: return Color.blue;
            case 4: return Color.yellow;
            case 5: return Color.magenta;
            case 6: return Color.cyan;
            case 7: return Color.grey;
            case 8: return new Color(1f, 0.5f, 0f); // �I�����W
            case 9: return new Color(0.5f, 0f, 0.5f); // �p�[�v��
            case 10: return new Color(0f, 0.5f, 0f); // �_�[�N�O���[��
            case 11: return new Color(0.5f, 0.5f, 0f); // �I���[�u
            default: return new Color(0.5f, 0.5f, 0.5f); // �f�t�H���g�̐F
        }
    }

    private static float GetAlphaByDepth(int depth)
    {
        // �K�w�̐[���ɉ����ăA���t�@�l���Ԃ���
        // ��F�Ő[���ŃA���t�@�l���ł��Ⴍ����
        const float maxDepth = 10f;  // ���̐[���ȍ~�̓A���t�@�l���ŏ��ɂȂ�
        const float minAlpha = 0.01f; // �A���t�@�l�̍ŏ��l
        const float maxAlpha = 0.05f; // �A���t�@�l�̍ő�l

        // �K�w�̐[���Ɋ�Â��ăA���t�@�l���v�Z����
        float alpha = maxAlpha - ((depth - 1) / maxDepth) * (maxAlpha - minAlpha);
        return Mathf.Clamp(alpha, minAlpha, maxAlpha);
    }
}
#endif

