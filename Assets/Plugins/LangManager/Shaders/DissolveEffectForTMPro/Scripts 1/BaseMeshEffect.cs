﻿//#define NOT_USE_TMPRO	// If your project does not use TMPro, uncomment this line.
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if !NOT_USE_TMPRO
using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Linq;
#endif
#endif

namespace Coffee.UIExtensions
{
	/// <summary>
	/// Base class for effects that modify the generated Mesh.
	/// It works well not only for standard Graphic components (Image, RawImage, Text, etc.) but also for TextMeshPro and TextMeshProUGUI.
	/// </summary>
	[ExecuteInEditMode]
	public abstract class BaseMeshEffect : UIBehaviour, IMeshModifier
	{
		//################################
		// Constant or Static Members.
		//################################
#if !NOT_USE_TMPRO
		static readonly List<Vector2> s_Uv0 = new List<Vector2> ();
		static readonly List<Vector2> s_Uv1 = new List<Vector2> ();
		static readonly List<Vector3> s_Vertices = new List<Vector3> ();
		static readonly List<int> s_Indices = new List<int> ();
		static readonly List<Vector3> s_Normals = new List<Vector3> ();
		static readonly List<Vector4> s_Tangents = new List<Vector4> ();
		static readonly List<Color32> s_Colors = new List<Color32> ();
		static readonly VertexHelper s_VertexHelper = new VertexHelper ();
		static readonly List<TMP_SubMeshUI> s_SubMeshUIs = new List<TMP_SubMeshUI> ();
		static readonly List<Mesh> s_Meshes = new List<Mesh> ();
#endif
		static readonly Material [] s_EmptyMaterials = new Material [0];


		//################################
		// Public Members.
		//################################
		/// <summary>
		/// The Graphic attached to this GameObject.
		/// </summary>
		public Graphic graphic { get { Initialize (); return _graphic; } }

		/// <summary>
		/// The CanvasRenderer attached to this GameObject.
		/// </summary>
		public CanvasRenderer canvasRenderer { get { Initialize (); return _canvasRenderer; } }

#if !NOT_USE_TMPRO
		/// <summary>
		/// The TMP_Text attached to this GameObject.
		/// </summary>
		public TMP_Text textMeshPro { get { Initialize (); return _textMeshPro; } }
#endif

		/// <summary>
		/// The RectTransform attached to this GameObject.
		/// </summary>
		public RectTransform rectTransform { get { Initialize (); return _rectTransform; } }

		/// <summary>
		/// Additional canvas shader channels to use this component.
		/// </summary>
		public virtual AdditionalCanvasShaderChannels requiredChannels { get { return AdditionalCanvasShaderChannels.TexCoord1; } }

		/// <summary>
		/// Is TextMeshPro or TextMeshProUGUI attached to this GameObject?
		/// </summary>
		public bool isTMPro
		{
			get
			{
#if !NOT_USE_TMPRO
				return textMeshPro != null;
#else
				return false;
#endif
			}
		}

		/// <summary>
		/// The material for rendering.
		/// </summary>
		public virtual Material material
		{
			get
			{

#if !NOT_USE_TMPRO
				if (textMeshPro)
				{
					return textMeshPro.fontSharedMaterial;
				}
				else
#endif
				if (graphic)
				{
					return graphic.material;
				}
				else
				{
					return null;
				}
			}
			set
			{
#if !NOT_USE_TMPRO
				if (textMeshPro)
				{
					textMeshPro.fontSharedMaterial = value;
				}
				else
#endif
				if (graphic)
				{
					graphic.material = value;
				}
			}
		}

		public virtual Material[] materials
		{
			get
			{

#if !NOT_USE_TMPRO
				if (textMeshPro)
				{
					return textMeshPro.fontSharedMaterials ?? s_EmptyMaterials;
				}
				else
#endif
				if (graphic)
				{
					_materials [0] = graphic.material;
					return _materials;
				}
				else
				{
					return s_EmptyMaterials;
				}
			}
		}

