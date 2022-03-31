//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль работы со спрайтами
// Подраздел: Расширенные элементы управления
// Группа: Тайловые(плиточные) элементы
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusSpriteTileContentEditor.cs
*		Редактор компонента элемента TileContent.
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
/// Редактор компонента элемента TileContent
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusSpriteTileContent))]
public class LotusSpriteTileContentEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Создание элемента TileContent с параметрами по умолчанию
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XSpriteEditorSettings.MenuPathExtended + "Tiled/Create TileContent", false, XSpriteEditorSettings.MenuOrderExtended + 4)]
	public static void CreateTileContent()
	{
		LotusSpriteTileContent tile = LotusSpriteTileContent.CreateTileContent(0, 0, 50, 50, Selection.activeTransform);
		Undo.RegisterCreatedObjectUndo(tile.gameObject, "TileContent");
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusSpriteTileContent mTileContent;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnEnable()
	{
		mTileContent = this.target as LotusSpriteTileContent;
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		// Параметры базового элемента (местоположение и интерактивность)
		mTileContent.mExpandedSize = XEditorInspector.DrawGroupFoldout("Parameters and size", mTileContent.mExpandedSize);
		if (mTileContent.mExpandedSize)
		{
			LotusSpriteElementEditor.DrawElementParam(mTileContent);
		}

		EditorGUI.BeginChangeCheck();
		{
			// Основные параметры элемента
			GUILayout.Space(4.0f);
			mTileContent.mExpandedParam = XEditorInspector.DrawGroupFoldout("Settings tile base", mTileContent.mExpandedParam);
			if (mTileContent.mExpandedParam)
			{
				LotusSpriteTileBaseEditor.DrawTileBaseParams(mTileContent);
			}

			DrawTileAnimationParams(mTileContent);
		}
		if (EditorGUI.EndChangeCheck())
		{
			mTileContent.SaveInEditor();
		}

		GUILayout.Space(2.0f);
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров анимации тайла
	/// </summary>
	/// <param name="tile">Элемент</param>
	//-----------------------------------------------------------------------------------------------------------------
	public static void DrawTileAnimationParams(LotusSpriteTileContent tile)
	{
		// Основные параметры элемента
		GUILayout.Space(4.0f);
		tile.mExpandedAnimation = XEditorInspector.DrawGroupFoldout("Animation settings", tile.mExpandedAnimation);
		if (tile.mExpandedAnimation)
		{
			XEditorInspector.DrawGroup("Translate");
			LotusTweenVector2DDrawer.Draw(tile.TweenTranslate, TTweenDrawParam.CurveForward|TTweenDrawParam.Duration);

			XEditorInspector.DrawGroup("Rotation Z");
			LotusTweenSingleDrawer.Draw(tile.TweenRotationZ, TTweenDrawParam.CurveForward | TTweenDrawParam.Duration);

			XEditorInspector.DrawGroup("Rotation X");
			LotusTweenSingleDrawer.Draw(tile.TweenRotationX, TTweenDrawParam.CurveForward | TTweenDrawParam.Duration);

			XEditorInspector.DrawGroup("Rotation Y");
			LotusTweenSingleDrawer.Draw(tile.TweenRotationY, TTweenDrawParam.CurveForward | TTweenDrawParam.Duration);

			XEditorInspector.DrawGroup("Color");
			LotusTweenColorDrawer.Draw(tile.TweenColor, TTweenDrawParam.CurveForward | TTweenDrawParam.Duration);

			XEditorInspector.DrawGroup("Sprite");
			LotusTweenSpriteDrawer.Draw(tile.TweenSprite, TTweenDrawParam.CurveForward | TTweenDrawParam.Duration);
		}
	}
	#endregion
}
//=====================================================================================================================