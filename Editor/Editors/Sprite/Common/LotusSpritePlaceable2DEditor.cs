//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль работы со спрайтами
// Подраздел: Общая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusSpritePlaceable2DEditor.cs
*		Редактор компонента размещения спрайта в двухмерной пространстве.
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
/// Редактор компонента размещения спрайта в двухмерной пространстве
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusSpritePlaceable2D))]
public class LotusSpritePlaceable2DEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Создание элемента SpritePlaceable2D с параметрами по умолчанию
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XSpriteEditorSettings.MenuPath + "Create Sprite Placeable2D", false, XSpriteEditorSettings.MenuOrderLast + 1)]
	public static void CreateSpritePlaceable2D()
	{
		LotusSpritePlaceable2D sprite_create = LotusSpritePlaceable2D.CreateSprite(0, 0, 300, 600, Selection.activeTransform);
		Undo.RegisterCreatedObjectUndo(sprite_create.gameObject, "Sprite2D");
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusSpritePlaceable2D mSprite;
	protected static GUIContent mContentDepthUp = new GUIContent(XString.TriangleUp);
	protected static GUIContent mContentDepthDown = new GUIContent(XString.TriangleDown);
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnEnable()
	{
		mSprite = this.target as LotusSpritePlaceable2D;
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		mSprite.mExpandedSize = XEditorInspector.DrawGroupFoldout("Parameters and size", mSprite.mExpandedSize);
		if (mSprite.mExpandedSize)
		{
			DrawBasePlaced2D(mSprite);
		}
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров местоположения компонента размещения спрайта
	/// </summary>
	/// <param name="element">Компонент размещения спрайта</param>
	//-----------------------------------------------------------------------------------------------------------------
	public static void DrawBasePlaced2D(LotusSpritePlaceable2D element)
	{
		EditorGUI.BeginChangeCheck();

		GUILayout.Space(2.0f);
		element.Left = XEditorInspector.PropertyFloat(nameof(element.Left), element.Left);

		GUILayout.Space(2.0f);
		element.Top = XEditorInspector.PropertyFloat(nameof(element.Top), element.Top);

		GUILayout.Space(2.0f);
		element.Width = XEditorInspector.PropertyFloatSlider(nameof(element.Width), element.Width, 1.0f, 3000);

		GUILayout.Space(2.0f);
		element.Height = XEditorInspector.PropertyFloatSlider(nameof(element.Height), element.Height, 1.0f, 3000);

		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		{
			element.Depth = XEditorInspector.PropertyInt(nameof(element.Depth), element.Depth);
			if (GUILayout.Button(mContentDepthUp, EditorStyles.miniButtonLeft))
			{
				element.Depth++;
			}
			if (GUILayout.Button(mContentDepthDown, EditorStyles.miniButtonRight))
			{
				element.Depth--;
			}

		}
		EditorGUILayout.EndHorizontal();

		if (EditorGUI.EndChangeCheck())
		{
			element.SaveInEditor();
		}
	}
	#endregion
}
//=====================================================================================================================