		/// <summary>
		/// Call used to modify mesh. (legacy)
		/// </summary>
		/// <param name="mesh">Mesh.</param>
		public virtual void ModifyMesh (Mesh mesh)
		{
		}

		/// <summary>
		/// Call used to modify mesh.
		/// </summary>
		/// <param name="vh">VertexHelper.</param>
		public virtual void ModifyMesh (VertexHelper vh)
		{
		}

		/// <summary>
		/// Mark the vertices as dirty.
		/// </summary>
		public virtual void SetVerticesDirty ()
		{
#if !NOT_USE_TMPRO
			if (!textMeshPro.Equals(null))
			{
				try
				{
					if (!textMeshPro.textInfo.Equals(null))
					{
						foreach (var info in textMeshPro.textInfo.meshInfo)
						{
							var mesh = info.mesh;
							if (mesh)
							{
								mesh.Clear();
								mesh.vertices = info.vertices;
								mesh.uv = System.Array.ConvertAll(info.uvs0, uv => new Vector2(uv.x, uv.y));
								mesh.uv2 = info.uvs2;
								mesh.colors32 = info.colors32;
								mesh.normals = info.normals;
								mesh.tangents = info.tangents;
								mesh.triangles = info.triangles;
							}
						}

						if (canvasRenderer)
						{
							canvasRenderer.SetMesh(textMeshPro.mesh);

							GetComponentsInChildren(false, s_SubMeshUIs);
							foreach (var sm in s_SubMeshUIs)
							{
								sm.canvasRenderer.SetMesh(sm.mesh);
							}
							s_SubMeshUIs.Clear();
						}
						textMeshPro.havePropertiesChanged = true;
					}
                }
                catch
                {

                }
			}
			else
#endif
			if (graphic)
			{
				graphic.SetVerticesDirty ();
			}
		}

		public void ShowTMProWarning (Shader shader, Shader mobileShader, Shader spriteShader, System.Action<Material> onCreatedMaterial)
		{
#if UNITY_EDITOR && !NOT_USE_TMPRO
			if(!textMeshPro || !textMeshPro.fontSharedMaterial)
			{
				return;
			}

			// Is the material preset for dissolve?
			Material m = textMeshPro.fontSharedMaterial;
			if (m.shader != shader && m.shader != mobileShader)
			{
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.HelpBox (string.Format("{0} requires '{1}' or '{2}' as a shader for material preset.", GetType().Name, shader.name, mobileShader.name), MessageType.Warning);
				if(GUILayout.Button ("Fix"))
				{
					var correctShader = m.shader.name.Contains ("Mobile") ? mobileShader : shader;
					textMeshPro.fontSharedMaterial = ModifyTMProMaterialPreset (m, correctShader, onCreatedMaterial);
				}
				EditorGUILayout.EndHorizontal ();
				return;
			}

			// Is the sprite asset for dissolve?
			TMP_SpriteAsset spriteAsset = textMeshPro.spriteAsset ?? TMP_Settings.GetSpriteAsset ();
			m = spriteAsset.material;
			if (m && m.shader != spriteShader && textMeshPro.richText && textMeshPro.text.Contains("<sprite="))
			{
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.HelpBox (string.Format ("{0} requires '{1}' as a shader for sprite asset.", GetType().Name,spriteShader.name), MessageType.Warning);
				if (GUILayout.Button ("Fix"))
				{
					GetComponentsInChildren<TMP_SubMesh> ().Select (x => x.gameObject).ToList ().ForEach (DestroyImmediate);
					GetComponentsInChildren<TMP_SubMeshUI> ().Select (x => x.gameObject).ToList ().ForEach (DestroyImmediate);
					textMeshPro.spriteAsset = ModifyTMProSpriteAsset (m, spriteShader, onCreatedMaterial);
				}
				EditorGUILayout.EndHorizontal ();
				return;
			}
		}

