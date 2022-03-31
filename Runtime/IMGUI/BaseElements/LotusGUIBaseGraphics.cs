﻿//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Базовые элементы интерфейса
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIBaseGraphics.cs
*		Специальные элементы для отображения графического содержимого.
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
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DImmedateGUIBase
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Изображение - элемент GUI предназначенный для отображения отдельной текстуры
		/// </summary>
		/// <remarks>
		/// Для рисования используется метод GUI.DrawTexture.
		/// Поддерживается фон элемента и различный режим масштабирования текстуры
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIImage : CGUIElement
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Texture2D mImage;
			[SerializeField]
			internal ScaleMode mScaleMode;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Текстура для рисования
			/// </summary>
			public Texture2D Image
			{
				get { return mImage; }
				set
				{
					mImage = value;
				}
			}

			/// <summary>
			/// Использовать фоновое изображение
			/// </summary>
			public ScaleMode ScaleMode
			{
				get { return mScaleMode; }
				set
				{
					mScaleMode = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIImage()
				: base()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIImage(String name)
				: base(name)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIImage(String name, Single x, Single y)
				: base(name, x, y)
			{

			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				GUI.enabled = IsEnabledElement;
				GUI.depth = mDepth;

				if (mUseBackground)
				{
					GUI.Label(mRectWorldScreenMain, "", mStyleMain);
				}

				if(mImage != null)
				GUI.DrawTexture(mRectWorldScreenMain, mImage, mScaleMode, true);
			}
			#endregion

			#region ======================================= МЕТОДЫ ПРЕОБРАЗОВАНИЯ =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение полного дубликата элемента
			/// </summary>
			/// <returns>Полный дубликат элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public override CGUIBaseElement Duplicate()
			{
				return MemberwiseClone() as CGUIImage;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование данных с другого элемента
			/// </summary>
			/// <param name="base_element">Источник данных</param>
			//---------------------------------------------------------------------------------------------------------
			public override void CopyFrom(CGUIBaseElement base_element)
			{
				base.CopyFrom(base_element);

				CGUIImage image = base_element as CGUIImage;
				if (image != null)
				{
					mImage = image.mImage;
					mUseBackground = image.mUseBackground;
					mScaleMode = image.mScaleMode;
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Спрайт - элемент GUI предназначенный для отображения отдельного спрайта
		/// </summary>
		/// <remarks>
		/// Для рисования используется метод GUI.DrawTextureWithTexCoords.
		/// Поддерживается фон элемента
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUISprite : CGUIElement
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Sprite mSprite;

			// Служебные данные
			[NonSerialized]
			internal Texture2D mSpriteTexture;
			[NonSerialized]
			internal Rect mSpriteTextureRectUV;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Спрайт
			/// </summary>
			public Sprite Sprite
			{
				get { return mSprite; }
				set
				{
					mSprite = value;
					if (mSprite != null)
					{
						mSpriteTexture = mSprite.texture;
						mSpriteTextureRectUV = mSprite.textureRect;
						mSpriteTextureRectUV.x /= mSprite.texture.width;
						mSpriteTextureRectUV.y /= mSprite.texture.height;
						mSpriteTextureRectUV.width /= mSprite.texture.width;
						mSpriteTextureRectUV.height /= mSprite.texture.height;
					}
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUISprite()
				: base()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUISprite(String name)
				: base(name)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUISprite(String name, Single x, Single y)
				: base(name, x, y)
			{

			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка/обновление параметров
			/// </summary>
			/// <remarks>
			/// Вызывается центральным диспетчером в момент добавления(регистрации) элемента
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public override void OnReset()
			{
				base.OnReset();

				if (mSprite != null)
				{
					mSpriteTexture = mSprite.texture;
					mSpriteTextureRectUV = mSprite.textureRect;
					mSpriteTextureRectUV.x /= mSprite.texture.width;
					mSpriteTextureRectUV.y /= mSprite.texture.height;
					mSpriteTextureRectUV.width /= mSprite.texture.width;
					mSpriteTextureRectUV.height /= mSprite.texture.height;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента GUI
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				GUI.enabled = IsEnabledElement;
				GUI.depth = mDepth;

				if (mUseBackground)
				{
					GUI.Label(mRectWorldScreenMain, "", mStyleMain);
				}

				if (mSpriteTexture != null)
					GUI.DrawTextureWithTexCoords(mRectWorldScreenMain, mSpriteTexture, mSpriteTextureRectUV, true);
			}
			#endregion

			#region ======================================= МЕТОДЫ ПРЕОБРАЗОВАНИЯ =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение полного дубликата элемента
			/// </summary>
			/// <returns>Полный дубликат элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public override CGUIBaseElement Duplicate()
			{
				return MemberwiseClone() as CGUISprite;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование данных с другого элемента
			/// </summary>
			/// <param name="base_element">Источник данных</param>
			//---------------------------------------------------------------------------------------------------------
			public override void CopyFrom(CGUIBaseElement base_element)
			{
				base.CopyFrom(base_element);

				CGUISprite sprite = base_element as CGUISprite;
				if (sprite != null)
				{
					mSprite = sprite.mSprite;
					mUseBackground = sprite.mUseBackground;
					mSpriteTexture = sprite.mSpriteTexture;
					mSpriteTextureRectUV = sprite.mSpriteTextureRectUV;
				}
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================