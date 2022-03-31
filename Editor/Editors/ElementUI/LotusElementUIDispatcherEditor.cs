//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusElementUIDispatcherEditor.cs
*		Редактор центрального диспетчера модуля компонентов Unity UI.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using UnityEngine;
using UnityEditor;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Graphics2D;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор центрального диспетчера модуля компонентов Unity UI
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusElementUIDispatcher))]
public class LotusElementUIDispatcherEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Добавления центрального диспетчера модуля компонентов Unity UI в сцену
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XElementUIEditorSettings.MenuPath + "Create ElementUIDispatcher", false, XElementUIEditorSettings.MenuOrderLast + 1)]
	public static void CreateElementUIDispatcherEditor()
	{
		LotusElementUIDispatcher lud = LotusElementUIDispatcher.Instance;
		lud.gameObject.layer = XLayer.UI_ID;
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusElementUIDispatcher mDispatcher;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnEnable()
	{
		mDispatcher = this.target as LotusElementUIDispatcher;
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		GUILayout.Space(4.0f);
		EditorGUI.BeginChangeCheck();
		{
			XEditorInspector.DrawGroup("Singleton settings");
			{
				GUILayout.Space(2.0f);
				mDispatcher.IsMainInstance = XEditorInspector.PropertyBoolean(nameof(mDispatcher.IsMainInstance), mDispatcher.IsMainInstance);

				GUILayout.Space(2.0f);
				EditorGUI.BeginDisabledGroup(mDispatcher.IsMainInstance);
				{
					mDispatcher.DestroyMode = (TSingletonDestroyMode)XEditorInspector.PropertyEnum(nameof(mDispatcher.DestroyMode), mDispatcher.DestroyMode);
				}
				EditorGUI.EndDisabledGroup();

				GUILayout.Space(2.0f);
				mDispatcher.IsDontDestroy = XEditorInspector.PropertyBoolean(nameof(mDispatcher.IsDontDestroy), mDispatcher.IsDontDestroy);
			}

			XEditorInspector.DrawGroup("UI");
			{
				GUILayout.Space(2.0f);
				mDispatcher.mMainCanvas = XEditorInspector.PropertyComponent("MainCanvas", mDispatcher.mMainCanvas);

				GUILayout.Space(2.0f);
				mDispatcher.mCanvasScaler = XEditorInspector.PropertyComponent("CanvasScaler", mDispatcher.mCanvasScaler);
			}
		}
		if(EditorGUI.EndChangeCheck())
		{
			this.serializedObject.Save();
		}

		GUILayout.Space(2.0f);
	}
	#endregion
}
//=====================================================================================================================