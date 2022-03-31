//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Общий раздел
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusSpriteDispatcherEditor.cs
*		Редактор диспетчера размещения спрайтов в двухмерной пространстве
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using UnityEngine;
using UnityEditor;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Graphics2D;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор диспетчера размещения спрайтов в двухмерной пространстве
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusSpriteDispatcher))]
public class LotusSpriteDispatcherEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Добавления диспетчера размещения спрайтов в двухмерной пространстве
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XSpriteEditorSettings.MenuPath + "Create SpriteDispatcher", false, XSpriteEditorSettings.MenuOrderLast + 4)]
	public static void Create()
	{
		LotusSpriteDispatcher sprite_dispatcher = LotusSpriteDispatcher.Instance;
		Undo.RegisterCreatedObjectUndo(sprite_dispatcher.gameObject, "SpriteDispatcher");
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusSpriteDispatcher mDispatcher;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnEnable()
	{
		mDispatcher = this.target as LotusSpriteDispatcher;
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		GUILayout.Space(4.0f);
		EditorGUI.BeginChangeCheck();
		{
			XEditorInspector.DrawGroup("Singleton settings");
			{
				GUILayout.Space(2.0f);
				mDispatcher.IsMainInstance = XEditorInspector.PropertyBoolean(nameof(mDispatcher.IsMainInstance), mDispatcher.IsMainInstance);

				GUILayout.Space(2.0f);
				EditorGUI.BeginDisabledGroup(mDispatcher.IsMainInstance);
				{
					mDispatcher.DestroyMode = (TSingletonDestroyMode)XEditorInspector.PropertyEnum(nameof(mDispatcher.DestroyMode), mDispatcher.DestroyMode);
				}
				EditorGUI.EndDisabledGroup();
			}

			XEditorInspector.DrawGroup("Sprite settings");
			{
				GUILayout.Space(2.0f);
				mDispatcher.mDesignScreenWidth = XEditorInspector.PropertyInt("Design Width", mDispatcher.mDesignScreenWidth);

				GUILayout.Space(2.0f);
				mDispatcher.mDesignScreenHeight = XEditorInspector.PropertyInt("Design Height", mDispatcher.mDesignScreenHeight);

				GUILayout.Space(2.0f);
				XEditorInspector.PropertyFloat("ScaledX", LotusSpriteDispatcher.ScaledScreenX);

				GUILayout.Space(2.0f);
				XEditorInspector.PropertyFloat("ScaledY", LotusSpriteDispatcher.ScaledScreenY);

				GUILayout.Space(2.0f);
				mDispatcher.mCameraSprite = XEditorInspector.PropertyComponent(nameof(LotusSpriteDispatcher.CameraSprite) , mDispatcher.mCameraSprite);

				GUILayout.Space(2.0f);
				mDispatcher.mCameraPixelsPerUnit = XEditorInspector.PropertyFloat(nameof(LotusSpriteDispatcher.CameraPixelsPerUnit), 
					mDispatcher.mCameraPixelsPerUnit);

				GUILayout.Space(2.0f);
				mDispatcher.mIsCameraZooming = XEditorInspector.PropertyBoolean(nameof(LotusSpriteDispatcher.IsCameraZooming), mDispatcher.mIsCameraZooming);

				GUILayout.Space(2.0f);
				mDispatcher.mCameraZoom = XEditorInspector.PropertyFloatSlider(nameof(LotusSpriteDispatcher.CameraZoom), mDispatcher.mCameraZoom, 0.5f, 4.0f);
			}

		}
		if(EditorGUI.EndChangeCheck())
		{
			mDispatcher.SaveInEditor();
		}

		GUILayout.Space(2.0f);
	}
	#endregion
}
//=====================================================================================================================