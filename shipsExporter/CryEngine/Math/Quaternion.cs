

using System;
using System.Globalization;

namespace CryEngine
{
	public struct Quaternion
	{
		public static readonly Quaternion Identity = new Quaternion(1, new Vector3(0, 0, 0));

		private Vector3 _v;
		private float _w;

		public float x { get { return _v.x; } set { _v.x = value; } }
		public float y { get { return _v.y; } set { _v.y = value; } }
		public float z { get { return _v.z; } set { _v.z = value; } }
		public float w { get { return _w; } set { _w = value; } }

		public Vector3 v { get { return _v; } set { v = value; } }

		public float X { get { return _v.x; } set { _v.x = value; } }
		public float Y { get { return _v.y; } set { _v.y = value; } }
		public float Z { get { return _v.z; } set { _v.z = value; } }
		public float W { get { return _w; } set { _w = value; } }

		public Vector3 V { get { return _v; } set { v = value; } }

		public Quaternion(Vector3 vector, float scalar)
		{
			_v = vector;
			_w = scalar;
		}

		public Quaternion(float scalar, Vector3 vector)
		{
			_v = vector;
			_w = scalar;
		}

		public Quaternion(float xCoord, float yCoord, float zCoord, float scalar)
		{
			_v = new Vector3(xCoord, yCoord, zCoord);
			_w = scalar;
		}

		public Quaternion(Matrix3x4 matrix)
		{
			float s, p, tr = matrix.m00 + matrix.m11 + matrix.m22;
			
			if (tr > 0)
			{
				s = (float)Math.Sqrt(tr + 1.0f);
				p = 0.5f / s;
				_w = s * 0.5f;
				_v = new Vector3((matrix.m21 - matrix.m12) * p,
								 (matrix.m02 - matrix.m20) * p,
								 (matrix.m10 - matrix.m01) * p);
			}
			else if ((matrix.m00 >= matrix.m11) && (matrix.m00 >= matrix.m22))
			{
				s = (float)Math.Sqrt(matrix.m00 - matrix.m11 - matrix.m22 + 1.0f);
				p = 0.5f / s;
				_w = (matrix.m21 - matrix.m12) * p;
				_v = new Vector3(s * 0.5f,
								(matrix.m10 + matrix.m01) * p,
								(matrix.m20 + matrix.m02) * p);
			}
			else if ((matrix.m11 >= matrix.m00) && (matrix.m11 >= matrix.m22))
			{
				s = (float)Math.Sqrt(matrix.m11 - matrix.m22 - matrix.m00 + 1.0f);
				p = 0.5f / s;
				_w = (matrix.m02 - matrix.m20) * p;
				_v = new Vector3((matrix.m01 + matrix.m10) * p,
								  s * 0.5f,
								 (matrix.m21 + matrix.m12) * p);
			}
			else if ((matrix.m22 >= matrix.m00) && (matrix.m22 >= matrix.m11))
			{
				s = (float)Math.Sqrt(matrix.m22 - matrix.m00 - matrix.m11 + 1.0f);
				p = 0.5f / s;
				_w = (matrix.m10 - matrix.m01) * p;
				_v = new Vector3((matrix.m02 + matrix.m20) * p,
								(matrix.m12 + matrix.m21) * p,
								s * 0.5f);
			}
			else
			{
				_w = 1;
				_v = new Vector3();
			}
		}

