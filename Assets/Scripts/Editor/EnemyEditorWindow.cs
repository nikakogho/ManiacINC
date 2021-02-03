using UnityEngine;
using UnityEditor;

public class EnemyEditorWindow : EditorWindow {
    Enemy e;
    AnimatorOverrideController controller;
    GameObject prefab;
    Vector3 spawnPosition;
    Vector3 spawnRotation;
    enum DeathEffectType { None, Animation }
    DeathEffectType deathEffectType = DeathEffectType.None;
    string path;

    [MenuItem("Window/Enemy Editor")]
    public static void ShowWindow()
    {
        GetWindow<EnemyEditorWindow>("Enemy Editor Window");
    }

    void OnGUI()
    {
        prefab = (GameObject)EditorGUILayout.ObjectField("Enemy Prefab", prefab, typeof(GameObject), false);
        path = EditorGUILayout.TextField("Death Effect Folder", path);

        if (prefab == null) return;

        spawnPosition = EditorGUILayout.Vector3Field("Position", spawnPosition);
        spawnRotation = EditorGUILayout.Vector3Field("Rotation", spawnRotation);
        deathEffectType = (DeathEffectType)EditorGUILayout.EnumPopup("Death Effect Type", deathEffectType);

        switch (deathEffectType)
        {
            case DeathEffectType.None:
                break;
            case DeathEffectType.Animation:
                controller = (AnimatorOverrideController)EditorGUILayout.ObjectField("Death Animator", controller, typeof(AnimatorOverrideController), false);

                if (GUILayout.Button("Apply Death Effect!"))
                {
                    GameObject effect = Instantiate(prefab, spawnPosition, Quaternion.Euler(spawnRotation));
                    effect.name = prefab.name + " Death Effect";
                    effect.GetComponent<Animator>().runtimeAnimatorController = controller;
                    effect.layer = 0;

                    DestroyImmediate(effect.GetComponent<Enemy>());
                    DestroyImmediate(effect.GetComponent<Rigidbody>());
                    DestroyImmediate(effect.GetComponent<Collider>());
                    DestroyImmediate(effect.GetComponent<AudioSource>());

                    PrefabUtility.CreatePrefab(path + "/" + effect.name + ".prefab", effect);
                    DestroyImmediate(effect);
                }
                break;
        }
    }
}
