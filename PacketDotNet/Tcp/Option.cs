/*
This file is part of PacketDotNet

PacketDotNet is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

PacketDotNet is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with PacketDotNet.  If not, see <http://www.gnu.org/licenses/>.
*/
/*
 *  Copyright 2010 Evan Plaice <evanplaice@gmail.com>
 */

using System;
using PacketDotNet.Utils;

namespace PacketDotNet.Tcp
{
    /// <summary>
    /// A TCP Option
    /// </summary>
    public abstract class Option
    {
        #region Constructors

        /// <summary>
        /// Creates an Option from a byte[]
        /// </summary>
        /// <param name="bytes">
        /// A <see cref="T:System.Byte[]" />
        /// </param>
        /// <param name="offset">
        /// A <see cref="System.Int32" />
        /// </param>
        /// <param name="length">
        /// A <see cref="System.Int32" />
        /// </param>
        protected Option(Byte[] bytes, Int32 offset, Int32 length)
        {
            OptionData = new ByteArraySegment(bytes, offset, length);
        }

        #endregion


        #region Methods

        /// <summary>
        /// Returns the Option info as a string
        /// </summary>
        /// <returns>
        /// A <see cref="string" />
        /// </returns>
        public override String ToString()
        {
            return "[" + Kind + "]";
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets the length of the option.
        /// </summary>
        public virtual Byte Length
        {
            get => OptionData.Bytes[OptionData.Offset + LengthFieldOffset];
            set => OptionData.Bytes[OptionData.Offset + LengthFieldOffset] = value;
        }

        /// <summary>
        /// Gets or sets the option kind.
        /// </summary>
        public OptionTypes Kind
        {
            get => (OptionTypes)OptionData.Bytes[OptionData.Offset + KindFieldOffset];
            set => OptionData.Bytes[OptionData.Offset + KindFieldOffset] = (byte)value;
        }

        /// <summary>
        /// Gets or sets a the underlying bytes.
        /// </summary>
        public Byte[] Bytes
        {
            get
            {
                var bytes = new Byte[OptionData.Length];
                Array.Copy(OptionData.Bytes, OptionData.Offset, bytes, 0, OptionData.Length);
                return bytes;
            }
            set
            {
                for (int i = 0; i < value.Length; i++)
                    OptionData.Bytes[OptionData.Offset + i] = value[i];
            }
        }

        #endregion


        #region Members

        // stores the data/length/offset of the option
        protected readonly ByteArraySegment OptionData;

        /// <summary>The length (in bytes) of the Kind field</summary>
        internal const Int32 KindFieldLength = 1;

        /// <summary>The length (in bytes) of the Length field</summary>
        internal const Int32 LengthFieldLength = 1;

        /// <summary>The offset (in bytes) of the Kind Field</summary>
        internal const Int32 KindFieldOffset = 0;

        /// <summary>The offset (in bytes) of the Length field</summary>
        internal const Int32 LengthFieldOffset = 1;

        #endregion
    }
}