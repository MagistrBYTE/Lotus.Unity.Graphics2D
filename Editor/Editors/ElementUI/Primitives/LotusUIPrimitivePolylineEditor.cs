//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Подсистема геометрических примитивов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIPrimitivePolylineEditor.cs
*		Редактор компонента векторного примитива полилинии.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Graphics2D;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор компонента векторного примитива полилинии
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusUIPrimitivePolyline), true)]
public class LotusUIPrimitivePolylineEditor : LotusUIPrimitiveBaseEditor
{
	#region =============================================== СТАТИЧЕСКИЕ ДАННЫЕ ========================================
	protected static GUIContent mContentDuplicate = new GUIContent("D", "Duplicate this point");
	protected static GUIContent mContentRemove = new GUIContent("X", "Remove this point");
	protected static GUIContent mContentAdd = new GUIContent("Add point");
	#endregion

	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Создание векторного примитива полилинии с параметрами по умолчанию
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XElementUIEditorSettings.MenuPathPrimitive + "Create Polyline", false, XElementUIEditorSettings.MenuOrderPrimitive + 1)]
	public static void CreatePolyline()
	{
		LotusUIPrimitivePolyline polyline = LotusUIPrimitivePolyline.CreatePolyline(30, 30, 60, 60, Selection.activeTransform as RectTransform);
		Undo.RegisterCreatedObjectUndo(polyline.gameObject, "Polyline");
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusUIPrimitivePolyline mPrimitiveLine;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	protected override void OnEnable()
	{
		base.OnEnable();
		mPrimitiveLine = this.target as LotusUIPrimitivePolyline;
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();
		{
			mPrimitiveLine.mExpandedSize = XEditorInspector.DrawGroupFoldout("Size and location", mPrimitiveLine.mExpandedSize);
			if (mPrimitiveLine.mExpandedSize)
			{
				GUILayout.Space(4.0f);
				LotusUIPlaceable2DEditor.DrawPlaceable(mPrimitiveLine, 2000, 2000);
			}

			mPrimitiveLine.mExpandedBase = XEditorInspector.DrawGroupFoldout("Main settings", mPrimitiveLine.mExpandedBase);
			if (mPrimitiveLine.mExpandedBase)
			{
				GUILayout.Space(4.0f);
				LotusUIPrimitiveBaseEditor.DrawPrimitiveBase(mPrimitiveLine);
			}

			mPrimitiveLine.mExpandedMainParam = XEditorInspector.DrawGroupFoldout("Settings line", mPrimitiveLine.mExpandedMainParam);
			if (mPrimitiveLine.mExpandedMainParam)
			{
				GUILayout.Space(4.0f);
				mPrimitiveLine.LineThickness = XEditorInspector.PropertyFloat("Thickness", mPrimitiveLine.LineThickness);

				GUILayout.Space(4.0f);
				mPrimitiveLine.LineJoins = (TPrimitiveJoinType)XEditorInspector.PropertyEnum("LineJoins", mPrimitiveLine.LineJoins);

				GUILayout.Space(4.0f);
				mPrimitiveLine.LineList = XEditorInspector.PropertyBoolean("LineList", mPrimitiveLine.LineList);

				GUILayout.Space(2.0f);
				for (Int32 i = 0; i < mPrimitiveLine.Points.Count; i++)
				{
					GUILayout.Space(2.0f);
					EditorGUILayout.BeginHorizontal();
					{
						mPrimitiveLine.Points[i] = EditorGUILayout.Vector2Field(i.ToString(), mPrimitiveLine.Points[i]);

						if (GUILayout.Button(mContentDuplicate, EditorStyles.miniButtonLeft, 
							GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
						{
							mPrimitiveLine.AddPoint(mPrimitiveLine.Points[i]);
							EditorGUILayout.EndHorizontal();
							break;
						}

						if (GUILayout.Button(mContentRemove, EditorStyles.miniButtonRight,
							GUILayout.Width(XInspectorViewParams.BUTTON_MINI_WIDTH)))
						{
							mPrimitiveLine.RemovePoint(i);
							EditorGUILayout.EndHorizontal();
							break;
						}
					}
					EditorGUILayout.EndHorizontal();
				}

				if (GUILayout.Button(mContentAdd))
				{
					mPrimitiveLine.AddPoint(Vector2.zero);
				}
			}
		}
		if (EditorGUI.EndChangeCheck())
		{
			mPrimitiveLine.UpdateGeometryForced();
			this.serializedObject.Save();
		}

		GUILayout.Space(2.0f);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование на сцене
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnSceneGUI()
	{
		//Vector3[] array_word = new Vector3[4];
		//mPrimitiveLine.UIRect.GetWorldCorners(array_word);
		////// Точка перемещения ручки в мировых координатах
		////Vector3 position_new = Handles.PositionHandle(mPrimitiveLine.UIRect.anchoredPosition, Quaternion.identity);

		//Vector3 world_point = new Vector3();
		//RectTransformUtility.ScreenPointToWorldPointInRectangle(mPrimitiveLine.UIRect,
		//	mPrimitiveLine.Location, Camera.current, out world_point);

		////Vector2 mousePos = mPrimitiveLine.Location;
		////mousePos.y = Camera.current.pixelHeight - mousePos.y;
		////Vector3 position = Camera.current.ScreenPointToRay(mousePos).origin;
		//Vector3 position = array_word[0];


		//Handles.DotHandleCap(-1, array_word[0], Quaternion.identity, HandleUtility.GetHandleSize(array_word[0]) * .1f, EventType.Repaint);
		//Handles.DotHandleCap(-1, array_word[1], Quaternion.identity, HandleUtility.GetHandleSize(array_word[1]) * .1f, EventType.Repaint);
		//Handles.BeginGUI();

		//GUILayout.BeginArea(new Rect(20, 20, 150, 60));

		//var rect = EditorGUILayout.BeginVertical();
		//GUI.color = Color.yellow;
		//GUI.Box(rect, GUIContent.none);

		//GUI.color = Color.white;

		//GUILayout.BeginHorizontal();
		//GUILayout.FlexibleSpace();
		//GUILayout.Label("Rotate");
		//GUILayout.FlexibleSpace();
		//GUILayout.EndHorizontal();

		//GUILayout.BeginVertical();
		//GUI.backgroundColor = Color.red;

		//if (GUILayout.Button(array_word[0].ToString()))
		//{
		//	//RotateLeft();
		//}

		//if (GUILayout.Button(array_word[1].ToString()))
		//{
		//	//RotateRight();
		//}

		//GUILayout.EndVertical();

		//EditorGUILayout.EndVertical();


		//GUILayout.EndArea();

		//Handles.EndGUI();
	}
	#endregion
}
//=====================================================================================================================