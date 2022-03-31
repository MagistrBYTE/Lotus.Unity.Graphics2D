//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль работы со спрайтами
// Подраздел: Расширенные элементы управления
// Группа: Тайловые(плиточные) элементы
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusSpriteGridTileEditor.cs
*		Редактор компонента элемента GridTile.
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
/// Редактор компонента элемента GridTile
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusSpriteGridTile))]
public class LotusSpriteGridTileEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Создание элемента GridTile с параметрами по умолчанию
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XSpriteEditorSettings.MenuPathExtended + "Tiled/Create GridTile", false, XSpriteEditorSettings.MenuOrderExtended + 6)]
	public static void CreateGridTile()
	{
		LotusSpriteGridTile tile_grid = LotusSpriteGridTile.CreateGridTile(0, 0, 300, 600, Selection.activeTransform);
		Undo.RegisterCreatedObjectUndo(tile_grid.gameObject, "GridTile");
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusSpriteGridTile mGridTile;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnEnable()
	{
		mGridTile = this.target as LotusSpriteGridTile;
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		// Параметры базового элемента (местоположение и интерактивность)
		mGridTile.mExpandedSize = XEditorInspector.DrawGroupFoldout("Parameters and size", mGridTile.mExpandedSize);
		if (mGridTile.mExpandedSize)
		{
			LotusSpriteElementEditor.DrawElementParam(mGridTile);
		}

		EditorGUI.BeginChangeCheck();
		{
			// Основные параметры элемента
			GUILayout.Space(4.0f);
			mGridTile.mExpandedParam = XEditorInspector.DrawGroupFoldout("Setting grid tile", mGridTile.mExpandedParam);
			if (mGridTile.mExpandedParam)
			{
				DrawGridTileParams(mGridTile);
			}
		}
		if (EditorGUI.EndChangeCheck())
		{
			mGridTile.SaveInEditor();
		}
		
		GUILayout.Space(2.0f);
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование основных параметров сетки тайлов
	/// </summary>
	/// <param name="grid_tile">Элемент</param>
	//-----------------------------------------------------------------------------------------------------------------
	public static void DrawGridTileParams(LotusSpriteGridTileBase grid_tile)
	{
		GUILayout.Space(2.0f);
		grid_tile.RowCount = XEditorInspector.PropertyInt("RowCount", grid_tile.RowCount);

		GUILayout.Space(2.0f);
		grid_tile.ColumnCount = XEditorInspector.PropertyInt("ColumnCount", grid_tile.ColumnCount);

		GUILayout.Space(2.0f);
		grid_tile.SpaceTileX = XEditorInspector.PropertyFloat("SpaceTileX", grid_tile.SpaceTileX);

		GUILayout.Space(2.0f);
		grid_tile.SpaceTileY = XEditorInspector.PropertyFloat("SpaceTileY", grid_tile.SpaceTileY);

		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		{
			EditorGUILayout.PrefixLabel("Tile width: " + grid_tile.SizeTileX.ToString("F2"), EditorStyles.label);

			if (GUILayout.Button("Update", EditorStyles.miniButton))
			{
				grid_tile.ComputeSizeTile();
			}
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		{
			EditorGUILayout.PrefixLabel("Tile height: " + grid_tile.SizeTileY.ToString("F2"), EditorStyles.label);

			if (GUILayout.Button("Update", EditorStyles.miniButton))
			{
				grid_tile.ComputeSizeTile();
			}
		}
		EditorGUILayout.EndHorizontal();

		Single dw = grid_tile.GetOptimalTileWidth();
		Single dh = grid_tile.GetOptimalTileHeight();

		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		{
			EditorGUILayout.PrefixLabel("Opt tile W:" + dw.ToString("F1"), EditorStyles.label);

			if (GUILayout.Button("Resize width element", EditorStyles.miniButton))
			{
				grid_tile.SetWidthGridFromOptimalTileWidth(dw);
			}
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		{
			EditorGUILayout.PrefixLabel("Opt tile H:" + dh.ToString("F1"), EditorStyles.label);

			if (GUILayout.Button("Resize height element", EditorStyles.miniButton))
			{
				grid_tile.SetHeightGridFromOptimalTileHeight(dh);
			}
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(2.0f);
		grid_tile.mIsDrawCell = XEditorInspector.PropertyBoolean("IsDrawCell", grid_tile.mIsDrawCell);

		GUILayout.Space(2.0f);
		grid_tile.mDrawCellColor = XEditorInspector.PropertyColor("DrawCellColor", grid_tile.mDrawCellColor);
	}
	#endregion
}
//=====================================================================================================================