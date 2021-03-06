//
// This file was generated by the BinaryNotes compiler.
// See http://bnotes.sourceforge.net 
// Any modifications to this file will be lost upon recompilation of the source ASN.1. 
//

using GSF.ASN1;
using GSF.ASN1.Attributes;
using GSF.ASN1.Coders;
using GSF.ASN1.Types;

namespace GSF.MMS.Model
{
    
    [ASN1PreparedElement]
    [ASN1BoxedType(Name = "FloatingPoint")]
    public class FloatingPoint : IASN1PreparedElement
    {
        private static readonly IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(FloatingPoint));
        private byte[] val;

        public FloatingPoint()
        {
        }

        public FloatingPoint(byte[] value)
        {
            Value = value;
        }

        public FloatingPoint(BitString value)
        {
            Value = value.Value;
        }

        [ASN1OctetString(Name = "FloatingPoint")]
        public byte[] Value
        {
            get
            {
                return val;
            }
            set
            {
                val = value;
            }
        }

        public void initWithDefaults()
        {
        }


        public IASN1PreparedElementData PreparedData
        {
            get
            {
                return preparedData;
            }
        }
    }
}