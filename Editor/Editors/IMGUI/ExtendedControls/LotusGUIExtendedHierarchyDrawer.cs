//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Расширенные элементы управления
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIExtendedHierarchyDrawer.cs
*		Редакторы для рисования параметров элементов с иерархическим (древовидным) отображением информации.
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
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента ListView
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIListView))]
public class LotusListViewDrawer : LotusElementDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountListViewProperties = 2;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentIsBackSelected = new GUIContent("IsBackSelected");
	protected static GUIContent mContentBackSelectedColor = new GUIContent("SelectedColor");
	protected Int32 mSelectedColumnStyleSimpleIndex;
	protected Int32 mSelectedColumnStyleHeaderIndex;
	protected Int32 mSelectedColumnStyleResultIndex;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение высоты свойства
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	/// <param name="label">Надпись</param>
	/// <returns>Высота свойства элемента</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public override Single GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		if (property.isExpanded)
		{
			return (GetPropertyHeightListView(property) + XInspectorViewParams.SPACE);
		}
		else
		{
			return (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
		}
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование данных конкретного элемента
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnDrawParamemtrs(ref Rect position, SerializedProperty property)
	{
		// Отображаем базовые свойства
		OnDrawElementParamemtrs(ref position, property);

		// Отображаем свойства дерева
		OnDrawListViewParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров дерева
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawListViewParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIListView list_view = property.GetValue <CGUIListView>();
		if (list_view != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += XInspectorViewParams.CONTROL_HEIGHT_SPACE;
				GUI.Label(position, "ListView settings", EditorStyles.boldLabel);

				// 2)
				position.y += XInspectorViewParams.CONTROL_HEIGHT_SPACE;
				list_view.IsBackSelected = EditorGUI.Toggle(position, mContentIsBackSelected, list_view.IsBackSelected);

				// 3)
				EditorGUI.BeginDisabledGroup(!list_view.IsBackSelected);
				{
					position.y += XInspectorViewParams.CONTROL_HEIGHT_SPACE;
					list_view.BackSelectedColor = EditorGUI.ColorField(position, mContentBackSelectedColor, list_view.BackSelectedColor);
				}
				EditorGUI.EndDisabledGroup();

				// 4)
				position.y += XInspectorViewParams.CONTROL_HEIGHT_SPACE;
				GUI.Label(position, "Columns", EditorStyles.boldLabel);

				for (Int32 i = 0; i < list_view.Columns.Count; i++)
				{
					// 1)
					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					Rect rect_name = position;
					Rect rect_size = position;
					Rect rect_type = position;
					Rect rect_remove = position;

					XEditorInspector.ComputeRectsWithButtons(position, out rect_name, 0.5f, out rect_size, 0.2f, out rect_type, 0.3f, out rect_remove);
					list_view.Columns[i].Name = EditorGUI.TextField(rect_name, list_view.Columns[i].Name);
					list_view.Columns[i].Width = EditorGUI.FloatField(rect_size, list_view.Columns[i].Width);
					list_view.Columns[i].ColumnType = (TListColumnType)EditorGUI.EnumPopup(rect_type, list_view.Columns[i].ColumnType);
					if (GUI.Button(rect_remove, "X", EditorStyles.miniButtonRight))
					{
						list_view.RemoveColumn(i);
						break;
					}

					// 2)
					// Отображение ячеек
					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					Int32 index_simple = LotusGUIDispatcher.GetStyleIndex(list_view.Columns[i].StyleSimpleName);
					mSelectedColumnStyleSimpleIndex = EditorGUI.Popup(position, "Cell Style", index_simple, LotusGUIDispatcher.GetStyleNames());
					if (index_simple != mSelectedColumnStyleSimpleIndex)
					{
						list_view.Columns[i].StyleSimpleName = LotusGUIDispatcher.GetStyleNameFromIndex(mSelectedColumnStyleSimpleIndex);
					}

					// 3)
					// Отображение заголовка
					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					Int32 index_header = LotusGUIDispatcher.GetStyleIndex(list_view.Columns[i].StyleHeaderName);
					mSelectedColumnStyleHeaderIndex = EditorGUI.Popup(position, "Header Style", index_header, LotusGUIDispatcher.GetStyleNames());
					if (index_header != mSelectedColumnStyleHeaderIndex)
					{
						list_view.Columns[i].StyleHeaderName = LotusGUIDispatcher.GetStyleNameFromIndex(mSelectedColumnStyleHeaderIndex);
					}

					// 4)
					// Отображение подвала
					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					Int32 index_result = LotusGUIDispatcher.GetStyleIndex(list_view.Columns[i].StyleResultsName);
					mSelectedColumnStyleResultIndex = EditorGUI.Popup(position, "Footer Style", index_result, LotusGUIDispatcher.GetStyleNames());
					if (index_result != mSelectedColumnStyleResultIndex)
					{
						list_view.Columns[i].StyleResultsName = LotusGUIDispatcher.GetStyleNameFromIndex(mSelectedColumnStyleResultIndex);
					}
				}

				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				var rect_optimal = position;
				rect_optimal.height = XInspectorViewParams.BUTTON_MINI_HEIGHT;
				if (GUI.Button(rect_optimal, "Add Column"))
				{
					list_view.AddColumn("Item", 30);
				}
			}
			if (EditorGUI.EndChangeCheck())
			{
				property.Save();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение совокупной высоты свойств списка
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	/// <returns>Высота свойств</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetPropertyHeightListView(SerializedProperty property)
	{
		Single base_height = GetPropertyHeightElement();
		Single list_view_height = XInspectorViewParams.CONTROL_HEIGHT_SPACE;

		CGUIListView list_view = property.GetValue<CGUIListView>();
		if (list_view != null)
		{
			list_view_height = ((XInspectorViewParams.CONTROL_HEIGHT_SPACE * 4) * (list_view.Columns.Count)) +
				XInspectorViewParams.CONTROL_HEIGHT_SPACE * 4 + XInspectorViewParams.BUTTON_MINI_HEIGHT;
		}

		return (base_height + list_view_height);
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента TreeView
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUITreeView))]
public class LotusTreeViewDrawer : LotusElementDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountTreeViewProperties = 8;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentSpaceItem = new GUIContent("SpaceItem");
	protected static GUIContent mContentOffsetDepth = new GUIContent("OffsetDepth");
	protected static GUIContent mContentButtonSize = new GUIContent("ButtonSize");
	protected static GUIContent mContentIsChecked = new GUIContent("IsChecked");
	protected static GUIContent mContentStyleItem = new GUIContent("StyleItem");
	protected static GUIContent mContentIsBackSelected = new GUIContent("IsBackSelected");
	protected static GUIContent mContentBackSelectedColor = new GUIContent("SelectedColor");
	protected Int32 mSelectedItemStyleIndex;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение высоты свойства
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	/// <param name="label">Надпись</param>
	/// <returns>Высота свойства элемента</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public override Single GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		if (property.isExpanded)
		{
			return (base.GetPropertyHeight(property, label) + (XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountTreeViewProperties));
		}
		else
		{
			return (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
		}
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование данных конкретного элемента
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnDrawParamemtrs(ref Rect position, SerializedProperty property)
	{
		// Отображаем базовые свойства
		OnDrawElementParamemtrs(ref position, property);

		// Отображаем свойства дерева
		OnDrawTreeViewParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров дерева
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawTreeViewParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUITreeView tree_view = property.GetValue<CGUITreeView>();
		if (tree_view != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "TreeView settings", EditorStyles.boldLabel);

				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				tree_view.OffsetDepth = EditorGUI.FloatField(position, mContentOffsetDepth, tree_view.OffsetDepth);

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				tree_view.SpaceItem = EditorGUI.FloatField(position, mContentSpaceItem, tree_view.SpaceItem);

				// 4)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				tree_view.ButtonSize = EditorGUI.FloatField(position, mContentButtonSize, tree_view.ButtonSize);

				// 5)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				tree_view.IsChecked = EditorGUI.Toggle(position, mContentIsChecked, tree_view.IsChecked);

				// 6)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				Int32 index = LotusGUIDispatcher.GetStyleIndex(tree_view.StyleItemName);
				mSelectedItemStyleIndex = EditorGUI.Popup(position, mContentStyleItem.text, index, LotusGUIDispatcher.GetStyleNames());
				if (index != mSelectedItemStyleIndex)
				{
					tree_view.StyleItemName = LotusGUIDispatcher.GetStyleNameFromIndex(mSelectedItemStyleIndex);
				}

				// 7)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				tree_view.IsBackSelected = EditorGUI.Toggle(position, mContentIsBackSelected, tree_view.IsBackSelected);

				// 8)
				EditorGUI.BeginDisabledGroup(!tree_view.IsBackSelected);
				{
					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
					tree_view.BackSelectedColor = EditorGUI.ColorField(position, mContentBackSelectedColor, tree_view.BackSelectedColor);
				}
				EditorGUI.EndDisabledGroup();
			}
			if (EditorGUI.EndChangeCheck())
			{
				property.serializedObject.ApplyModifiedProperties();
			}
		}
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента TreeListView
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUITreeListView))]
public class LotusTreeListViewPropertyDrawer : LotusListViewDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountTreeListViewProperties = 5;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentSpaceItem = new GUIContent("SpaceItem");
	protected static GUIContent mContentOffsetDepth = new GUIContent("OffsetDepth");
	protected static GUIContent mContentButtonSize = new GUIContent("ButtonSize");
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение высоты свойства
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	/// <param name="label">Надпись</param>
	/// <returns>Высота свойства элемента</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public override Single GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		if (property.isExpanded)
		{
			return (GetPropertyHeightTreeListView(property) + XInspectorViewParams.SPACE);
		}
		else
		{
			return (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
		}
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование данных конкретного элемента
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnDrawParamemtrs(ref Rect position, SerializedProperty property)
	{
		// Отображаем базовые свойства
		OnDrawElementParamemtrs(ref position, property);

		// Отображаем свойства дерева
		OnDrawTreeListViewParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров дерева
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawTreeListViewParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUITreeListView tree_list_view = property.GetValue<CGUITreeListView>();
		if (tree_list_view != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += XInspectorViewParams.CONTROL_HEIGHT_SPACE;
				GUI.Label(position, "ListView settings", EditorStyles.boldLabel);

				// 2)
				position.y += XInspectorViewParams.CONTROL_HEIGHT_SPACE;
				tree_list_view.IsBackSelected = EditorGUI.Toggle(position, mContentIsBackSelected, tree_list_view.IsBackSelected);

				// 3)
				EditorGUI.BeginDisabledGroup(!tree_list_view.IsBackSelected);
				{
					position.y += XInspectorViewParams.CONTROL_HEIGHT_SPACE;
					tree_list_view.BackSelectedColor = EditorGUI.ColorField(position, mContentBackSelectedColor, tree_list_view.BackSelectedColor);
				}
				EditorGUI.EndDisabledGroup();

				// 4)
				position.y += XInspectorViewParams.CONTROL_HEIGHT_SPACE;
				GUI.Label(position, "Columns", EditorStyles.boldLabel);

				for (Int32 i = 0; i < tree_list_view.Columns.Count; i++)
				{
					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE) + XInspectorViewParams.SPACE * 2;
					Rect rect_name = position;
					Rect rect_size = position;
					Rect rect_type = position;
					Rect rect_remove = position;

					XEditorInspector.ComputeRectsWithButtons(position, out rect_name, 0.5f, out rect_size, 0.2f, out rect_type, 0.3f, out rect_remove);

					tree_list_view.Columns[i].Name = EditorGUI.TextField(rect_name, tree_list_view.Columns[i].Name);
					tree_list_view.Columns[i].Width = EditorGUI.FloatField(rect_size, tree_list_view.Columns[i].Width);
					tree_list_view.Columns[i].ColumnType = (TListColumnType)EditorGUI.EnumPopup(rect_type, tree_list_view.Columns[i].ColumnType);
					if (GUI.Button(rect_remove, "X", EditorStyles.miniButtonRight))
					{
						tree_list_view.RemoveColumn(i);
						break;
					}

					position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);

					Rect rect_style_simple = position;
					Rect rect_style_header = position;
					Rect rect_style_result = position;

					XEditorInspector.ComputeRects(position, out rect_style_simple, 0.33f, out rect_style_header, 0.33f, out rect_style_result, 0.33f);

					Int32 index_simple = LotusGUIDispatcher.GetStyleIndex(tree_list_view.Columns[i].StyleSimpleName);
					mSelectedColumnStyleSimpleIndex = EditorGUI.Popup(rect_style_simple, index_simple, LotusGUIDispatcher.GetStyleNames());
					if (index_simple != mSelectedColumnStyleSimpleIndex)
					{
						tree_list_view.Columns[i].StyleSimpleName = LotusGUIDispatcher.GetStyleNameFromIndex(mSelectedColumnStyleSimpleIndex);
					}

					Int32 index_header = LotusGUIDispatcher.GetStyleIndex(tree_list_view.Columns[i].StyleHeaderName);
					mSelectedColumnStyleHeaderIndex = EditorGUI.Popup(rect_style_header, index_header, LotusGUIDispatcher.GetStyleNames());
					if (index_header != mSelectedColumnStyleHeaderIndex)
					{
						tree_list_view.Columns[i].StyleHeaderName = LotusGUIDispatcher.GetStyleNameFromIndex(mSelectedColumnStyleHeaderIndex);
					}

					Int32 index_result = LotusGUIDispatcher.GetStyleIndex(tree_list_view.Columns[i].StyleResultsName);
					mSelectedColumnStyleResultIndex = EditorGUI.Popup(rect_style_result, index_result, LotusGUIDispatcher.GetStyleNames());
					if (index_result != mSelectedColumnStyleResultIndex)
					{
						tree_list_view.Columns[i].StyleResultsName = LotusGUIDispatcher.GetStyleNameFromIndex(mSelectedColumnStyleResultIndex);
					}
				}

				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				var rect_optimal = position;
				rect_optimal.height = XInspectorViewParams.BUTTON_MINI_HEIGHT;
				if (GUI.Button(rect_optimal, "Add Column"))
				{
					tree_list_view.AddColumn("Item", 30);
				}

				// 5)
				position.y += XInspectorViewParams.BUTTON_MINI_HEIGHT + XInspectorViewParams.SPACE;
				GUI.Label(position, "TreeView settings", EditorStyles.boldLabel);

				// 6)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				tree_list_view.OffsetDepth = EditorGUI.FloatField(position, mContentOffsetDepth, tree_list_view.OffsetDepth);

				// 7)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				tree_list_view.SpaceItem = EditorGUI.FloatField(position, mContentSpaceItem, tree_list_view.SpaceItem);

				// 8)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				tree_list_view.ButtonSize = EditorGUI.FloatField(position, mContentButtonSize, tree_list_view.ButtonSize);

			}
			if (EditorGUI.EndChangeCheck())
			{
				property.Save();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение совокупной высоты свойств древовидного списка
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	/// <returns>Высота свойств</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Single GetPropertyHeightTreeListView(SerializedProperty property)
	{
		Single base_height = GetPropertyHeightElement();
		Single list_view_height = XInspectorViewParams.CONTROL_HEIGHT_SPACE;

		CGUITreeListView tree_list_view = property.GetValue<CGUITreeListView>();
		if (tree_list_view != null)
		{
			list_view_height = ((XInspectorViewParams.CONTROL_HEIGHT_SPACE * 2) * (tree_list_view.Columns.Count)) +
				XInspectorViewParams.CONTROL_HEIGHT_SPACE * 4 + XInspectorViewParams.BUTTON_MINI_HEIGHT;

			list_view_height += XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountTreeListViewProperties;
		}

		return (base_height + list_view_height);
	}
	#endregion
}
//=====================================================================================================================