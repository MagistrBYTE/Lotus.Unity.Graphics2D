//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Компоненты IMGUI Unity
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIElementEditor.cs
*		Редактор компонента представляющего основной элемент интерфейса модуля IMGUI Unity.
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
/// Редактор компонента представляющего основной элемент интерфейса модуля IMGUI Unity
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusGUIElement))]
public class LotusGUIElementEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Создание базового элемента
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XGUIEditorSettings.MenuPathComponent + "Create Element", false, XGUIEditorSettings.MenuOrderComponent + 2)]
	public static void CreateElement()
	{
		LotusGUIElement.CreateElement();
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusGUIElement mElement;
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
		mElement = this.target as LotusGUIElement;
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		// Рисование свойств элемента
		DrawElementParamemtrs(mElement);

		GUILayout.Space(2.0f);
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров компонента основного элемента
	/// </summary>
	/// <param name="element">Компонент основного элемента</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawElementParamemtrs(LotusGUIElement element)
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

				// 2)
				GUILayout.Space(2.0f);
				element.IsEnabled = XEditorInspector.PropertyBoolean("IsEnabled", element.IsEnabled);

				// 3)
				GUILayout.Space(2.0f);
				EditorGUILayout.BeginHorizontal();
				element.Left = XEditorInspector.PropertyFloat("Left", element.Left);
				element.Right = XEditorInspector.PropertyFloat("Right", element.Right);
				EditorGUILayout.EndHorizontal();

				// 4)
				GUILayout.Space(2.0f);
				EditorGUILayout.BeginHorizontal();
				element.Top = XEditorInspector.PropertyFloat("Top", element.Top);
				element.Bottom = XEditorInspector.PropertyFloat("Bottom", element.Bottom);
				EditorGUILayout.EndHorizontal();

				// 5)
				GUILayout.Space(2.0f);
				element.AspectMode = (TAspectMode)XEditorInspector.PropertyEnum("AspectMode", element.AspectMode);

				// 6)
				EditorGUI.BeginDisabledGroup(element.HorizontalAlignment == THorizontalAlignment.Stretch);
				{
					GUILayout.Space(2.0f);
					element.Width = XEditorInspector.PropertyFloatSlider("Width", element.Width, 1, 2000);
				}
				EditorGUI.EndDisabledGroup();

				// 7)
				EditorGUI.BeginDisabledGroup(element.VerticalAlignment == TVerticalAlignment.Stretch);
				{
					GUILayout.Space(2.0f);
					element.Height = XEditorInspector.PropertyFloatSlider("Height", element.Height, 1, 2000);
				}
				EditorGUI.EndDisabledGroup();

				// 8)
				GUILayout.Space(2.0f);
				element.HorizontalAlignment = (THorizontalAlignment)XEditorInspector.PropertyEnum("HorizontalAlign", element.HorizontalAlignment);

				// 9)
				GUILayout.Space(2.0f);
				element.VerticalAlignment = (TVerticalAlignment)XEditorInspector.PropertyEnum("VerticalAlign", element.VerticalAlignment);

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