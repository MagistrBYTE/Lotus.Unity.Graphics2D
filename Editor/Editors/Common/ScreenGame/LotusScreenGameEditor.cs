//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Общий модуль 2D графики
// Подраздел: Подсистема игровых экранов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusScreenGameEditor.cs
*		Редактор компонента определяющего игровой экран.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
#if UNITY_EDITOR
//=====================================================================================================================
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Graphics2D;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор компонента определяющего игровой экран
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusScreenGame))]
public class LotusScreenGameEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Добавления игрового экрана на сцену
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XGraphics2DEditorSettings.MenuPathScreenGame + "Create ScreenGame", false, XGraphics2DEditorSettings.MenuOrderScreenGame + 2)]
	public static void CreateScreenGame()
	{
		LotusScreenGame.CreateScreenGame();
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected static readonly GUIContent mContentGetPos = new GUIContent("G", "Get position from screen game");
	protected static readonly GUIContent mContentSetPos = new GUIContent("S", "Set position to screen game");
	private LotusScreenGame mScreenGame;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnEnable()
	{
		mScreenGame = this.target as LotusScreenGame;
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		// Рисование свойств игрового экрана
		DrawScreenGameParamemtrs(mScreenGame);

		GUILayout.Space(4.0f);
		mScreenGame.mExpandedTween = XEditorInspector.DrawGroupFoldout("Tween Animator", mScreenGame.mExpandedTween);
		if (mScreenGame.mExpandedTween)
		{
			LotusTweenVector2DDrawer.Draw(mScreenGame.mTweenAnimator);
		}

		GUILayout.Space(2.0f);
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров компонента ScreenGame
	/// </summary>
	/// <param name="screen_game">Компонент ScreenGame</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawScreenGameParamemtrs(LotusScreenGame screen_game)
	{
		GUILayout.Space(4.0f);
		screen_game.mExpandedScreenGame = XEditorInspector.DrawGroupFoldout("ScreenGame settings", screen_game.mExpandedScreenGame);
		if (screen_game.mExpandedScreenGame)
		{
			EditorGUI.BeginChangeCheck();
			{
				GUILayout.Space(2.0f);
				screen_game.NumberWindow = XEditorInspector.PropertyInt(nameof(screen_game.NumberWindow), screen_game.NumberWindow);

				GUILayout.Space(2.0f);
				screen_game.GroupID = XEditorInspector.PropertyInt(nameof(screen_game.GroupID), screen_game.GroupID);

				GUILayout.Space(2.0f);
				screen_game.IsDeactivatedChild = XEditorInspector.PropertyBoolean(nameof(screen_game.IsDeactivatedChild), screen_game.IsDeactivatedChild);

				GUILayout.Space(4.0f);
				screen_game.ViewType = (TUIScreenGameViewType)XEditorInspector.PropertyEnum(nameof(screen_game.ViewType), screen_game.ViewType);

				GUILayout.Space(4.0f);
				screen_game.ModeBehavior = (TUIScreenGameModeBehavior)XEditorInspector.PropertyEnum(nameof(screen_game.ModeBehavior), screen_game.ModeBehavior);

				GUILayout.Space(4.0f);
				EditorGUILayout.BeginHorizontal();
				{
					screen_game.InvisiblePosStart = EditorGUILayout.Vector2Field("Invisible Start", screen_game.InvisiblePosStart);
					if (GUILayout.Button(mContentGetPos, EditorStyles.miniButtonLeft, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
					{
						//screen_game.InvisiblePosStart = screen_game.Location;
					}
					if (GUILayout.Button(mContentSetPos, EditorStyles.miniButtonRight, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
					{
						screen_game.SetToInvisiblePosStart();
					}
				}
				EditorGUILayout.EndHorizontal();

				GUILayout.Space(4.0f);
				EditorGUILayout.BeginHorizontal();
				{
					screen_game.VisiblePos = EditorGUILayout.Vector2Field("Visible Pos", screen_game.VisiblePos);

					if (GUILayout.Button(mContentGetPos, EditorStyles.miniButtonLeft, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
					{
						//screen_game.VisiblePos = screen_game.Location;
					}

					if (GUILayout.Button(mContentSetPos, EditorStyles.miniButtonRight, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
					{
						screen_game.SetToVisiblePos();
					}
				}
				EditorGUILayout.EndHorizontal();

				GUILayout.Space(4.0f);
				EditorGUILayout.BeginHorizontal();
				{
					screen_game.InvisiblePosNext = EditorGUILayout.Vector2Field("Invisible Next", screen_game.InvisiblePosNext);

					if (GUILayout.Button(mContentGetPos, EditorStyles.miniButtonLeft, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
					{
						//screen_game.InvisiblePosNext = screen_game.Location;
					}

					if (GUILayout.Button(mContentSetPos, EditorStyles.miniButtonRight, GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
					{
						screen_game.SetToInvisiblePosNext();
					}
				}
				EditorGUILayout.EndHorizontal();


				GUILayout.Space(2.0f);
				screen_game.IsCorrectPosition = XEditorInspector.PropertyBoolean("CorrectPosition", screen_game.IsCorrectPosition);

				GUILayout.Space(2.0f);
				screen_game.ButtonActivate = XEditorInspector.PropertyComponent<Button>("ButtonActivate", screen_game.mButtonActivate);
			}
			if (EditorGUI.EndChangeCheck())
			{
				this.serializedObject.Save();
			}
		}
	}
	#endregion
}
//=====================================================================================================================
#endif
//=====================================================================================================================
