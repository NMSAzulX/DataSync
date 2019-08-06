using System;

namespace DataSync.Common
{
    public class Column
    {
        private Type _type;

        private object _value;

        private int _byteSize;

        public Column(object value, Type type, int byteSize)
        {
            _value = value;
            _type = type;
            _byteSize = byteSize;
        }

        public enum Type
        {
            Bad,
            Null,
            Int,
            Long,
            Double,
            String,
            Bool,
            Date,
            Bytes
        }
    }
}