		public Quaternion(Vector3 forwardDirection)
		{
			//set default initialization for up-vector
			_w = 0.70710676908493042f;
			_v = new Vector3(forwardDirection.z * 0.70710676908493042f, 0, 0);
			float l = (float)Math.Sqrt(forwardDirection.x * forwardDirection.x + forwardDirection.y * forwardDirection.y);
			if (l > 0.00001f)
			{
				//calculate LookAt quaternion
				var hv = new Vector3(forwardDirection.x / l, forwardDirection.y / l + 1.0f, l + 1.0f);
				float r = (float)Math.Sqrt(hv.x * hv.x + hv.y * hv.y);
				float s = (float)Math.Sqrt(hv.z * hv.z + forwardDirection.z * forwardDirection.z);
				//generate the half-angle sine&cosine
				float hacos0 = 0.0f;
				float hasin0 = -1.0f;
				if (r > 0.00001f) { hacos0 = hv.y / r; hasin0 = -hv.x / r; }  //yaw
				float hacos1 = hv.z / s;
				float hasin1 = forwardDirection.z / s;                        //pitch
				_w = hacos0 * hacos1;
				_v.x = hacos0 * hasin1;
				_v.y = hasin0 * hasin1;
				_v.z = hasin0 * hacos1;
			}
		}

	

		

		#region Overrides
		public override int GetHashCode()
		{
			unchecked // Overflow is fine, just wrap
			{
				int hash = 17;

				hash = hash * 23 + X.GetHashCode();
				hash = hash * 23 + Y.GetHashCode();
				hash = hash * 23 + Z.GetHashCode();
				hash = hash * 23 + W.GetHashCode();

				return hash;
			}
		}

		

		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{0},{1},{2},{3}", X, Y, Z, W);
		}
        #endregion

        #region Operators
        public static Quaternion operator *(Quaternion value, float scale)
        {
            return new Quaternion(value.V * scale, value.W * scale);
        }

        public static Quaternion operator -(Quaternion q)
        {
            return new Quaternion(-q.W, -q.V);
        }

        public static Quaternion operator !(Quaternion q)
        {
            return new Quaternion(q.W, -q.V);
        }

        public static Quaternion operator *(Quaternion left, Quaternion right)
        {
            return new Quaternion(
                left.W * right.W - (left.V.X * right.V.X + left.V.Y * right.V.Y + left.V.Z * right.V.Z),
                new Vector3(left.V.Y * right.V.Z - left.V.Z * right.V.Y + left.W * right.V.X + left.V.X * right.W,
                left.V.Z * right.V.X - left.V.X * right.V.Z + left.W * right.V.Y + left.V.Y * right.W,
                left.V.X * right.V.Y - left.V.Y * right.V.X + left.W * right.V.Z + left.V.Z * right.W));
        }

        public static Quaternion operator /(Quaternion left, Quaternion right)
        {
            return (!right * left);
        }

        public static Quaternion operator %(Quaternion left, Quaternion right)
        {
            var p = right;
            if ((p.Dot(left)) < 0) p = -p;
            return new Quaternion(left.W + p.W, left.V + p.V);
        }

        public static Quaternion operator -(Quaternion left, Quaternion right)
        {
            return new Quaternion(left.W - right.W, left.V - right.V);
        }

        public static Quaternion operator /(Quaternion left, float right)
        {
            return new Quaternion(left.W / right, left.V / right);
        }

        public static Vector3 operator *(Quaternion quat, Vector3 vector)
        {
            var vOut = new Vector3();
            var r2 = new Vector3();

            r2.X = (quat.V.Y * vector.Z - quat.V.Z * vector.Y) + quat.W * vector.X;
            r2.Y = (quat.V.Z * vector.X - quat.V.X * vector.Z) + quat.W * vector.Y;
            r2.Z = (quat.V.X * vector.Y - quat.V.Y * vector.X) + quat.W * vector.Z;
            vOut.X = (r2.Z * quat.V.Y - r2.Y * quat.V.Z); vOut.X += vOut.X + vector.X;
            vOut.Y = (r2.X * quat.V.Z - r2.Z * quat.V.X); vOut.Y += vOut.Y + vector.Y;
            vOut.Z = (r2.Y * quat.V.X - r2.X * quat.V.Y); vOut.Z += vOut.Z + vector.Z;
            return vOut;
        }

