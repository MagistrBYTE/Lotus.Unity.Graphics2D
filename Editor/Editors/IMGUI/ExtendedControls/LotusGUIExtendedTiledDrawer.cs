//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Расширенные элементы управления
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIExtendedTiledDrawer.cs
*		Редакторы для рисования параметров плиточных элементов интерфейса.
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
/// Редактор для рисования элемента TileContentItem
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUITileContentItem))]
public class LotusTileContentItemDrawer : LotusBaseElementDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountTileContentItemProperties = 2;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentStyle = new GUIContent("Style");
	protected Int32 mSelectedStyleIndex;
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
			return (base.GetPropertyHeight(property, label) + (XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountTileContentItemProperties));
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
		// Отображаем свойства тайла
		OnDrawTileContentItemParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров тайла
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawTileContentItemParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUITileContentItem tile_item = property.GetValue<CGUITileContentItem>();
		if (tile_item != null)
		{
			EditorGUI.BeginChangeCheck();

			// 1)
			position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
			GUI.Label(position, "TileItem settings", EditorStyles.boldLabel);

			// 2)
			position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
			Int32 index = LotusGUIDispatcher.GetStyleIndex(tile_item.StyleMainName);
			mSelectedStyleIndex = EditorGUI.Popup(position, mContentStyle.text, index, LotusGUIDispatcher.GetStyleNames());
			if (index != mSelectedStyleIndex)
			{
				tile_item.StyleMainName = LotusGUIDispatcher.GetStyleNameFromIndex(mSelectedStyleIndex);
			}

			// 3)
			//position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
			//GUI.Label(position, "TileItem settings", EditorStyles.boldLabel);

			if (EditorGUI.EndChangeCheck())
			{
				property.Save();
			}
		}
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента TileItem
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUITileItem))]
public class LotusTileItemPropertyDrawer : LotusBaseElementDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountTileItemProperties = 2;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentStyle = new GUIContent("Style");
	protected static GUIContent mContentChangeParam = new GUIContent("ChangeParam");
	protected static GUIContent mContentDuration = new GUIContent("Duration");
	protected static GUIContent mContentStorageIndex = new GUIContent("StorageIndex");
	protected static GUIContent mContentFrameStart = new GUIContent("FrameStart");
	protected static GUIContent mContentFrameTarget = new GUIContent("FrameTarget");
	protected static GUIContent mContentLocationStart = new GUIContent("LocationStart");
	protected static GUIContent mContentLocationTarget = new GUIContent("LocationTarget");
	protected static GUIContent mContentColorStart = new GUIContent("ColorStart");
	protected static GUIContent mContentColorTarget = new GUIContent("ColorTarget");
	protected Int32 mSelectedStyleIndex;
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
			return (base.GetPropertyHeight(property, label) + (XInspectorViewParams.CONTROL_HEIGHT_SPACE * CountTileItemProperties));
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
		// Отображаем свойства тайла
		OnDrawTileItemParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров тайла
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawTileItemParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUITileItem tile_item = property.GetValue<CGUITileItem>();
		if (tile_item != null)
		{
			EditorGUI.BeginChangeCheck();

			// 1)
			position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
			GUI.Label(position, "TileItem settings", EditorStyles.boldLabel);

			// 2)
			position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
			Int32 index = LotusGUIDispatcher.GetStyleIndex(tile_item.StyleMainName);
			mSelectedStyleIndex = EditorGUI.Popup(position, mContentStyle.text, index, LotusGUIDispatcher.GetStyleNames());
			if (index != mSelectedStyleIndex)
			{
				tile_item.StyleMainName = LotusGUIDispatcher.GetStyleNameFromIndex(mSelectedStyleIndex);
			}

			// 3)
			if (EditorGUI.EndChangeCheck())
			{
				property.Save();
			}
		}
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования элемента GridTile
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CGUIGridTile))]
public class LotusGridTilePropertyDrawer : LotusElementDrawer
{
	#region =============================================== КОНСТАНТНЫЕ ДАННЫЕ ========================================
	/// <summary>
	/// Количество собственных свойств
	/// </summary>
	public const Int32 CountTileManagerProperties = 6;
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static GUIContent mContentColumnCount = new GUIContent("ColumnCount");
	protected static GUIContent mContentRowCount = new GUIContent("RowCount");
	protected static GUIContent mContentSpaceTile = new GUIContent("SpaceTile");
	protected static GUIContent mContentTileWidth = new GUIContent("TileWidth");
	protected static GUIContent mContentTileHeight = new GUIContent("TileHeight");
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
			return (base.GetPropertyHeight(property, label) + (XInspectorViewParams.CONTROL_HEIGHT_SPACE) * CountTileManagerProperties + SpaceEndProperty);
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

		// Отображаем свойства менеджера тайлов
		OnDrawTileManagerParamemtrs(ref position, property);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров менеджера тайлов
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnDrawTileManagerParamemtrs(ref Rect position, SerializedProperty property)
	{
		CGUIGridTile tile_manager = property.GetValue<CGUIGridTile>();
		if (tile_manager != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				// 1)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				GUI.Label(position, "TileManager settings", EditorStyles.boldLabel);

				// 2)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				tile_manager.ColumnCount = EditorGUI.IntField(position, mContentColumnCount, tile_manager.ColumnCount);

				// 3)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				tile_manager.RowCount = EditorGUI.IntField(position, mContentRowCount, tile_manager.RowCount);

				// 4)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				tile_manager.SpaceTile = EditorGUI.Vector2Field(position, mContentSpaceTile, tile_manager.SpaceTile);

				// 5)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				EditorGUI.FloatField(position, mContentTileWidth, tile_manager.SizeTileX);

				// 6)
				position.y += (XInspectorViewParams.CONTROL_HEIGHT_SPACE);
				EditorGUI.FloatField(position, mContentTileHeight, tile_manager.SizeTileY);
			}
			if (EditorGUI.EndChangeCheck())
			{
				property.Save();
			}
		}
	}
	#endregion
}
//=====================================================================================================================