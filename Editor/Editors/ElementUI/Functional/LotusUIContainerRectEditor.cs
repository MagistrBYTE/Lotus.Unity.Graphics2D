//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль 2D графики
// Подраздел: Функциональные компоненты подсистемы Unity UI
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIContainerRectEditor.cs
*		Редактор функционального компонента обеспечивающего управление списком однотипных дочерних элементов.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Graphics2D;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор функционального компонента обеспечивающего управление списком однотипных дочерних элементов
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusUIContainerRect))]
public class LotusUIContainerRectEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ ДАННЫЕ ========================================
	protected static GUIContent mContentDublicate = new GUIContent("Dublicate selected");
	protected static GUIContent mContentDeleteAll = new GUIContent("Delete all");
	protected static GUIContent mContentPlaceAll = new GUIContent("Place all");
	protected static GUIContent mContentUpdateFirst = new GUIContent("Update from first element:");
	protected static GUIContent mContentConfigurate = new GUIContent("Configurate");
	protected static GUIContent mContentInteractive = new GUIContent("Interactive");
	protected static GUIContent mContentVisualStyle = new GUIContent("VisualStyle");
	protected static GUIContent mContentWidth = new GUIContent("Width");
	protected static GUIContent mContentHeight = new GUIContent("Height");
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected LotusUIContainerRect mContainer;
	protected CReorderableList mReorderableList;
	protected Boolean mIsNumber;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnEnable()
	{
		mContainer = this.target as LotusUIContainerRect;
		SerializedProperty property = this.serializedObject.FindProperty(nameof(LotusUIContainerRect.mContainer));
		mReorderableList = new CReorderableList(property);

		mReorderableList.OnAddItem = AddItem;
		mReorderableList.OnRemoveItem = RemoveItem;
		mReorderableList.OnCanRemoveItem = CanRemoveItem;
		mReorderableList.OnReorderItem = ReorderItem;
		//mReorderableList.OnHeightItem = GetHeightItem;
		mReorderableList.List.drawElementCallback = DrawItem;
		mReorderableList.OnDrawHeader = DrawHeader;
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
			// Основные параметры элемента
			GUILayout.Space(4.0f);
			mContainer.mExpandedContainer = XEditorInspector.DrawGroupFoldout("Settings container", mContainer.mExpandedContainer);
			if (mContainer.mExpandedContainer)
			{
				DrawParamsContainer();
				DrawItemsContainer();
				DrawOperationContainer();
			}
		}
		if (EditorGUI.EndChangeCheck())
		{
			mContainer.SaveInEditor();
		}

		GUILayout.Space(2.0f);
	}
	#endregion

	#region =============================================== МЕТОДЫ УПРАВЛЯЕМОГО СПИСКА ================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Добавление элемента в список
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void AddItem()
	{
		//mContainer.Container.AddNewItemFromPrefab();
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Дублирование выделенного элемента
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void DublicateItem()
	{
		//mContainer.Container.DublicateItem(mReorderableList.List.index);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Удаление элемента из списка
	/// </summary>
	/// <param name="index">Индекс удаляемого элемента</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void RemoveItem(Int32 index)
	{
		//mContainer.Container.DeleteItem(index);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Удаление всех элемента из списка
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void RemoveAll()
	{
		//mContainer.Container.DeleteItems();
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Возможность удаления элемента из списка
	/// </summary>
	/// <param name="index">Индекс удаляемого элемента</param>
	/// <returns>Статус удаления</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public Boolean CanRemoveItem(Int32 index)
	{
		//return (mContainer.Container.Count > 0);
		return (false);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Изменение позиции элемента из списка
	/// </summary>
	/// <param name="old_index">Старый индекс</param>
	/// <param name="new_index">Новый индекс</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void ReorderItem(Int32 old_index, Int32 new_index)
	{
		//mContainer.Container.UpdateIndexTransformSibling();
	}

	//---------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение высоты элемента по его индексу
	/// </summary>
	/// <param name="index">Индекс элемента</param>
	/// <returns>Высота элемента</returns>
	//---------------------------------------------------------------------------------------------------------
	public Single GetHeightItem(Int32 index)
	{
		return (XInspectorViewParams.CONTROL_HEIGHT_SPACE * 2);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование заголовка списка
	/// </summary>
	/// <param name="rect">Прямоугольник вывода</param>
	//---------------------------------------------------------------------------------------------------------
	public void DrawHeader(Rect rect)
	{
		// Рисуем
		//EditorGUI.LabelField(rect, String.Format("Items[Count={0}, MaxCount={1}]", mContainer.Container.Count,
		//	mContainer.Container.MaxCount));
	}

	//---------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование элемента списка
	/// </summary>
	/// <param name="rect">Прямоугольник вывода</param>
	/// <param name="index">Индекс</param>
	/// <param name="is_active">Статус активности</param>
	/// <param name="is_focused">Статус фокуса</param>
	//---------------------------------------------------------------------------------------------------------
	public void DrawItem(Rect rect, Int32 index, Boolean is_active, Boolean is_focused)
	{
		//Transform item = mContainer.Container[index];
		//if (item != null)
		//{
		//	Text text;
		//	Image image;
		//	text = mContainer.mContainer[index].GetComponentInChildren<Text>();
		//	image = mContainer.mContainer[index].GetComponentInChildren<Image>();
		//	DrawItemData(rect, index, text, image);
		//}
		//else
		//{
		//	GUI.Label(rect, "NULL", EditorStyles.boldLabel);
		//}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование содержимого элемента списка
	/// </summary>
	/// <param name="rect">Прямоугольник вывода</param>
	/// <param name="index">Индекс элемента</param>
	/// <param name="text">Текст</param>
	/// <param name="image">Изображение</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawItemData(Rect rect, Int32 index, Text text, Image image)
	{
		EditorGUI.BeginChangeCheck();

		if (text != null && image != null)
		{
			if (mIsNumber)
			{

			}
			else
			{
				text.text = EditorGUI.TextField(rect.GetColumnFromIndex(2, 0).Inflate(-2, 0), text.text);
				image.sprite = EditorGUI.ObjectField(rect.GetColumnFromIndex(2, 1).Inflate(-2, -1), image.sprite,
					typeof(Sprite), false) as Sprite;
			}
		}
		else
		{
			if (text != null)
			{
				if (mIsNumber)
				{

				}
				else
				{
					text.text = EditorGUI.TextField(rect.Inflate(-2, 0), text.text);
				}
			}
			if (image != null)
			{
				if (mIsNumber)
				{
				}
				else
				{
					image.sprite = EditorGUI.ObjectField(rect.Inflate(-2, -1), image.sprite, typeof(Sprite), false) as Sprite;
				}
			}
		}

		if (EditorGUI.EndChangeCheck())
		{
			if (text != null)
			{
				EditorUtility.SetDirty(text);
			}
			if (image != null)
			{
				EditorUtility.SetDirty(image);
			}
		}
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование основных параметров контейнера
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawParamsContainer()
	{
		//GUILayout.Space(2.0f);
		//XEditorInspector.PropertyLabel("Selected", mContainer.SelectedIndex.ToString());

		//GUILayout.Space(2.0f);
		//mContainer.IsEnabledUnselectingItem = XEditorInspector.PropertyBoolean("IsUnselectingItem", mContainer.IsEnabledUnselectingItem);

		////GUILayout.Space(2.0f);
		////mContainer.IsDesignElementExist = XEditorInspector.PropertyBoolean("IsDesignElementExist", mContainer.IsDesignElementExist);

		//GUILayout.Space(2.0f);
		//mContainer.IsMultiSelected = XEditorInspector.PropertyBoolean("IsMultiSelected", mContainer.IsMultiSelected);

		//if (mContainer.IsMultiSelected)
		//{
		//	EditorGUI.indentLevel++;
		//	GUILayout.Space(2.0f);
		//	mContainer.ModeSelectAddRemove = XEditorInspector.PropertyBoolean("SelectAddRemove", mContainer.ModeSelectAddRemove);

		//	GUILayout.Space(2.0f);
		//	mContainer.AlwaysSelectedItem = XEditorInspector.PropertyBoolean("AlwaysSelected", mContainer.AlwaysSelectedItem);
		//	EditorGUI.indentLevel--;
		//}

		//GUILayout.Space(4.0f);
		//mContainer.ItemPrefab = XEditorInspector.PropertyComponent("ItemPrefab", mContainer.ItemPrefab);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование элементов контейнера
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawItemsContainer()
	{
		GUILayout.Space(2.0f);
		serializedObject.Update();
		if (mReorderableList != null)
		{
			mReorderableList.DrawLayout();
		}
		serializedObject.ApplyModifiedProperties();
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование основных операций с элементами контейнера
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawOperationContainer()
	{
		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		{
			if (GUILayout.Button(mContentDublicate, EditorStyles.miniButtonLeft))
			{
				DublicateItem();
			}
			if (GUILayout.Button(mContentDeleteAll, EditorStyles.miniButtonMid))
			{
				RemoveAll();
			}
			if (GUILayout.Button(mContentPlaceAll, EditorStyles.miniButtonRight))
			{
				//mContainer.Container.UpdateIndexTransformSibling();
			}
		}
		EditorGUILayout.EndHorizontal();

		//GUILayout.Space(2.0f);
		//GUILayout.Label(mContentUpdateFirst, EditorStyles.label);
		//EditorGUILayout.BeginHorizontal();
		//{
		//	if (GUILayout.Button(mContentConfigurate, EditorStyles.miniButtonLeft))
		//	{
		//		mContainer.UpdateFromFirstConfigurate();
		//		XEditorUtil.SaveSerializedObject(serializedObject);
		//		LotusElementUIDispatcher.ForceUpdateEditor();
		//	}
		//	if (GUILayout.Button(mContentInteractive, EditorStyles.miniButtonMid))
		//	{
		//		mContainer.UpdateFromFirstInteractive();
		//		XEditorUtil.SaveSerializedObject(serializedObject);
		//		LotusElementUIDispatcher.ForceUpdateEditor();
		//	}
		//	if (GUILayout.Button(mContentVisualStyle, EditorStyles.miniButtonMid))
		//	{
		//		mContainer.UpdateFromFirstVisualStyle();
		//		XEditorUtil.SaveSerializedObject(serializedObject);
		//		LotusElementUIDispatcher.ForceUpdateEditor();
		//	}
		//	if (GUILayout.Button(mContentWidth, EditorStyles.miniButtonMid))
		//	{
		//		mContainer.UpdateFromFirstSize(0);
		//		XEditorUtil.SaveSerializedObject(serializedObject);
		//		LotusElementUIDispatcher.ForceUpdateEditor();
		//	}
		//	if (GUILayout.Button(mContentHeight, EditorStyles.miniButtonRight))
		//	{
		//		mContainer.UpdateFromFirstSize(1);
		//		XEditorUtil.SaveSerializedObject(serializedObject);
		//		LotusElementUIDispatcher.ForceUpdateEditor();
		//	}
		//}
		//EditorGUILayout.EndHorizontal();

		//if (mDrawAdditionalOperation != null) mDrawAdditionalOperation();
	}
	#endregion
}
//=====================================================================================================================