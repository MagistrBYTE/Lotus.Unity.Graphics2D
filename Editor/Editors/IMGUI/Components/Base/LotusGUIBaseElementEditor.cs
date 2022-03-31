//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Компоненты IMGUI Unity
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIBaseElementEditor.cs
*		Редактор компонента представляющего базовый элемент интерфейса модуля IMGUI Unity.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Graphics2D;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор компонента представляющего базовый элемент интерфейса подсистемы модуля Unity
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusGUIBaseElement))]
public class LotusGUIBaseElementEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Создание базового элемента
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XGUIEditorSettings.MenuPathComponent + "Create BaseElement", false, XGUIEditorSettings.MenuOrderComponent + 1)]
	public static void CreateElement()
	{
		LotusGUIElement.CreateElement();
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusGUIBaseElement mElement;
	protected Int32 mSelectedStyleIndex;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnEnable()
	{
		mElement = this.target as LotusGUIBaseElement;
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		// Рисование свойств базового элемента
		DrawBaseElementParamemtrs(mElement);

		GUILayout.Space(2.0f);
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров компонента базового элемента
	/// </summary>
	/// <param name="element">Компонент базового элемента</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawBaseElementParamemtrs(LotusGUIBaseElement element)
	{
		GUILayout.Space(4.0f);
		element.mExpandedElement = XEditorInspector.DrawGroupFoldout("Base settings", element.mExpandedElement);
		if (element.mExpandedElement)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				GUILayout.Space(2.0f);
				element.IsVisible = XEditorInspector.PropertyBoolean("IsVisible", element.IsVisible);

				// 10)
				GUILayout.Space(2.0f);
				element.Depth = XEditorInspector.PropertyInt("Depth", element.Depth);

				// 11)
				GUILayout.Space(2.0f);
				element.Opacity = XEditorInspector.PropertyFloatSlider("Opacity", element.Opacity, 0, 1);

				// 12)
				GUILayout.Space(2.0f);
				element.BackgroundColor = XEditorInspector.PropertyColor("BackgroudColor", element.BackgroundColor);

				// 13)
				GUILayout.Space(2.0f);
				Int32 index = LotusGUIDispatcher.GetStyleIndex(element.StyleMainName);
				mSelectedStyleIndex = XEditorInspector.SelectorIndex("Style", index, LotusGUIDispatcher.GetStyleNames());
				if (index != mSelectedStyleIndex)
				{
					element.StyleMainName = LotusGUIDispatcher.GetStyleNameFromIndex(mSelectedStyleIndex);
				}
			}
			if (EditorGUI.EndChangeCheck())
			{
				this.serializedObject.Save();
			}
		}
	}
	#endregion
}
//=====================================================================================================================