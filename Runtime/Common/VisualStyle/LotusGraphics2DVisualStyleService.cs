//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Общий модуль 2D графики
// Подраздел: Визуальные стили
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGraphics2DVisualStyleService.cs
*		Центральный сервис для управления визуальными стилями и скинами.
*		Реализация центрального сервиса для управления, поиска и получения визуального стиля от текущего скина а также
*	управления скинами.
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
		/// Центральный сервис для управления визуальными стилями и скинами
		/// </summary>
		/// <remarks>
		/// Реализация центрального сервиса для управления, поиска и получения визуального стиля от текущего скина
		/// а также управления скинами
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class LotusGraphics2DVisualStyleService : ScriptableObject
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Имя файла сервиса для управления визуальными стилями и скинами
			/// </summary>
			public const String VISUAL_STYLE_SERVICE_NAME = "VisualStyleService";

			/// <summary>
			/// Путь файла сервиса для управления визуальными стилями и скинами
			/// </summary>
			public const String VISUAL_STYLE_SERVICE_PATH = XGraphics2DEditorSettings.SourcePath + "Resources/";
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			// Глобальный экземпляр
			internal static LotusGraphics2DVisualStyleService mInstance;

			/// <summary>
			/// Глобальное событие для информирование о смене скина
			/// </summary>
			public static event Action OnSkinChanged = delegate { };
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ СВОЙСТВА ======================================
			/// <summary>
			/// Глобальный экземпляр для управления визуальными стилями и скинами
			/// </summary>
			public static LotusGraphics2DVisualStyleService Instance
			{
				get
				{
					if (mInstance == null)
					{
						mInstance = Resources.Load<LotusGraphics2DVisualStyleService>(VISUAL_STYLE_SERVICE_NAME);
					}

					if (mInstance == null)
					{
#if UNITY_EDITOR
						// Создаем сервис
						mInstance = ScriptableObject.CreateInstance<LotusGraphics2DVisualStyleService>();
						mInstance.Create();

						// Создаем ресурс для хранения сервиса
						String path = VISUAL_STYLE_SERVICE_PATH + VISUAL_STYLE_SERVICE_NAME + ".asset";
						if (!Directory.Exists(VISUAL_STYLE_SERVICE_PATH))
						{
							Directory.CreateDirectory(VISUAL_STYLE_SERVICE_PATH);
						}
						UnityEditor.AssetDatabase.CreateAsset(mInstance, path);

						// Обновляем в редакторе
						UnityEditor.AssetDatabase.Refresh(UnityEditor.ImportAssetOptions.Default);
						UnityEditor.EditorUtility.DisplayDialog("Visual Style service service successfully created", "Path\n" + path, "OK");
#endif
					}

					return mInstance;
				}
			}

			/// <summary>
			/// Скин по умолчанию
			/// </summary>
			public static LotusGraphics2DVisualStyleStorage DefaultSkin
			{
				get
				{
					if(Instance.mSkins[0] == null)
					{
						Instance.mSkins[0] = LotusGraphics2DVisualStyleStorage.Default;
					}

					return Instance.mSkins[0];
				}
			}

			/// <summary>
			/// Текущий скин визуальных стилей
			/// </summary>
			public static LotusGraphics2DVisualStyleStorage CurrentSkin
			{
				get
				{
					if(Instance.mCurrentSkin == null)
					{
						Instance.mCurrentSkin = DefaultSkin;
					}
					return Instance.mCurrentSkin;
				}
				set
				{
					Instance.mCurrentSkin = value;
				}
			}

			/// <summary>
			/// Спрайт для фоновой панели при модальном режиме
			/// </summary>
			public static Sprite SpriteModalPanel
			{
				get { return Instance.mSpriteModalPanel; }
				set { Instance.mSpriteModalPanel = value; }
			}

			/// <summary>
			/// Материал визуализации недоступного(отключенного) элемента для компонента изображения
			/// </summary>
			public static Material MaterialDisableImage
			{
				get { return Instance.mMaterialDisableImage; }
			}

			/// <summary>
			/// Материал визуализации недоступного(отключенного) элемента для компонента текста
			/// </summary>
			public static Material MaterialDisableText
			{
				get { return Instance.mMaterialDisableText; }
			}

			/// <summary>
			/// Полупрозрачное изображение для маски
			/// </summary>
			public static Sprite MaskOpacity02
			{
				get { return Instance.mMaskOpacity02; }
				set { Instance.mMaskOpacity02 = value; }
			}

			/// <summary>
			/// Полупрозрачное изображение для маски
			/// </summary>
			public static Sprite MaskOpacity05
			{
				get { return Instance.mMaskOpacity05; }
				set { Instance.mMaskOpacity05 = value; }
			}

			/// <summary>
			/// Полупрозрачное изображение для маски
			/// </summary>
			public static Sprite MaskOpacity10
			{
				get { return Instance.mMaskOpacity10; }
				set { Instance.mMaskOpacity10 = value; }
			}

			/// <summary>
			/// Полупрозрачное изображение для маски
			/// </summary>
			public static Sprite MaskOpacity20
			{
				get { return Instance.mMaskOpacity20; }
				set { Instance.mMaskOpacity20 = value; }
			}

			/// <summary>
			/// Полупрозрачное изображение для маски
			/// </summary>
			public static Sprite MaskOpacity50
			{
				get { return Instance.mMaskOpacity50; }
				set { Instance.mMaskOpacity50 = value; }
			}
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ МЕТОДЫ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление данных визуальных стилей текущего скина
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void UpdateVisualStyles()
			{
				// Обновляем визуальные стили
				//LotusElementUIDispatcher.UpdateElementsVisualStyle();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение списка базовых(основных) визуальных стилей соответствующего типа элемента
			/// </summary>
			/// <param name="element_type">Тип элемента</param>
			/// <returns>Список базовых(основных) визуальных стилей</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CVisualStyleBase[] GetBaseVisualStyles(TElementType element_type)
			{
				return CurrentSkin.GetBaseVisualStyles(element_type);
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
			public static CVisualStyle[] GetAdditionalVisualStyles(TElementType element_type)
			{
				return CurrentSkin.GetAdditionalVisualStyles(element_type);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение списка имен базовых(основных) визуальных стилей соответствующего типа элемента
			/// </summary>
			/// <param name="element_type">Тип элемента</param>
			/// <param name="add_first_empty">Добавить первым пустое имя стиля</param>
			/// <returns>Список имен стилей</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String[] GetListNamesBaseVisualStyles(TElementType element_type, Boolean add_first_empty)
			{
				return CurrentSkin.GetListNamesBaseVisualStyles(element_type, add_first_empty);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение списка имен дополнительных визуальных стилей соответствующего типа элемента
			/// </summary>
			/// <param name="element_type">Тип элемента</param>
			/// <param name="add_first_empty">Добавить первым пустое имя стиля</param>
			/// <returns>Список имен стилей</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String[] GetListNamesAdditionalVisualStyles(TElementType element_type, Boolean add_first_empty)
			{
				return CurrentSkin.GetListNamesAdditionalVisualStyles(element_type, add_first_empty);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение базового(основного) визуального стиля по индексу и типу элемента
			/// </summary>
			/// <param name="index">Индекс</param>
			/// <param name="element_type">Тип элемента</param>
			/// <returns>Визуальный стиль</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CVisualStyle GetBaseVisualStyle(Int32 index, TElementType element_type)
			{
				return CurrentSkin.GetBaseVisualStyle(index, element_type);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение дополнительного визуального стиля по индексу и типу элемента
			/// </summary>
			/// <param name="index">Индекс</param>
			/// <param name="element_type">Тип элемента</param>
			/// <returns>Визуальный стиль</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CVisualStyle GetAdditionalVisualStyle(Int32 index, TElementType element_type)
			{
				return CurrentSkin.GetAdditionalVisualStyle(index, element_type);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение имени базового(основного) визуального стиля по индексу и типу элемента
			/// </summary>
			/// <param name="index">Индекс</param>
			/// <param name="element_type">Тип элемента</param>
			/// <returns>Имя визуального стиля</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String GetNameBaseVisualStyle(Int32 index, TElementType element_type)
			{
				return CurrentSkin.GetNameBaseVisualStyle(index, element_type);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение имени дополнительного визуального стиля по индексу и типу элемента
			/// </summary>
			/// <param name="index">Индекс</param>
			/// <param name="element_type">Тип элемента</param>
			/// <returns>Имя визуального стиля</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String GetNameAdditionalVisualStyle(Int32 index, TElementType element_type)
			{
				return CurrentSkin.GetNameAdditionalVisualStyle(index, element_type);
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
			public static Int32 FindIndexBaseVisualStyle(String style_name, TElementType element_type, Boolean strict = false)
			{
				return CurrentSkin.FindIndexBaseVisualStyle(style_name, element_type, strict);
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
			public static Int32 FindIndexAdditionalVisualStyle(String style_name, TElementType element_type, Boolean strict = false)
			{
				return CurrentSkin.FindIndexAdditionalVisualStyle(style_name, element_type, strict);
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal List<LotusGraphics2DVisualStyleStorage> mSkins;
			[SerializeField]
			internal LotusGraphics2DVisualStyleStorage mCurrentSkin;
			[SerializeField]
			internal Sprite mSpriteModalPanel;
			[SerializeField]
			internal Material mMaterialDisableImage;
			[SerializeField]
			internal Material mMaterialDisableText;
			[SerializeField]
			internal Sprite mMaskOpacity02;
			[SerializeField]
			internal Sprite mMaskOpacity05;
			[SerializeField]
			internal Sprite mMaskOpacity10;
			[SerializeField]
			internal Sprite mMaskOpacity20;
			[SerializeField]
			internal Sprite mMaskOpacity50;

			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			internal Boolean mExpandedSkins;
			[SerializeField]
			internal Boolean mExpandedMaterial;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Включение сервиса
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnEnable()
			{
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичное создание сервиса для управления визуальными стилями и скинами
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Create()
			{
				if (mSkins == null)
				{
					mSkins = new List<LotusGraphics2DVisualStyleStorage>();
					mSkins.Add(LotusGraphics2DVisualStyleStorage.Default);
					mCurrentSkin = LotusGraphics2DVisualStyleStorage.Default;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная безопасная инициализация несериализуемых данных
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Init()
			{
				if(mCurrentSkin == null)
				{
					mCurrentSkin = DefaultSkin;
				}
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
			/// Добавление ссылки для хранения скина
			/// </summary>
			/// <remarks>
			/// Соответственно нужно создать хранилище визуальных стилей и присоединить его
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void AddSkin()
			{
				mSkins.Add(null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление ссылки для хранения скина
			/// </summary>
			/// <remarks>
			/// Соответственно хранилище визуальных стилей можно/нужно удалить отдельно
			/// </remarks>
			/// <param name="index">Индекс удаляемой ссылки</param>
			//---------------------------------------------------------------------------------------------------------
			public void RemoveSkin(Int32 index)
			{
				mSkins.RemoveAt(index);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение текущего скина
			/// </summary>
			/// <param name="index">Индекс нового текущего стиля</param>
			//---------------------------------------------------------------------------------------------------------
			public void ChangeSkin(Int32 index)
			{
				CurrentSkin = mSkins[index];

				OnSkinChanged();

				// Обновляем визуальные стили
				//LotusElementUIDispatcher.UpdateElementsVisualStyle();
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================