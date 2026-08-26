using System;
using System.Linq;

using UnityEngine;

namespace Ja2.Editor
{
	/// <summary>
	/// Texture atlas creator.
	/// </summary>
	public static class TextureAtlas
	{
#region Methods Static Public
		/// <summary>
		/// Pack the texture into the atlas.
		/// </summary>
		/// <param name="Atlas">Texture atlas (destination).</param>
		/// <param name="Textures">Source textures.</param>
		/// <param name="Padding">Padding to add to every texture.</param>
		/// <returns>Returns the UV coordinates for each source texture.</returns>
		/// <exception cref="ArgumentException">Thrown, when the destination texture is too small for packing all the source textures.</exception>
		public static Rect[] Create(Texture2D Atlas, Texture2D[] Textures, ushort Padding)
		{
			var ret = new Rect[Textures.Length];

			// This is the root node, from which all other nodes will be created
			var root = new Node(
				new RectInt(0,
					0,
					Atlas.width,
					Atlas.height
				)
			);

			// Traverse each texture, the largest textures first
			foreach((Texture2D Texture, int Index) it in Textures.Select((Texture, Index) => (Texture, Index)).OrderByDescending(Value => Value.Texture.width * Value.Texture.height))
			{
				// Traverse from root node to the first valid child
				Node? node = root.Insert(it.Texture,
					Padding,
					Atlas
				);

				// If image does fit
				if(node is not null)
				{
					// Generate UV coordinate for the right texture
					ret[it.Index] = new Rect(
						(float)(node.rect.x + Padding) / root.rect.width,
						(float)(root.rect.height - node.rect.y - it.Texture.height - Padding) / root.rect.height,
						(float)(it.Texture.width) / root.rect.width,
						(float)(it.Texture.height) / root.rect.height
					);
				}
				// If the texture couldn't be fit
				else
				{
					throw new ArgumentException("Atlas texture too small",
						nameof(Atlas)
					);
				}
			}

			return ret;
		}
#endregion
	}

	/// <summary>
	/// Node representing one texture.
	/// </summary>
	internal class Node
	{
#region Fields
		/// <summary>
		/// Left node.
		/// </summary>
		private Node? m_Left;

		/// <summary>
		/// Right node.
		/// </summary>
		private Node? m_Right;

		/// <summary>
		/// Does the node hold the image.
		/// </summary>
		private bool m_HasImage;
#endregion

#region Properties
		/// <summary>
		/// Actual rect of the node.
		/// </summary>
		public RectInt rect {get; private set;}
#endregion

#region Methods Public
		/// <summary>
		/// The insert function traverses the tree looking for a place to insert the texture.
		/// It returns the node of the atlas the texture can go into or null to say it can't fit.
		/// Note we really don't have to store the rectangle for each node.
		/// All we need is a split direction and coordinate like in a kd-tree, but it's more convenient with rects.
		/// </summary>
		/// <param name="Texture">Texture to pack to (destination).</param>
		/// <param name="Padding">Padding around the texture.</param>
		/// <param name="Target">Source texture.</param>
		/// <returns>Node instance, if texture could be packed. Otherwise, null.</returns>
		public Node? Insert(Texture2D Texture, ushort Padding, Texture2D Target)
		{
			Node? new_node = null;

			// Not a leaf node
			if(m_Left is not null || m_Right is not null)
			{
				// If this node is not a leaf, try inserting into first child
				if(m_Left is not null)
				{
					new_node = m_Left.Insert(Texture,
						Padding,
						Target
					);
				}

				// No more room in first child, insert into second child!
				if(m_Right is not null && new_node is null)
				{
					new_node ??= m_Right.Insert(Texture,
						Padding,
						Target
					);
				}
			}
			else
			{
				// If there is already an image in this node, early out
				if(!m_HasImage)
				{
					// If this node has enough space for the image
					if(ImageFits((ushort)(Texture.width + Padding * 2), (ushort)(Texture.height + Padding * 2)))
					{
						// If the image is perfect
						if(PerfectFit((ushort)(Texture.width + Padding * 2), (ushort)(Texture.height + Padding * 2)))
						{
							new_node = this;
							m_HasImage = true;

							// Set the new pixels; Need to adjust to the unity coordinates
							Target.SetPixels32(rect.x + Padding,
								Target.height - rect.y - Texture.height - Padding,
								Texture.width,
								Texture.height,
								Texture.GetPixels32()
							);
						}
						else
						{
							// If we made it this far, this node must be split.
							m_Left = new Node();
							m_Right = new Node();

							// Decide which way to split image
							int delta_w = rect.width - Texture.width;
							int delta_h = rect.height - Texture.height;

							if(delta_w > delta_h)
							{
								// Padding for both edges
								m_Left.rect = new RectInt(rect.x,
									rect.y,
									Texture.width + Padding * 2,
									rect.height
								);

								// Padding for both edges
								m_Right.rect = new RectInt(rect.x + Texture.width + Padding * 2,
									rect.y,
									rect.width - (Texture.width + Padding * 2),
									rect.height
								);
							}
							else
							{
								// Padding for both edges
								m_Left.rect = new RectInt(rect.x,
									rect.y,
									rect.width,
									Texture.height + Padding * 2
								);

								// Padding for both edges
								m_Right.rect = new RectInt(rect.x,
									rect.y + Texture.height + Padding * 2,
									rect.width,
									rect.height - (Texture.height + Padding * 2)
								);
							}

							// Let's try inserting into first child
							new_node = m_Left.Insert(Texture,
								Padding,
								Target
							);
						}
					}
				}
			}

			return new_node;

		}
#endregion

#region Methods Private
		/// <summary>
		/// Check if image fits into the node.
		/// </summary>
		/// <param name="TextureWidth">Texture width.</param>
		/// <param name="TextureHeight">Texture height.</param>
		/// <returns></returns>
		private bool ImageFits(ushort TextureWidth, ushort TextureHeight)
		{
			return rect.width >= TextureWidth && rect.height >= TextureHeight;

		}

		/// <summary>
		/// Is the node perfect fit for the image.
		/// </summary>
		/// <param name="TextureWidth">Texture width.</param>
		/// <param name="TextureHeight">Texture height.</param>
		/// <returns></returns>
		private bool PerfectFit(ushort TextureWidth, ushort TextureHeight)
		{
			return rect.width == TextureWidth && rect.height == TextureHeight;

		}
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="Rect"></param>
		public Node(RectInt Rect = default)
		{
			rect = Rect;
		}
#endregion
	};
}
