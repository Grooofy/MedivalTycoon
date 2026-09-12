using UnityEngine;

namespace MedivalTycoon
{
    [DisallowMultipleComponent]
    public class GameTestControls : MonoBehaviour
    {
        [SerializeField] private Timer _timer;

        public bool CanSubtractMinute => Application.isPlaying && Time.timeScale > 0f && _timer != null;

        public void SubtractMinute()
        {
            if (CanSubtractMinute)
                _timer.SubtractSeconds(60f);
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(GameTestControls))]
    public class GameTestControlsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var controls = (GameTestControls)target;
            using (new UnityEditor.EditorGUI.DisabledScope(!controls.CanSubtractMinute))
            {
                if (GUILayout.Button("Вычесть 1 минуту"))
                    controls.SubtractMinute();
            }
        }
    }
#endif
}
