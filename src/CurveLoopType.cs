#region License
/* FNA - XNA4 Reimplementation for Desktop Platforms
 * Copyright 2009-2023 Ethan Lee and the MonoGame Team
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */

/* Derived from code by the Mono.Xna Team (Copyright 2006).
 * Released under the MIT License. See monoxna.LICENSE for details.
 */
#endregion

namespace Microsoft.Xna.Framework
{
	/// <summary>
	/// Defines how the <see cref="Curve"/> value is determined for position before first point or after the end point on the <see cref="Curve"/>.
	/// </summary>
	public enum CurveLoopType
	{
		/// <summary>
		/// The value of <see cref="Curve"/> will be evaluated as first point for positions before the beginning and end point for positions after the end.
		/// </summary>
		Constant,
		/// <summary>
		/// The positions will wrap around from the end to beginning of the <see cref="Curve"/> for determined the value.
		/// </summary>
		Cycle,
		/// <summary>
		/// The positions will wrap around from the end to beginning of the <see cref="Curve"/>.
		/// The value will be offset by the difference between the values of first and end <see cref="CurveKey"/> multiplied by the wrap amount.
		/// If the position is before the beginning of the <see cref="Curve"/> the difference will be subtracted from its value; otherwise the difference will be added.
		/// </summary>
		CycleOffset,
		/// <summary>
		/// The value at the end of the <see cref="Curve"/> act as an offset from the same side of the <see cref="Curve"/> toward the opposite side.
		/// </summary>
		Oscillate,
		/// <summary>
		/// The linear interpolation will be performed for determined the value.
		/// </summary>
		Linear
	}
}
