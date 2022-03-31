//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Функциональные компоненты подсистемы Unity UI
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIScrollRectEditor.cs
*		Редактор расширенного компонента ScrollRect определяющего фиксированное перемещение контента в ограниченной области.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Graphics2D;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор расширенного компонента ScrollRect определяющего фиксированное перемещение контента в ограниченной области
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusUIScrollRect), true)]
public class LotusUIScrollRectEditor : ScrollRectEditor
{
	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Создание компонента ScrollRect с параметрами по умолчанию
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XElementUIEditorSettings.MenuPathFunc + "Create ScrollRect", false, XElementUIEditorSettings.MenuOrderFunc + 3)]
	public static void CreateScrollRect()
	{
		LotusUIScrollRect scroll_rect = LotusUIScrollRect.Create(300, 300, Selection.activeTransform as RectTransform);
		Undo.RegisterCreatedObjectUndo(scroll_rect.gameObject, "ScrollRect");
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusUIScrollRect mUIScrollRect;
	private static Int32 mCountContentWidth = 1;
	private static Int32 mCountContentHeight = 1;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	protected override void OnEnable()
	{
		base.OnEnable();
		mUIScrollRect = this.target as LotusUIScrollRect;
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Отключение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	protected override void OnDisable()
	{
		base.OnDisable();
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		GUILayout.Space(4.0f);
		mUIScrollRect.mExpandedSize = XEditorInspector.DrawGroupFoldout("Size and location", mUIScrollRect.mExpandedSize);
		if (mUIScrollRect.mExpandedSize)
		{
			LotusUIPlaceable2DEditor.DrawPlaceable(mUIScrollRect);
		}

		GUILayout.Space(4.0f);
		mUIScrollRect.mExpandedDefault = XEditorInspector.DrawGroupFoldout("Main setings", mUIScrollRect.mExpandedDefault);
		if (mUIScrollRect.mExpandedDefault)
		{
			base.OnInspectorGUI();
		}

		DrawScrollRect(mUIScrollRect);

		GUILayout.Space(2.0f);
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров компонента
	/// </summary>
	/// <param name="scroll_rect">Компонент</param>
	//-----------------------------------------------------------------------------------------------------------------
	public static void DrawScrollRect(LotusUIScrollRect scroll_rect)
	{
		GUILayout.Space(4.0f);
		scroll_rect.mExpandedView = XEditorInspector.DrawGroupFoldout("Settings view content", scroll_rect.mExpandedView);
		if (scroll_rect.mExpandedView)
		{
			if (scroll_rect.UIRectContent != null)
			{
				GUILayout.Space(4.0f);
				scroll_rect.ScrollDirection = (TScrollDirection)XEditorInspector.PropertyEnum("ScrollDirection", scroll_rect.ScrollDirection);

				GUILayout.Space(4.0f);
				scroll_rect.ContentOffsetX = XEditorInspector.PropertyFloat("ContentOffsetX", scroll_rect.ContentOffsetX);

				GUILayout.Space(2.0f);
				scroll_rect.ContentOffsetY = XEditorInspector.PropertyFloat("ContentOffsetY", scroll_rect.ContentOffsetY);

				GUILayout.Space(2.0f);
				scroll_rect.ContentWidth = XEditorInspector.PropertyFloat("ContentWidth", scroll_rect.ContentWidth);

				GUILayout.Space(2.0f);
				scroll_rect.ContentHeight = XEditorInspector.PropertyFloat("ContentHeight", scroll_rect.ContentHeight);

				GUILayout.Space(4.0f);
				XEditorInspector.DrawGroup("Fixed offset");

				GUILayout.Space(4.0f);
				scroll_rect.VelocityOffset = XEditorInspector.PropertyFloatSlider("VelocityOffset", scroll_rect.VelocityOffset, 0.1f, 10f);

				GUILayout.Space(4.0f);
				scroll_rect.UseFixedOffsetX = XEditorInspector.PropertyBoolean("UseFixedOffsetX", scroll_rect.UseFixedOffsetX);
				if (scroll_rect.UseFixedOffsetX)
				{
					GUILayout.Space(2.0f);
					scroll_rect.CountFixedOffsetX = XEditorInspector.PropertyIntSlider("CountFixedOffsetX", scroll_rect.CountFixedOffsetX, 1, 32);

					GUILayout.Space(2.0f);
					Single content_width = scroll_rect.ContentWidth / scroll_rect.CountFixedOffsetX;
					XEditorInspector.PropertyLabel("Width page/content", scroll_rect.Width.ToString("F2") + "/" + content_width.ToString("F2"));

					GUILayout.Space(2.0f);
					EditorGUILayout.BeginHorizontal();
					if (GUILayout.Button("Width page on: "))
					{
						if (scroll_rect.mOnViewWindowSizeChanged == null)
						{
							scroll_rect.OnResizeWindowView(mCountContentWidth * content_width, scroll_rect.Height);
						}
						else
						{
							scroll_rect.mOnViewWindowSizeChanged(mCountContentWidth * content_width, scroll_rect.Height);
						}
					}
					mCountContentWidth = EditorGUILayout.IntField(mCountContentWidth);
					EditorGUILayout.EndHorizontal();


					GUILayout.Space(2.0f);
					scroll_rect.DeltaFixedOffsetX = XEditorInspector.PropertyFloat("DeltaFixedOffsetX", scroll_rect.DeltaFixedOffsetX);
				}

				GUILayout.Space(10.0f);
				scroll_rect.UseFixedOffsetY = XEditorInspector.PropertyBoolean("UseFixedOffsetY", scroll_rect.UseFixedOffsetY);

				if (scroll_rect.UseFixedOffsetY)
				{
					GUILayout.Space(2.0f);
					scroll_rect.CountFixedOffsetY = XEditorInspector.PropertyIntSlider("CountFixedOffsetY", scroll_rect.CountFixedOffsetY, 1, 32);

					GUILayout.Space(2.0f);
					Single content_height = scroll_rect.ContentHeight / scroll_rect.CountFixedOffsetY;
					XEditorInspector.PropertyLabel("Height page/content", content_height.ToString("F2") + "/" + scroll_rect.Height.ToString("F2"));

					GUILayout.Space(2.0f);
					EditorGUILayout.BeginHorizontal();
					if (GUILayout.Button("Height page on: "))
					{
						if (scroll_rect.mOnViewWindowSizeChanged == null)
						{
							scroll_rect.OnResizeWindowView(scroll_rect.Width, mCountContentHeight * content_height);
						}
						else
						{
							scroll_rect.mOnViewWindowSizeChanged(scroll_rect.Width, mCountContentHeight * content_height);
						}
					}
					mCountContentHeight = EditorGUILayout.IntField(mCountContentHeight);
					EditorGUILayout.EndHorizontal();

					GUILayout.Space(2.0f);
					scroll_rect.DeltaFixedOffsetY = XEditorInspector.PropertyFloat("DeltaFixedOffsetY", scroll_rect.DeltaFixedOffsetY);
				}
			}
			else
			{
				XEditorInspector.DrawGroup("Set the content area");
			}
		}
	}
	#endregion
}
//=====================================================================================================================