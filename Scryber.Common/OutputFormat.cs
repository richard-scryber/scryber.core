using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Tracing;
using System.Reflection;
using System.Reflection.Metadata;

namespace Scryber
{
    public class OutputFormat : IEquatable<OutputFormat>
    {
        private string _name;

        private OutputFormat(string name)
        {
            this._name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj is OutputFormat format)
                return this.Equals((OutputFormat)format);
            else
                return false;
        }

        public bool Equals(OutputFormat other)
        {
            return OutputFormat.ReferenceEquals(this, other);
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }

        public override string ToString()
        {
            return _name;
        }

        

        // operators


        public static bool operator ==(OutputFormat one, OutputFormat two)
        {
            return OutputFormat.ReferenceEquals(one, two);
        }

        public static bool operator !=(OutputFormat one, OutputFormat two)
        {
            return !OutputFormat.ReferenceEquals(one, two);
        }

        // known

        public static readonly OutputFormat PDF;
        public static readonly OutputFormat None;

        // static factory methods

        public static string[] GetNames()
        {
            return _names.ToArray();
        }

        public static bool IsDefined(string name)
        {
            return _names.IndexOf(name) > -1;
        }

        static OutputFormat()
        {
            None = Register("None");
            PDF = Register("PDF");
        }

        private static List<OutputFormat> _formats = new List<OutputFormat>();
        private static List<string> _names = new List<string>();
        private static object _formatLock = new object();

        public static OutputFormat Register(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            OutputFormat format;

            lock (_formatLock)
            {
                foreach(var known in _formats)
                {
                    if (known._name == name)
                        throw new ArgumentOutOfRangeException(nameof(name), "The name '" + name + " is already registered");
                }

                format = new OutputFormat(name);
                _formats.Add(format);
                _names.Add(name);
            }

            return format;
        }
    }
}
