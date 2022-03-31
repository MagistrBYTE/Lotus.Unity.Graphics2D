//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Макетное расположение и группировка
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUILayoutGroupEditor.cs
*		Редактор компонента группировки дочерних элементов в родительской области.
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
using Lotus.Editor;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор компонента группировки дочерних элементов в родительской области
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(LotusUILayoutGroup), true)]
public class LotusUILayoutGroupEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ ДАННЫЕ ========================================
	protected static GUIContent mContentSetWidth = new GUIContent("Set width for items");
	protected static GUIContent mContentSetHeight = new GUIContent("Set height for items");
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private LotusUILayoutGroup mUILayoutGroup;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	protected void OnEnable()
	{
		mUILayoutGroup = this.target as LotusUILayoutGroup;
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
			mUILayoutGroup.Padding = XEditorInspector.PropertyBorderNormal("Padding(L,T,R,B)", mUILayoutGroup.Padding);

			GUILayout.Space(2.0f);
			mUILayoutGroup.SpacingX = XEditorInspector.PropertyFloat("SpacingX", mUILayoutGroup.SpacingX);

			GUILayout.Space(2.0f);
			mUILayoutGroup.SpacingY = XEditorInspector.PropertyFloat("SpacingY", mUILayoutGroup.SpacingY);

			GUILayout.Space(2.0f);
			mUILayoutGroup.ItemWidth = XEditorInspector.PropertyFloat("ItemWidth", mUILayoutGroup.ItemWidth);

			GUILayout.Space(2.0f);
			mUILayoutGroup.ItemHeight = XEditorInspector.PropertyFloat("ItemHeight", mUILayoutGroup.ItemHeight);

			GUILayout.Space(2.0f);
			mUILayoutGroup.IsAutoSizeWidth = XEditorInspector.PropertyBoolean("IsAutoSizeWidth", mUILayoutGroup.IsAutoSizeWidth);

			GUILayout.Space(2.0f);
			mUILayoutGroup.IsAutoSizeHeight = XEditorInspector.PropertyBoolean("IsAutoSizeHeight", mUILayoutGroup.IsAutoSizeHeight);

			GUILayout.Space(2.0f);
			mUILayoutGroup.GroupType = (TLayoutGroupType)XEditorInspector.PropertyEnum("GroupType", mUILayoutGroup.GroupType);

			switch (mUILayoutGroup.GroupType)
			{
				case TLayoutGroupType.Horizontal:
					{
						GUILayout.Space(4.0f);
						mUILayoutGroup.GroupPlacement = (TLayoutGroupPlacement)XEditorInspector.PropertyEnum("GroupPlacement", mUILayoutGroup.GroupPlacement);

						GUILayout.Space(4.0f);
						mUILayoutGroup.GroupVerticalAlign = (TLayoutGroupVerticalAlign)XEditorInspector.PropertyEnum("VerticalAlign", mUILayoutGroup.GroupVerticalAlign);

						GUILayout.Space(4.0f);
						mUILayoutGroup.IsHeightGroupOfItem = XEditorInspector.PropertyBoolean("IsHeightGroupOfItem", mUILayoutGroup.IsHeightGroupOfItem);

						GUILayout.Space(4.0f);
						if (GUILayout.Button(mContentSetWidth))
						{
							mUILayoutGroup.Width = mUILayoutGroup.GetTotalWidthItems();
						}
					}
					break;
				case TLayoutGroupType.Vertical:
					{
						GUILayout.Space(4.0f);
						mUILayoutGroup.GroupPlacement = (TLayoutGroupPlacement)XEditorInspector.PropertyEnum("GroupPlacement", mUILayoutGroup.GroupPlacement);

						GUILayout.Space(4.0f);
						mUILayoutGroup.GroupHorizontalAlign = (TLayoutGroupHorizontalAlign)XEditorInspector.PropertyEnum("HorizontalAlign", mUILayoutGroup.GroupHorizontalAlign);

						GUILayout.Space(4.0f);
						mUILayoutGroup.IsWidthGroupOfItem = XEditorInspector.PropertyBoolean("IsWidthGroupOfItem", mUILayoutGroup.IsWidthGroupOfItem);

						GUILayout.Space(4.0f);
						if (GUILayout.Button(mContentSetHeight))
						{
							mUILayoutGroup.Height = mUILayoutGroup.GetTotalHeightItems();
						}
					}
					break;
				case TLayoutGroupType.Grid:
					break;
				case TLayoutGroupType.Flow:
					break;
				default:
					break;
			}
		}
		if (EditorGUI.EndChangeCheck())
		{
			mUILayoutGroup.SaveInEditor();
		}

		GUILayout.Space(2.0f);
	}
	#endregion
}
//=====================================================================================================================