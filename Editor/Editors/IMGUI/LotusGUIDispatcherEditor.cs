//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIDispatcherEditor.cs
*		Редактор центрального диспетчера элементов модуля IMGUI Unity
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
/// Редактор центрального диспетчера элементов модуля IMGUI Unity
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusGUIDispatcher))]
public class LotusGUIDispatcherEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Добавления центрального диспетчера элементов модуля IMGUI Unity в сцену
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XGUIEditorSettings.MenuPath + "Create GUIDispatcher", false, XGUIEditorSettings.MenuOrderLast + 2)]
	public static void CreateGUIDispatcher()
	{
		LotusGUIDispatcher lud = LotusGUIDispatcher.Instance;
		lud.gameObject.layer = XLayer.UI_ID;
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusGUIDispatcher mDispatcher;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnEnable()
	{
		mDispatcher = this.target as LotusGUIDispatcher;
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();
		{
			GUILayout.Space(4.0f);
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

			XEditorInspector.DrawGroup("Singleton settings");
			{
				GUILayout.Space(2.0f);
				mDispatcher.mCurrentSkin = XEditorInspector.PropertyResource("CurrentSkin", mDispatcher.mCurrentSkin);

				GUILayout.Space(2.0f);
				mDispatcher.DesignWidth = XEditorInspector.PropertyInt("Design Width", mDispatcher.DesignWidth);

				GUILayout.Space(2.0f);
				mDispatcher.DesignHeight = XEditorInspector.PropertyInt("Design Height", mDispatcher.DesignHeight);
			}
		}
		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.Save();
		}

		GUILayout.Space(2.0f);
	}
	#endregion
}
//=====================================================================================================================