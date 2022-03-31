//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль работы со спрайтами
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusSpriteDispatcher.cs
*		Диспетчер размещения спрайтов в двухмерной пространстве.
*		Диспетчер обеспечивает размещение спрайтов в двухмерной пространстве согласно классической системе координат.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup Unity2DSprite МОДУЛЬ РАБОТЫ СО СПРАЙТАМИ
		//! Модуль работы со спрайтами унифицирует работу по размещению спрайтов в двухмерном пространстве
		//! \ingroup UnityGraphics2D
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Диспетчер размещения спрайтов в двухмерной пространстве
		/// </summary>
		/// <remarks>
		/// Диспетчер обеспечивает размещение спрайтов в двухмерной пространстве согласно классической системе координат
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[ExecuteInEditMode]
		[AddComponentMenu(XSpriteEditorSettings.MenuPath + "Sprite Dispatcher")]
		public class LotusSpriteDispatcher : LotusBaseGraphics2DDispatcher<LotusSpriteDispatcher>,
			ILotusBaseGraphics2DDispatcher
		{
			#region ======================================= СТАТИЧЕСКИЕ СВОЙСТВА ======================================
			//
			// ПАРАМЕТРЫ РАЗРАБОТКИ
			//
			/// <summary>
			/// Камера для отображения спрайтов
			/// </summary>
			/// <remarks>
			/// Должна быть в ортографическом режиме
			/// </remarks>
			public static Camera CameraSprite
			{
				get { return Instance.mCameraSprite; }
				set { Instance.mCameraSprite = value; }
			}

			/// <summary>
			/// Количество пикселей на единицу в мировых координатах
			/// </summary>
			/// <remarks>
			/// Значение по умолчанию - 100
			/// </remarks>
			public static Single CameraPixelsPerUnit
			{
				get { return Instance.mCameraPixelsPerUnit; }
				set { Instance.mCameraPixelsPerUnit = value; }
			}

			/// <summary>
			/// Статус масштабирование камеры
			/// </summary>
			public static Boolean IsCameraZooming
			{
				get { return Instance.mIsCameraZooming; }
				set { Instance.mIsCameraZooming = value; }
			}

			/// <summary>
			/// Увеличение камеры
			/// </summary>
			public static Single CameraZoom
			{
				get { return Instance.mCameraZoom; }
				set { Instance.mCameraZoom = value; }
			}

			/// <summary>
			/// Размер ортографической камеры по высоте
			/// </summary>
			public static Single CameraOrthoHeight
			{
				get { return Instance.mCameraOrthoHeight; }
			}

			/// <summary>
			/// Размер ортографической камеры по ширине
			/// </summary>
			public static Single CameraOrthoWidth
			{
				get { return Instance.mCameraOrthoWidth; }
			}

			/// <summary>
			/// Минимальная глубина расположения спрайта за котором он будет не виден
			/// </summary>
			public static Single MinPositionDepth
			{
				get { return Instance.mCameraSprite.transform.position.z + Instance.mCameraSprite.nearClipPlane + 100; }
			}

			/// <summary>
			/// Максимальная глубина расположения спрайта за котором он будет не виден
			/// </summary>
			public static Single MaxPositionDepth
			{
				get { return Instance.mCameraSprite.transform.position.z + Instance.mCameraSprite.farClipPlane; }
			}
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ МЕТОДЫ ========================================
			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение мировых координат трансформации из экранных координат
			/// </summary>
			/// <param name="left">Позиция по X в экранных координатах</param>
			/// <param name="top">Позиция по Y в экранных координатах</param>
			/// <param name="depth">Глубина расположения спрайта (позиция по Z)</param>
			/// <returns>Мировые координаты</returns>
			//-------------------------------------------------------------------------------------------------------------
			public static Vector3 GetWorldTransformFromWorldScreen(Single left, Single top, Single depth)
			{
				Single w = ScreenWidth;
				Single h = ScreenHeight;
				Single pos_x = CameraOrthoWidth * ((((left / w) * 2) - 1));
				Single pos_y = CameraOrthoHeight * ((((top / h) * 2) - 1));
				return (new Vector3(pos_x, -pos_y, depth));
			}

			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение экранных координат из мировых координат трансформации
			/// </summary>
			/// <param name="transform">Компонент трансформации</param>
			/// <returns>Экранные координаты</returns>
			//-------------------------------------------------------------------------------------------------------------
			public static Vector2 GetWorldScreenFromWorldTransform(Transform transform)
			{
				Single w = ScreenWidth;
				Single h = ScreenHeight;

				Single pos_x = transform.position.x;
				Single pos_y = -transform.position.y;

				Single left = (((pos_x / CameraOrthoWidth) + 1) / 2) * w;
				Single top = (((pos_y / CameraOrthoHeight) + 1) / 2) * h;

				return (new Vector2(left, top));
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Параметры разработки
			[SerializeField]
			internal Camera mCameraSprite;
			[SerializeField]
			internal Single mCameraPixelsPerUnit = 100;
			[SerializeField]
			internal Boolean mIsCameraZooming;
			[SerializeField]
			internal Single mCameraZoom = 1;
			[NonSerialized]
			internal Single mCameraOrthoHeight;
			[NonSerialized]
			internal Single mCameraOrthoWidth;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
			#endregion

			#region ======================================= МЕТОДЫ ILotusBaseGraphics2DDispatcher =====================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация диспетчера при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void ResetDispatcher()
			{
			}
#endif

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор диспетчера
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void ConstructorDispatcher()
			{
				if(mCameraSprite == null)
				{
					mCameraSprite = Camera.main;
				}

				if (mDesignScreenWidth == 0) mDesignScreenWidth = 1280;
				if (mDesignScreenHeight == 0) mDesignScreenHeight = 800;

				UpdateSpriteCamera();
			}

#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение размеров экрана
			/// </summary>
			/// <remarks>
			/// Метод вызывается одни раз в стартовом методе, и в режиме редактора каждый раз когда меняются размеры экрана
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			protected override void ChangeSizeScreenDispatcher()
			{
				UpdateSpriteCamera();
			}
#endif

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление диспетчера
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void UpdateDispatcher()
			{

			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление камеры
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void UpdateSpriteCamera()
			{
				Single h = ScreenHeight;
				if (mIsCameraZooming)
				{
					Camera.main.orthographicSize = h / 2f / CameraPixelsPerUnit / CameraZoom;
				}
				else
				{
					Camera.main.orthographicSize = h / 2f / CameraPixelsPerUnit;
				}

				mCameraOrthoHeight = mCameraSprite.orthographicSize;
				mCameraOrthoWidth = mCameraSprite.aspect * CameraOrthoHeight;
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С ЭЛЕМЕНТАМИ ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента и добавление его к основной канве в качестве дочернего
			/// </summary>
			/// <typeparam name="TElementUI">Тип элемента</typeparam>
			/// <param name="element_name">Имя элемента</param>
			/// <returns>Созданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TElementUI CreateElement<TElementUI>(String element_name) where TElementUI : LotusSpritePlaceable2D
			{
				GameObject go_element = new GameObject(element_name);
				//go_element.layer = XLayer.S;

				SpriteRenderer sprite_renderer = go_element.transform.EnsureComponent<SpriteRenderer>();
				sprite_renderer.sprite = XSprite.Default;

				TElementUI element = go_element.AddComponent<TElementUI>();

				// Добавляем на канву
				go_element.transform.SetParent(Instance.transform, false);

				return element;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента и добавление его к основной канве в качестве дочернего
			/// </summary>
			/// <typeparam name="TElementUI">Тип элемента</typeparam>
			/// <param name="element_name">Имя элемента</param>
			/// <param name="left">Позиция по X</param>
			/// <param name="top">Позиция по Y</param>
			/// <param name="width">Ширина</param>
			/// <param name="height">Высота</param>
			/// <returns>Созданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TElementUI CreateElement<TElementUI>(String element_name, Single left, Single top,
				Single width, Single height) where TElementUI : LotusSpritePlaceable2D
			{
				GameObject go_element = new GameObject(element_name);
				//go_element.layer = LayerMask.NameToLayer(LayerName);

				SpriteRenderer sprite_renderer = go_element.transform.EnsureComponent<SpriteRenderer>();
				sprite_renderer.sprite = XSprite.Default;

				TElementUI element = go_element.AddComponent<TElementUI>();

				// Добавляем на канву
				go_element.transform.SetParent(Instance.transform, false);

				element.Left = left;
				element.Top = top;
				element.Width = width;
				element.Height = height;

				return element;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================