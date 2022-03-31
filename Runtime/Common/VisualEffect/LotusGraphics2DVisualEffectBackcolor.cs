//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Общий модуль 2D графики
// Подраздел: Визуальные эффекты
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGraphics2DVisualEffectBackcolor.cs
*		Визуальный эффект линейной интерполяции фонового цвета элемента.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections;
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
		/// Визуальный эффект линейной интерполяции фонового цвета элемента
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu("Lotus/Graphics2D/VisualEffect/Backcolor")]
		public class LotusGraphics2DVisualEffectBackcolor : LotusGraphics2DVisualEffectBase
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[NonSerialized]
			internal Color mOriginalColor = Color.white;
			[SerializeField]
			[LotusDisplayName(nameof(TargetColor))]
			internal Color mTargetColor = Color.red;
			[NonSerialized]
			internal ILotusPresentationBackcolor mBackcolor;

			// Служебные данные
			[NonSerialized]
			internal Coroutine mCoroutineBlink;
			[NonSerialized]
			internal Boolean mIsCoroutineBlinkStop;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Оригинальный фоновый цвет элемента
			/// </summary>
			public Color OriginalColor
			{
				get { return mOriginalColor; }
				set
				{
					mOriginalColor = value;
				}
			}

			/// <summary>
			/// Целевой фоновый цвет элемента
			/// </summary>
			public Color TargetColor
			{
				get { return mTargetColor; }
				set
				{
					mTargetColor = value;
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
				mBackcolor = mElement as ILotusPresentationBackcolor;
				if (mBackcolor == null)
				{
					if (mElement != null)
					{
						mBackcolor = mElement.GetComponent<ILotusPresentationBackcolor>();
					}
					else
					{
						mBackcolor = this.GetComponent<ILotusPresentationBackcolor>();
					}
				}
				if (mBackcolor != null)
				{
					mOriginalColor = mBackcolor.BackgroundColor;
				}
				else
				{
					Debug.LogErrorFormat("Element <{0}> does not support interface ILotusPresentationBackcolor", mElement);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение визуального эффекта активации элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void SetActivedEffect()
			{
				switch (mEffectMode)
				{
					case TVisualEffectMode.Swap:
						{
							mBackcolor.BackgroundColor = mTargetColor;
						}
						break;
					case TVisualEffectMode.Interpolation:
						{
							IEnumerator routine = mBackcolor.BackcolorColorLinearIteration(mEffectDuration, TargetColor);
							StartCoroutine(routine);
						}
						break;
					case TVisualEffectMode.Blink:
						{
							StartBlinkEffect();
						}
						break;
					case TVisualEffectMode.Loop:
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение визуального эффекта деактивации элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void SetDeactivedEffect()
			{
				switch (mEffectMode)
				{
					case TVisualEffectMode.Swap:
						{
							mBackcolor.BackgroundColor = mOriginalColor;
						}
						break;
					case TVisualEffectMode.Interpolation:
						{
							IEnumerator routine = mBackcolor.BackcolorColorLinearIteration(mEffectDuration, OriginalColor);
							StartCoroutine(routine);
						}
						break;
					case TVisualEffectMode.Blink:
						{
							StopBlinkEffect();
						}
						break;
					case TVisualEffectMode.Loop:
						break;
					default:
						break;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ПОДПРОГРАММ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт мигания
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void StartBlinkEffect()
			{
				mIsCoroutineBlinkStop = false;
				mCoroutineBlink = StartCoroutine(BlinkingCoroutine());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Приостановление мигания
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void StopBlinkEffect()
			{
				mIsCoroutineBlinkStop = true;
				//StopCoroutine(mCoroutineBlink);
				
				//IEnumerator routine = mBackcolor.BackcolorColorLinearIteration(mEffectDuration, OriginalColor);
				//StartCoroutine(routine);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма мигания
			/// </summary>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			protected IEnumerator BlinkingCoroutine()
			{
				if (mIsCoroutineBlinkStop)
				{
					IEnumerator routine_active = mBackcolor.BackcolorColorLinearIteration(mEffectDuration, OriginalColor);
					StartCoroutine(routine_active);
				}
				else
				{
					while (true)
					{
						if (!mIsCoroutineBlinkStop)
						{
							IEnumerator routine_active = mBackcolor.BackcolorColorLinearIteration(mEffectDuration, TargetColor);
							StartCoroutine(routine_active);
							yield return new WaitForSeconds(mEffectDuration);
						}

						//if (!mIsCoroutineBlinkStop)
						//{
							IEnumerator routine_deactive = mBackcolor.BackcolorColorLinearIteration(mEffectDuration, OriginalColor);
							StartCoroutine(routine_deactive);
							yield return new WaitForSeconds(mEffectDuration);
						//}
					}
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