//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Общая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIPlaceable2DEditor.cs
*		Редактор функционального компонента определяющего размещения элемента подсистемы Unity UI в 2D пространстве.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using UnityEditor;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Graphics2D;
using Lotus.Editor;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор функционального определяющего размещения элемента подсистемы Unity UI в 2D пространстве
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusUIPlaceable2D))]
public class LotusUIPlaceable2DEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ ДАННЫЕ ========================================
	protected static GUIContent mContentLeft = new GUIContent("Left");
	protected static GUIContent mContentRight = new GUIContent("Right");
	protected static GUIContent mContentTop = new GUIContent("Top");
	protected static GUIContent mContentBottom = new GUIContent("Bottom");
	protected static GUIContent mContentToFront = new GUIContent(XString.TriangleDown, "To front element");
	protected static GUIContent mContentToBack = new GUIContent(XString.TriangleUp, "To back element");
	protected static GUIContent mContentToLast = new GUIContent("L", "Set last sibling");
	protected static GUIContent mContentToFirst = new GUIContent("F", "Set first sibling");
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusUIPlaceable2D mUIPlaceable;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnEnable()
	{
		mUIPlaceable = this.target as LotusUIPlaceable2D;
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
			if (mUIPlaceable.mDisableInspector == false)
			{
				// Местоположение и размеры
				DrawPlaceableFoldout(mUIPlaceable);
			}
			else
			{
				XEditorInspector.DrawGroup("Controlled by another element", XEditorStyles.ColorRed, TextAnchor.MiddleCenter);
			}
		}
		if(EditorGUI.EndChangeCheck())
		{
			this.serializedObject.Save();
		}

		GUILayout.Space(2.0f);
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров размера и местоположения интерфейса размещаемого элемента
	/// </summary>
	/// <param name="placeable">Интерфейс размещаемого элемента</param>
	/// <param name="width">Максимальная ширина элемента</param>
	/// <param name="height">Максимальная высота элемента</param>
	//-----------------------------------------------------------------------------------------------------------------
	public static void DrawPlaceable(ILotusPlaceable2D placeable, Single width = 3000, Single height = 3000)
	{
		DrawPlaceablePlacement(placeable, width, height);
		DrawPlaceableDepth(placeable);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров размера и местоположения интерфейса размещаемого элемента
	/// </summary>
	/// <param name="placeable">Интерфейс размещаемого элемента</param>
	/// <param name="width">Максимальная ширина элемента</param>
	/// <param name="height">Максимальная высота элемента</param>
	//-----------------------------------------------------------------------------------------------------------------
	public static void DrawPlaceablePlacement(ILotusPlaceable2D placeable, Single width = 3000, Single height = 3000)
	{
		GUILayout.Space(2.0f);
		Rect rect_total_h = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, EditorStyles.numberField);
		Rect rect_left = rect_total_h;
		Rect rect_left_label = rect_total_h;
		Rect rect_left_field = rect_total_h;
		Rect rect_right = rect_total_h;
		Rect rect_right_label = rect_total_h;
		Rect rect_right_field = rect_total_h;

		XEditorInspector.ComputeRects(rect_total_h, out rect_left, 0.5f, out rect_right, 0.5f);
		rect_left_label = rect_left;
		rect_left_label.width = EditorGUIUtility.labelWidth;
		rect_left_field.x = rect_left_label.xMax;
		rect_left_field.width = rect_left.width - EditorGUIUtility.labelWidth - 2;

		EditorGUI.PrefixLabel(rect_left_label, mContentLeft);
		placeable.Left = EditorGUI.FloatField(rect_left_field, placeable.Left);

		rect_right_label = rect_right;
		rect_right_label.width = rect_right.width / 2 - 2.0f;
		rect_right_field.x = rect_right_label.xMax + 2;
		rect_right_field.width = rect_right.width / 2 - 2.0f;

		EditorGUI.PrefixLabel(rect_right_label, mContentRight);
		placeable.Right = EditorGUI.FloatField(rect_right_field, placeable.Right);


		GUILayout.Space(2.0f);
		Rect rect_total_v = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, EditorStyles.numberField);
		Rect rect_top = rect_total_v;
		Rect rect_top_label = rect_total_v;
		Rect rect_top_field = rect_total_v;
		Rect rect_bottom = rect_total_v;
		Rect rect_bottom_label = rect_total_v;
		Rect rect_bottom_field = rect_total_v;

		XEditorInspector.ComputeRects(rect_total_v, out rect_top, 0.5f, out rect_bottom, 0.5f);
		rect_top_label = rect_top;
		rect_top_label.width = EditorGUIUtility.labelWidth;
		rect_top_field.x = rect_top_label.xMax;
		rect_top_field.width = rect_top.width - EditorGUIUtility.labelWidth - 2;

		EditorGUI.PrefixLabel(rect_top_label, mContentTop);
		placeable.Top = EditorGUI.FloatField(rect_top_field, placeable.Top);

		rect_bottom_label = rect_bottom;
		rect_bottom_label.width = rect_bottom.width / 2 - 2.0f;
		rect_bottom_field.x = rect_bottom_label.xMax + 2;
		rect_bottom_field.width = rect_bottom.width / 2 - 2.0f;

		EditorGUI.PrefixLabel(rect_bottom_label, mContentBottom);
		placeable.Bottom = EditorGUI.FloatField(rect_bottom_field, placeable.Bottom);

		GUILayout.Space(2.0f);
		placeable.Width = XEditorInspector.PropertyFloatSlider("Width", placeable.Width, 0, width);

		GUILayout.Space(2.0f);
		placeable.Height = XEditorInspector.PropertyFloatSlider("Height", placeable.Height, 0, height);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров порядка расположения интерфейса размещаемого элемента
	/// </summary>
	/// <param name="placeable">Интерфейс размещаемого элемента</param>
	//-----------------------------------------------------------------------------------------------------------------
	public static void DrawPlaceableDepth(ILotusPlaceable2D placeable)
	{
		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
		{
			placeable.Depth = XEditorInspector.PropertyInt("Depth", placeable.Depth);

			if (GUILayout.Button(mContentToFront, EditorStyles.miniButtonLeft))
			{
				placeable.ToFrontSibling();
			}

			if (GUILayout.Button(mContentToBack, EditorStyles.miniButtonMid))
			{
				placeable.ToBackSibling();
			}

			if (GUILayout.Button(mContentToLast, EditorStyles.miniButtonMid))
			{
				placeable.SetAsLastSibling();
			}

			if (GUILayout.Button(mContentToFirst, EditorStyles.miniButtonRight))
			{
				placeable.SetAsFirstSibling();
			}
		}
		EditorGUILayout.EndHorizontal();
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров размера и местоположения компонента
	/// </summary>
	/// <param name="placeable">Компонент определяющий размещения элемента в пространстве 2D</param>
	/// <param name="panel_label">Надпись панели</param>
	//-----------------------------------------------------------------------------------------------------------------
	public static void DrawPlaceableFoldout(LotusUIPlaceable2D placeable, String panel_label = "Size and location element")
	{
		GUILayout.Space(4.0f);
		placeable.mExpandedSize = XEditorInspector.DrawGroupFoldout(panel_label, placeable.mExpandedSize);
		if (placeable.mExpandedSize)
		{
			DrawPlaceable(placeable);
		}
	}
	#endregion
}
//=====================================================================================================================