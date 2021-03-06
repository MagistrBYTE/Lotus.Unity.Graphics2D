//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Общий модуль 2D графики
// Подраздел: Визуальные эффекты
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGraphics2DVisualEffectSpriteRender.cs
*		Визуальный эффекты связанные со спрайтом.
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
		//! \addtogroup Unity2DCommonVisualEffect
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Визуальный эффекты связанные со спрайтом
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu("Lotus/Graphics2D/VisualEffect/Sprite Render")]
		public class LotusGraphics2DVisualEffectSpriteRender : LotusGraphics2DVisualEffectBase
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			[LotusDisplayName(nameof(OriginalSprite))]
			internal Sprite mOriginalSprite;
			[SerializeField]
			[LotusDisplayName(nameof(TargetSprite))]
			internal Sprite mTargetSprite;

			// Служебные данные
			[NonSerialized]
			internal SpriteRenderer mSpriteRenderer;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Оригинальный спрайт элемента
			/// </summary>
			public Sprite OriginalSprite
			{
				get { return mOriginalSprite; }
				set
				{
					mOriginalSprite = value;
				}
			}

			/// <summary>
			/// Целевой спрайт элемента
			/// </summary>
			public Sprite TargetSprite
			{
				get { return mTargetSprite; }
				set
				{
					mTargetSprite = value;
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установить оригинальное значение эффекта на основании текущего значения элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void SetOriginalValue()
			{
				if(mElement != null)
				{
					mSpriteRenderer = mElement.GetComponent<SpriteRenderer>();
				}
				if (mSpriteRenderer == null)
				{
					mSpriteRenderer = this.GetComponent<SpriteRenderer>();
				}
				if (mSpriteRenderer != null)
				{
					mOriginalSprite = mSpriteRenderer.sprite;
				}
				else
				{
					Debug.LogErrorFormat("Element <{0}> does not component SpriteRenderer", mElement);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение визуального эффекта активации элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void SetActivedEffect()
			{
				if(mSpriteRenderer != null)
				{
					mSpriteRenderer.sprite = mTargetSprite;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение визуального эффекта деактивации элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void SetDeactivedEffect()
			{
				if (mSpriteRenderer != null)
				{
					mSpriteRenderer.sprite = mOriginalSprite;
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