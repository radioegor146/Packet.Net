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

namespace PacketDotNet.Tcp
{
    /// <summary>
    /// AlternateChecksumRequest Option
    /// </summary>
    public class AlternateChecksumRequest : Option
    {
        #region Members

        // the offset (in bytes) of the Checksum field
        const Int32 ChecksumFieldOffset = 2;

        #endregion


        #region Constructors

        /// <summary>
        /// Creates an Alternate Checksum Request Option
        /// Used to negotiate an alternative checksum algorithm in a connection
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
        /// <remarks>
        /// References:
        /// http://datatracker.ietf.org/doc/rfc1146/
        /// </remarks>
        public AlternateChecksumRequest(Byte[] bytes, Int32 offset, Int32 length) :
            base(bytes, offset, length)
        { }

        #endregion


        #region Properties

        /// <summary>
        /// The Checksum
        /// </summary>
        public ChecksumAlgorithmType Checksum
        {
            get => (ChecksumAlgorithmType) OptionData.Bytes[OptionData.Offset + ChecksumFieldOffset];
            set => OptionData.Bytes[OptionData.Offset + ChecksumFieldOffset] = (byte) value;
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
            return "[" + Kind + ": ChecksumType=" + Checksum + "]";
        }

        #endregion
    }
}