        public static Vector3 operator *(Vector3 vector, Quaternion quat)
        {
            var vOut = new Vector3();
            var r2 = new Vector3();

            r2.X = (quat.V.Z * vector.Y - quat.V.Y * vector.Z) + quat.W * vector.X;
            r2.Y = (quat.V.X * vector.Z - quat.V.Z * vector.X) + quat.W * vector.Y;
            r2.Z = (quat.V.Y * vector.X - quat.V.X * vector.Y) + quat.W * vector.Z;
            vOut.X = (r2.Y * quat.V.Z - r2.Z * quat.V.Y); vOut.X += vOut.X + vector.X;
            vOut.Y = (r2.Z * quat.V.X - r2.X * quat.V.Z); vOut.Y += vOut.Y + vector.Y;
            vOut.Z = (r2.X * quat.V.Y - r2.Y * quat.V.X); vOut.Z += vOut.Z + vector.Z;
            return vOut;
        }

        public static bool operator ==(Quaternion left, Quaternion right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Quaternion left, Quaternion right)
        {
            return !(left == right);
        }
        #endregion
        
        #region Functions
        public void Normalize()
		{
			float inverseLength = MathHelpers.ISqrt(X * X + Y * Y + Z * Z + W * W);
			X *= inverseLength;
			Y *= inverseLength;
			Z *= inverseLength;
			W *= inverseLength;
		}

		public float Dot(Quaternion other)
		{
			return (V.X * V.X + V.Y * other.V.Y + V.Z * other.V.Z + W * other.W);
		}

		

		

		
		
		public void SetFromToRotation(Vector3 fromDirection, Vector3 toDirection)
		{
			float dot = fromDirection.x * toDirection.x + fromDirection.y * toDirection.y + fromDirection.z * toDirection.z + 1.0f;
			if (dot > 0.0001f)
			{
				float vx = fromDirection.y * toDirection.z - fromDirection.z * toDirection.y;
				float vy = fromDirection.z * toDirection.x - fromDirection.x * toDirection.z;
				float vz = fromDirection.x * toDirection.y - fromDirection.y * toDirection.x;
				float d = MathHelpers.ISqrt(dot * dot + vx * vx + vy * vy + vz * vz);
				_w = dot * d;
				_v.x = vx * d;
				_v.y = vy * d;
				_v.z = vz * d;
				return;
			}
			w = 0;
			v = fromDirection.Orthogonal.Normalized;
		}
        /// <summary>
		/// Spherical-Interpolation between unit quaternions.
		/// </summary>
		/// <param name="start">Starting quaternion, will be returned if time is 0</param>
		/// <param name="end">End quaternion, will be returned if time is 1</param>
		/// <param name="timeRatio">The ratio to slerp between, e.g. 0 = start, 1 = end</param>
		/// <returns>Spherically interpolated return value</returns>
		public static Quaternion Slerp(Quaternion start, Quaternion end, float timeRatio)
        {
            var p = start;
            var q = end;
            var q2 = new Quaternion();

            var cosine = p.Dot(q);
            if (cosine < 0.0f) { cosine = -cosine; q = -q; } //take shortest arc
            if (cosine > 0.9999f)
            {
                return Lerp(p, q, timeRatio);
            }

            q2.w = q.w - p.w * cosine;
            q2._v.x = q.v.x - p.v.x * cosine;
            q2._v.y = q.v.y - p.v.y * cosine;
            q2._v.z = q.v.z - p.v.z * cosine;
            var sine = (float)Math.Sqrt(q2.Dot(q2));

            float s, c;
            MathHelpers.SinCos((float)Math.Atan2(sine, cosine) * timeRatio, out s, out c);

            var returnValue = new Quaternion();
            returnValue.w = p.w * c + q2.w * s / sine;
            returnValue._v.x = p.v.x * c + q2.v.x * s / sine;
            returnValue._v.y = p.v.y * c + q2.v.y * s / sine;
            returnValue._v.z = p.v.z * c + q2.v.z * s / sine;

            return returnValue;
        }

