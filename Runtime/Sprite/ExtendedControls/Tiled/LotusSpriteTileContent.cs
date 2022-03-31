//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль работы со спрайтами
// Подраздел: Расширенные элементы управления
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusSpriteTileContent.cs
*		TileContent - элемент представляющий тайловый элемент с контентом и расширенной анимацией.
*		Реализация элемент представляющий тайловый элемент с расширенной анимацией различных параметров и дополнительным
*	контентом.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using UnityEngine;
using UnityEngine.UI;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DSpriteExtended
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// TileContent - элемент представляющий тайловый элемент с контентом и расширенной анимацией
		/// </summary>
		/// <remarks>
		/// Реализация элемент представляющий тайловый элемент с расширенной анимацией различных параметров и дополнительным контентом
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu(XSpriteEditorSettings.MenuPathExtended + "Tiled/Tile Content")]
		public class LotusSpriteTileContent : LotusSpriteTileBase
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ===========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента TileContent
			/// </summary>
			/// <param name="left">Позиция по X</param>
			/// <param name="top">Позиция по Y</param>
			/// <param name="width">Ширина</param>
			/// <param name="height">Высота</param>
			/// <param name="parent">Родительский элемент</param>
			/// <returns>Созданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static LotusSpriteTileContent CreateTileContent(Single left, Single top, Single width, Single height, Transform parent = null)
			{
				// 1) Создание объекта
				LotusSpriteTileContent element = LotusSpriteDispatcher.CreateElement<LotusSpriteTileContent>("TileContent", left, top, width, height);

				// 2) Определение в иерархии
				element.SetParent(parent);

				return element;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal CTweenColor mTweenColor;
			[SerializeField]
			internal CTweenSingle mTweenRotationZ;
			[SerializeField]
			internal CTweenSingle mTweenRotationY;
			[SerializeField]
			internal CTweenSingle mTweenRotationX;
			[SerializeField]
			internal CTweenVector2D mTweenScale;
			[SerializeField]
			internal CTweenSprite mTweenSprite;

			// Параметры по исполнению задачи
			internal TTileTaskSet mTaskSet;

			// Данные для редактора
#if UNITY_EDITOR
			[SerializeField]
			internal Boolean mExpandedAnimation;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Пауза анимации
			/// </summary>
			public override Boolean IsPauseAnimation
			{
				get { return mTweenTranslate.IsPause; }
				set
				{
					mTweenColor.IsPause = value;
					mTweenRotationZ.IsPause = value;
					mTweenRotationY.IsPause = value;
					mTweenRotationZ.IsPause = value;
					mTweenScale.IsPause = value;
					mTweenTranslate.IsPause = value;
					mTweenSprite.IsPause = value;
				}
			}

			/// <summary>
			/// Аниматор цвета
			/// </summary>
			public CTweenColor TweenColor
			{
				get { return mTweenColor; }
			}

			/// <summary>
			/// Аниматор вращения по оси Z
			/// </summary>
			public CTweenSingle TweenRotationZ
			{
				get { return mTweenRotationZ; }
			}

			/// <summary>
			/// Аниматор вращения по оси Y
			/// </summary>
			public CTweenSingle TweenRotationY
			{
				get { return mTweenRotationY; }
			}

			/// <summary>
			/// Аниматор вращения по оси X
			/// </summary>
			public CTweenSingle TweenRotationX
			{
				get { return mTweenRotationX; }
			}

			/// <summary>
			/// Аниматор масштабирования
			/// </summary>
			public CTweenVector2D TweenScale
			{
				get { return mTweenScale; }
			}

			/// <summary>
			/// Аниматор спрайтов
			/// </summary>
			public CTweenSprite TweenSprite
			{
				get { return mTweenSprite; }
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusTask =======================================
			/// <summary>
			/// Статус завершение задачи
			/// </summary>
			/// <remarks>
			/// Свойство обязательное для реализации
			/// </remarks>
			public override Boolean IsTaskCompleted
			{
				get
				{
					return mTweenColor.IsTaskCompleted &&
						   mTweenRotationX.IsTaskCompleted &&
						   mTweenRotationY.IsTaskCompleted &&
						   mTweenRotationZ.IsTaskCompleted &&
						   mTweenScale.IsTaskCompleted &&
						   mTweenTranslate.IsTaskCompleted &&
						   mTweenSprite.IsTaskCompleted;
				}
			}

			/// <summary>
			/// Набор действий для выполнения в задачи
			/// </summary>
			public TTileTaskSet TaskSet
			{
				get { return mTaskSet; }
				set { mTaskSet = value; }
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация данных тайла
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void OnInitTileContent()
			{
				if (mTweenSprite == null) mTweenSprite = new CTweenSprite();
				mTweenSprite.Name = "Sprite";
				mTweenSprite.OnAnimationCompleted = AnimationSpriteCompletedTileInternal;

				if (mTweenColor == null) mTweenColor = new CTweenColor();
				mTweenColor.Name = "Color";
				mTweenColor.OnAnimationCompleted = AnimationColorCompletedTileInternal;

				if (mTweenRotationZ == null) mTweenRotationZ = new CTweenSingle();
				mTweenRotationZ.Name = "Z";
				mTweenRotationZ.OnAnimationCompleted = AnimationRotationCompletedTileInternal;

				if (mTweenRotationY == null) mTweenRotationY = new CTweenSingle();
				mTweenRotationY.Name = "Y";
				mTweenRotationY.OnAnimationCompleted = AnimationRotationCompletedTileInternal;

				if (mTweenRotationX == null) mTweenRotationX = new CTweenSingle();
				mTweenRotationX.Name = "X";
				mTweenRotationX.OnAnimationCompleted = AnimationRotationCompletedTileInternal;

				if (mTweenScale == null) mTweenScale = new CTweenVector2D();
				mTweenScale.Name = "Scale";
				mTweenScale.StartValue = mThisTransform.localScale;
				mTweenScale.OnAnimationCompleted = AnimationScaleCompletedTileInternal;
			}

#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация элемента при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void ResetElement()
			{
				OnInitTileBase();
				OnInitTileContent();
			}
#endif
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void ConstructorElement()
			{
				OnInitTileBase();
				OnInitTileContent();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление скрипта каждый кадр
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public new void Update()
			{
#if UNITY_EDITOR
				OnUpdateInEditorMode();
#endif

				if (mTweenTranslate.IsPlay)
				{
					mTweenTranslate.UpdateAnimation();
					this.mThisTransform.position = new Vector3(mTweenTranslate.Value.x, mTweenTranslate.Value.y,
						this.mThisTransform.position.z);
				}

				if (mTweenColor.IsPlay)
				{
					mTweenColor.UpdateAnimation();
					mSpriteRenderer.color = mTweenColor.Value;
				}

				if(mTweenRotationZ.IsPlay)
				{
					mTweenRotationZ.UpdateAnimation();
					mThisTransform.eulerAngles = new Vector3(mThisTransform.eulerAngles.x, mThisTransform.eulerAngles.y, mTweenRotationZ.Value);
				}

				if(mTweenRotationY.IsPlay)
				{
					mTweenRotationY.UpdateAnimation();
					mThisTransform.eulerAngles = new Vector3(mThisTransform.eulerAngles.x, mTweenRotationY.Value, mThisTransform.eulerAngles.z);
				}
				
				if(mTweenRotationX.IsPlay)
				{
					mTweenRotationX.UpdateAnimation();
					mThisTransform.eulerAngles = new Vector3(mTweenRotationX.Value, mThisTransform.eulerAngles.y, mThisTransform.eulerAngles.z);
				}
				
				if(mTweenScale.IsPlay)
				{
					mTweenScale.UpdateAnimation();
					mThisTransform.localScale = new Vector3(mTweenScale.Value.x, mTweenScale.Value.y, 1);
				}

				if (mTweenSprite.IsPlay)
				{
					mTweenSprite.UpdateAnimation();
					mSpriteRenderer.sprite = mTweenSprite.FrameSprite;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusTask =========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнение задачи
			/// </summary>
			/// <remarks>
			/// Здесь должна выполняться первоначальня работа по подготовки к выполнению задачи
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public override void RunTask()
			{
				if (mTaskSet.IsFlagSet(TTileTaskSet.Color))
				{
					mTweenColor.StartAnimation();
				}
				if (mTaskSet.IsFlagSet(TTileTaskSet.RotationZ))
				{
					mTweenRotationZ.StartAnimation();
				}
				if (mTaskSet.IsFlagSet(TTileTaskSet.RotationY))
				{
					mTweenRotationY.StartAnimation();
				}
				if (mTaskSet.IsFlagSet(TTileTaskSet.RotationX))
				{
					mTweenRotationX.StartAnimation();
				}
				if (mTaskSet.IsFlagSet(TTileTaskSet.Move))
				{
					mTweenTranslate.StartAnimation();
				}
				if (mTaskSet.IsFlagSet(TTileTaskSet.Scale))
				{
					mTweenScale.StartAnimation();
				}
				if (mTaskSet.IsFlagSet(TTileTaskSet.Sprite))
				{
					mTweenSprite.StartAnimation();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выполнение задачи
			/// </summary>
			/// <remarks>
			/// Непосредственное выполнение задачи. Метод будет вызываться каждый раз в зависимости от способа
			/// и режима выполнения задачи
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public override void ExecuteTask()
			{
				if (mTaskSet.IsFlagSet(TTileTaskSet.Color))
				{
					if (mTweenColor.IsPlay)
					{
						mTweenColor.UpdateAnimation();
						mSpriteRenderer.color = mTweenColor.Value;
					}
				}
				if (mTaskSet.IsFlagSet(TTileTaskSet.RotationZ))
				{
					if (mTweenRotationZ.IsPlay)
					{
						mTweenRotationZ.UpdateAnimation();
						mThisTransform.eulerAngles = new Vector3(mThisTransform.eulerAngles.x, mThisTransform.eulerAngles.y, mTweenRotationZ.Value);
					}
				}
				if (mTaskSet.IsFlagSet(TTileTaskSet.RotationY))
				{
					if (mTweenRotationY.IsPlay)
					{
						mTweenRotationY.UpdateAnimation();
						mThisTransform.eulerAngles = new Vector3(mThisTransform.eulerAngles.x, mTweenRotationY.Value, mThisTransform.eulerAngles.z);
					}
				}
				if (mTaskSet.IsFlagSet(TTileTaskSet.RotationX))
				{
					if (mTweenRotationX.IsPlay)
					{
						mTweenRotationX.UpdateAnimation();
						mThisTransform.eulerAngles = new Vector3(mTweenRotationX.Value, mThisTransform.eulerAngles.y, mThisTransform.eulerAngles.z);
					}
				}
				if (mTaskSet.IsFlagSet(TTileTaskSet.Move))
				{
					if (mTweenTranslate.IsPlay)
					{
						mTweenTranslate.UpdateAnimation();
						this.mThisTransform.position = mTweenTranslate.Value;
					}
				}
				if (mTaskSet.IsFlagSet(TTileTaskSet.Scale))
				{
					if (mTweenScale.IsPlay)
					{
						mTweenScale.UpdateAnimation();
						mThisTransform.localScale = new Vector3(mTweenScale.Value.x, mTweenScale.Value.y, 1);
					}
				}
				if (mTaskSet.IsFlagSet(TTileTaskSet.Sprite))
				{
					if (mTweenSprite.IsPlay)
					{
						mTweenSprite.UpdateAnimation();
						mSpriteRenderer.sprite = mTweenSprite.FrameSprite;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Принудительная остановка выполнения задачи
			/// </summary>
			/// <remarks>
			/// Если задачи будет принудительно остановлена то здесь можно/нужно реализовать необходимые действия
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public override void StopTask()
			{
				if (mTaskSet.IsFlagSet(TTileTaskSet.Color))
				{
					mTweenColor.StopAnimation();
				}
				if (mTaskSet.IsFlagSet(TTileTaskSet.RotationZ))
				{
					mTweenRotationZ.StopAnimation();
				}
				if (mTaskSet.IsFlagSet(TTileTaskSet.RotationY))
				{
					mTweenRotationY.StopAnimation();
				}
				if (mTaskSet.IsFlagSet(TTileTaskSet.RotationX))
				{
					mTweenRotationX.StopAnimation();
				}
				if (mTaskSet.IsFlagSet(TTileTaskSet.Move))
				{
					mTweenTranslate.StopAnimation();
				}
				if (mTaskSet.IsFlagSet(TTileTaskSet.Scale))
				{
					mTweenScale.StopAnimation();
				}
				if (mTaskSet.IsFlagSet(TTileTaskSet.Sprite))
				{
					mTweenSprite.StopAnimation();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка данных задачи
			/// </summary>
			/// <remarks>
			/// Здесь может быть выполняться работа по подготовки к выполнению задачи, но без запуска на выполнение
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public override void ResetTask()
			{

			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			#endregion

			#region ======================================= МЕТОДЫ АНИМИРОВАНИЯ =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Окончание анимации цвета спрайта тайла
			/// </summary>
			/// <param name="animation_name">Имя анимации</param>
			//---------------------------------------------------------------------------------------------------------
			protected void AnimationColorCompletedTileInternal(String animation_name)
			{
				if (mOwnerGrid != null)
					mOwnerGrid.OnTileAnimationColorCompleted(animation_name, this);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Окончание анимации вращения тайла
			/// </summary>
			/// <param name="animation_name">Имя анимации</param>
			//---------------------------------------------------------------------------------------------------------
			protected void AnimationRotationCompletedTileInternal(String animation_name)
			{
				if (mOwnerGrid != null)
					mOwnerGrid.OnTileAnimationRotationCompleted(animation_name, this);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Окончание анимации масштабирования тайла
			/// </summary>
			/// <param name="animation_name">Имя анимации</param>
			//---------------------------------------------------------------------------------------------------------
			protected void AnimationScaleCompletedTileInternal(String animation_name)
			{
				if (mOwnerGrid != null)
					mOwnerGrid.OnTileAnimationScaleCompleted(animation_name, this);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Окончание анимации спрайтов тайла
			/// </summary>
			/// <param name="animation_name">Имя анимации</param>
			//---------------------------------------------------------------------------------------------------------
			protected void AnimationSpriteCompletedTileInternal(String animation_name)
			{
				if (mOwnerGrid != null)
					mOwnerGrid.OnTileAnimationSpriteCompleted(animation_name, this);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вращение на заданный угол (анимация)
			/// </summary>
			/// <param name="delta">Угол вращения в градусах</param>
			/// <param name="wrap_mode">Режим проигрывания анимации</param>
			//---------------------------------------------------------------------------------------------------------
			public void AnimationRotationZ(Single delta, TTweenWrapMode wrap_mode = TTweenWrapMode.Once)
			{
				if (mTweenRotationZ.IsPlay == false)
				{
					mTweenRotationZ.StartValue = mThisTransform.eulerAngles.z;
					mTweenRotationZ.TargetValue = delta;
					mTweenRotationZ.WrapMode = wrap_mode;
					mTweenRotationZ.StartAnimation();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вращение на заданный угол (анимация)
			/// </summary>
			/// <param name="delta">Угол вращения в градусах</param>
			/// <param name="wrap_mode">Режим проигрывания анимации</param>
			//---------------------------------------------------------------------------------------------------------
			public void AnimationRotationZDelta(Single delta, TTweenWrapMode wrap_mode = TTweenWrapMode.Once)
			{
				if (mTweenRotationZ.IsPlay == false)
				{
					mTweenRotationZ.StartValue = mThisTransform.eulerAngles.z;
					mTweenRotationZ.TargetValue = mTweenRotationZ.StartValue + delta;
					mTweenRotationZ.WrapMode = wrap_mode;
					mTweenRotationZ.StartAnimation();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вращение на туда сюда
			/// </summary>
			/// <param name="delta">Угол вращения в градусах</param>
			/// <param name="wrap_mode">Режим проигрывания анимации</param>
			//---------------------------------------------------------------------------------------------------------
			public void AnimationRotationZPingpong(Single delta, TTweenWrapMode wrap_mode = TTweenWrapMode.Once)
			{
				if (mTweenRotationZ.IsPlay == false)
				{
					mTweenRotationZ.StartValue = mThisTransform.eulerAngles.z;
					mTweenRotationZ.TargetValue = mTweenRotationZ.StartValue + delta;
					mTweenRotationZ.WrapMode = wrap_mode;
					mTweenRotationZ.StartAnimation();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вращение вокруг оси Y на заданный угол (анимация)
			/// </summary>
			/// <param name="delta">Угол вращения в градусах</param>
			/// <param name="wrap_mode">Режим проигрывания анимации</param>
			//---------------------------------------------------------------------------------------------------------
			public void AnimationRotationY(Single delta, TTweenWrapMode wrap_mode = TTweenWrapMode.Once)
			{
				if (mTweenRotationY.IsPlay == false)
				{
					mTweenRotationY.StartValue = mThisTransform.eulerAngles.y;
					mTweenRotationY.TargetValue = delta;
					mTweenRotationY.WrapMode = wrap_mode;
					mTweenRotationY.StartAnimation();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вращение вокруг оси X на заданный угол (анимация)
			/// </summary>
			/// <param name="delta">Угол вращения в градусах</param>
			/// <param name="wrap_mode">Режим проигрывания анимации</param>
			//---------------------------------------------------------------------------------------------------------
			public void AnimationRotationX(Single delta, TTweenWrapMode wrap_mode = TTweenWrapMode.Once)
			{
				if (mTweenRotationX.IsPlay == false)
				{
					mTweenRotationX.StartValue = mThisTransform.eulerAngles.x;
					mTweenRotationX.TargetValue = delta;
					mTweenRotationX.WrapMode = wrap_mode;
					mTweenRotationX.StartAnimation();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Анимация масштабирования
			/// </summary>
			/// <param name="min_scale">Минимальный масштаб</param>
			/// <param name="wrap_mode">Режим проигрывания анимации</param>
			//---------------------------------------------------------------------------------------------------------
			public void AnimationScale(Single min_scale, TTweenWrapMode wrap_mode = TTweenWrapMode.Once)
			{
				if (mTweenScale.IsPlay == false)
				{
					mTweenScale.StartValue = mThisTransform.localScale;
					mTweenScale.TargetValue = new Vector2(min_scale, min_scale);
					mTweenScale.WrapMode = wrap_mode;
					mTweenScale.StartAnimation();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Анимация спрайтов
			/// </summary>
			/// <param name="animation_name">Имя анимационной цепочки</param>
			//---------------------------------------------------------------------------------------------------------
			public void AnimationSprite(String animation_name)
			{
				if (mTweenSprite.IsPlay == false)
				{
					mTweenSprite.StartAnimation(animation_name);
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