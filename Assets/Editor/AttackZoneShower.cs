using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LowSpellBase))]
public class AttackZoneShower : Editor
{
	public override void OnInspectorGUI()
	{
		// �⺻ �ν����� �׸���
		base.OnInspectorGUI();

		LowSpellBase lowSpell = (LowSpellBase)target;

		// AttackZone �ð������� ǥ��
		if (lowSpell.AttackZone != null && lowSpell.AttackZone.Count > 0)
		{
			EditorGUILayout.LabelField("Attack Zones", EditorStyles.boldLabel);
			for (int lineX = 0; lineX < lowSpell.AttackZone.Count; lineX++)
			{
				var attackZoneElement = lowSpell.AttackZone[lineX];

				// �ٵ��� ���·� �������� ǥ��
				GUILayout.BeginHorizontal();
				for (int lineY = 0; lineY < attackZoneElement.Line.Count; lineY++)
				{
					// ��� �����ϰ� ���� ���� �Ҵ�
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