		Material ModifyTMProMaterialPreset (Material baseMaterial, Shader shader, System.Action<Material> onCreatedMaterial)
		{
			string path = AssetDatabase.GetAssetPath (baseMaterial);
			string filename = Path.GetFileNameWithoutExtension (path) + " (" + GetType ().Name + ")";
			Material mat = Resources.Load<Material> (TMP_Settings.defaultFontAssetPath + filename);
			if (!mat)
			{
				mat = new Material (baseMaterial)
				{
					shaderKeywords = baseMaterial.shaderKeywords,
					shader = shader,
				};
				onCreatedMaterial (mat);
				AssetDatabase.CreateAsset (mat, Path.GetDirectoryName (path) + "/" + filename + ".mat");

				EditorUtility.FocusProjectWindow ();
				EditorGUIUtility.PingObject (mat);
			}
			else
			{
				mat.shader = shader;
			}
			EditorUtility.SetDirty (mat);
			return mat;
		}

		TMP_SpriteAsset ModifyTMProSpriteAsset (Material baseMaterial, Shader shader, System.Action<Material> onCreatedMaterial)
		{
			string path = AssetDatabase.GetAssetPath (baseMaterial);
			string filename = Path.GetFileNameWithoutExtension (path) + " (" + this.GetType ().Name + ")";
			TMP_SpriteAsset spriteAsset = Resources.Load<TMP_SpriteAsset> (TMP_Settings.defaultSpriteAssetPath + filename);
			if (!spriteAsset)
			{
				AssetDatabase.CopyAsset (path, Path.GetDirectoryName (path) + "/" + filename + ".mat");
				spriteAsset = Resources.Load<TMP_SpriteAsset> (TMP_Settings.defaultSpriteAssetPath + filename);
				spriteAsset.material.shader = shader;
				spriteAsset.material.name = shader.name;
				onCreatedMaterial (spriteAsset.material);

				EditorUtility.FocusProjectWindow ();
				EditorGUIUtility.PingObject (spriteAsset);
			}
			else
			{
				spriteAsset.material.shader = shader;
			}
			EditorUtility.SetDirty (spriteAsset);
			return spriteAsset;
#endif
		}


		//################################
		// Protected Members.
		//################################
		/// <summary>
		/// Should the effect modify the mesh directly for TMPro?
		/// </summary>
		protected virtual bool isLegacyMeshModifier { get { return false; } }


		protected virtual void Initialize ()
		{
			if (!_initialized)
			{
				_initialized = true;
				_graphic = _graphic ?? GetComponent<Graphic> ();
				_canvasRenderer = _canvasRenderer ?? GetComponent<CanvasRenderer> ();
				_rectTransform = _rectTransform ?? GetComponent<RectTransform> ();
#if !NOT_USE_TMPRO
				_textMeshPro = _textMeshPro ?? GetComponent<TMP_Text> ();
#endif
			}
		}

		/// <summary>
		/// This function is called when the object becomes enabled and active.
		/// </summary>
		protected override void OnEnable ()
		{
			_initialized = false;
			SetVerticesDirty ();
#if !NOT_USE_TMPRO
			if (textMeshPro)
			{
				TMPro_EventManager.TEXT_CHANGED_EVENT.Add (OnTextChanged);
			}
#endif

#if UNITY_EDITOR && !NOT_USE_TMPRO
			if (graphic && textMeshPro)
			{
				GraphicRebuildTracker.TrackGraphic (graphic);
			}
#endif
			if (graphic)
			{
				AdditionalCanvasShaderChannels channels = requiredChannels;
				var canvas = graphic.canvas;
				if (canvas && (canvas.additionalShaderChannels & channels) != channels)
				{
					Debug.LogWarningFormat (this, "Enable {1} of Canvas.additionalShaderChannels to use {0}.", GetType ().Name, channels);
				}
			}
		}

		/// <summary>
		/// This function is called when the behaviour becomes disabled () or inactive.
		/// </summary>
		protected override void OnDisable ()
		{
#if !NOT_USE_TMPRO
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove (OnTextChanged);
#endif
			SetVerticesDirty ();

#if UNITY_EDITOR && !NOT_USE_TMPRO
			if (graphic && textMeshPro)
			{
				GraphicRebuildTracker.UnTrackGraphic (graphic);
			}
#endif
		}


