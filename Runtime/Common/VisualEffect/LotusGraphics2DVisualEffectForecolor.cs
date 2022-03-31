//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Общий модуль 2D графики
// Подраздел: Визуальные эффекты
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGraphics2DVisualEffectForecolor.cs
*		Визуальный эффект линейной интерполяции переднего цвета элемента.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections;
using UnityEngine;
using TMPro;
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
		/// Визуальный эффект линейной интерполяции переднего цвета элемента
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu("Lotus/Graphics2D/VisualEffect/Forecolor")]
		public class LotusGraphics2DVisualEffectForecolor : LotusGraphics2DVisualEffectBase
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[NonSerialized]
			internal Color mOriginalColor;
			[SerializeField]
			[LotusDisplayName(nameof(TargetColor))]
			internal Color mTargetColor;
			[NonSerialized]
			internal ILotusPresentationForecolor mForecolor;

			// Служебные данные
			[NonSerialized]
			internal Coroutine mCoroutineBlink;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Оригинальный передний цвет элемента
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
			/// Целевой передний цвет элемента
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
				mForecolor = mElement as ILotusPresentationForecolor;
				if (mForecolor == null)
				{
					if (mElement != null)
					{
						mForecolor = mElement.GetComponent<ILotusPresentationForecolor>();
					}
					else
					{
						mForecolor = this.GetComponent<ILotusPresentationForecolor>();
					}
				}
				if (mForecolor != null)
				{
					mOriginalColor = mForecolor.ForegroundColor;
				}
				else
				{
					mTextMeshProUGU = this.GetComponent<TextMeshProUGUI>();
					if (mTextMeshProUGU == null)
					{
						Debug.LogErrorFormat("Element <{0}> does not support interface ILotusPresentationForecolor", mElement);
					}
					else
					{
						mOriginalColor = mTextMeshProUGU.color;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение визуального эффекта активации элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void SetActivedEffect()
			{
				if (mForecolor != null)
				{
					switch (mEffectMode)
					{
						case TVisualEffectMode.Swap:
							{
								mForecolor.ForegroundColor = mTargetColor;
							}
							break;
						case TVisualEffectMode.Interpolation:
							{
								IEnumerator routine = mForecolor.ForecolorColorLinearIteration(mEffectDuration, TargetColor);
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
				else
				{
					if (mTextMeshProUGU)
					{
						switch (mEffectMode)
						{
							case TVisualEffectMode.Swap:
								{
									mTextMeshProUGU.color = mTargetColor;
								}
								break;
							case TVisualEffectMode.Interpolation:
								{
									IEnumerator routine = mTextMeshProUGU.ForecolorColorLinearIteration(mEffectDuration, TargetColor);
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
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение визуального эффекта деактивации элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void SetDeactivedEffect()
			{
				if (mForecolor != null)
				{
					switch (mEffectMode)
					{
						case TVisualEffectMode.Swap:
							{
								mForecolor.ForegroundColor = mOriginalColor;
							}
							break;
						case TVisualEffectMode.Interpolation:
							{
								IEnumerator routine = mForecolor.ForecolorColorLinearIteration(mEffectDuration, OriginalColor);
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
				else
				{
					if (mTextMeshProUGU != null)
					{
						switch (mEffectMode)
						{
							case TVisualEffectMode.Swap:
								{
									mTextMeshProUGU.color = mOriginalColor;
								}
								break;
							case TVisualEffectMode.Interpolation:
								{
									IEnumerator routine = mTextMeshProUGU.ForecolorColorLinearIteration(mEffectDuration, OriginalColor);
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
				mCoroutineBlink = StartCoroutine(BlinkingCoroutine());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Приостановление мигания
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void StopBlinkEffect()
			{
				if (mForecolor != null)
				{
					StopCoroutine(mCoroutineBlink);
					IEnumerator routine = mForecolor.ForecolorColorLinearIteration(mEffectDuration, OriginalColor);
					StartCoroutine(routine);
				}
				else
				{
					StopCoroutine(mCoroutineBlink);
					IEnumerator routine = mTextMeshProUGU.ForecolorColorLinearIteration(mEffectDuration, OriginalColor);
					StartCoroutine(routine);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма мигания
			/// </summary>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			protected IEnumerator BlinkingCoroutine()
			{
				while (true)
				{
					if (mForecolor != null)
					{
						IEnumerator routine_active = mForecolor.ForecolorColorLinearIteration(mEffectDuration, TargetColor);
						StartCoroutine(routine_active);
						yield return new WaitForSeconds(mEffectDuration);

						IEnumerator routine_deactive = mForecolor.ForecolorColorLinearIteration(mEffectDuration, OriginalColor);
						StartCoroutine(routine_deactive);
						yield return new WaitForSeconds(mEffectDuration);
					}
					else
					{
						IEnumerator routine_active = mTextMeshProUGU.ForecolorColorLinearIteration(mEffectDuration, TargetColor);
						StartCoroutine(routine_active);
						yield return new WaitForSeconds(mEffectDuration);

						IEnumerator routine_deactive = mTextMeshProUGU.ForecolorColorLinearIteration(mEffectDuration, OriginalColor);
						StartCoroutine(routine_deactive);
						yield return new WaitForSeconds(mEffectDuration);
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