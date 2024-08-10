#region License
/* FNA - XNA4 Reimplementation for Desktop Platforms
 * Copyright 2009-2024 Ethan Lee and the MonoGame Team
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

#region Using Statements
using System;
using System.IO;

using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Microsoft.Xna.Framework.Media
{
	public sealed class Video
	{
		#region Public Properties

		public int Width
		{
			get;
			private set;
		}

		public int Height
		{
			get;
			private set;
		}

		public float FramesPerSecond
		{
			get;
			private set;
		}

		public VideoSoundtrackType VideoSoundtrackType
		{
			get;
			private set;
		}

		// FIXME: This is hacked, look up "This is a part of the Duration hack!"
		public TimeSpan Duration
		{
			get;
			internal set;
		}

		#endregion

		#region Internal Properties

		internal GraphicsDevice GraphicsDevice
		{
			get;
			private set;
		}

		#endregion

		#region Internal Variables

		internal string handle;
		internal bool needsDurationHack;

		#endregion

		#region Internal Constructors

		internal Video(string fileName, GraphicsDevice device)
		{
			handle = fileName;
			GraphicsDevice = device;

			/* This is the raw file constructor; unlike the XNB
			 * constructor we can be up front about files not
			 * existing, so let's do that!
			 */
			if (!File.Exists(fileName))
			{
				throw new FileNotFoundException(fileName);
			}

			IntPtr theora;
			int width;
			int height;
			double fps;
			Theorafile.th_pixel_fmt fmt;
			Theorafile.tf_fopen(fileName, out theora);
			Theorafile.tf_videoinfo(
				theora,
				out width,
				out height,
				out fps,
				out fmt
			);
			Theorafile.tf_close(ref theora);

			Width = width;
			Height = height;
			FramesPerSecond = (float) fps;

			// FIXME: This is a part of the Duration hack!
			Duration = TimeSpan.MaxValue;
			needsDurationHack = true;
		}

		internal Video(
			string fileName,
			GraphicsDevice device,
			int durationMS,
			int width,
			int height,
			float framesPerSecond,
			VideoSoundtrackType soundtrackType
		) {
			handle = fileName;
			GraphicsDevice = device;

			/* This is the XNB constructor, which really just loads
			 * the metadata without actually loading the video. For
			 * accuracy's sake we have to wait until VideoPlayer
			 * tries to load this before throwing Exceptions.
			 */
			Width = width;
			Height = height;
			FramesPerSecond = framesPerSecond;

			// FIXME: Oh, hey! I wish we had this info in Theora!
			Duration = TimeSpan.FromMilliseconds(durationMS);
			needsDurationHack = false;

			VideoSoundtrackType = soundtrackType;
		}

		#endregion

		#region Public Extensions
		
		public static Video FromUriEXT(Uri uri, GraphicsDevice graphicsDevice)
		{
			string path;
			if (uri.IsAbsoluteUri)
			{
				// If it's absolute, be sure we can actually get it...
				if (!uri.IsFile)
				{
					throw new InvalidOperationException(
						"Only local file URIs are supported for now"
					);
				}
				path = uri.LocalPath;
			}
			else
			{
				path = Path.Combine(
					TitleLocation.Path,
					uri.ToString()
				);
			}

			return new Video(path, graphicsDevice);
		}

		// FIXME: These should be in VideoPlayer instead!

		internal int audioTrack = -1;
		internal int videoTrack = -1;
		internal VideoPlayer parent;

		public void SetAudioTrackEXT(int track)
		{
			audioTrack = track;
			if (parent != null)
			{
				parent.SetAudioTrackEXT(track);
			}
		}

		public void SetVideoTrackEXT(int track)
		{
			videoTrack = track;
			if (parent != null)
			{
				parent.SetVideoTrackEXT(track);
			}
		}

		#endregion
	}
}
