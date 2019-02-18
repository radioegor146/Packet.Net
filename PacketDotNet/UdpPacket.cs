﻿/*
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
 *  Copyright 2009 Chris Morgan <chmorgan@gmail.com>
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using PacketDotNet.MiscUtil.Conversion;
using PacketDotNet.Utils;

#if DEBUG
using log4net;
#endif

namespace PacketDotNet
{
    /// <summary>
    /// User datagram protocol
    /// See http://en.wikipedia.org/wiki/Udp
    /// </summary>
    [Serializable]
    public sealed class UdpPacket : TransportPacket
    {
#if DEBUG
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
#else
// NOTE: No need to warn about lack of use, the compiler won't
//       put any calls to 'log' here but we need 'log' to exist to compile
#pragma warning disable 0169, 0649
        private static readonly ILogInactive Log;
#pragma warning restore 0169, 0649
#endif

        /// <summary> Fetch the port number on the source host.</summary>
        public override UInt16 SourcePort
        {
            get => EndianBitConverter.Big.ToUInt16(Header.Bytes, Header.Offset + UdpFields.SourcePortPosition);

            set
            {
                var val = value;
                EndianBitConverter.Big.CopyBytes(val, Header.Bytes, Header.Offset + UdpFields.SourcePortPosition);
            }
        }

        /// <summary> Fetch the port number on the target host.</summary>
        public override UInt16 DestinationPort
        {
            get => EndianBitConverter.Big.ToUInt16(Header.Bytes,
                                                   Header.Offset + UdpFields.DestinationPortPosition);

            set
            {
                var val = value;
                EndianBitConverter.Big.CopyBytes(val,
                                                 Header.Bytes,
                                                 Header.Offset + UdpFields.DestinationPortPosition);
            }
        }

        /// <value>
        /// Length in bytes of the header and payload, minimum size of 8,
        /// the size of the Udp header
        /// </value>
        public Int32 Length
        {
            get => EndianBitConverter.Big.ToInt16(Header.Bytes,
                                                  Header.Offset + UdpFields.HeaderLengthPosition);

            // Internal because it is updated based on the payload when
            // its bytes are retrieved
            internal set
            {
                var val = (Int16) value;
                EndianBitConverter.Big.CopyBytes(val,
                                                 Header.Bytes,
                                                 Header.Offset + UdpFields.HeaderLengthPosition);
            }
        }

        /// <summary> Fetch the header checksum.</summary>
        public override UInt16 Checksum
        {
            get => EndianBitConverter.Big.ToUInt16(Header.Bytes,
                                                   Header.Offset + UdpFields.ChecksumPosition);

            set
            {
                var val = value;
                EndianBitConverter.Big.CopyBytes(val,
                                                 Header.Bytes,
                                                 Header.Offset + UdpFields.ChecksumPosition);
            }
        }

        /// <summary> Check if the UDP packet is valid, checksum-wise.</summary>
        public Boolean ValidChecksum
        {
            get
            {
                // IPv6 has no checksum so only the TCP checksum needs evaluation
                if (ParentPacket is IPv6Packet)
                    return ValidUDPChecksum;
                // For IPv4 both the IP layer and the TCP layer contain checksums


                return ((IPv4Packet) ParentPacket).ValidIPChecksum && ValidUDPChecksum;
            }
        }

        /// <value>
        /// True if the udp checksum is valid
        /// </value>
        public Boolean ValidUDPChecksum
        {
            get
            {
                Log.Debug("ValidUDPChecksum");
                var retval = IsValidChecksum(TransportChecksumOption.IncludePseudoIPHeader);
                Log.DebugFormat("ValidUDPChecksum {0}", retval);
                return retval;
            }
        }

        /// <summary> Fetch ascii escape sequence of the color associated with this packet type.</summary>
        public override String Color => AnsiEscapeSequences.LightGreen;

        /// <summary>
        /// Update the Udp length
        /// </summary>
        public override void UpdateCalculatedValues()
        {
            // update the length field based on the length of this packet header
            // plus the length of all of the packets it contains
            Length = TotalPacketLength;
        }

        /// <summary>
        /// Create from values
        /// </summary>
        /// <param name="sourcePort">
        /// A <see cref="System.UInt16" />
        /// </param>
        /// <param name="destinationPort">
        /// A <see cref="System.UInt16" />
        /// </param>
        public UdpPacket(UInt16 sourcePort, UInt16 destinationPort)
        {
            Log.Debug("");

            // allocate memory for this packet
            var offset = 0;
            var length = UdpFields.HeaderLength;
            var headerBytes = new Byte[length];
            Header = new ByteArraySegment(headerBytes, offset, length);

            // set instance values
            SourcePort = sourcePort;
            DestinationPort = destinationPort;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bas">
        /// A <see cref="ByteArraySegment" />
        /// </param>
        public UdpPacket(ByteArraySegment bas)
        {
            Log.DebugFormat("ByteArraySegment {0}", bas);

            // set the header field, header field values are retrieved from this byte array
            Header = new ByteArraySegment(bas)
            {
                Length = UdpFields.HeaderLength
            };

            PayloadPacketOrData = new Lazy<PacketOrByteArraySegment>(() =>
            {
                const Int32 wakeOnLanPort0 = 0;
                const Int32 wakeOnLanPort7 = 7;
                const Int32 wakeOnLanPort9 = 9;
                const Int32 l2TpPort = 1701;
                const Int32 teredoPort = 3544;

                var result = new PacketOrByteArraySegment();
                var destinationPort = DestinationPort;
                var sourcePort = SourcePort;
                var payload = Header.EncapsulatedBytes();

                // If this packet is going to port 0, 7 or 9, then it might be a WakeOnLan packet.
                if (destinationPort == wakeOnLanPort0 || destinationPort == wakeOnLanPort7 || destinationPort == wakeOnLanPort9)
                {
                    if (WakeOnLanPacket.IsValid(payload))
                    {
                        result.Packet = new WakeOnLanPacket(payload);
                        return result;
                    }
                }
                
                if (destinationPort == l2TpPort || sourcePort == l2TpPort)
                {
                    result.Packet = new L2TPPacket(payload, this);
                    return result;
                }
                
                // Teredo encapsulates IPv6 traffic into UDP packets, parse out the bytes in the payload into packets.
                // If it contains a IPV6 packet, it to this current packet as a payload.
                // https://tools.ietf.org/html/rfc4380#section-5.1.1
                if (destinationPort == teredoPort || sourcePort == teredoPort)
                {
                    if (ContainsIPv6Packet(payload))
                    {
                        result.Packet = new IPv6Packet(payload);
                        return result;
                    }
                }

                // store the payload bytes
                result.ByteArraySegment = payload;
                return result;
            }, LazyThreadSafetyMode.PublicationOnly);
        }

        /// <summary>
        /// Determines whether the specified byte array segment contains an IPv6 packet.
        /// </summary>
        /// <param name="packetBytes">The packet bytes.</param>
        /// <returns>
        ///   <c>true</c> if it contains an IPv6 packet; otherwise, <c>false</c>.
        /// </returns>
        private static bool ContainsIPv6Packet(ByteArraySegment packetBytes)
        {
            // Packet bytes must be greater than or equal to the IPV6 header length, start with the version number, 
            // and be greater in length than the payload length + the header length.
            return packetBytes.Length >= IPv6Packet.HeaderMinimumLength
                   && packetBytes.Bytes[packetBytes.Offset] >> 4 == (int)RawIPPacketProtocol.IPv6
                   && packetBytes.Length >= IPv6Packet.HeaderMinimumLength + packetBytes.Bytes[packetBytes.Offset + IPv6Fields.PayloadLengthPosition];
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bas">
        /// A <see cref="ByteArraySegment" />
        /// </param>
        /// <param name="parentPacket">
        /// A <see cref="Packet" />
        /// </param>
        public UdpPacket
        (
            ByteArraySegment bas,
            Packet parentPacket) :
            this(bas)
        {
            ParentPacket = parentPacket;
        }

        /// <summary>
        /// Calculates the UDP checksum, optionally updating the UDP checksum header.
        /// </summary>
        /// <returns>The calculated UDP checksum.</returns>
        public UInt16 CalculateUDPChecksum()
        {
            return (ushort) CalculateChecksum(TransportChecksumOption.IncludePseudoIPHeader);
        }

        /// <summary>
        /// Update the checksum value.
        /// </summary>
        public void UpdateUDPChecksum()
        {
            Checksum = CalculateUDPChecksum();
        }

        /// <summary cref="Packet.ToString(StringOutputType)" />
        public override String ToString(StringOutputType outputFormat)
        {
            var buffer = new StringBuilder();
            var color = "";
            var colorEscape = "";

            if (outputFormat == StringOutputType.Colored || outputFormat == StringOutputType.VerboseColored)
            {
                color = Color;
                colorEscape = AnsiEscapeSequences.Reset;
            }

            if (outputFormat == StringOutputType.Normal || outputFormat == StringOutputType.Colored)
            {
                buffer.AppendFormat("{0}[UDPPacket: SourcePort={2}, DestinationPort={3}]{1}",
                                    color,
                                    colorEscape,
                                    SourcePort,
                                    DestinationPort);
            }

            if (outputFormat == StringOutputType.Verbose || outputFormat == StringOutputType.VerboseColored)
            {
                // collect the properties and their value
                var properties = new Dictionary<String, String>
                {
                    {"source", SourcePort.ToString()},
                    {"destination", DestinationPort.ToString()},
                    {"length", Length.ToString()},
                    {"checksum", "0x" + Checksum.ToString("x") + " [" + (ValidUDPChecksum ? "valid" : "invalid") + "]"}
                };

                // calculate the padding needed to right-justify the property names
                var padLength = RandomUtils.LongestStringLength(new List<String>(properties.Keys));

                // build the output string
                buffer.AppendLine("UDP:  ******* UDP - \"User Datagram Protocol\" - offset=? length=" + TotalPacketLength);
                buffer.AppendLine("UDP:");
                foreach (var property in properties)
                {
                    buffer.AppendLine("UDP: " + property.Key.PadLeft(padLength) + " = " + property.Value);
                }

                buffer.AppendLine("UDP:");
            }

            // append the base string output
            buffer.Append(base.ToString(outputFormat));

            return buffer.ToString();
        }

        /// <summary>
        /// Generate a random packet
        /// </summary>
        /// <returns>
        /// A <see cref="UdpPacket" />
        /// </returns>
        public static UdpPacket RandomPacket()
        {
            var rnd = new Random();
            var sourcePort = (UInt16) rnd.Next(UInt16.MinValue, UInt16.MaxValue);
            var destinationPort = (UInt16) rnd.Next(UInt16.MinValue, UInt16.MaxValue);

            return new UdpPacket(sourcePort, destinationPort);
        }
    }
}