		/// <summary>
		/// LateUpdate is called every frame, if the Behaviour is enabled.
		/// </summary>
		protected virtual void LateUpdate ()
		{
#if !NOT_USE_TMPRO
			if (textMeshPro)
			{
				if (textMeshPro.havePropertiesChanged || _isTextMeshProActive != textMeshPro.isActiveAndEnabled)
				{
					SetVerticesDirty ();
				}
				_isTextMeshProActive = textMeshPro.isActiveAndEnabled;
			}
#endif
		}

		/// <summary>
		/// Callback for when properties have been changed by animation.
		/// </summary>
		protected override void OnDidApplyAnimationProperties ()
		{
			SetVerticesDirty ();
		}

#if UNITY_EDITOR
		/// <summary>
		/// This function is called when the script is loaded or a value is changed in the inspector (Called in the editor only).
		/// </summary>
		protected override void OnValidate ()
		{
			SetVerticesDirty ();
		}
#endif


		//################################
		// Private Members.
		//################################
		bool _initialized;
		CanvasRenderer _canvasRenderer;
		RectTransform _rectTransform;
		Graphic _graphic;
		Material [] _materials = new Material [1];

#if !NOT_USE_TMPRO
		bool _isTextMeshProActive;
		TMP_Text _textMeshPro;

		/// <summary>
		/// Called when any TextMeshPro generated the mesh.
		/// </summary>
		/// <param name="obj">TextMeshPro object.</param>
		void OnTextChanged (Object obj)
		{
			// Skip if the object is different from the current object or the text is empty.
			var textInfo = textMeshPro.textInfo;
			if (textMeshPro != obj || textInfo.characterCount - textInfo.spaceCount <= 0)
			{
				return;
			}

			// Collect the meshes.
			s_Meshes.Clear ();
			foreach (var info in textInfo.meshInfo)
			{
				s_Meshes.Add (info.mesh);
			}

			// Modify the meshes.
			if (isLegacyMeshModifier)
			{
				// Legacy mode: Modify the meshes directly.
				foreach (var m in s_Meshes)
				{
					if (m)
					{
						ModifyMesh (m);
					}
				}
			}
			else
			{
				// Convert meshes to VertexHelpers and modify them.
				foreach (var m in s_Meshes)
				{
					if(m)
					{
						FillVertexHelper (s_VertexHelper, m);
						ModifyMesh (s_VertexHelper);
						s_VertexHelper.FillMesh (m);
					}
				}
			}

			// Set the modified meshes to the CanvasRenderers (for UI only).
			if (canvasRenderer)
			{
				canvasRenderer.SetMesh (textMeshPro.mesh);
				GetComponentsInChildren (false, s_SubMeshUIs);
				foreach (var sm in s_SubMeshUIs)
				{
					sm.canvasRenderer.SetMesh (sm.mesh);
				}
				s_SubMeshUIs.Clear ();
			}

			// Clear.
			s_Meshes.Clear ();
		}

		void FillVertexHelper (VertexHelper vh, Mesh mesh)
		{
			vh.Clear ();

			mesh.GetVertices (s_Vertices);
			mesh.GetColors (s_Colors);
			mesh.GetUVs (0, s_Uv0);
			mesh.GetUVs (1, s_Uv1);
			mesh.GetNormals (s_Normals);
			mesh.GetTangents (s_Tangents);
			mesh.GetIndices (s_Indices, 0);

			for (int i = 0; i < s_Vertices.Count; i++)
			{
				s_VertexHelper.AddVert (s_Vertices [i], s_Colors [i], s_Uv0 [i], s_Uv1 [i], s_Normals [i], s_Tangents [i]);
			}

			for (int i = 0; i < s_Indices.Count; i += 3)
			{
				vh.AddTriangle (s_Indices [i], s_Indices [i + 1], s_Indices [i + 2]);
			}
		}
#endif
	}
}