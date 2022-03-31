//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Общий модуль 2D графики
// Подраздел: Визуальные стили
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGraphics2DVisualStyleStorage.cs
*		Хранилище (ресурс) визуальных стилей.
*		Реализация хранилища(скина) (ресурса Unity) для хранения и доступа ко всем визуальным стилям элементов, а также
*	а также общих графических параметров.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DCommonVisualStyle
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Хранилище (ресурс) визуальных стилей
		/// </summary>
		/// <remarks>
		/// Реализация хранилища (скина) (ресурса Unity) для хранения и доступа ко всем визуальным стилям элементов, 
		/// а также общих графических параметров
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class LotusGraphics2DVisualStyleStorage : ScriptableObject
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Имя файла скина с визуальными стилями по умолчанию
			/// </summary>
			public const String VISUAL_STYLE_STORAGE_DEFAULT_NAME = "VisualStyleStorageDefault";

			/// <summary>
			/// Путь файлов для скинов с визуальными стилями
			/// </summary>
			public const String VISUAL_STYLE_STORAGE_PATH = XGraphics2DEditorSettings.SourcePath + "Resources/";
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			// Глобальный экземпляр скина с визуальными стилями по умолчанию
			internal static LotusGraphics2DVisualStyleStorage mDefault;
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ СВОЙСТВА ======================================
			/// <summary>
			/// Глобальный экземпляр скина с визуальными стилями по умолчанию
			/// </summary>
			public static LotusGraphics2DVisualStyleStorage Default
			{
				get
				{
					if (mDefault == null)
					{
						mDefault = Resources.Load<LotusGraphics2DVisualStyleStorage>(VISUAL_STYLE_STORAGE_DEFAULT_NAME);
					}

					if (mDefault == null)
					{
#if UNITY_EDITOR
						// Создаем хранилище
						mDefault = ScriptableObject.CreateInstance<LotusGraphics2DVisualStyleStorage>();
						mDefault.Create();

						// Создаем ресурс для хранения хранилища
						String path = VISUAL_STYLE_STORAGE_PATH + VISUAL_STYLE_STORAGE_DEFAULT_NAME + ".asset";
						if (!Directory.Exists(VISUAL_STYLE_STORAGE_PATH))
						{
							Directory.CreateDirectory(VISUAL_STYLE_STORAGE_PATH);
						}
						UnityEditor.AssetDatabase.CreateAsset(mDefault, path);

						// Обновляем в редакторе
						UnityEditor.AssetDatabase.Refresh(UnityEditor.ImportAssetOptions.Default);
						UnityEditor.EditorUtility.DisplayDialog("Visual Style storage default successfully created", "Path\n" + path, "OK");
#endif
					}

					return mDefault;
				}
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			[SerializeField]
			internal Color mCaptionColor;
			[SerializeField]
			internal Color mCaptionActiveColor;
			[SerializeField]
			internal Color mValueColor;
			[SerializeField]
			internal Color mValueActiveColor;
			[SerializeField]
			internal Color mBackgroundActiveColor;
			[SerializeField]
			internal Color mIconActiveColor;
			[SerializeField]
			internal Color mHeaderActiveColor;
			[SerializeField]
			internal List<CVisualStyleBase> mBaseStyles;
			[SerializeField]
			internal List<CVisualStyleBase> mCheckBoxStyles;
			[SerializeField]
			internal List<CVisualStyleBase> mJoystickStyles;
			[SerializeField]
			internal List<CVisualStyleScroll> mScrollStyles;
			[SerializeField]
			internal List<CVisualStyleHeader> mHeaderStyles;
			[SerializeField]
			internal List<CVisualStyleSpinner> mSpinnerStyles;

			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			internal Boolean mExpandedInfo;
			[SerializeField]
			internal Boolean mExpandedBase;
			[SerializeField]
			internal Boolean mExpandedCheckBox;
			[SerializeField]
			internal Boolean mExpandedJoystick;
			[SerializeField]
			internal Boolean mExpandedScrolls;
			[SerializeField]
			internal Boolean mExpandedHeaders;
			[SerializeField]
			internal Boolean mExpandedSpinners;
			[SerializeField]
			internal Boolean mExpandedSerialized;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Цвет текста элементов надписи по умолчанию
			/// </summary>
			public Color CaptionColor
			{
				get { return mCaptionColor; }
				set { mCaptionColor = value; }
			}

			/// <summary>
			/// Цвет активированного текста элементов надписи по умолчанию
			/// </summary>
			public Color CaptionActiveColor
			{
				get { return mCaptionActiveColor; }
				set { mCaptionActiveColor = value; }
			}

			/// <summary>
			/// Цвет текста элементов значения по умолчанию
			/// </summary>
			public Color ValueColor
			{
				get { return mValueColor; }
				set { mValueColor = value; }
			}

			/// <summary>
			/// Цвет активированного текста элементов значения по умолчанию
			/// </summary>
			public Color ValueActiveColor
			{
				get { return mValueActiveColor; }
				set { mValueActiveColor = value; }
			}

			/// <summary>
			/// Цвет активированного фонового изображения элементов по умолчанию
			/// </summary>
			public Color BackgroundActiveColor
			{
				get { return mBackgroundActiveColor; }
				set { mBackgroundActiveColor = value; }
			}

			/// <summary>
			/// Цвет активированного изображения иконки элементов по умолчанию
			/// </summary>
			public Color IconActiveColor
			{
				get { return mIconActiveColor; }
				set { mIconActiveColor = value; }
			}

			/// <summary>
			/// Цвет активированного изображения заголовочного элемента по умолчанию
			/// </summary>
			public Color HeaderActiveColor
			{
				get { return mHeaderActiveColor; }
				set { mHeaderActiveColor = value; }
			}

			/// <summary>
			/// Список базовых визуальных стилей
			/// </summary>
			public List<CVisualStyleBase> BaseStyles
			{
				get { return mBaseStyles; }
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Включение хранилища
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnEnable()
			{
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичное создание хранилища визуальных стилей
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Create()
			{
				if (mBaseStyles == null) mBaseStyles = new List<CVisualStyleBase>();
				if (mCheckBoxStyles == null) mCheckBoxStyles = new List<CVisualStyleBase>();
				if (mJoystickStyles == null) mJoystickStyles = new List<CVisualStyleBase>();
				if (mScrollStyles == null) mScrollStyles = new List<CVisualStyleScroll>();
				if (mHeaderStyles == null) mHeaderStyles = new List<CVisualStyleHeader>();
				if (mSpinnerStyles == null) mSpinnerStyles = new List<CVisualStyleSpinner>();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная безопасная инициализация несериализуемых данных
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Init()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Принудительный сброс записанных данных на диск
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Flush()
			{
#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(this);
#endif
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Очистка всех визуальных стилей
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Clear()
			{
				mBaseStyles.Clear();
				mCheckBoxStyles.Clear();
				mJoystickStyles.Clear();
				mScrollStyles.Clear();
				mHeaderStyles.Clear();
				mSpinnerStyles.Clear();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение списка базовых(основных) визуальных стилей соответствующего типа элемента
			/// </summary>
			/// <param name="element_type">Тип элемента</param>
			/// <returns>Список базовых(основных) визуальных стилей</returns>
			//---------------------------------------------------------------------------------------------------------
			public CVisualStyleBase[] GetBaseVisualStyles(TElementType element_type)
			{
				CVisualStyleBase[] result = null;

				switch (element_type)
				{
					//
					// БАЗОВЫЕ ЭЛЕМЕНТЫ
					//

					// Базовые элементы
					case TElementType.Element:			result = mBaseStyles.ToArray(); break;
					case TElementType.Label:			result = mBaseStyles.ToArray(); break;
					case TElementType.LabelValue:		result = mBaseStyles.ToArray(); break;
					case TElementType.LabelPresent:		result = mBaseStyles.ToArray(); break;
					
					// Заголовочные элементы
					case TElementType.PanelHeader:		result = mBaseStyles.ToArray(); break;
					case TElementType.PanelSpoiler:		result = mBaseStyles.ToArray(); break;

					// Элементы индикации
					case TElementType.ProgressBar:		result = mBaseStyles.ToArray(); break;
					case TElementType.Raiting:			result = mBaseStyles.ToArray(); break;

					// Оконные элементы
					case TElementType.WindowBase:		result = mBaseStyles.ToArray(); break;
					case TElementType.WindowDialog:		result = mBaseStyles.ToArray(); break;
					case TElementType.WindowWaitProcess: result = mBaseStyles.ToArray(); break;

					// Динамические элементы
					
					//
					// ОБЩИЕ УПРАВЛЯЮЩИЕ ЭЛЕМЕНТЫ
					//

					// Стандартные элементы
					case TElementType.Button:			result = mBaseStyles.ToArray(); break;
					case TElementType.CheckBox:			result = mCheckBoxStyles.ToArray(); break;
					case TElementType.ScrollBar:		result = mBaseStyles.ToArray(); break;
					case TElementType.ScrollView:		result = mBaseStyles.ToArray(); break;
					case TElementType.Slider:			result = mBaseStyles.ToArray(); break;

					//Контейнерные элементы
					//case TElementType.Container:		result = mBaseStyles.ToArray(); break;
					//case TElementType.PanelContainer:	result = mBaseStyles.ToArray(); break;
					case TElementType.DropDownList:		result = mBaseStyles.ToArray(); break;
					case TElementType.RadialMenu:		result = mBaseStyles.ToArray(); break;
					case TElementType.ListView:			result = mBaseStyles.ToArray(); break;

					// Дополнительные элементы
					case TElementType.Joystick:			result = mJoystickStyles.ToArray(); break;
					case TElementType.Spinner:			result = mBaseStyles.ToArray(); break;
					case TElementType.TabControl:		result = mBaseStyles.ToArray(); break;
					case TElementType.Accordion:		result = mBaseStyles.ToArray(); break;

					//
					// РАСШИРЕННЫЕ УПРАВЛЯЮЩИЕ ЭЛЕМЕНТЫ
					//

					// Иерархические элементы
					case TElementType.TreeView:			result = mBaseStyles.ToArray(); break;

					// Тайловые элементы
					case TElementType.Tile:				result = mBaseStyles.ToArray(); break;
					case TElementType.GridTile:			result = mBaseStyles.ToArray(); break;

					// Специализированные элементы
					case TElementType.ColorPicker:		result = mBaseStyles.ToArray(); break;
					case TElementType.SelectorLevel:	result = mBaseStyles.ToArray(); break;

					default:
						break;
				}

				return result;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение списка дополнительных визуальных стилей соответствующего типа элемента
			/// </summary>
			/// <remarks>
			/// Если элемент не имеет дополнительного стиля то возвращается его базовый визуальный стиль
			/// </remarks>
			/// <param name="element_type">Тип элемента</param>
			/// <returns>Список визуальных стилей</returns>
			//---------------------------------------------------------------------------------------------------------
			public CVisualStyle[] GetAdditionalVisualStyles(TElementType element_type)
			{
				CVisualStyle[] result = null;

				switch (element_type)
				{
					//
					// БАЗОВЫЕ ЭЛЕМЕНТЫ
					//

					// Базовые элементы
					case TElementType.Element:			result = mBaseStyles.ToArray(); break;
					case TElementType.Label:			result = mBaseStyles.ToArray(); break;
					case TElementType.LabelValue:		result = mBaseStyles.ToArray(); break;
					case TElementType.LabelPresent:		result = mBaseStyles.ToArray(); break;

					// Заголовочные элементы
					case TElementType.PanelHeader:		result = mHeaderStyles.ToArray(); break;
					case TElementType.PanelSpoiler:		result = mHeaderStyles.ToArray(); break;

					// Элементы индикации
					case TElementType.ProgressBar:		result = mScrollStyles.ToArray(); break;
					case TElementType.Raiting:			result = mBaseStyles.ToArray(); break;

					// Оконные элементы
					case TElementType.WindowBase:		result = mHeaderStyles.ToArray(); break;
					case TElementType.WindowDialog:		result = mHeaderStyles.ToArray(); break;
					case TElementType.WindowWaitProcess: result = mHeaderStyles.ToArray(); break;

					// Динамические элементы

					//
					// ОБЩИЕ УПРАВЛЯЮЩИЕ ЭЛЕМЕНТЫ
					//

					// Стандартные элементы
					case TElementType.Button:			result = mBaseStyles.ToArray(); break;
					case TElementType.CheckBox:			result = mCheckBoxStyles.ToArray(); break;
					case TElementType.ScrollBar:		result = mScrollStyles.ToArray(); break;
					case TElementType.ScrollView:		result = mBaseStyles.ToArray(); break;
					case TElementType.Slider:			result = mScrollStyles.ToArray(); break;

					//Контейнерные элементы
					//case TElementType.Container:		result = mBaseStyles.ToArray(); break;
					//case TElementType.PanelContainer:	result = mBaseStyles.ToArray(); break;
					case TElementType.DropDownList:		result = mBaseStyles.ToArray(); break;
					case TElementType.RadialMenu:		result = mBaseStyles.ToArray(); break;
					case TElementType.ListView:			result = mBaseStyles.ToArray(); break;

					// Дополнительные элементы
					case TElementType.Joystick:			result = mJoystickStyles.ToArray(); break;
					case TElementType.Spinner:			result = mSpinnerStyles.ToArray(); break;
					case TElementType.TabControl:		result = mHeaderStyles.ToArray(); break;
					case TElementType.Accordion:		result = mHeaderStyles.ToArray(); break;

					//
					// РАСШИРЕННЫЕ УПРАВЛЯЮЩИЕ ЭЛЕМЕНТЫ
					//

					// Иерархические элементы
					case TElementType.TreeView:			result = mBaseStyles.ToArray(); break;

					// Тайловые элементы
					case TElementType.Tile:				result = mBaseStyles.ToArray(); break;
					case TElementType.GridTile:			result = mBaseStyles.ToArray(); break;

					// Специализированные элементы
					case TElementType.ColorPicker:		result = mBaseStyles.ToArray(); break;
					case TElementType.SelectorLevel:		result = mBaseStyles.ToArray(); break;

					default:
						break;
				}

				return result;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение списка имен базовых(основных) визуальных стилей соответствующего типа элемента
			/// </summary>
			/// <param name="element_type">Тип элемента</param>
			/// <param name="add_first_empty">Добавить первым пустое имя стиля</param>
			/// <returns>Список имен стилей</returns>
			//---------------------------------------------------------------------------------------------------------
			public String[] GetListNamesBaseVisualStyles(TElementType element_type, Boolean add_first_empty)
			{
				CVisualStyle[] styles = GetBaseVisualStyles(element_type);
				String[] names = null;
				if (styles != null)
				{
					if (add_first_empty)
					{
						names = new String[styles.Length + 1];
						names[0] = "None";

						for (Int32 i = 1; i < names.Length; i++)
						{
							names[i] = styles[i - 1].Name;
						}
					}
					else
					{
						names = new String[styles.Length];

						for (Int32 i = 0; i < styles.Length; i++)
						{

							names[i] = styles[i].Name;
						}
					}

					return names;
				}

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение списка имен дополнительных визуальных стилей соответствующего типа элемента
			/// </summary>
			/// <param name="element_type">Тип элемента</param>
			/// <param name="add_first_empty">Добавить первым пустое имя стиля</param>
			/// <returns>Список имен стилей</returns>
			//---------------------------------------------------------------------------------------------------------
			public String[] GetListNamesAdditionalVisualStyles(TElementType element_type, Boolean add_first_empty)
			{
				CVisualStyle[] styles = GetAdditionalVisualStyles(element_type);
				String[] names = null;
				if (styles != null)
				{
					if (add_first_empty)
					{
						names = new String[styles.Length + 1];
						names[0] = "None";

						for (Int32 i = 1; i < names.Length; i++)
						{
							names[i] = styles[i - 1].Name;
						}
					}
					else
					{
						names = new String[styles.Length];

						for (Int32 i = 0; i < styles.Length; i++)
						{

							names[i] = styles[i].Name;
						}
					}

					return names;
				}

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение базового(основного) визуального стиля по индексу и типу элемента
			/// </summary>
			/// <param name="index">Индекс</param>
			/// <param name="element_type">Тип элемента</param>
			/// <returns>Визуальный стиль</returns>
			//---------------------------------------------------------------------------------------------------------
			public CVisualStyle GetBaseVisualStyle(Int32 index, TElementType element_type)
			{
				CVisualStyle result = null;
				if (index >= 0)
				{
					CVisualStyle[] styles = GetBaseVisualStyles(element_type);
					if (styles != null)
					{
						result = styles[index];
					}
				}

				return result;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение дополнительного визуального стиля по индексу и типу элемента
			/// </summary>
			/// <param name="index">Индекс</param>
			/// <param name="element_type">Тип элемента</param>
			/// <returns>Визуальный стиль</returns>
			//---------------------------------------------------------------------------------------------------------
			public CVisualStyle GetAdditionalVisualStyle(Int32 index, TElementType element_type)
			{
				CVisualStyle result = null;
				if (index >= 0)
				{
					CVisualStyle[] styles = GetAdditionalVisualStyles(element_type);
					if (styles != null)
					{
						result = styles[index];
					}
				}

				return result;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение имени базового(основного) визуального стиля по индексу и типу элемента
			/// </summary>
			/// <param name="index">Индекс</param>
			/// <param name="element_type">Тип элемента</param>
			/// <returns>Имя визуального стиля</returns>
			//---------------------------------------------------------------------------------------------------------
			public String GetNameBaseVisualStyle(Int32 index, TElementType element_type)
			{
				CVisualStyle visual_style = GetBaseVisualStyle(index, element_type);
				if (visual_style != null)
				{
					return visual_style.Name;
				}

				return "None";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение имени дополнительного визуального стиля по индексу и типу элемента
			/// </summary>
			/// <param name="index">Индекс</param>
			/// <param name="element_type">Тип элемента</param>
			/// <returns>Имя визуального стиля</returns>
			//---------------------------------------------------------------------------------------------------------
			public String GetNameAdditionalVisualStyle(Int32 index, TElementType element_type)
			{
				CVisualStyle visual_style = GetAdditionalVisualStyle(index, element_type);
				if (visual_style != null)
				{
					return visual_style.Name;
				}

				return "None";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск индекса базового(основного) визуального стиля на основе имени
			/// </summary>
			/// <param name="style_name">Имя визуального стиля</param>
			/// <param name="element_type">Тип элемента</param>
			/// <param name="strict">Строгое соответствие или нет</param>
			/// <returns>Найденный индекс или 0</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 FindIndexBaseVisualStyle(String style_name, TElementType element_type, Boolean strict = false)
			{
				CVisualStyle[] styles = GetBaseVisualStyles(element_type);
				if (styles != null)
				{
					if (strict)
					{
						for (Int32 i = 0; i < styles.Length; i++)
						{
							if (styles[i].Name == style_name)
							{
								return i;
							}
						}
					}
					else
					{
						for (Int32 i = 0; i < styles.Length; i++)
						{
							if (styles[i].Name.IndexOf(style_name) > -1)
							{
								return i;
							}
						}
					}
				}

				if (style_name == "Default")
				{
					return 0;
				}

				return -1;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск индекса дополнительного визуального стиля на основе имени
			/// </summary>
			/// <param name="style_name">Имя визуального стиля</param>
			/// <param name="element_type">Тип элемента</param>
			/// <param name="strict">Строгое соответствие или нет</param>
			/// <returns>Найденный индекс или 0</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 FindIndexAdditionalVisualStyle(String style_name, TElementType element_type, Boolean strict = false)
			{
				CVisualStyle[] styles = GetAdditionalVisualStyles(element_type);
				if (styles != null)
				{
					if (strict)
					{
						for (Int32 i = 0; i < styles.Length; i++)
						{
							if (styles[i].Name == style_name)
							{
								return i;
							}
						}
					}
					else
					{
						for (Int32 i = 0; i < styles.Length; i++)
						{
							if (styles[i].Name.IndexOf(style_name) > -1)
							{
								return i;
							}
						}
					}
				}

				if (style_name == "Default")
				{
					return 0;
				}

				return -1;
			}
			#endregion

			#region ======================================= МЕТОДЫ СЕРИАЛИЗАЦИИ =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранения списка визуальных стилей в формат XML
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			public void SaveToXml(String file_name)
			{
#if UNITY_EDITOR
				XmlWriterSettings xws = new XmlWriterSettings();
				xws.Indent = true;

				String path = XEditorSettings.ASSETS_PATH + file_name + ".xml";
				XmlWriter writer = XmlWriter.Create(path, xws);
				writer.WriteStartElement("VisualStyles");

				writer.WriteColorToAttribute("CaptionColor", mCaptionColor);
				writer.WriteColorToAttribute("CaptionActiveColor", mCaptionActiveColor);
				writer.WriteColorToAttribute("ValueColor", mValueColor);
				writer.WriteColorToAttribute("ValueActiveColor", mValueActiveColor);
				writer.WriteColorToAttribute("BackgroundActiveColor", mBackgroundActiveColor);
				writer.WriteColorToAttribute("IconActiveColor", mIconActiveColor);
				writer.WriteColorToAttribute("HeaderActiveColor", mHeaderActiveColor);
				
				for (Int32 i = 0; i < mBaseStyles.Count; i++)
				{
					writer.WriteStartElement("Base");
					mBaseStyles[i].WriteToXml(writer);
					writer.WriteEndElement();
				}

				for (Int32 i = 0; i < mCheckBoxStyles.Count; i++)
				{
					writer.WriteStartElement("CheckBox");
					mCheckBoxStyles[i].WriteToXml(writer);
					writer.WriteEndElement();
				}

				for (Int32 i = 0; i < mJoystickStyles.Count; i++)
				{
					writer.WriteStartElement("Joystick");
					mJoystickStyles[i].WriteToXml(writer);
					writer.WriteEndElement();
				}

				for (Int32 i = 0; i < mScrollStyles.Count; i++)
				{
					writer.WriteStartElement("Scroll");
					mScrollStyles[i].WriteToXml(writer);
					writer.WriteEndElement();
				}

				for (Int32 i = 0; i < mHeaderStyles.Count; i++)
				{
					writer.WriteStartElement("Header");
					mHeaderStyles[i].WriteToXml(writer);
					writer.WriteEndElement();
				}

				for (Int32 i = 0; i < mSpinnerStyles.Count; i++)
				{
					writer.WriteStartElement("Spinner");
					mHeaderStyles[i].WriteToXml(writer);
					writer.WriteEndElement();
				}

				writer.WriteEndElement();
				writer.Close();

				// Обновляем в редакторе
				UnityEditor.AssetDatabase.Refresh(UnityEditor.ImportAssetOptions.Default);
				UnityEditor.EditorUtility.DisplayDialog(XFileDialog.FILE_SAVE_SUCCESSFULLY, "Path\n" + path, "OK");
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка списка визуальных стилей из формата XML
			/// </summary>
			/// <param name="file_asset">Текстовый ресурс - данные в формате XML</param>
			/// <param name="adding">Добавить или заменить данные</param>
			//---------------------------------------------------------------------------------------------------------
			public void LoadFromXml(TextAsset file_asset, Boolean adding)
			{
				// 1) Создаем читателя
				StringReader asset_stream = new StringReader(file_asset.text);
				XmlReader reader = XmlReader.Create(asset_stream);

				if (adding == false)
				{
					Clear();
				}
				
				// 2) Читаем данные
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						switch (reader.Name)
						{
							case "VisualStyles":
								{
									mCaptionColor = XUnityColor.DeserializeFromString(reader.GetAttribute("CaptionColor"));
									mCaptionActiveColor = XUnityColor.DeserializeFromString(reader.GetAttribute("CaptionActiveColor"));
									mValueColor = XUnityColor.DeserializeFromString(reader.GetAttribute("ValueColor"));
									mValueActiveColor = XUnityColor.DeserializeFromString(reader.GetAttribute("ValueActiveColor"));
									mBackgroundActiveColor = XUnityColor.DeserializeFromString(reader.GetAttribute("BackgroundActiveColor"));
									mIconActiveColor = XUnityColor.DeserializeFromString(reader.GetAttribute("IconActiveColor"));
									mHeaderActiveColor = XUnityColor.DeserializeFromString(reader.GetAttribute("HeaderActiveColor"));
								}
								break;
							case "Base":
								{
									CVisualStyleBase style_base = new CVisualStyleBase();
									style_base.ReadFromXml(reader);
									mBaseStyles.Add(style_base);
								}
								break;
							case "CheckBox":
								{
									CVisualStyleBase style_base = new CVisualStyleBase();
									style_base.ReadFromXml(reader);
									mCheckBoxStyles.Add(style_base);
								}
								break;
							case "Joystick":
								{
									CVisualStyleBase style_base = new CVisualStyleBase();
									style_base.ReadFromXml(reader);
									mJoystickStyles.Add(style_base);
								}
								break;
							case "Scroll":
								{
									CVisualStyleScroll style_scroll = new CVisualStyleScroll();
									style_scroll.ReadFromXml(reader);
									mScrollStyles.Add(style_scroll);
								}
								break;
							case "Header":
								{
									CVisualStyleHeader style_header = new CVisualStyleHeader();
									style_header.ReadFromXml(reader);
									mHeaderStyles.Add(style_header);
								}
								break;
							case "Spinner":
								{
									CVisualStyleSpinner style_spinner = new CVisualStyleSpinner();
									style_spinner.ReadFromXml(reader);
									mSpinnerStyles.Add(style_spinner);
								}
								break;
							default:
								break;
						}
					}
				}

				reader.Close();

#if UNITY_EDITOR
				UnityEditor.EditorUtility.DisplayDialog(XFileDialog.FILE_LOAD_SUCCESSFULLY, "Path\n" + file_asset.name, "OK");
#endif
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================