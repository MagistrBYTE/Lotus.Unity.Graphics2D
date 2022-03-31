//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Общий модуль 2D графики
// Подраздел: Визуальные стили
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGraphics2DVisualStyleServiceEditor.cs
*		Редактор центрального сервиса для управления визуальными стилями и скинами подсистемы Unity UI.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Graphics2D;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор центрального сервиса для управления визуальными стилями и скинами подсистемы Unity UI
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusGraphics2DVisualStyleService))]
public class LotusGraphics2DVisualStyleServiceEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ ДАННЫЕ ========================================
	protected const String mDescriptionService = "Центральный сервис для управления скинами, общими параметрами и визуальными стилями";
	protected static readonly GUIContent mContentDeleteSkin = new GUIContent("X", "Delete reference to skin");
	protected static readonly GUIContent mContentSetSkin = new GUIContent("S", "Set skin to current");
	protected static readonly GUIContent mContentAddSkin = new GUIContent("Add", "Add reference to skin");
	protected static readonly GUIContent mContentFind = new GUIContent("Find resources", "Find and load resources");
	#endregion

	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Создания центрального сервиса для управления визуальными стилями и скинами
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XGraphics2DEditorSettings.MenuPath + "Create Visual Style Service", false, XGraphics2DEditorSettings.MenuOrderLast + 5)]
	public static void Create()
	{
#pragma warning disable 0219
		LotusGraphics2DVisualStyleService visual_style_service = LotusGraphics2DVisualStyleService.Instance;
#pragma warning restore 0219
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusGraphics2DVisualStyleService mUIVisualStyleService;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnEnable()
	{
		mUIVisualStyleService = this.target as LotusGraphics2DVisualStyleService;
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		GUILayout.Space(4.0f);
		EditorGUILayout.HelpBox(mDescriptionService, MessageType.Info);

		// Скины
		DrawSkins();

		// Общие данные
		DrawCommonData();

		GUILayout.Space(2.0f);
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров скинов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawSkins()
	{
		EditorGUI.BeginChangeCheck();

		GUILayout.Space(2.0f);
		mUIVisualStyleService.mExpandedSkins = XEditorInspector.DrawGroupFoldout("Skins", mUIVisualStyleService.mExpandedSkins);
		if (mUIVisualStyleService.mExpandedSkins)
		{
			GUILayout.Space(2.0f);
			LotusGraphics2DVisualStyleService.CurrentSkin = XEditorInspector.PropertyResource("CurrentSkin", LotusGraphics2DVisualStyleService.CurrentSkin);

			List<LotusGraphics2DVisualStyleStorage> list = mUIVisualStyleService.mSkins;
			for (Int32 i = 0; i < list.Count; i++)
			{
				GUILayout.Space(2.0f);
				EditorGUILayout.BeginHorizontal();
				{
					list[i] = XEditorInspector.PropertyResource(i.ToString() +  ". Skin", list[i]);
					if (GUILayout.Button(mContentDeleteSkin, EditorStyles.miniButtonLeft))
					{
						if (i > 0)
						{
							mUIVisualStyleService.RemoveSkin(i);
							EditorGUILayout.EndHorizontal();
							mUIVisualStyleService.Flush();
							break;
						}
					}
					if (GUILayout.Button(mContentSetSkin, EditorStyles.miniButtonRight))
					{
						mUIVisualStyleService.ChangeSkin(i);
					}
				}
				EditorGUILayout.EndHorizontal();
			}


			GUILayout.Space(4.0f);
			if (GUILayout.Button(mContentAddSkin))
			{
				mUIVisualStyleService.AddSkin();
			}
		}

		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.Save();
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование общих данных
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawCommonData()
	{
		EditorGUI.BeginChangeCheck();

		GUILayout.Space(2.0f);
		mUIVisualStyleService.mExpandedMaterial = XEditorInspector.DrawGroupFoldout("Common data", mUIVisualStyleService.mExpandedMaterial);
		if (mUIVisualStyleService.mExpandedMaterial)
		{
			GUILayout.Space(2.0f);
			mUIVisualStyleService.mSpriteModalPanel = XEditorInspector.PropertyResource("Sprite Modal Panel", mUIVisualStyleService.mSpriteModalPanel);

			XEditorInspector.DrawGroup("Materials");
			GUILayout.Space(2.0f);
			mUIVisualStyleService.mMaterialDisableImage = XEditorInspector.PropertyResource("Disable Image", mUIVisualStyleService.mMaterialDisableImage);

			GUILayout.Space(2.0f);
			mUIVisualStyleService.mMaterialDisableText = XEditorInspector.PropertyResource("Disable Text", mUIVisualStyleService.mMaterialDisableText);

			XEditorInspector.DrawGroup("Opacity Image");
			GUILayout.Space(2.0f);
			mUIVisualStyleService.mMaskOpacity02 = XEditorInspector.PropertyResource("MaskOpacity 2%", mUIVisualStyleService.mMaskOpacity02);

			GUILayout.Space(2.0f);
			mUIVisualStyleService.mMaskOpacity05 = XEditorInspector.PropertyResource("MaskOpacity 5%", mUIVisualStyleService.mMaskOpacity05);

			GUILayout.Space(2.0f);
			mUIVisualStyleService.mMaskOpacity10 = XEditorInspector.PropertyResource( "MaskOpacity 10%", mUIVisualStyleService.mMaskOpacity10);

			GUILayout.Space(2.0f);
			mUIVisualStyleService.mMaskOpacity20 = XEditorInspector.PropertyResource("MaskOpacity 20%", mUIVisualStyleService.mMaskOpacity20);

			GUILayout.Space(2.0f);
			mUIVisualStyleService.mMaskOpacity50 = XEditorInspector.PropertyResource("MaskOpacity 50%", mUIVisualStyleService.mMaskOpacity50);

			GUILayout.Space(4.0f);
			if (GUILayout.Button(mContentFind))
			{
				mUIVisualStyleService.mMaterialDisableImage = XResourcesDispatcher.Find<Material>("UIDisableImage");
				mUIVisualStyleService.mMaterialDisableText = XResourcesDispatcher.Find<Material>("UIDisableText");
				mUIVisualStyleService.mSpriteModalPanel = XSprite.Find("BoxTransparent50");
				mUIVisualStyleService.mMaskOpacity02 = XSprite.Find("BoxTransparent02");
				mUIVisualStyleService.mMaskOpacity05 = XSprite.Find("BoxTransparent05");
				mUIVisualStyleService.mMaskOpacity10 = XSprite.Find("BoxTransparent10");
				mUIVisualStyleService.mMaskOpacity20 = XSprite.Find("BoxTransparent20");
				mUIVisualStyleService.mMaskOpacity50 = XSprite.Find("BoxTransparent50");
			}
		}

		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.Save();
		}
	}
	#endregion
}
//=====================================================================================================================