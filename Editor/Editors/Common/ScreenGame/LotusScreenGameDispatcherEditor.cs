//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Общий модуль 2D графики
// Подраздел: Подсистема игровых экранов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusScreenGameDispatcherEditor.cs
*		Редактор компонента диспетчера управления игровыми экранами.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
#if UNITY_EDITOR
//=====================================================================================================================
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Graphics2D;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор компонента диспетчера управления игровыми экранами
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusScreenGameDispatcher))]
public class LotusScreenGameDispatcherEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Добавления диспетчера игровых экранов на сцену
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XGraphics2DEditorSettings.MenuPathScreenGame + "Create ScreenGame Dispatcher", false, XGraphics2DEditorSettings.MenuOrderScreenGame + 5)]
	public static void CreateScreenGameDispatcher()
	{
		LotusScreenGameDispatcher.CreateScreenGameDispatcher();
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusScreenGameDispatcher mScreenGameDispatcher;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnEnable()
	{
		mScreenGameDispatcher = this.target as LotusScreenGameDispatcher;
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
			DrawDispatcher(mScreenGameDispatcher);
		}
		if (EditorGUI.EndChangeCheck())
		{
			this.serializedObject.Save();
		}
		GUILayout.Space(2.0f);
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование элементов диспетчера игровых экранов
	/// </summary>
	/// <param name="game_dispatcher">Диспетчер игровых экранов</param>
	//-----------------------------------------------------------------------------------------------------------------
	public static void DrawDispatcher(LotusScreenGameDispatcher game_dispatcher)
	{
		GUILayout.Space(4.0f);
		game_dispatcher.IsEnabledUIScreen = XEditorInspector.PropertyBoolean("EnabledUIScreen",
			game_dispatcher.IsEnabledUIScreen);

		GUILayout.Space(2.0f);
		game_dispatcher.IsStartFirstScreen = XEditorInspector.PropertyBoolean("IsStartFirstScreen",
			game_dispatcher.IsStartFirstScreen);

		GUILayout.Space(2.0f);
		game_dispatcher.GroupID = XEditorInspector.PropertyInt("GroupID", game_dispatcher.GroupID);

		GUILayout.Space(4.0f);
		game_dispatcher.mStartUIScreen = XEditorInspector.PropertyComponent("StartUIScreen", game_dispatcher.mStartUIScreen);
	}
	#endregion
}
//=====================================================================================================================
#endif
//=====================================================================================================================