//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Макетное расположение и группировка
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUILayoutElementEditor.cs
*		Редактор компонента макета расположения элемента.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using UnityEditor;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Graphics2D;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор компонента макета расположения элемента
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusUILayoutElement), true)]
public class LotusUILayoutElementEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ ДАННЫЕ ========================================
	protected static GUIContent mContentGetMinWidth = new GUIContent("Get", "Get min width from element");
	protected static GUIContent mContentSetMinWidth = new GUIContent("Set", "Set min width to element");
	protected static GUIContent mContentGetMinHeight = new GUIContent("Get", "Get min height from element");
	protected static GUIContent mContentSetMinHeight = new GUIContent("Set", "Set min height to element");
	protected static GUIContent mContentGetPreferredWidth = new GUIContent("Get", "Get preferred width from element");
	protected static GUIContent mContentSetPreferredWidth = new GUIContent("Set", "Set preferred width to element");
	protected static GUIContent mContentGetPreferredHeight = new GUIContent("Get", "Get preferred height from element");
	protected static GUIContent mContentSetPreferredHeight = new GUIContent("Set", "Set preferred height to element");
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusUILayoutElement mUILayoutElement;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	protected void OnEnable()
	{
		mUILayoutElement = this.target as LotusUILayoutElement;
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
			GUILayout.Space(4.0f);
			mUILayoutElement.ignoreLayout = XEditorInspector.PropertyBoolean("Ignore Layout", mUILayoutElement.ignoreLayout);

			GUILayout.Space(2.0f);
			EditorGUILayout.BeginHorizontal();
			{
				mUILayoutElement.minWidth = XEditorInspector.PropertyFloat("Min Width", mUILayoutElement.minWidth);

				if (GUILayout.Button(mContentGetMinWidth, EditorStyles.miniButtonLeft))
				{
					mUILayoutElement.SetMinWidthFromRect();
				}
				if (GUILayout.Button(mContentSetMinWidth, EditorStyles.miniButtonRight))
				{
					mUILayoutElement.SetMinWidthToRect();
				}
			}
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(2.0f);
			EditorGUILayout.BeginHorizontal();
			{
				mUILayoutElement.minHeight = XEditorInspector.PropertyFloat("Min Height", mUILayoutElement.minHeight);

				if (GUILayout.Button(mContentGetMinHeight, EditorStyles.miniButtonLeft))
				{
					mUILayoutElement.SetMinHeightFromRect();
				}
				if (GUILayout.Button(mContentSetMinHeight, EditorStyles.miniButtonRight))
				{
					mUILayoutElement.SetMinHeightToRect();
				}
			}
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(2.0f);
			EditorGUILayout.BeginHorizontal();
			{
				mUILayoutElement.preferredWidth = XEditorInspector.PropertyFloat("Preferred Width", mUILayoutElement.preferredWidth);

				if (GUILayout.Button(mContentGetPreferredWidth, EditorStyles.miniButtonLeft))
				{
					mUILayoutElement.SetPreferredWidthFromRect();
				}

				if (GUILayout.Button(mContentSetPreferredWidth, EditorStyles.miniButtonRight))
				{
					mUILayoutElement.SetPreferredWidthToRect();
				}
			}
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(2.0f);
			EditorGUILayout.BeginHorizontal();
			{
				mUILayoutElement.preferredHeight = XEditorInspector.PropertyFloat("Preferred Height", mUILayoutElement.preferredHeight);

				if (GUILayout.Button(mContentGetPreferredHeight, EditorStyles.miniButtonLeft))
				{
					mUILayoutElement.SetPreferredHeightFromRect();
				}

				if (GUILayout.Button(mContentSetPreferredHeight, EditorStyles.miniButtonRight))
				{
					mUILayoutElement.SetPreferredHeightToRect();
				}
			}
			EditorGUILayout.EndHorizontal();


			GUILayout.Space(4.0f);
			mUILayoutElement.LayoutAlignment = (TLayoutAlignment)XEditorInspector.PropertyEnum("Alignment", mUILayoutElement.LayoutAlignment);

			GUILayout.Space(4.0f);
			mUILayoutElement.LayoutWidthMode = (TLayoutSizeMode)XEditorInspector.PropertyEnum("WidthMode", mUILayoutElement.LayoutWidthMode);

			GUILayout.Space(4.0f);
			mUILayoutElement.LayoutHeightMode = (TLayoutSizeMode)XEditorInspector.PropertyEnum("HeightMode", mUILayoutElement.LayoutHeightMode);

			GUILayout.Space(4.0f);
			mUILayoutElement.AutoAnchor = XEditorInspector.PropertyBoolean("AutoAnchor", mUILayoutElement.AutoAnchor);

			GUILayout.Space(2.0f);
			mUILayoutElement.Padding = XEditorInspector.PropertyBorderNormal("Padding(L,T,R,B)", mUILayoutElement.Padding);

		}
		if (EditorGUI.EndChangeCheck())
		{
			mUILayoutElement.SaveInEditor();
		}

		GUILayout.Space(2.0f);
	}
	#endregion
}
//=====================================================================================================================