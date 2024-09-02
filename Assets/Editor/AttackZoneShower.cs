using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LowSpellBase))]
public class AttackZoneShower : Editor
{
	public override void OnInspectorGUI()
	{
		// 기본 인스펙터 그리기
		base.OnInspectorGUI();

		LowSpellBase lowSpell = (LowSpellBase)target;

		// AttackZone 시각적으로 표시
		if (lowSpell.AttackZone != null && lowSpell.AttackZone.Count > 0)
		{
			EditorGUILayout.LabelField("Attack Zones", EditorStyles.boldLabel);
			for (int lineX = 0; lineX < lowSpell.AttackZone.Count; lineX++)
			{
				var attackZoneElement = lowSpell.AttackZone[lineX];

				// 바둑판 형태로 수평으로 표시
				GUILayout.BeginHorizontal();
				for (int lineY = 0; lineY < attackZoneElement.Line.Count; lineY++)
				{
					// 토글 생성하고 값을 직접 할당
					attackZoneElement.Line[lineY] = EditorGUILayout.Toggle(attackZoneElement.Line[lineY], GUILayout.Width(15));
				}
				GUILayout.EndHorizontal();
			}
		}
		else
		{
			EditorGUILayout.LabelField("No Attack Zones configured.");
		}
	}
}
