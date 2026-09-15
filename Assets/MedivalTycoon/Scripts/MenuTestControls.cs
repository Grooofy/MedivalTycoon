using UI.MainMenu;
using UnityEngine;

namespace MedivalTycoon
{
    [DisallowMultipleComponent]
    public class MenuTestControls : MonoBehaviour
    {
        [SerializeField] private CharacterUpgradesPanel _upgradesPanel;

        public bool CanAddCoins => Application.isPlaying && LevelRewards.Balance <= int.MaxValue - 1000;

        public void AddThousandCoins()
        {
            if (!CanAddCoins) return;

            LevelRewards.AddTestCoins();
            if (_upgradesPanel != null && _upgradesPanel.isActiveAndEnabled)
                _upgradesPanel.Refresh();
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(MenuTestControls))]
    public class MenuTestControlsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var controls = (MenuTestControls)target;
            using (new UnityEditor.EditorGUI.DisabledScope(!controls.CanAddCoins))
            {
                if (GUILayout.Button("Добавить 1000 монет"))
                    controls.AddThousandCoins();
            }
        }
    }
#endif
}
