//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль работы со спрайтами
// Подраздел: Расширенные элементы управления
// Группа: Тайловые(плиточные) элементы
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusSpriteTileBaseEditor.cs
*		Редактор компонента элемента TileBase.
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
/// Редактор компонента элемента TileBase
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusSpriteTileBase))]
public class LotusSpriteTileBaseEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Создание элемента TileBase с параметрами по умолчанию
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XSpriteEditorSettings.MenuPathExtended + "Tiled/Create TileBase", false, XSpriteEditorSettings.MenuOrderExtended + 2)]
	public static void CreateTileBase()
	{
		LotusSpriteTileBase tile = LotusSpriteTileBase.CreateTileBase(0, 0, 50, 50, Selection.activeTransform);
		Undo.RegisterCreatedObjectUndo(tile.gameObject, "TileBase");
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusSpriteTileBase mTileBase;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnEnable()
	{
		mTileBase = this.target as LotusSpriteTileBase;
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		// Параметры базового элемента (местоположение и интерактивность)
		mTileBase.mExpandedSize = XEditorInspector.DrawGroupFoldout("Parameters and size", mTileBase.mExpandedSize);
		if (mTileBase.mExpandedSize)
		{
			LotusSpriteElementEditor.DrawElementParam(mTileBase);
		}

		EditorGUI.BeginChangeCheck();
		{
			// Основные параметры элемента
			GUILayout.Space(4.0f);
			mTileBase.mExpandedParam = XEditorInspector.DrawGroupFoldout("Settings tile base", mTileBase.mExpandedParam);
			if (mTileBase.mExpandedParam)
			{
				DrawTileBaseParams(mTileBase);
				LotusTweenVector2DDrawer.Draw(mTileBase.TweenTranslate);
			}
		}
		if (EditorGUI.EndChangeCheck())
		{
			mTileBase.SaveInEditor();
		}

		GUILayout.Space(2.0f);
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование основных параметров базового тайла
	/// </summary>
	/// <param name="tile_base">Элемент</param>
	//-----------------------------------------------------------------------------------------------------------------
	public static void DrawTileBaseParams(LotusSpriteTileBase tile_base)
	{
		GUILayout.Space(2.0f);
		tile_base.OnPlacementGrid = XEditorInspector.PropertyBoolean("OnPlacementGrid", tile_base.OnPlacementGrid);

		GUILayout.Space(2.0f);
		tile_base.IsDraggEvent = XEditorInspector.PropertyBoolean("IsDraggEvent", tile_base.IsDraggEvent);

		GUILayout.Space(2.0f);
		tile_base.IsOverOwner = XEditorInspector.PropertyBoolean("IsOverOwner", tile_base.IsOverOwner);
	}
	#endregion
}
//=====================================================================================================================