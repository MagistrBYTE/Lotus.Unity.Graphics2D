//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Общий модуль 2D графики
// Подраздел: Визуальные стили
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGraphics2DVisualStyleStorageEditor.cs
*		Редактор хранилища (ресурса) визуальных стилей Unity UI.
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
/// Редактор хранилища (ресурса) визуальных стилей Unity UI
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusGraphics2DVisualStyleStorage))]
public class LotusGraphics2DVisualStyleStorageEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ ДАННЫЕ ========================================
	protected const String mDescriptionStorage = "Хранилище визуальных стилей и общих цветовых параметров";
	protected static readonly GUIContent mContentRemove = new GUIContent("X", "Remove style");
	protected static readonly GUIContent mContentDuplicate = new GUIContent("D", "Duplicate style");
	protected static readonly GUIContent mContentCopy = new GUIContent("C", "Copy style");
	protected static readonly GUIContent mContentPaste = new GUIContent("P", "Paste style");
	protected static readonly GUIContent mContentSave = new GUIContent("Save", "Save styles to file");
	protected static readonly GUIContent mContentLoadNew = new GUIContent("Load New", "Clear and load new style");
	protected static readonly GUIContent mContentLoadAdd = new GUIContent("Load Add", "Load and add style");
	protected static readonly GUIContent mContentDefaultValue = new GUIContent("D", "Set default value");
	#endregion

	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Создания ресурса для хранения визуальных стилей
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XGraphics2DEditorSettings.MenuPath + "Create Visual Style Storage", false, XGraphics2DEditorSettings.MenuOrderLast + 51)]
	public static void Create()
	{
		// Открываем панель
		String path = EditorUtility.SaveFilePanelInProject("Create Visual Storage", "Style Name", "asset",
			"Input name resource Visual Style Storage", LotusGraphics2DVisualStyleStorage.VISUAL_STYLE_STORAGE_PATH);
		if (path.Length != 0)
		{
			LotusGraphics2DVisualStyleStorage ui_visual_style_storage = ScriptableObject.CreateInstance<LotusGraphics2DVisualStyleStorage>();
			ui_visual_style_storage.Create();
			AssetDatabase.CreateAsset(ui_visual_style_storage, path);
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Создания ресурса для хранения визуальных стилей
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XGraphics2DEditorSettings.MenuPath + "Create Visual Style Storage (Default)", false, XGraphics2DEditorSettings.MenuOrderLast + 52)]
	public static void CreateDefault()
	{
#pragma warning disable 0219
		LotusGraphics2DVisualStyleStorage visual_style_storage = LotusGraphics2DVisualStyleStorage.Default;
#pragma warning restore 0219
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusGraphics2DVisualStyleStorage mUIVisualStyleStorage;
	private String mFileNameXML;
	private TextAsset mFileAssetXML;
	private Int32 mCountView;
	private GUIStyle mGUIStyleText;
	private CVisualStyleBase mSelectedStyleBase;
	private CVisualStyleHeader mSelectedStyleHeader;
	private CVisualStyleScroll mSelectedStyleScroll;
	private CVisualStyleSpinner mSelectedStyleSpinner;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnEnable()
	{
		mUIVisualStyleStorage = this.target as LotusGraphics2DVisualStyleStorage;
		mGUIStyleText = new GUIStyle();
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		GUILayout.Space(4.0f);
		EditorGUILayout.HelpBox(mDescriptionStorage, MessageType.Info);

		// Общие данные
		DrawCommonData();

		// Визуальные стили
		DrawVisualStyles();

		// Сохранение
		DrawLoadSave();

		GUILayout.Space(2.0f);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Имеется реализация предпросмотра
	/// </summary>
	/// <returns>Статус реализации предпросмотра</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public override Boolean HasPreviewGUI()
	{
		return (true);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Реализация предпросмотра
	/// </summary>
	/// <param name="rect">Область предпросмотра</param>
	/// <param name="background">Фоновый стиль</param>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnPreviewGUI(Rect rect, GUIStyle background)
	{
		// Ищем первый открытий стиль
		CVisualStyleBase vsb = null;
		for (Int32 i = 0; i < mUIVisualStyleStorage.BaseStyles.Count; i++)
		{
			if (mUIVisualStyleStorage.BaseStyles[i].mExpandedInfo)
			{
				vsb = mUIVisualStyleStorage.BaseStyles[i];
				break;
			}
		}

		if(vsb != null)
		{
			if(vsb.BackgroundImage != null && vsb.BackgroundImagePressed == null && vsb.BackgroundImageSelected == null)
			{
				mCountView = 1;
			}
			if (vsb.BackgroundImage != null && vsb.BackgroundImagePressed != null && vsb.BackgroundImageSelected == null)
			{
				mCountView = 2;
			}
			if (vsb.BackgroundImage != null && vsb.BackgroundImagePressed == null && vsb.BackgroundImageSelected != null)
			{
				mCountView = 3;
			}
			if (vsb.BackgroundImage != null && vsb.BackgroundImagePressed != null && vsb.BackgroundImageSelected != null)
			{
				mCountView = 4;
			}

			switch (mCountView)
			{
				case 1:
					{
						Rect rect_normal = rect.Inflate(-10, -10);
						DrawSpritePreview(rect_normal, vsb.BackgroundImage);
						DrawTextStyleNormal(rect_normal, vsb, "Normal");
					}
					break;
				case 2:
					{
						Rect rect_normal = rect.GetColumnFromIndex(2, 0);
						rect_normal = rect_normal.Inflate(-10, -10);
						DrawSpritePreview(rect_normal, vsb.BackgroundImage);
						DrawTextStyleNormal(rect_normal, vsb, "Normal");

						Rect rect_pressed = rect.GetColumnFromIndex(2, 1);
						rect_pressed = rect_pressed.Inflate(-10, -10);
						DrawSpritePreview(rect_pressed, vsb.BackgroundImagePressed);
						DrawTextStyleActive(rect_pressed, vsb, "Pressed");
					}
					break;
				case 3:
					{
						Rect rect_normal = rect.GetColumnFromIndex(2, 0);
						rect_normal = rect_normal.Inflate(-10, -10);
						DrawSpritePreview(rect_normal, vsb.BackgroundImage);
						DrawTextStyleNormal(rect_normal, vsb, "Normal");

						Rect rect_selected = rect.GetColumnFromIndex(2, 1);
						rect_selected = rect_selected.Inflate(-10, -10);
						DrawSpritePreview(rect_selected, vsb.BackgroundImageSelected);
						DrawTextStyleActive(rect_selected, vsb, "Selected");
					}
					break;

				case 4:
					{
						Rect rect_normal = rect.GetColumnFromIndex(3, 0);
						rect_normal = rect_normal.Inflate(-10, -10);
						DrawSpritePreview(rect_normal, vsb.BackgroundImage);
						DrawTextStyleNormal(rect_normal, vsb, "Normal");

						Rect rect_pressed = rect.GetColumnFromIndex(3, 1);
						rect_pressed = rect_pressed.Inflate(-10, -10);
						DrawSpritePreview(rect_pressed, vsb.BackgroundImagePressed);
						DrawTextStyleActive(rect_pressed, vsb, "Pressed");

						Rect rect_selected = rect.GetColumnFromIndex(3, 2);
						rect_selected = rect_selected.Inflate(-10, -10);
						DrawSpritePreview(rect_selected, vsb.BackgroundImageSelected);
						DrawTextStyleActive(rect_selected, vsb, "Selected");
					}
					break;
				default:
					break;
			}
		}
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование спрайта в области предпросмотра
	/// </summary>
	/// <param name="rect">Прямоугольник</param>
	/// <param name="sprite">Спрайт</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawSpritePreview(Rect rect, Sprite sprite)
	{
		if (sprite != null)
		{
			Rect rect_source = sprite.textureRect;
			rect_source.x = rect_source.x / sprite.texture.width;
			rect_source.y = rect_source.y / sprite.texture.height;
			rect_source.width = rect_source.width / sprite.texture.width;
			rect_source.height = rect_source.height / sprite.texture.height;

			Graphics.DrawTexture(rect, sprite.texture, rect_source, (Int32)sprite.border.x,
				(Int32)sprite.border.z, (Int32)sprite.border.w, (Int32)sprite.border.y);
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование текста стиля в области предпросмотра
	/// </summary>
	/// <param name="rect">Прямоугольник</param>
	/// <param name="style">Стиль</param>
	/// <param name="text">Текст</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawTextStyleNormal(Rect rect, CVisualStyleBase style, String text)
	{
		mGUIStyleText.normal.textColor = style.TextColor;
		mGUIStyleText.font = style.FontFamily;
		mGUIStyleText.fontSize = style.FontSize;
		mGUIStyleText.fontStyle = style.FontStyle;
		mGUIStyleText.alignment = style.TextAnchor;
		GUI.Label(rect, text, mGUIStyleText);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование текста стиля в области предпросмотра
	/// </summary>
	/// <param name="rect">Прямоугольник</param>
	/// <param name="style">Стиль</param>
	/// <param name="text">Текст</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawTextStyleActive(Rect rect, CVisualStyleBase style, String text)
	{
		mGUIStyleText.normal.textColor = style.TextColorActive;
		mGUIStyleText.font = style.FontFamily;
		mGUIStyleText.fontSize = style.FontSize;
		mGUIStyleText.fontStyle = style.FontStyle;
		mGUIStyleText.alignment = style.TextAnchor;
		GUI.Label(rect, text, mGUIStyleText);
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
		mUIVisualStyleStorage.mExpandedInfo = XEditorInspector.DrawSectionFoldout("Common data", mUIVisualStyleStorage.mExpandedInfo);
		if (mUIVisualStyleStorage.mExpandedInfo)
		{
			XEditorInspector.DrawGroup("Colors");
			{
				GUILayout.Space(2.0f);
				mUIVisualStyleStorage.mCaptionColor = XEditorInspector.PropertyColor("Caption", mUIVisualStyleStorage.mCaptionColor);

				GUILayout.Space(2.0f);
				mUIVisualStyleStorage.mCaptionActiveColor = XEditorInspector.PropertyColor("Caption Active", mUIVisualStyleStorage.mCaptionActiveColor);

				GUILayout.Space(2.0f);
				mUIVisualStyleStorage.mBackgroundActiveColor = XEditorInspector.PropertyColor("Background Active", mUIVisualStyleStorage.mBackgroundActiveColor);

				GUILayout.Space(2.0f);
				mUIVisualStyleStorage.mIconActiveColor = XEditorInspector.PropertyColor("Icon Active", mUIVisualStyleStorage.mIconActiveColor);

				GUILayout.Space(2.0f);
				mUIVisualStyleStorage.mHeaderActiveColor = XEditorInspector.PropertyColor("Header Active", mUIVisualStyleStorage.mHeaderActiveColor);
			}
		}

		if (EditorGUI.EndChangeCheck())
		{
			EditorUtility.SetDirty(mUIVisualStyleStorage);
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование данных визуальных стилей
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawVisualStyles()
	{
		// Визуальные стили
		DrawBaseStyles("Visual Style (Base)", mUIVisualStyleStorage.mBaseStyles, 
			ref mUIVisualStyleStorage.mExpandedBase, DrawStyleBase);

		DrawBaseStyles("Visual Style (CheckBox)", mUIVisualStyleStorage.mCheckBoxStyles, 
			ref mUIVisualStyleStorage.mExpandedCheckBox, DrawStyleCheckBox);

		DrawBaseStyles("Visual Style (Joysticks)", mUIVisualStyleStorage.mJoystickStyles,
			ref mUIVisualStyleStorage.mExpandedJoystick, DrawStyleBase);

		DrawScrollStyles("Visual Style (Scroll)", mUIVisualStyleStorage.mScrollStyles);
		DrawHeaderStyles("Visual Style (Header)", mUIVisualStyleStorage.mHeaderStyles);
		DrawSpinnerStyles("Visual Style (Spinner)", mUIVisualStyleStorage.mSpinnerStyles);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров сохранения/загрузки
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawLoadSave()
	{
		EditorGUI.BeginChangeCheck();

		GUILayout.Space(2.0f);
		mUIVisualStyleStorage.mExpandedSerialized = XEditorInspector.DrawSectionFoldout("Save/Load XML", mUIVisualStyleStorage.mExpandedSerialized);
		if (mUIVisualStyleStorage.mExpandedSerialized)
		{
			EditorGUI.indentLevel++;
			XEditorInspector.DrawGroup("Save");
			{
				GUILayout.Space(2.0f);
				EditorGUILayout.BeginHorizontal();
				{
					mFileNameXML = XEditorInspector.PropertyString("File Name", mFileNameXML);
					if (GUILayout.Button(mContentSave, EditorStyles.miniButtonRight))
					{
						if (!String.IsNullOrEmpty(mFileNameXML))
						{
							mUIVisualStyleStorage.SaveToXml(mFileNameXML);
						}
					}
				}
				EditorGUILayout.EndHorizontal();
			}

			GUILayout.Space(2.0f);
			XEditorInspector.DrawGroup("Load");
			{
				GUILayout.Space(2.0f);
				EditorGUILayout.BeginHorizontal();
				{
					mFileAssetXML = XEditorInspector.PropertyResource("Text Asset (XML)", mFileAssetXML);
					if (GUILayout.Button(mContentLoadNew, EditorStyles.miniButtonLeft))
					{
						if (mFileAssetXML != null)
						{
							mUIVisualStyleStorage.LoadFromXml(mFileAssetXML, false);
							LotusGraphics2DVisualStyleService.UpdateVisualStyles();
							EditorUtility.SetDirty(mUIVisualStyleStorage);
						}
					}
					if (GUILayout.Button(mContentLoadAdd, EditorStyles.miniButtonRight))
					{
						if (mFileAssetXML != null)
						{
							mUIVisualStyleStorage.LoadFromXml(mFileAssetXML, true);
							LotusGraphics2DVisualStyleService.UpdateVisualStyles();
							EditorUtility.SetDirty(mUIVisualStyleStorage);
						}
					}
					EditorGUILayout.EndHorizontal();
				}
			}
			EditorGUI.indentLevel--;
		}

		if (EditorGUI.EndChangeCheck())
		{
			EditorUtility.SetDirty(mUIVisualStyleStorage);
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование базовых визуальных стилей
	/// </summary>
	/// <param name="panel_label">Надпись панели</param>
	/// <param name="style_bases">Список базовых визуальных стилей</param>
	/// <param name="opened">Параметр для сворачивания/разворачивания панели</param>
	/// <param name="draw_style">Делегат рисования стиля</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawBaseStyles(String panel_label, List<CVisualStyleBase> style_bases, ref Boolean opened, 
		Action<CVisualStyleBase, Boolean> draw_style)
	{
		EditorGUI.BeginChangeCheck();

		GUILayout.Space(2.0f);
		opened = XEditorInspector.DrawSectionFoldout(panel_label, opened);
		if (opened)
		{
			for (Int32 i = 0; i < style_bases.Count; i++)
			{
				CVisualStyleBase style_base = style_bases[i];
				style_base.mExpandedInfo = XEditorInspector.DrawGroupFoldout(1, style_base.Name, ref style_base.mExpandedInfo,
					()=>
					{
						EditorGUI.indentLevel++;
						draw_style(style_base, true);
						EditorGUI.indentLevel--;
					},
					()=>
					{
						style_bases.MoveElementUp(i);
					},
					()=>
					{
						style_bases.MoveElementDown(i);
					},
					() =>
					{
						style_bases.Insert(i, style_bases[i].CloneBase());
						EditorUtility.SetDirty(mUIVisualStyleStorage);
					},
					()=>
					{
						style_bases.RemoveAt(i);
						EditorUtility.SetDirty(mUIVisualStyleStorage);
					});
			}

			GUILayout.Space(4.0f);
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Space(XInspectorViewParams.OFFSET_INDENT);
				if (GUILayout.Button("Add"))
				{
					style_bases.Add(new CVisualStyleBase());
					EditorUtility.SetDirty(mUIVisualStyleStorage);
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		if (EditorGUI.EndChangeCheck())
		{
			EditorUtility.SetDirty(mUIVisualStyleStorage);
			LotusGraphics2DVisualStyleService.UpdateVisualStyles();
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование визуальных стилей для прогресса/скроллов
	/// </summary>
	/// <param name="panel_label">Надпись панели</param>
	/// <param name="style_scrolls">Список визуальных стилей для прогресса/скроллов</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawScrollStyles(String panel_label, List<CVisualStyleScroll> style_scrolls)
	{
		EditorGUI.BeginChangeCheck();

		GUILayout.Space(2.0f);
		mUIVisualStyleStorage.mExpandedScrolls = XEditorInspector.DrawSectionFoldout(panel_label, mUIVisualStyleStorage.mExpandedScrolls);
		if (mUIVisualStyleStorage.mExpandedScrolls)
		{
			for (Int32 i = 0; i < style_scrolls.Count; i++)
			{
				CVisualStyleScroll style_base = style_scrolls[i];
				style_base.mExpandedInfo = XEditorInspector.DrawGroupFoldout(1, style_base.Name, ref style_base.mExpandedInfo,
					() =>
					{
						EditorGUI.indentLevel++;
						DrawStyleScroll(style_base);
						EditorGUI.indentLevel--;
					},
					() =>
					{
						style_scrolls.MoveElementUp(i);
					},
					() =>
					{
						style_scrolls.MoveElementDown(i);
					},
					() =>
					{
						style_scrolls.Insert(i, style_scrolls[i].CloneScroll());
						EditorUtility.SetDirty(mUIVisualStyleStorage);
					},
					() =>
					{
						style_scrolls.RemoveAt(i);
						EditorUtility.SetDirty(mUIVisualStyleStorage);
					});
			}

			GUILayout.Space(4.0f);
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Space(XInspectorViewParams.OFFSET_INDENT);
				if (GUILayout.Button("Add"))
				{
					style_scrolls.Add(new CVisualStyleScroll());
					EditorUtility.SetDirty(mUIVisualStyleStorage);
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		if (EditorGUI.EndChangeCheck())
		{
			EditorUtility.SetDirty(mUIVisualStyleStorage);
			LotusGraphics2DVisualStyleService.UpdateVisualStyles();
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование визуальных стилей для заголовочных элементов
	/// </summary>
	/// <param name="panel_label">Надпись панели</param>
	/// <param name="style_headers">Список визуальных стилей для заголовочных элементов</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawHeaderStyles(String panel_label, List<CVisualStyleHeader> style_headers)
	{
		EditorGUI.BeginChangeCheck();

		GUILayout.Space(2.0f);
		mUIVisualStyleStorage.mExpandedHeaders = XEditorInspector.DrawSectionFoldout(panel_label, mUIVisualStyleStorage.mExpandedHeaders);
		if (mUIVisualStyleStorage.mExpandedHeaders)
		{
			for (Int32 i = 0; i < style_headers.Count; i++)
			{
				CVisualStyleHeader style_base = style_headers[i];
				style_base.mExpandedInfo = XEditorInspector.DrawGroupFoldout(1, style_base.Name, ref style_base.mExpandedInfo,
					() =>
					{
						EditorGUI.indentLevel++;
						DrawStyleHeader(style_base);
						EditorGUI.indentLevel--;
					},
					() =>
					{
						style_headers.MoveElementUp(i);
					},
					() =>
					{
						style_headers.MoveElementDown(i);
					},
					() =>
					{
						style_headers.Insert(i, style_headers[i].CloneHeader());
						EditorUtility.SetDirty(mUIVisualStyleStorage);
					},
					() =>
					{
						style_headers.RemoveAt(i);
						EditorUtility.SetDirty(mUIVisualStyleStorage);
					});
			}

			GUILayout.Space(4.0f);
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Space(XInspectorViewParams.OFFSET_INDENT);
				if (GUILayout.Button("Add"))
				{
					style_headers.Add(new CVisualStyleHeader());
					EditorUtility.SetDirty(mUIVisualStyleStorage);
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		if (EditorGUI.EndChangeCheck())
		{
			EditorUtility.SetDirty(mUIVisualStyleStorage);
			LotusGraphics2DVisualStyleService.UpdateVisualStyles();
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование визуальных стилей для счетчиков
	/// </summary>
	/// <param name="panel_label">Надпись панели</param>
	/// <param name="style_spinners">Список визуальных стилей для счетчиков</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawSpinnerStyles(String panel_label, List<CVisualStyleSpinner> style_spinners)
	{
		EditorGUI.BeginChangeCheck();

		GUILayout.Space(2.0f);
		mUIVisualStyleStorage.mExpandedSpinners = XEditorInspector.DrawSectionFoldout(panel_label, mUIVisualStyleStorage.mExpandedSpinners);
		if (mUIVisualStyleStorage.mExpandedSpinners)
		{
			for (Int32 i = 0; i < style_spinners.Count; i++)
			{
				CVisualStyleSpinner style_base = style_spinners[i];
				style_base.mExpandedInfo = XEditorInspector.DrawGroupFoldout(1, style_base.Name, ref style_base.mExpandedInfo,
					() =>
					{
						EditorGUI.indentLevel++;
						DrawStyleSpinner(style_base);
						EditorGUI.indentLevel--;
					},
					() =>
					{
						style_spinners.MoveElementUp(i);
					},
					() =>
					{
						style_spinners.MoveElementDown(i);
					},
					() =>
					{
						style_spinners.Insert(i, style_spinners[i].CloneSpinner());
						EditorUtility.SetDirty(mUIVisualStyleStorage);
					},
					() =>
					{
						style_spinners.RemoveAt(i);
						EditorUtility.SetDirty(mUIVisualStyleStorage);
					});
			}

			GUILayout.Space(4.0f);
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Space(XInspectorViewParams.OFFSET_INDENT);
				if (GUILayout.Button("Add"))
				{
					style_spinners.Add(new CVisualStyleSpinner());
					EditorUtility.SetDirty(mUIVisualStyleStorage);
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		if (EditorGUI.EndChangeCheck())
		{
			EditorUtility.SetDirty(mUIVisualStyleStorage);
			LotusGraphics2DVisualStyleService.UpdateVisualStyles();
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование визуального стиля текста
	/// </summary>
	/// <param name="style_text">Визуальный стиль текста</param>
	/// <param name="draw_name">Рисование имени визуального стиля</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawStyleText(CVisualStyleText style_text, Boolean draw_name = true)
	{
		if (draw_name)
		{
			GUILayout.Space(2.0f);
			style_text.Name = XEditorInspector.PropertyString("Name", style_text.Name);
		}

		GUILayout.Space(2.0f);
		style_text.FontFamily = XEditorInspector.PropertyResource("FontFamily", style_text.FontFamily);

		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		{
			style_text.TextColor = XEditorInspector.PropertyColor("TextColor", style_text.TextColor);
			if (GUILayout.Button(mContentDefaultValue, EditorStyles.miniButtonRight, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
			{
				style_text.TextColor = mUIVisualStyleStorage.CaptionColor;
			}
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		{
			style_text.TextColorActive = XEditorInspector.PropertyColor("TextColorActive", style_text.TextColorActive);
			if (GUILayout.Button(mContentDefaultValue, EditorStyles.miniButtonRight, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
			{
				style_text.TextColorActive = mUIVisualStyleStorage.CaptionActiveColor;
			}
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(2.0f);
		style_text.FontSize = XEditorInspector.PropertyIntSlider("FontSize", style_text.FontSize, 10, 40);

		GUILayout.Space(4.0f);
		style_text.FontStyle = (FontStyle)XEditorInspector.PropertyEnum("FontStyle", style_text.FontStyle);

		GUILayout.Space(4.0f);
		style_text.TextAnchor = (TextAnchor)XEditorInspector.PropertyEnum("TextAnchor", style_text.TextAnchor);

		GUILayout.Space(2.0f);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование базового визуального стиля
	/// </summary>
	/// <param name="style_base">Базовый визуальный стиль</param>
	/// <param name="draw_name">Рисование имени визуального стиля</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawStyleBase(CVisualStyleBase style_base, Boolean draw_name = true)
	{
		if (draw_name)
		{
			GUILayout.Space(2.0f);
			style_base.Name = XEditorInspector.PropertyString("Name", style_base.Name);
		}

		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		style_base.BackgroundImage = XEditorInspector.PropertyResource("Background", style_base.BackgroundImage);
		style_base.BackgroundColor = EditorGUILayout.ColorField(style_base.BackgroundColor, GUILayout.Width(60));
		if (GUILayout.Button(mContentDefaultValue, EditorStyles.miniButtonRight, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
		{
			style_base.BackgroundColor = Color.white;
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(2.0f);
		style_base.BackgroundImagePressed = XEditorInspector.PropertyResource("Background Pressed", style_base.BackgroundImagePressed);

		GUILayout.Space(2.0f);
		style_base.BackgroundImageSelected = XEditorInspector.PropertyResource("Background Selected", style_base.BackgroundImageSelected);

		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		{
			style_base.BackgroundColorActive = XEditorInspector.PropertyColor("Background Active", style_base.BackgroundColorActive);
			if (GUILayout.Button(mContentDefaultValue, EditorStyles.miniButtonRight, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
			{
				style_base.BackgroundColorActive = mUIVisualStyleStorage.BackgroundActiveColor;
			}
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		{
			style_base.IconImage = XEditorInspector.PropertyResource("Icon", style_base.IconImage);
			style_base.IconColor = EditorGUILayout.ColorField(style_base.IconColor, GUILayout.Width(60));
			if (GUILayout.Button(mContentDefaultValue, EditorStyles.miniButtonRight, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
			{
				style_base.IconColor = Color.white;
			}
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		{
			style_base.IconImageActive = XEditorInspector.PropertyResource("Icon Active", style_base.IconImageActive);
			style_base.IconColorActive = EditorGUILayout.ColorField(style_base.IconColorActive, GUILayout.Width(60));
			if (GUILayout.Button(mContentDefaultValue, EditorStyles.miniButtonRight, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
			{
				style_base.IconColorActive = mUIVisualStyleStorage.IconActiveColor;
			}
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(2.0f);
		style_base.FontFamily = XEditorInspector.PropertyResource("FontFamily", style_base.FontFamily);

		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		{
			style_base.TextColor = XEditorInspector.PropertyColor("TextColor", style_base.TextColor);
			if (GUILayout.Button(mContentDefaultValue, EditorStyles.miniButtonRight, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
			{
				style_base.TextColor = mUIVisualStyleStorage.CaptionColor;
			}
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		{
			style_base.TextColorActive = XEditorInspector.PropertyColor("TextColorActive", style_base.TextColorActive);
			if (GUILayout.Button(mContentDefaultValue, EditorStyles.miniButtonRight, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
			{
				style_base.TextColorActive = mUIVisualStyleStorage.CaptionActiveColor;
			}
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(2.0f);
		style_base.FontSize = XEditorInspector.PropertyIntSlider("FontSize", style_base.FontSize, 10, 40);

		GUILayout.Space(4.0f);
		style_base.FontStyle = (FontStyle)XEditorInspector.PropertyEnum("FontStyle", style_base.FontStyle);

		GUILayout.Space(4.0f);
		style_base.TextAnchor = (TextAnchor)XEditorInspector.PropertyEnum("TextAnchor", style_base.TextAnchor);

		GUILayout.Space(2.0f);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование базового визуального стиля для переключателя
	/// </summary>
	/// <param name="style_checkbox">Базовый визуальный стиль</param>
	/// <param name="draw_name">Рисование имени визуального стиля</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawStyleCheckBox(CVisualStyleBase style_checkbox, Boolean draw_name = true)
	{
		if (draw_name)
		{
			GUILayout.Space(2.0f);
			style_checkbox.Name = XEditorInspector.PropertyString("Name", style_checkbox.Name);
		}

		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		{
			style_checkbox.BackgroundImage = XEditorInspector.PropertyResource("Background", style_checkbox.BackgroundImage);
			style_checkbox.BackgroundColor = EditorGUILayout.ColorField(style_checkbox.BackgroundColor, GUILayout.Width(60));
			if (GUILayout.Button(mContentDefaultValue, EditorStyles.miniButtonRight, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
			{
				style_checkbox.BackgroundColor = Color.white;
			}
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(2.0f);
		style_checkbox.BackgroundImagePressed = XEditorInspector.PropertyResource("Background Pressed", style_checkbox.BackgroundImagePressed);

		GUILayout.Space(2.0f);
		style_checkbox.BackgroundImageSelected = XEditorInspector.PropertyResource("Background Selected", style_checkbox.BackgroundImageSelected);

		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		{
			style_checkbox.BackgroundColorActive = XEditorInspector.PropertyColor("Background Active", style_checkbox.BackgroundColorActive);
			if (GUILayout.Button(mContentDefaultValue, EditorStyles.miniButtonRight, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
			{
				style_checkbox.BackgroundColorActive = mUIVisualStyleStorage.BackgroundActiveColor;
			}
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		{
			style_checkbox.IconImage = XEditorInspector.PropertyResource("Checkmark Base", style_checkbox.IconImage);
			style_checkbox.IconColor = EditorGUILayout.ColorField(style_checkbox.IconColor, GUILayout.Width(60));
			if (GUILayout.Button(mContentDefaultValue, EditorStyles.miniButtonRight, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
			{
				style_checkbox.IconColor = Color.white;
			}
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		{
			style_checkbox.IconImageActive = XEditorInspector.PropertyResource("Checkmark Active", style_checkbox.IconImageActive);
			style_checkbox.IconColorActive = EditorGUILayout.ColorField(style_checkbox.IconColorActive, GUILayout.Width(60));
			if (GUILayout.Button(mContentDefaultValue, EditorStyles.miniButtonRight, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
			{
				style_checkbox.IconColorActive = mUIVisualStyleStorage.IconActiveColor;
			}
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(2.0f);
		style_checkbox.FontFamily = XEditorInspector.PropertyResource("FontFamily", style_checkbox.FontFamily);

		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		{
			style_checkbox.TextColor = XEditorInspector.PropertyColor("TextColor", style_checkbox.TextColor);
			if (GUILayout.Button(mContentDefaultValue, EditorStyles.miniButtonRight, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
			{
				style_checkbox.TextColor = mUIVisualStyleStorage.CaptionColor;
			}
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		{
			style_checkbox.TextColorActive = XEditorInspector.PropertyColor("TextColorActive", style_checkbox.TextColorActive);
			if (GUILayout.Button(mContentDefaultValue, EditorStyles.miniButtonRight, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
			{ 
				style_checkbox.TextColorActive = mUIVisualStyleStorage.CaptionActiveColor;
			}
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(2.0f);
		style_checkbox.FontSize = XEditorInspector.PropertyIntSlider("FontSize", style_checkbox.FontSize, 10, 40);

		GUILayout.Space(4.0f);
		style_checkbox.FontStyle = (FontStyle)XEditorInspector.PropertyEnum("FontStyle", style_checkbox.FontStyle);

		GUILayout.Space(4.0f);
		style_checkbox.TextAnchor = (TextAnchor)XEditorInspector.PropertyEnum("TextAnchor", style_checkbox.TextAnchor);

	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование визуального стиля для прогресса/скролла
	/// </summary>
	/// <param name="style_scroll">Визуальный стиль для прогресса/скролла</param>
	/// <param name="draw_name">Рисование имени визуального стиля</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawStyleScroll(CVisualStyleScroll style_scroll, Boolean draw_name = true)
	{
		if (draw_name)
		{
			GUILayout.Space(2.0f);
			style_scroll.Name = XEditorInspector.PropertyString("Name", style_scroll.Name);
		}

		GUILayout.Space(2.0f);
		style_scroll.FillBackgroundImage = XEditorInspector.PropertyResource("FillBackground", style_scroll.FillBackgroundImage);

		GUILayout.Space(2.0f);
		style_scroll.FillImage = XEditorInspector.PropertyResource("Fill", style_scroll.FillImage);

		GUILayout.Space(2.0f);
		style_scroll.HandleImage = XEditorInspector.PropertyResource("Handle", style_scroll.HandleImage);

		GUILayout.Space(2.0f);
		style_scroll.HandleImagePressed = XEditorInspector.PropertyResource("Handle Pressed", style_scroll.HandleImagePressed);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование визуального стиля для заголовочного элемента
	/// </summary>
	/// <param name="style_header">Визуальный стиль для заголовочного элемента</param>
	/// <param name="draw_name">Рисование имени визуального стиля</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawStyleHeader(CVisualStyleHeader style_header, Boolean draw_name = true)
	{
		if (draw_name)
		{
			GUILayout.Space(2.0f);
			style_header.Name = XEditorInspector.PropertyString("Name", style_header.Name);
		}

		GUILayout.Space(2.0f);
		style_header.ContentImage = XEditorInspector.PropertyResource("Content", style_header.ContentImage);

		GUILayout.Space(2.0f);
		style_header.HeaderImage = XEditorInspector.PropertyResource("Header", style_header.HeaderImage);

		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		{
			style_header.HeaderColor = XEditorInspector.PropertyColor("HeaderColor", style_header.HeaderColor);
			if (GUILayout.Button(mContentDefaultValue, EditorStyles.miniButtonRight, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
			{
				style_header.HeaderColor = Color.white;
			}
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		{
			style_header.HeaderColorActive = XEditorInspector.PropertyColor("HeaderColorActive", style_header.HeaderColorActive);
			if (GUILayout.Button(mContentDefaultValue, EditorStyles.miniButtonRight, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
			{
				style_header.HeaderColorActive = mUIVisualStyleStorage.HeaderActiveColor;
			}
		}
		EditorGUILayout.EndHorizontal();
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование визуального стиля для счетчика
	/// </summary>
	/// <param name="style_spinner">Визуальный стиль счетчика</param>
	/// <param name="draw_name">Рисование имени визуального стиля</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawStyleSpinner(CVisualStyleSpinner style_spinner, Boolean draw_name = true)
	{
		if (draw_name)
		{
			GUILayout.Space(2.0f);
			style_spinner.Name = XEditorInspector.PropertyString("Name", style_spinner.Name);
		}

		GUILayout.Space(2.0f);
		style_spinner.ButtonUpImage = XEditorInspector.PropertyResource("ButtonUp", style_spinner.ButtonUpImage);

		GUILayout.Space(2.0f);
		style_spinner.ButtonUpImageActive = XEditorInspector.PropertyResource("ButtonUp Active", style_spinner.ButtonUpImageActive);

		GUILayout.Space(2.0f);
		style_spinner.ButtonDownImage = XEditorInspector.PropertyResource("ButtonDown", style_spinner.ButtonDownImage);

		GUILayout.Space(2.0f);
		style_spinner.ButtonDownImageActive = XEditorInspector.PropertyResource("ButtonDown Active", style_spinner.ButtonDownImageActive);

		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		{
			style_spinner.ButtonColor = XEditorInspector.PropertyColor("ButtonColor", style_spinner.ButtonColor);
			if (GUILayout.Button(mContentDefaultValue, EditorStyles.miniButtonRight, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
			{
				style_spinner.ButtonColor = Color.white;
			}
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(2.0f);
		EditorGUILayout.BeginHorizontal();
		{
			style_spinner.ButtonColorActive = XEditorInspector.PropertyColor("HeaderColorActive", style_spinner.ButtonColorActive);
			if (GUILayout.Button(mContentDefaultValue, EditorStyles.miniButtonRight, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
			{
				style_spinner.ButtonColorActive = mUIVisualStyleStorage.HeaderActiveColor;
			}
		}
		EditorGUILayout.EndHorizontal();
	}
	#endregion
}
//=====================================================================================================================