        /// <summary>
        /// Linear-Interpolation between unit quaternions.
        /// </summary>
        /// <param name="start">Starting quaternion, will be returned if time is 0</param>
        /// <param name="end">End quaternion, will be returned if time is 1</param>
        /// <param name="timeRatio">The ratio to slerp between, e.g. 0 = start, 1 = end</param>
        /// <returns>Linearly interpolated return value</returns>
        public static Quaternion Lerp(Quaternion start, Quaternion end, float timeRatio)
        {
            var returnValue = new Quaternion();

            if (start.Dot(end) < 0)
            {
                end = -end;
            }

            returnValue._v.x = start.v.x * (1.0f - timeRatio) + end.v.x * timeRatio;
            returnValue._v.y = start.v.y * (1.0f - timeRatio) + end.v.y * timeRatio;
            returnValue._v.z = start.v.z * (1.0f - timeRatio) + end.v.z * timeRatio;
            returnValue.w = start.w * (1.0f - timeRatio) + end.w * timeRatio;
            returnValue.Normalize();

            return returnValue;
        }
        #endregion

        #region Static functions
        public static Quaternion CreateRotationX(float radians)
		{
			var quat = new Quaternion();

			float s, c;
			MathHelpers.SinCos(radians * 0.5f, out s, out c);
			quat._w = c;
			quat._v.x = s;
			quat._v.y = 0;
			quat._v.z = 0;

			return quat;
		}

		public static Quaternion CreateRotationY(float radians)
		{
			var quat = new Quaternion();

			float s, c;
			MathHelpers.SinCos(radians * 0.5f, out s, out c);
			quat._w = c;
			quat._v.x = 0;
			quat._v.y = s;
			quat._v.z = 0;

			return quat;
		}

		public static Quaternion CreateRotationZ(float radians)
		{
			var quat = new Quaternion();

			float s, c;
			MathHelpers.SinCos(radians * 0.5f, out s, out c);
			quat._w = c;
			quat._v.x = 0;
			quat._v.y = 0;
			quat._v.z = s;

			return quat;
		}

		

		/// <summary>
		/// Spherical-Interpolation between unit quaternions.
		/// </summary>
		/// <param name="start">Starting quaternion, will be returned if time is 0</param>
		/// <param name="end">End quaternion, will be returned if time is 1</param>
		/// <param name="timeRatio">The ratio to slerp between, e.g. 0 = start, 1 = end</param>
		/// <returns>Spherically interpolated return value</returns>
		

		/// <summary>
		/// Linear-Interpolation between unit quaternions.
		/// </summary>
		/// <param name="start">Starting quaternion, will be returned if time is 0</param>
		/// <param name="end">End quaternion, will be returned if time is 1</param>
		/// <param name="timeRatio">The ratio to slerp between, e.g. 0 = start, 1 = end</param>
		/// <returns>Linearly interpolated return value</returns>
		
		#endregion

		#region Properties
		public Vector3 Right { get { return new Vector3(2 * (v.x * v.x + w * w) - 1, 2 * (v.y * v.x + v.z * w), 2 * (v.z * v.x - v.y * w)); } }
		public Vector3 Forward { get { return new Vector3(2 * (v.x * v.y - v.z * w), 2 * (v.y * v.y + w * w) - 1, 2 * (v.z * v.y + v.x * w)); } }
		public Vector3 Up { get { return new Vector3(2 * (v.x * v.z + v.y * w), 2 * (v.y * v.z - v.x * w), 2 * (v.z * v.z + w * w) - 1); } }

		

		

		public Quaternion Normalized
		{
			get
			{
				var returnValue = this;
				returnValue.Normalize();
				return returnValue;
			}
		}

		public float Length { get { return (float)Math.Sqrt(W * W + X * X + Y * Y + Z * Z); } }

		public float Magnitude { get { return Length; } }

		public float LengthSquared { get { return W * W + X * X + Y * Y + Z * Z; } }

		public bool IsIdentity {  get { return w == 1 && x == 0 && y == 0 && z == 0; } }

		#endregion
	}
}
