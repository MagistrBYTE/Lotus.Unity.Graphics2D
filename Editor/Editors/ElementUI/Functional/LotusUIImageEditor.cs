//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Общая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIImageEditor.cs
*		Редактор визуального компонента расширяющего возможности базового компонента изображения модуля компонентов Unity UI.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Graphics2D;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор визуального компонента расширяющего возможности базового компонента изображения модуля компонентов Unity UI
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusUIImage), true)]
public class LotusUIImageEditor : ImageEditor
{
	#region =============================================== СТАТИЧЕСКИЕ ДАННЫЕ ========================================
	protected static GUIContent mContentRemoveEffect = new GUIContent("X", "Remove this effect");
	protected static GUIContent mContentAddEffect = new GUIContent("Add effect for");
	#endregion

	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Создание компонента Image с параметрами по умолчанию
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XElementUIEditorSettings.MenuPathFunc + "Create Image", false, XElementUIEditorSettings.MenuOrderFunc + 1)]
	public static void CreateImage()
	{
		LotusUIImage image = LotusUIImage.CreateImage(128, 128);
		Undo.RegisterCreatedObjectUndo(image.gameObject, "Image");
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusUIImage mUIImage;
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
		mUIImage = this.target as LotusUIImage;
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
		EditorGUI.BeginChangeCheck();
		{
			GUILayout.Space(4.0f);
			mUIImage.mExpandedSize = XEditorInspector.DrawGroupFoldout("Size and location element", mUIImage.mExpandedSize);
			if (mUIImage.mExpandedSize)
			{
				LotusUIPlaceable2DEditor.DrawPlaceable(mUIImage, 3000, 3000);
			}

			GUILayout.Space(4.0f);
			mUIImage.mExpandedParam = XEditorInspector.DrawGroupFoldout("Main settings", mUIImage.mExpandedParam);
			if (mUIImage.mExpandedParam)
			{
				base.OnInspectorGUI();
			}

			DrawOtherImage(mUIImage);
		}
		if (EditorGUI.EndChangeCheck())
		{
			this.serializedObject.Save();
		}

		GUILayout.Space(2.0f);
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование дополнительных параметров компонента изображения
	/// </summary>
	/// <param name="ui_image">Компонент изображения</param>
	//-----------------------------------------------------------------------------------------------------------------
	public static void DrawOtherImage(LotusUIImage ui_image)
	{
		GUILayout.Space(4.0f);
		ui_image.mExpandedOther = XEditorInspector.DrawGroupFoldout("Other settings", ui_image.mExpandedOther);
		if (ui_image.mExpandedOther)
		{
			GUILayout.Space(4.0f);
			XEditorInspector.DrawGroup("Additionally");
			{
				EditorGUI.indentLevel++;

				GUILayout.Space(2.0f);
				EditorGUI.BeginChangeCheck();
				{
					ui_image.mUseShadow = XEditorInspector.PropertyBoolean("UseShadow", ui_image.mUseShadow);
				}
				if (EditorGUI.EndChangeCheck())
				{
					ui_image.AutoComponent<Shadow>(ui_image.mUseShadow);
				}

				GUILayout.Space(2.0f);
				EditorGUI.BeginChangeCheck();
				{
					ui_image.mUseOutline = XEditorInspector.PropertyBoolean("UseOutline", ui_image.mUseOutline);
				}
				if (EditorGUI.EndChangeCheck())
				{
					ui_image.AutoComponent<Outline>(ui_image.mUseOutline);
				}

				EditorGUI.indentLevel--;
			}
		}
	}
	#endregion
}
//=====================================================================================================================